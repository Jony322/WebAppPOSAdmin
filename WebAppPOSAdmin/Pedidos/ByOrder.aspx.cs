using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Scripts;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Pedidos
{
    public partial class ByOrder : System.Web.UI.Page
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
                txtFecha_ini.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFecha_fin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Session.Remove("id_pedido");
                Session.Remove("id_compra");
                Session.Remove("purchases");
                Session.Remove("backorder");
            }
        }

        public void limpiarCampos()
        {
            try
            {
                ddlProveedor.ClearSelection();
                txtFecha_ini.Text = string.Empty;
                txtFecha_fin.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: ByOrder " + "Acción: limpiarCampos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void cargaInicial()
        {
            try
            {
                ddlProveedor.getProveedores();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: ByOrder " + "Acción: cargaInicial " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public bool validarFechas()
        {
            bool result = false;
            try
            {
                if (!txtFecha_ini.Text.Equals("") && !txtFecha_fin.Text.Equals(""))
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: ByOrder " + "Acción: validarFechas " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
                return result;
            }
        }

        public void BindDataGrid()
        {
            try
            {
                gvRelacion.DataSource = (List<CompraRelacionExtended>)Session["relacionCompra"];
                gvRelacion.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: ByOrder " + "Acción: BindDataGrid " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            List<ComprasPorPedidoExtended> list = null;
            try
            {
                if (validarFechas())
                {
                    DateTime fecha_ini = Convert.ToDateTime(txtFecha_ini.Text + " 00:01:00");
                    DateTime fecha_fin = Convert.ToDateTime(txtFecha_fin.Text + " 23:59:59");
                    gvRelacion.DataSource = null;
                    list = ((ddlProveedor.SelectedIndex != 0) ? new RepositorioCompras().getComprasPorPedido(fecha_ini, fecha_fin, Guid.Parse(ddlProveedor.SelectedValue)) : new RepositorioCompras().getComprasPorPedido(fecha_ini, fecha_fin));
                    gvRelacion.DataSource = list;
                    gvRelacion.DataBind();
                }
                else
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "validacionFechas()", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: ByOrder " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void gvRelacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "view":
                    Session.Remove("backorder");
                    gvDetailPurchase.DataSource = null;
                    gvDetailPurchase.DataBind();
                    lblTotal.Text = "$0.00";
                    ddlCapturas.Items.Clear();
                    Session["id_pedido"] = new Guid(e.CommandArgument.ToString());
                    ddlCapturas.getPurchases(Guid.Parse(e.CommandArgument.ToString()));
                    break;
                case "backorder":
                    {
                        Session.Remove("purchases");
                        Session["id_pedido"] = new Guid(e.CommandArgument.ToString());
                        Session["backorder"] = new Procedures().getBackOrder(Guid.Parse(e.CommandArgument.ToString()));
                        string arg2 = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/BackOrder.aspx";
                        string script2 = $"window.open('{arg2}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "modal", script2, addScriptTags: true);
                        break;
                    }
                case "concentrado":
                    {
                        Session.Remove("purchases");
                        Session.Remove("backorder");
                        Session["id_pedido"] = new Guid(e.CommandArgument.ToString());
                        string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/ConcentradoCompras.aspx";
                        string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                        break;
                    }
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            gvDetailPurchase.DataSource = null;
            gvDetailPurchase.DataBind();
            List<CompraArticuloExtended> purchaseByOrderDetail = new Procedures().getPurchaseByOrderDetail(Guid.Parse(Session["id_pedido"].ToString()), Guid.Parse(ddlCapturas.SelectedValue.ToString()));
            gvDetailPurchase.DataSource = purchaseByOrderDetail;
            gvDetailPurchase.DataBind();
            if (purchaseByOrderDetail != null)
            {
                lblTotal.Text = purchaseByOrderDetail.Sum((CompraArticuloExtended i) => i.cant_pza * i.costo).ToString("C2");
                btnPDF.Enabled = purchaseByOrderDetail.Count > 0;
                if (purchaseByOrderDetail.Count > 0)
                {
                    Session["id_compra"] = Guid.Parse(ddlCapturas.SelectedValue.ToString());
                    Session["purchases"] = purchaseByOrderDetail;
                }
                else
                {
                    Session.Remove("id_compra");
                    Session.Remove("purchases");
                }
            }
            txtNumFactura.Enabled = !Guid.Parse(ddlCapturas.SelectedValue.ToString()).Equals(default(Guid));
            btnGuardarFactura.Enabled = txtNumFactura.Enabled;
            txtNumFactura.Text = "";
            if (txtNumFactura.Enabled)
            {
                txtNumFactura.Text = new RepositorioCompras().getNumFactura(Guid.Parse(Session["id_pedido"].ToString()), Guid.Parse(ddlCapturas.SelectedValue.ToString()));
            }
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Compras.aspx";
            string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
        }

        protected void btnGuardarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNumFactura.Text.Trim().Length < 1)
                {
                    throw new Exception("Debe ingresar el número de factura.");
                }
                new RepositorioCompras().GuardarFactura(Guid.Parse(Session["id_pedido"].ToString()), Guid.Parse(ddlCapturas.SelectedValue.ToString()), txtNumFactura.Text);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: ByOrder " + "Acción: btnGuardarFactura_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }
    }
}