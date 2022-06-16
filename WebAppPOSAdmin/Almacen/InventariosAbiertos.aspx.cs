using System;
using System.Linq;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Almacen
{
    public partial class InventariosAbiertos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                fillGridView();
        }

        private void fillGridView()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                var queryable = from i in dcContextoSuPlazaDataContext.inventario_fisico
                                orderby i.fecha_ini
                                where i.fecha_fin == null
                                select new
                                {
                                    i.fecha_ini,
                                    i.proveedor.razon_social,
                                    i.user_name
                                };
                if (queryable.Count() > 0)
                {
                    gvInventarios.DataSource = queryable;
                    gvInventarios.DataBind();
                    return;
                }
                throw new Exception("No hay inventarios abiertos.");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}