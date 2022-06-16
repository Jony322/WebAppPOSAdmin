using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;

namespace WebAppPOSAdmin.Cajas
{
    public partial class frmVentasSuspendidas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void llenarSiguienteGrid(Guid id)
        {
            try
            {
                List<VentaArticuloExtended> dataSource = ((ICajas)new RepositorioCajas()).listaArticulosByid(id);
                gvCancelacionArticulo.DataSource = dataSource;
                gvCancelacionArticulo.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void DataBindGrid()
        {
            try
            {
                gvCancelaciones.DataSource = (List<VentaCanceladaExtended>)Session["cancelaciones"];
                gvCancelaciones.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            List<VentaSuspendidaExtended> list = null;
            try
            {
                DateTime dateTime = Convert.ToDateTime($"{txtFechaIni.Text} 00:00:00");
                DateTime dateTime2 = Convert.ToDateTime($"{txtFechaFin.Text} 23:59:59");
                Session["dateIni"] = dateTime;
                Session["dateFin"] = dateTime2;
                Session["SalesSuspended"] = null;
                if (!ddlCaja.SelectedValue.Equals("0") && !ddlCajero.SelectedValue.Equals(""))
                {
                    Session["caja"] = ddlCaja.SelectedValue;
                    Session["cajero"] = ddlCajero.SelectedValue;
                    Session["SalesSuspended"] = new RepositorioCajas().getVentasSuspendidas(dateTime, dateTime2, int.Parse(ddlCaja.SelectedValue), ddlCajero.SelectedValue);
                }
                else if (!ddlCaja.SelectedValue.Equals("0"))
                {
                    Session["caja"] = ddlCaja.SelectedValue;
                    Session["cajero"] = null;
                    Session["SalesSuspended"] = new RepositorioCajas().getVentasSuspendidas(dateTime, dateTime2, int.Parse(ddlCaja.SelectedValue));
                }
                else if (!ddlCajero.SelectedValue.Equals(""))
                {
                    Session["caja"] = null;
                    Session["cajero"] = ddlCajero.SelectedValue;
                    Session["SalesSuspended"] = new RepositorioCajas().getVentasSuspendidas(dateTime, dateTime2, ddlCajero.SelectedValue);
                }
                else
                {
                    Session["caja"] = null;
                    Session["cajero"] = null;
                    Session["SalesSuspended"] = new RepositorioCajas().getVentasSuspendidas(dateTime, dateTime2);
                }
                list = (List<VentaSuspendidaExtended>)Session["SalesSuspended"];
                if (list.Count == 0)
                {
                    throw new Exception("No se encontraron coincidencias.");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
            finally
            {
                btnExportarExcel.Enabled = list.Count > 0;
                btnExportarPdf.Enabled = list.Count > 0;
                gvCancelaciones.DataSource = list;
                gvCancelaciones.DataBind();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/VentaSuspendida.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/DocsXLS/VentaSuspendida.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void gvCancelaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (!(commandName == "view"))
                {
                    return;
                }
                Guid VentaID = Guid.Parse(e.CommandArgument.ToString());
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                var dataSource = (from va in dcContextoSuPlazaDataContext.venta_cancelada_articulo
                                  where va.id_venta_cancel.Equals(VentaID)
                                  select new
                                  {
                                      cod_barras = va.cod_barras,
                                      descripcion = va.articulo.descripcion,
                                      cantidad = va.cantidad,
                                      total = va.total().ToString("F2")
                                  }).ToList();
                gvCancelacionArticulo.DataSource = dataSource;
                gvCancelacionArticulo.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void crearPDF(string fecha_ini, string fecha_fin)
        {
            try
            {
                if (gvCancelaciones.Rows.Count <= 0)
                {
                    return;
                }
                Document document = new Document(PageSize.LETTER, 36f, 36f, 36f, 36f);
                PdfWriter.GetInstance(document, base.Response.OutputStream);
                document.Open();
                iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "/Images/plaza.png");
                instance.SetAbsolutePosition(50f, 710f);
                instance.ScaleAbsoluteWidth(100f);
                instance.ScaleAbsoluteHeight(75f);
                document.Add(instance);
                Paragraph paragraph = new Paragraph();
                paragraph.Alignment = 1;
                paragraph.Font = FontFactory.GetFont("Arial", 10f, 1);
                paragraph.Add("Cancelación de ventas  " + fecha_ini + " al día " + fecha_fin);
                document.Add(paragraph);
                paragraph.Clear();
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                gvCancelaciones.AllowPaging = false;
                DataBindGrid();
                PdfPTable pdfPTable = new PdfPTable(gvCancelaciones.Columns.Count);
                int[] array = new int[gvCancelaciones.Columns.Count];
                for (int i = 0; i < gvCancelaciones.Columns.Count; i++)
                {
                    array[i] = (int)gvCancelaciones.Columns[i].ItemStyle.Width.Value;
                    PdfPCell pdfPCell = new PdfPCell(new Phrase(base.Server.HtmlDecode(gvCancelaciones.HeaderRow.Cells[i].Text)));
                    pdfPCell.BackgroundColor = new BaseColor(Color.LightGray);
                    pdfPTable.AddCell(pdfPCell);
                }
                for (int j = 0; j < gvCancelaciones.Rows.Count; j++)
                {
                    if (gvCancelaciones.Rows[j].RowType != DataControlRowType.DataRow)
                    {
                        continue;
                    }
                    for (int k = 0; k < gvCancelaciones.Columns.Count; k++)
                    {
                        PdfPCell pdfPCell2 = new PdfPCell(new Phrase(base.Server.HtmlDecode(gvCancelaciones.Rows[j].Cells[k].Text)));
                        if (j % 2 == 0)
                        {
                            pdfPCell2.BackgroundColor = new BaseColor(Color.LightGreen);
                        }
                        pdfPTable.AddCell(pdfPCell2);
                    }
                }
                document.Add(pdfPTable);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", "attachment;filename=cancelacionVenta.pdf");
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