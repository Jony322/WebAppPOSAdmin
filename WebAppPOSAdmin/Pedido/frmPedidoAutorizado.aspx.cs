using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Pedido
{
    public partial class frmPedidoAutorizado : System.Web.UI.Page
    {

        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion
        private empleado acceso = new empleado();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                Session["OrderID"] = default(Guid);
                cargaInicial();
            }
        }

        public void cargaInicial()
        {
            try
            {
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddlProveedor.getProveedores("--TODOS--");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmPedidoAutorizado " + "Acción: cargaInicial " + ex.Message);
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
                                          where p.fecha_autorizado >= DateTime.Parse(txtFechaIni.Text + " 07:00:00") && p.fecha_autorizado <= DateTime.Parse(txtFechaFin.Text + " 23:59:59") && p.status_pedido.Equals("autorizado")
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
                                          where p.fecha_pedido.Date >= DateTime.Parse(txtFechaIni.Text) && p.fecha_pedido.Date <= DateTime.Parse(txtFechaFin.Text) && p.status_pedido.Equals("autorizado") && p.id_proveedor.Equals(Guid.Parse(ddlProveedor.SelectedValue))
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
                Log.Error(ex, "Excepción Generada en: frmPedidoAutorizado " + "Acción: loadOrders " + ex.Message);
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
                _ = ex.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                base.Response.Clear();
                base.Response.Buffer = true;
                base.Response.AddHeader("content-disposition", "attachment;filename=PedidosAutorizados.xls");
                base.Response.Charset = "";
                base.Response.ContentType = "application/vnd.ms-excel";
                using StringWriter stringWriter = new StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                gvOrderDetail.AllowPaging = false;
                gvOrderDetail.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvOrderDetail.HeaderRow.Cells)
                {
                    cell.BackColor = gvOrderDetail.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvOrderDetail.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell2 in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell2.BackColor = gvOrderDetail.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell2.BackColor = gvOrderDetail.RowStyle.BackColor;
                        }
                        cell2.CssClass = "textmode";
                    }
                }
                gvOrderDetail.RenderControl(writer);
                string s = "<style> .textmode { } </style>";
                base.Response.Write(s);
                base.Response.Output.Write(stringWriter.ToString());
                base.Response.Flush();
                base.Response.End();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (!(commandName == "view"))
                {
                    return;
                }
                dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
                try
                {
                    Session["OrderID"] = Guid.Parse(e.CommandArgument.ToString());
                    var source = from pa in dc.pedido_articulo
                                 where pa.id_pedido == Guid.Parse(Session["OrderID"].ToString())
                                 select new
                                 {
                                     cod_barras = pa.cod_barras,
                                     descripcion = dc.articulo.First((articulo a) => a.cod_barras.Equals(pa.cod_anexo)).descripcion,
                                     cantidad = pa.cantidad,
                                     unidad = dc.articulo.First((articulo a) => a.cod_barras.Equals(pa.cod_anexo)).unidad_medida.descripcion
                                 };
                    gvOrderDetail.DataSource = source.OrderBy(d => d.descripcion);
                    gvOrderDetail.DataBind();
                }
                finally
                {
                    if (dc != null)
                    {
                        ((IDisposable)dc).Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            createPDF();
        }

        public void createPDF()
        {
            try
            {
                if (Session["OrderID"].Equals(default(Guid)))
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                pedido pedido = dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido p) => p.id_pedido.Equals(Guid.Parse(Session["OrderID"].ToString())));
                pedido.pedido_articulo.OrderBy((pedido_articulo pa) => pa.articulo.descripcion);
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
                paragraph.Add("Pedido: " + pedido.num_pedido + ", " + pedido.proveedor.razon_social);
                document.Add(paragraph);
                paragraph.Clear();
                paragraph.Add("Autorizado:" + pedido.fecha_autorizado);
                document.Add(paragraph);
                paragraph.Clear();
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                PdfPTable pdfPTable = new PdfPTable(4);
                pdfPTable.WidthPercentage = 100f;
                pdfPTable.SetWidths(new float[4] { 20f, 50f, 15f, 15f });
                string[] array = new string[4] { "Código barras", "Descripción", "Unidad", "Cantidad" };
                for (int i = 0; i < array.Length; i++)
                {
                    PdfPCell pdfPCell = new PdfPCell(new Phrase(array[i], FontFactory.GetFont("Arial", 9f, BaseColor.BLACK)));
                    pdfPCell.HorizontalAlignment = 1;
                    pdfPCell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPTable.AddCell(pdfPCell);
                }
                int num = 0;
                foreach (pedido_articulo pa2 in pedido.pedido_articulo)
                {
                    PdfPCell pdfPCell2 = new PdfPCell(new Phrase(pa2.cod_barras, FontFactory.GetFont("Arial", 8f, BaseColor.BLACK)));
                    if (num % 2 == 1)
                    {
                        pdfPCell2.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    }
                    pdfPCell2.HorizontalAlignment = 1;
                    pdfPTable.AddCell(pdfPCell2);
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(dcContextoSuPlazaDataContext.articulo.First((articulo a) => a.cod_barras.Equals(pa2.cod_anexo)).descripcion, FontFactory.GetFont("Arial", 8f, BaseColor.BLACK)));
                    if (num % 2 == 1)
                    {
                        pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    }
                    pdfPCell3.HorizontalAlignment = 0;
                    pdfPTable.AddCell(pdfPCell3);
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase(dcContextoSuPlazaDataContext.articulo.First((articulo a) => a.cod_barras.Equals(pa2.cod_anexo)).unidad_medida.descripcion, FontFactory.GetFont("Arial", 8f, BaseColor.BLACK)));
                    if (num % 2 == 1)
                    {
                        pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    }
                    pdfPCell4.HorizontalAlignment = 1;
                    pdfPTable.AddCell(pdfPCell4);
                    PdfPCell pdfPCell5 = new PdfPCell(new Phrase(pa2.cantidad.ToString(), FontFactory.GetFont("Arial", 8f, BaseColor.BLACK)));
                    if (num % 2 == 1)
                    {
                        pdfPCell5.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    }
                    pdfPCell5.HorizontalAlignment = 1;
                    pdfPTable.AddCell(pdfPCell5);
                    num++;
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
                _ = ex.Message;
            }
        }
    }
}