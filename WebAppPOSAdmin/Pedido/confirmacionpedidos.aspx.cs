using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Pedido
{
    public partial class confirmacionpedidos : System.Web.UI.Page
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
                ddlPedido.getPedidosPendientes();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: cargaInicial " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void cargarCampos(Guid valor)
        {
            try
            {
                RepositorioPedidos repositorioPedidos = new RepositorioPedidos();
                pedido pedido = new pedido();
                pedido = ((IPedidos)repositorioPedidos).pedidoPendienteById(valor);
                if (pedido != null)
                {
                    txtDias.Text = pedido.no_dias.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: cargarcampos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void cargarGridView()
        {
            try
            {
                IPedidos pedidos = new RepositorioPedidos();
                Session["pendientes"] = pedidos.listaPedidoArticulosPendiente(Guid.Parse(ddlPedido.SelectedValue));
                BindDataGrid();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: cargarGridView " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void recorrerGridActualizar()
        {
            try
            {
                IPedidos pedidos = new RepositorioPedidos();
                List<pedido_articulo> list = new List<pedido_articulo>();
                Guid id_pedido = Guid.Parse(ddlPedido.SelectedValue);
                new TextBox();
                foreach (GridViewRow row in gvpedidoconfirmacion.Rows)
                {
                    pedido_articulo pedido_articulo = new pedido_articulo();
                    pedido_articulo.id_pedido = id_pedido;
                    pedido_articulo.cod_barras = gvpedidoconfirmacion.Rows[row.RowIndex].Cells[0].Text;
                    decimal num = decimal.Parse(((TextBox)gvpedidoconfirmacion.Rows[row.RowIndex].FindControl("txtReal")).Text);
                    decimal num2 = decimal.Parse(gvpedidoconfirmacion.Rows[row.RowIndex].Cells[7].Text);
                    if (num != 0m || num != num2)
                    {
                        pedido_articulo.cantidad = num;
                    }
                    else
                    {
                        pedido_articulo.cantidad = num2;
                    }
                    list.Add(pedido_articulo);
                }
                pedidos.listaPedidoArticuloActualizar(list, id_pedido);
                cargarGridView();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: recorrerGridActualizar " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void actualizarDatosSession(string cod_barras, decimal cantidad)
        {
            try
            {
                List<PedidoCapturaPendiente> list = (List<PedidoCapturaPendiente>)Session["pendientes"];
                foreach (PedidoCapturaPendiente item in list)
                {
                    if (item.cod_barras == cod_barras)
                    {
                        item.pedido_real = cantidad;
                        break;
                    }
                }
                Session["pendientes"] = list;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: actualizarDatosSession " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void BindDataGrid()
        {
            try
            {
                gvpedidoconfirmacion.DataSource = (List<PedidoCapturaPendiente>)Session["pendientes"];
                gvpedidoconfirmacion.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: BindDataGrid " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void calcularCostoCompra()
        {
            try
            {
                decimal num = default(decimal);
                foreach (GridViewRow row in gvpedidoconfirmacion.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        decimal num2 = decimal.Parse(((Literal)gvpedidoconfirmacion.Rows[row.RowIndex].Cells[4].FindControl("ltPrecioCosto")).Text);
                        decimal.Parse(gvpedidoconfirmacion.Rows[row.RowIndex].Cells[7].Text);
                        decimal num3 = decimal.Parse(((TextBox)gvpedidoconfirmacion.Rows[row.RowIndex].Cells[8].FindControl("txtReal")).Text.Trim());
                        num += num2 * num3;
                    }
                }
                txtTotal.Text = num.ToString("C2");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: calcularCostoCompra " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlPedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlPedido.SelectedItem.Equals("--SELECCIONAR--"))
                {
                    cargarCampos(Guid.Parse(ddlPedido.SelectedValue));
                    cargarGridView();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: ddlPedido_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                ex.Message.Replace("'", " ").Replace("\n", " ").Replace("\r", " ");
            }
        }

        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioPedidos().autorizarPedido(Guid.Parse(ddlPedido.SelectedValue));
                SaveOrder();
                new RepositorioPedidos().OrderAuthorized(Guid.Parse(ddlPedido.SelectedValue));
                base.Response.Redirect("~/Pedido/confirmacionpedidos.aspx", endResponse: false);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: btnAutorizar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveOrder();
            base.Response.Redirect("~/Pedido/confirmacionpedidos.aspx", endResponse: false);
        }

        public void ResetForm()
        {
            ddlPedido.Items.Clear();
            ddlPedido.getPedidosPendientes();
        }

        private void SaveOrder()
        {
            try
            {
                calcularCostoCompra();
                recorrerGridActualizar();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: SaveOrder " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected void gvpedidoconfirmacion_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void gvpedidoconfirmacion_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                _ = gvpedidoconfirmacion.Rows[e.NewEditIndex].Cells[0].Text;
                _ = gvpedidoconfirmacion.Rows[e.NewEditIndex].Cells[2].Text;
                decimal.Parse(gvpedidoconfirmacion.Rows[e.NewEditIndex].Cells[3].Text);
                gvpedidoconfirmacion.EditIndex = e.NewEditIndex;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: gvpedidoconfirmacion_RowEditing " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected void gvpedidoconfirmacion_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string cod_barras = gvpedidoconfirmacion.DataKeys[e.RowIndex].Values[0].ToString();
                decimal cantidad = decimal.Parse(((TextBox)gvpedidoconfirmacion.Rows[e.RowIndex].FindControl("txtReal")).Text);
                actualizarDatosSession(cod_barras, cantidad);
                gvpedidoconfirmacion.EditIndex = -1;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: gvpedidoconfirmacion_RowUpdating " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected void gvpedidoconfirmacion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvpedidoconfirmacion.EditIndex = -1;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: gvpedidoconfirmacion_RowCancelingEdit " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                calcularCostoCompra();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: btnCalcular_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected void gvpedidoconfirmacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((TextBox)e.Row.FindControl("txtReal")).Attributes.Add("onFocus", $"selected_row(this);getEstadistica('{e.Row.Cells[0].Text}')");
            }
        }

        public void generarPDF()
        {
            try
            {
                if (gvpedidoconfirmacion.Rows.Count <= 0)
                {
                    return;
                }
                Document document = new Document(PageSize.LETTER.Rotate(), 36f, 36f, 36f, 36f);
                PdfWriter.GetInstance(document, base.Response.OutputStream);
                document.Open();
                iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "/Images/plaza.png");
                instance.SetAbsolutePosition(50f, 530f);
                instance.ScaleAbsoluteWidth(165f);
                instance.ScaleAbsoluteHeight(70f);
                document.Add(instance);
                Paragraph paragraph = new Paragraph();
                paragraph.Alignment = 1;
                paragraph.Font = FontFactory.GetFont("Arial", 10f, 1);
                paragraph.Add("Pedido: " + new RepositorioPedidos().getOrderNumber(Guid.Parse(ddlPedido.SelectedValue)) + ", " + new RepositorioPedidos().getOrderProvider(Guid.Parse(ddlPedido.SelectedValue)) + ",  " + txtDias.Text + " días");
                document.Add(paragraph);
                paragraph.Clear();
                paragraph.Add("Total de pedido " + txtTotal.Text);
                document.Add(paragraph);
                paragraph.Clear();
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                gvpedidoconfirmacion.AllowPaging = false;
                PdfPTable pdfPTable = new PdfPTable(gvpedidoconfirmacion.Columns.Count);
                pdfPTable.WidthPercentage = 100f;
                pdfPTable.SetWidths(new float[9] { 14f, 38f, 8f, 8f, 8f, 8f, 8f, 8f, 8f });
                for (int i = 0; i < gvpedidoconfirmacion.Columns.Count; i++)
                {
                    PdfPCell pdfPCell = new PdfPCell(new Phrase(base.Server.HtmlDecode(gvpedidoconfirmacion.HeaderRow.Cells[i].Text).Trim(), FontFactory.GetFont("Arial", 9f, BaseColor.BLACK)));
                    pdfPCell.HorizontalAlignment = 1;
                    pdfPCell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPTable.AddCell(pdfPCell);
                }
                int[] array = new int[9] { 1, 0, 1, 1, 2, 1, 1, 1, 1 };
                for (int j = 0; j < gvpedidoconfirmacion.Rows.Count; j++)
                {
                    if (gvpedidoconfirmacion.Rows[j].RowType != DataControlRowType.DataRow)
                    {
                        continue;
                    }
                    Literal literal = (Literal)gvpedidoconfirmacion.Rows[j].Cells[4].FindControl("ltPrecioCosto");
                    for (int k = 0; k < gvpedidoconfirmacion.Columns.Count; k++)
                    {
                        TextBox textBox = (TextBox)gvpedidoconfirmacion.Rows[j].Cells[k].FindControl("txtReal");
                        string str = base.Server.HtmlDecode(gvpedidoconfirmacion.Rows[j].Cells[k].Text);
                        switch (k)
                        {
                            case 8:
                                str = textBox.Text.ToString();
                                break;
                            case 4:
                                str = literal.Text.ToString();
                                break;
                        }
                        PdfPCell pdfPCell2 = new PdfPCell(new Phrase(str, FontFactory.GetFont("Arial", 8f, BaseColor.BLACK)));
                        if (j % 2 == 1)
                        {
                            pdfPCell2.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                        }
                        pdfPCell2.HorizontalAlignment = array[k];
                        pdfPTable.AddCell(pdfPCell2);
                    }
                }
                document.Add(pdfPTable);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", "attachment;filename=PedidoCap.pdf");
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.Write(document);
                base.Response.Flush();
                base.Response.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: confirmacionpedidos " + "Acción: generarPDF " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}