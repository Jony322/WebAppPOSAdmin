using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Pedido
{
    public partial class frmPedidoNoAutorizado : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                cargaInicial();
            }
        }

        public void cargaInicial()
        {
            try
            {
                ddlProveedor.getProveedores("--TODOS--");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedidoNoAutorizado " + "Acción: cargaInicial " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void loadOrders()
        {
            try
            {
                if (ddlProveedor.SelectedValue == "00000000-0000-0000-0000-000000000000")
                {
                    using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                    gvOrders.DataSource = from p in dcContextoSuPlazaDataContext.pedido
                                          orderby p.num_pedido
                                          where p.status_pedido.Equals("pendiente")
                                          select new
                                          {
                                              p.id_pedido,
                                              p.num_pedido,
                                              p.proveedor.razon_social,
                                              p.fecha_pedido,
                                              p.fecha_autorizado
                                          };
                    gvOrders.DataBind();
                }
                else
                {
                    using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext2 = new dcContextoSuPlazaDataContext();
                    gvOrders.DataSource = from p in dcContextoSuPlazaDataContext2.pedido
                                          orderby p.num_pedido
                                          where p.status_pedido.Equals("pendiente") && p.id_proveedor.Equals(Guid.Parse(ddlProveedor.SelectedValue))
                                          select new
                                          {
                                              p.id_pedido,
                                              p.num_pedido,
                                              p.proveedor.razon_social,
                                              p.fecha_pedido,
                                              p.fecha_autorizado
                                          };
                    gvOrders.DataBind();
                }
                if (gvOrders.Rows.Count <= 0)
                {
                    throw new Exception("No se encontraron resultados.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedidoNoAutorizado " + "Acción: loadOrders " + ex.Message);
                loggerdb.Error(ex);
                string arg = ex.Message.Replace("'", " ").Replace("\n", " ").Replace("\r", " ");
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{arg}');", addScriptTags: true);
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                loadOrders();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedidoNoAutorizado " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (commandName == "delete")
                {
                    new RepositorioPedidos().eliminarPedidoById(Guid.Parse(e.CommandArgument.ToString()));
                    loadOrders();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedidoNoAutorizado " + "Acción: gvOrders_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}