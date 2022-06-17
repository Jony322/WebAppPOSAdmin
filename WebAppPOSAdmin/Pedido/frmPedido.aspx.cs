using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Scripts;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Pedido
{
    public partial class frmPedido : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private enum operations
        {
            NEW,
            RECOVERY
        }

        private operations operation;

        private List<PedidoArticulosExtended> lista;

        private pedido order;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["id"] != null && new RepositorioPedidos().existsOrder(Guid.Parse(base.Request.QueryString["id"].ToString())))
            {
                operation = operations.RECOVERY;
                order = new RepositorioPedidos().getOrderById(Guid.Parse(base.Request.QueryString["id"].ToString()));
                Session["OrderID"] = Guid.Parse(base.Request.QueryString["id"].ToString());
            }
            if (!base.IsPostBack)
            {
                cargaInicial();
                Session["suspendido"] = false;
                Session["pedido"] = null;
                if (operation.Equals(operations.RECOVERY))
                {
                    llenarDatosSuspendidos();
                    return;
                }
                Session.Remove("OrderID");
                Session["OrderID"] = Guid.NewGuid();
            }
        }

        public void cargaInicial()
        {
            try
            {
                ddlProveedor.getProveedores();
                ddlTipoPlan.getListaTipoPlan();
                ddlHistorico.getListaMeses();
                ddlDiasPedir.getListaDias();
                ddlSuspendidos.getListaPedidosSuspendidos();
                ddlHistorico.SelectedValue = DateTime.Now.Month.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: cargaInicial " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public bool validarDrop()
        {
            try
            {
                if (!ddlProveedor.SelectedItem.Equals("--SELECCIONAR--"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: validarDrop " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
                return false;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        public pedido getDatosPedido(string status)
        {
            pedido pedido = new pedido();
            pedido.id_pedido = Guid.Parse(Session["OrderID"].ToString());
            pedido.status_pedido = status;
            pedido.no_dias = short.Parse(ddlDiasPedir.SelectedItem.ToString());
            pedido.id_proveedor = Guid.Parse(ddlProveedor.SelectedValue);
            pedido.mes = short.Parse(ddlHistorico.SelectedValue);
            pedido.plan = ddlTipoPlan.SelectedValue;
            switch (short.Parse(ddlTipoPlan.Text))
            {
                case 1:
                case 3:
                    pedido.anio = (short)DateTime.Now.Year;
                    break;
                case 2:
                    pedido.anio = (short)(DateTime.Now.Year - 1);
                    break;
                default:
                    pedido.anio = (short)DateTime.Now.Year;
                    break;
            }
            return pedido;
        }

        public void recorrerGridInsertar(Guid id)
        {
            IPedidos pedidos = new RepositorioPedidos();
            try
            {
                short num = 1;
                foreach (GridViewRow row in gvPedidos.Rows)
                {
                    pedido_articulo pedido_articulo = new pedido_articulo();
                    pedido_articulo.id_pedido = id;
                    pedido_articulo.no_articulo = num;
                    pedido_articulo.cod_barras = gvPedidos.Rows[row.RowIndex].Cells[0].Text;
                    pedido_articulo.precio_articulo = decimal.Parse(gvPedidos.Rows[row.RowIndex].Cells[4].Text);
                    pedido_articulo.sugerido = decimal.Parse(gvPedidos.Rows[row.RowIndex].Cells[7].Text);
                    TextBox textBox = (TextBox)gvPedidos.Rows[row.RowIndex].FindControl("txtNuevoValor");
                    HiddenField hiddenField = (HiddenField)gvPedidos.Rows[row.RowIndex].FindControl("txtCodigoAnexo");
                    pedido_articulo.cantidad = decimal.Parse(textBox.Text);
                    pedido_articulo.cant_original = pedido_articulo.cantidad;
                    pedido_articulo.cod_anexo = hiddenField.Value;
                    pedidos.insertarPedidoArticulo(pedido_articulo);
                    num = (short)(num + 1);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: recorrerGridInsertar " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void recorrerGridActualizar(string status)
        {
            IPedidos pedidos = new RepositorioPedidos();
            List<pedido_articulo> list = new List<pedido_articulo>();
            Guid id_pedido = Guid.Parse(Session["OrderID"].ToString());
            try
            {
                foreach (GridViewRow row in gvPedidos.Rows)
                {
                    pedido_articulo pedido_articulo = new pedido_articulo();
                    pedido_articulo.cod_barras = gvPedidos.Rows[row.RowIndex].Cells[0].Text;
                    TextBox textBox = (TextBox)gvPedidos.Rows[row.RowIndex].FindControl("txtNuevoValor");
                    pedido_articulo.cantidad = decimal.Parse(textBox.Text);
                    pedido_articulo.cant_original = pedido_articulo.cantidad;
                    list.Add(pedido_articulo);
                }
                pedidos.actualizarPedidoArticulo(list, id_pedido, status);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: recorrerGridActualizar " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void llenarDatosSuspendidos()
        {
            try
            {
                txtID.Value = order.id_pedido.ToString();
                etEstado.Text = order.status_pedido;
                ddlTipoPlan.Text = order.plan;
                setItem(ref ddlHistorico, order.mes);
                setItemValue(ref ddlDiasPedir, order.no_dias);
                ddlProveedor.SelectedValue = order.id_proveedor.ToString();
                bloquearDrop();
                switch (int.Parse(ddlTipoPlan.SelectedValue))
                {
                    case 1:
                        _ = DateTime.Now.Year;
                        break;
                    case 2:
                        _ = DateTime.Now.Year;
                        break;
                    case 3:
                        _ = DateTime.Now.Year;
                        break;
                    default:
                        _ = DateTime.Now.Year;
                        break;
                }
                gvPedidos.DataSource = new Procedures().getPedidoSuspendido(order.id_pedido);
                gvPedidos.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: llenarDatosSuspendidos " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void bloquearDrop()
        {
            ddlProveedor.Enabled = false;
            ddlTipoPlan.Enabled = false;
            ddlHistorico.Enabled = false;
            ddlDiasPedir.Enabled = false;
        }

        public void setItem(ref DropDownList _control, long value)
        {
            string text = Convert.ToString(value);
            try
            {
                foreach (ListItem item in _control.Items)
                {
                    if (item.Value == text)
                    {
                        _control.SelectedValue = item.Value.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: setItem " + ex.Message);
                loggerdb.Error(ex);
                throw ex;
            }
        }

        public void setItemValue(ref DropDownList _control, int value)
        {
            string text = Convert.ToString(value);
            try
            {
                foreach (ListItem item in _control.Items)
                {
                    if (item.Text == text)
                    {
                        _control.SelectedValue = item.Value.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: setItemValue " + ex.Message);
                loggerdb.Error(ex);
                throw ex;
            }
        }

        public void showDetailOrder()
        {
            try
            {
                Guid id_proveedor = Guid.Parse(ddlProveedor.SelectedValue);
                short num = (short)DateTime.Today.Year;
                short mes_val = short.Parse(ddlHistorico.SelectedValue);
                ddlHistorico.SelectedItem.ToString();
                short dias_pedido = short.Parse(ddlDiasPedir.SelectedItem.ToString());
                switch (Convert.ToInt32(ddlTipoPlan.SelectedValue.ToString()))
                {
                    case 1:
                        gvPedidos.DataSource = new Procedures().listaPedidoArticuloByIdProveedor(id_proveedor, num, mes_val, dias_pedido);
                        break;
                    case 2:
                        gvPedidos.DataSource = new Procedures().listaPedidoArticuloByIdProveedor(id_proveedor, (short)(num - 1), mes_val, dias_pedido);
                        break;
                    case 3:
                        gvPedidos.DataSource = new Procedures().listaPedidoArticuloByIdProveedorPromedio(id_proveedor, num, mes_val, dias_pedido);
                        break;
                }
                gvPedidos.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: showDetailOrder " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void actualizarDatosSession(string cod_barras, decimal cantidad)
        {
            try
            {
                lista = (List<PedidoArticulosExtended>)Session["pedido"];
                foreach (PedidoArticulosExtended listum in lista)
                {
                    if (listum.cod_barras == cod_barras)
                    {
                        listum.num_articulos = cantidad;
                        break;
                    }
                }
                Session["pedido"] = lista;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: actualizarDatosSession " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioPedidos();
                lista = new List<PedidoArticulosExtended>();
                if (validarDrop().Equals(obj: true))
                {
                    showDetailOrder();
                }
                btnExportarPDF.Enabled = gvPedidos.Rows.Count > 0;
                btnExportarXLS.Enabled = gvPedidos.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                string arg = ex.Message.Replace("'", " ");
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{arg}');", addScriptTags: true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                IPedidos pedidos = new RepositorioPedidos();
                if (gvPedidos.Rows.Count > 0)
                {
                    if (operation.Equals(operations.RECOVERY))
                    {
                        recorrerGridActualizar("pendiente");
                    }
                    else
                    {
                        Guid id = pedidos.insertarPedido(getDatosPedido("pendiente"));
                        recorrerGridInsertar(id);
                        txtID.Value = id.ToString();
                    }
                    btnSuspender.Enabled = false;
                    btnExportarPDF.Enabled = true;
                    btnExportarXLS.Enabled = true;
                }
                else
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "validacionFiltro()", addScriptTags: true);
                }
                btnExportarPDF_Click(sender, e);
                ResetForm();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                string arg = ex.Message.Replace("'", " ");
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{arg}');", addScriptTags: true);
            }
        }

        protected void btnSuspender_Click(object sender, EventArgs e)
        {
            try
            {
                IPedidos pedidos = new RepositorioPedidos();
                if (operation.Equals(operations.RECOVERY))
                {
                    recorrerGridActualizar("suspendido");
                }
                else
                {
                    Guid id = pedidos.insertarPedido(getDatosPedido("suspendido"));
                    recorrerGridInsertar(id);
                }
                base.Response.Redirect("~/Pedido/frmPedido.aspx", endResponse: false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: btnSuspender_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void ResetForm()
        {
            ddlTipoPlan.SelectedIndex = 0;
            ddlDiasPedir.SelectedIndex = 0;
            ddlHistorico.SelectedIndex = 0;
            ddlProveedor.SelectedIndex = 0;
            gvPedidos.DataSource = null;
            gvPedidos.DataBind();
            Session["OrderID"] = Guid.NewGuid();
            btnExportarXLS.Enabled = false;
            btnExportarPDF.Enabled = false;
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            try
            {
                base.Response.Redirect($"~/Pedido/frmPedido.aspx?id={ddlSuspendidos.SelectedValue}", endResponse: false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: btnRecuperar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvPedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvPedidos.Rows)
                {
                    if (row.RowIndex == gvPedidos.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#F9F9F9");
                        row.ToolTip = string.Empty;
                        gvPedidos.DataKeys[row.RowIndex].Value.ToString();
                        _ = gvPedidos.Rows[row.RowIndex].Cells[2].Text;
                        decimal.Parse(gvPedidos.Rows[row.RowIndex].Cells[3].Text);
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                        row.ToolTip = "Click para seleccionar esta fila.";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: gvPedidos_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox obj = (TextBox)e.Row.FindControl("txtNuevoValor");
                    obj.Attributes.Add("onFocus", $"getEstadistica('{e.Row.Cells[0].Text}'); selected_row(this)");
                    obj.Attributes.Add("onKeyDown", $"return validateNumberAndGotoNextCtrlPedido(event,this,'{e.Row.Cells[2].Text}')");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: gvPedidos_RowDataBound " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvPedidos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                _ = gvPedidos.Rows[e.NewEditIndex].Cells[0].Text;
                _ = gvPedidos.Rows[e.NewEditIndex].Cells[2].Text;
                decimal.Parse(gvPedidos.Rows[e.NewEditIndex].Cells[3].Text);
                gvPedidos.EditIndex = e.NewEditIndex;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: gvPedidos_RowEditing " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void gvPedidos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string cod_barras = gvPedidos.DataKeys[e.RowIndex].Values[0].ToString();
                decimal cantidad = decimal.Parse(((TextBox)gvPedidos.Rows[e.RowIndex].FindControl("txtNuevoValor")).Text);
                actualizarDatosSession(cod_barras, cantidad);
                gvPedidos.EditIndex = -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: gvPedidos_RowUpdating " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void gvPedidos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvPedidos.EditIndex = -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: gvPedidos_RowCancelingEdit " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnExportarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvPedidos.Rows.Count > 0)
                {
                    Session["Order"] = (((Button)sender).ID.ToString().Equals("btnGuardar") ? generarPedido(Guid.Parse(Session["OrderID"].ToString())) : generarPedido(default(Guid)));
                    string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Pedidos.aspx";
                    string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                }
                else
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "sinResultados()", addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: btnExportarPDF_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void gvPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (commandName == "obsolete")
                {
                    new RepositorioArticulos().setObsoleteItem(e.CommandArgument.ToString());
                    showDetailOrder(e.CommandArgument.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: gvPedidos_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void showDetailOrder(string deleteItem)
        {
            List<ArticuloAnexoExtended> list = new List<ArticuloAnexoExtended>();
            for (int i = 0; i < gvPedidos.Rows.Count; i++)
            {
                if (gvPedidos.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    ArticuloAnexoExtended articuloAnexoExtended = new ArticuloAnexoExtended();
                    articuloAnexoExtended.cod_barras = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[0].Text);
                    articuloAnexoExtended.cod_anexo = ((HiddenField)gvPedidos.Rows[i].FindControl("txtCodigoAnexo")).Value;
                    articuloAnexoExtended.articulo = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[1].Text);
                    articuloAnexoExtended.unidad = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[2].Text);
                    articuloAnexoExtended.cantidad_um = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[3].Text));
                    articuloAnexoExtended.costo = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[4].Text));
                    articuloAnexoExtended.stock_cja = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[5].Text));
                    articuloAnexoExtended.stock_pza = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[6].Text));
                    articuloAnexoExtended.sugerido = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[7].Text));
                    articuloAnexoExtended.pedir = decimal.Parse(((TextBox)gvPedidos.Rows[i].FindControl("txtNuevoValor")).Text);
                    if (articuloAnexoExtended.cod_barras.CompareTo(deleteItem) != 0)
                    {
                        list.Add(articuloAnexoExtended);
                    }
                }
            }
            gvPedidos.DataSource = list;
            gvPedidos.DataBind();
        }

        public OrderExtended generarPedido(Guid OrderID)
        {
            OrderExtended orderExtended = new OrderExtended();
            orderExtended.id_pedido = OrderID;
            orderExtended.proveedor = ddlProveedor.SelectedItem.ToString();
            orderExtended.no_dias = short.Parse(ddlDiasPedir.SelectedItem.ToString());
            for (int i = 0; i < gvPedidos.Rows.Count; i++)
            {
                if (gvPedidos.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    PedidoArticulosExtended pedidoArticulosExtended = new PedidoArticulosExtended();
                    pedidoArticulosExtended.cod_barras = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[0].Text);
                    pedidoArticulosExtended.descripcion = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[1].Text);
                    pedidoArticulosExtended.unidad = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[2].Text);
                    pedidoArticulosExtended.umc = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[3].Text));
                    pedidoArticulosExtended.costo = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[4].Text));
                    pedidoArticulosExtended.existencia_caja = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[5].Text));
                    pedidoArticulosExtended.existencias_pieza = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[6].Text));
                    pedidoArticulosExtended.sugerido = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[7].Text));
                    pedidoArticulosExtended.a_pedir = decimal.Parse(((TextBox)gvPedidos.Rows[i].FindControl("txtNuevoValor")).Text);
                    orderExtended.items.Add(pedidoArticulosExtended);
                }
            }
            return orderExtended;
        }

        public OrderExtended generarPedido()
        {
            OrderExtended orderExtended = new OrderExtended();
            orderExtended.proveedor = ddlProveedor.SelectedItem.ToString();
            orderExtended.no_dias = short.Parse(ddlDiasPedir.SelectedItem.ToString());
            for (int i = 0; i < gvPedidos.Rows.Count; i++)
            {
                if (gvPedidos.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    PedidoArticulosExtended pedidoArticulosExtended = new PedidoArticulosExtended();
                    pedidoArticulosExtended.cod_barras = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[0].Text);
                    pedidoArticulosExtended.descripcion = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[1].Text);
                    pedidoArticulosExtended.unidad = base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[2].Text);
                    pedidoArticulosExtended.umc = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[3].Text));
                    pedidoArticulosExtended.costo = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[4].Text));
                    pedidoArticulosExtended.existencia_caja = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[5].Text));
                    pedidoArticulosExtended.existencias_pieza = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[6].Text));
                    pedidoArticulosExtended.sugerido = decimal.Parse(base.Server.HtmlDecode(gvPedidos.Rows[i].Cells[7].Text));
                    pedidoArticulosExtended.a_pedir = decimal.Parse(((TextBox)gvPedidos.Rows[i].FindControl("txtNuevoValor")).Text);
                    orderExtended.items.Add(pedidoArticulosExtended);
                }
            }
            return orderExtended;
        }

        protected void btnExportarXLS_Click(object sender, EventArgs e)
        {
            Session["Order"] = generarPedido();
            string arg = "http://" + base.Request["HTTP_HOST"] + "/DocsXLS/Pedidos.aspx";
            string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
        }

        protected void btnNewOrder_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("~/Pedido/frmPedido.aspx", endResponse: false);
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                decimal num = 0.0m;
                decimal num2 = 0.0m;
                for (int i = 0; i < gvPedidos.Rows.Count; i++)
                {
                    if (gvPedidos.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        decimal num3 = decimal.Parse(gvPedidos.Rows[i].Cells[4].Text);
                        decimal num4 = decimal.Parse(((TextBox)gvPedidos.Rows[i].Cells[8].FindControl("txtNuevoValor")).Text);
                        if ("Cja".Equals(gvPedidos.Rows[i].Cells[2].Text))
                        {
                            num2 += num4;
                        }
                        num += num3 * num4;
                    }
                }
                txtTotalCjs.Text = num2.ToString();
                txtTotalPedido.Text = num.ToString("C2");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedido " + "Acción: btnCalculate_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}