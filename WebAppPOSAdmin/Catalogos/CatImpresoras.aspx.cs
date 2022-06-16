using NLog;
using System;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Util.Impresora;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatImpresoras : System.Web.UI.Page
    {
        #region
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["id_pos"] = 0;
                string[] avaiblePrinters = ServerPrinters.getAvaiblePrinters();
                foreach (string text in avaiblePrinters)
                {
                    availablePrinters.Items.Add(new ListItem(text));
                }
                BindDataGrid();
            }
        }

        public pos_admin_settings getDatos()
        {
            return new pos_admin_settings
            {
                path_ptr_label = availablePrinters.Text.Trim()
            };
        }

        public void BindDataGrid()
        {
            try
            {
                pos_admin_settings settingsPrinter = new RepositorioGeneralidades().getSettingsPrinter();
                if (settingsPrinter != null)
                {
                    ListItem listItem = availablePrinters.Items.FindByValue(settingsPrinter.path_ptr_label);
                    if (listItem != null)
                    {
                        availablePrinters.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatImpresoras " + "Acción: BindDataGrid " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioGeneralidades().saveSetting(availablePrinters.Text.Trim().ToString());
                Session["id_pos"] = 0;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatImpresoras " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}