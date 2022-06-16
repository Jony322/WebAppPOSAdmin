using System;
using System.Linq;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Entidad;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class Settings : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadSettings();
            }
        }

        public void loadSettings()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                pos_admin_settings pos_admin_settings = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
                if (pos_admin_settings != null)
                {
                    txtCodNormal.Text = pos_admin_settings.cod_normal.ToString();
                    txtCodPesable.Text = pos_admin_settings.cod_pesable.ToString();
                    txtCodNoPesable.Text = pos_admin_settings.cod_nopesable.ToString();
                    txtIVA.Text = (pos_admin_settings.iva * 100.0m).ToString();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Settings " + "Acción: loadSettings " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                pos_admin_settings pos_admin_settings = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
                if (pos_admin_settings == null)
                {
                    pos_admin_settings pos_admin_settings2 = new pos_admin_settings();
                    pos_admin_settings2.id_setting = Guid.NewGuid();
                    pos_admin_settings2.cod_normal = long.Parse(txtCodNormal.Text.Trim());
                    pos_admin_settings2.cod_pesable = long.Parse(txtCodPesable.Text.Trim());
                    pos_admin_settings2.cod_nopesable = long.Parse(txtCodNoPesable.Text.Trim());
                    pos_admin_settings2.iva = decimal.Parse(txtIVA.Text.Trim()) / 100.0m;
                    dcContextoSuPlazaDataContext.pos_admin_settings.InsertOnSubmit(pos_admin_settings2);
                }
                else
                {
                    pos_admin_settings.cod_normal = long.Parse(txtCodNormal.Text.Trim());
                    pos_admin_settings.cod_pesable = long.Parse(txtCodPesable.Text.Trim());
                    pos_admin_settings.cod_nopesable = long.Parse(txtCodNoPesable.Text.Trim());
                    pos_admin_settings.iva = decimal.Parse(txtIVA.Text.Trim()) / 100.0m;
                }
                dcContextoSuPlazaDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Settings " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }
    }
}