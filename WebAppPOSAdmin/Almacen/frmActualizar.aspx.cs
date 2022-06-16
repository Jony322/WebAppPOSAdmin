using System;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.DropDownListExtender;

namespace WebAppPOSAdmin.Almacen
{
    public partial class frmActualizar : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ddlProveedor.getListaInventariosAbierto();
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlProveedor.SelectedIndex > 0)
                {
                    new RepositorioAlmacen().UpdateAndLock(Guid.Parse(ddlProveedor.SelectedValue));
                    ddlProveedor.Items.Clear();
                    ddlProveedor.getListaInventariosAbierto();
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", string.Format("alert('{0}');", "El Inventario se actualizó y cerró exitosamente"), addScriptTags: true);
                    return;
                }
                throw new Exception("Debe elegir un Proveedor");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmActualizar " + "Acción: btnActualizar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}