using System;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Security.SeguridadSession;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Almacen
{
    public partial class frmFijarAlmacen : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                cargarDropInicial();
        }

        public void cargarDropInicial()
        {
            try
            {
                ddlProveedor.getProveedores();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmFijarAlmacen " + "Acción: cargarDropInicial " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnFijar_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioAlmacen().lookStoreHouse(new inventario_fisico
                {
                    id_proveedor = Guid.Parse(ddlProveedor.SelectedValue),
                    user_name = ((empleado)((SessionManager)Session["usuarioSession"]).Parametros["usuarioSession"]).user_name
                });
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", string.Format("alert('{0}');", "El inventario se fijó exitosamente"), addScriptTags: true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmFijarAlmacen " + "Acción: btnFijar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}