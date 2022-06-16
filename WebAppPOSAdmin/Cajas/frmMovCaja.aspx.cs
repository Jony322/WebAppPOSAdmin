using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.DropDownListExtender;

namespace WebAppPOSAdmin.Cajas
{
    public partial class frmMovCaja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                cargarDropIniciales();
            }
        }

        public void cargarDropIniciales()
        {
            try
            {
                ddlCaja.getListPOS();
                ddlCajero.getListaCajeros();
                ddlSupervisor.getSupervisoresMovimientos();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        public void crearPDF(object fecha_inicial, object fecha_fin, object caja)
        {
            try
            {
                if (gvMovimiento.Rows.Count <= 0)
                {
                    return;
                }
                Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(document, base.Response.OutputStream);
                document.Open();
                iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "/Images/plaza.png");
                instance.SetAbsolutePosition(50f, 530f);
                instance.ScaleAbsoluteWidth(100f);
                instance.ScaleAbsoluteHeight(75f);
                document.Add(instance);
                Paragraph paragraph = new Paragraph();
                paragraph.Alignment = 1;
                paragraph.Font = FontFactory.GetFont("Arial", 10f, 1);
                paragraph.Add(string.Concat("Relación de movimientos descuentos y cambios de precio del día  ", fecha_inicial, " al día ", fecha_fin));
                document.Add(paragraph);
                paragraph.Clear();
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                gvMovimiento.AllowPaging = false;
                DataBindGrid();
                PdfPTable pdfPTable = new PdfPTable(gvMovimiento.Columns.Count);
                for (int i = 0; i < gvMovimiento.Columns.Count; i++)
                {
                    PdfPCell pdfPCell = new PdfPCell(new Phrase(base.Server.HtmlDecode(gvMovimiento.HeaderRow.Cells[i].Text)));
                    pdfPCell.BackgroundColor = new BaseColor(Color.LightGray);
                    pdfPTable.AddCell(pdfPCell);
                }
                for (int j = 0; j < gvMovimiento.Rows.Count; j++)
                {
                    if (gvMovimiento.Rows[j].RowType != DataControlRowType.DataRow)
                    {
                        continue;
                    }
                    for (int k = 0; k < gvMovimiento.Columns.Count; k++)
                    {
                        PdfPCell pdfPCell2 = new PdfPCell(new Phrase(base.Server.HtmlDecode(gvMovimiento.Rows[j].Cells[k].Text)));
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
                base.Response.AppendHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
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

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioCajas();
                List<VentaMovimientoExtended> list = new List<VentaMovimientoExtended>();
                gvMovimiento.DataSource = null;
                gvMovimiento.DataBind();
                if (validarFechas().Equals(obj: true))
                {
                    DateTime fecha_ini = Convert.ToDateTime($"{txtFechaIni.Text} 00:00:00");
                    DateTime fecha_fin = Convert.ToDateTime($"{txtFechaFin.Text} 23:59:59");
                    list = ((ddlCaja.SelectedIndex != 0 && ddlSupervisor.SelectedIndex != 0 && ddlCajero.SelectedIndex != 0) ? new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin, ddlCajero.SelectedValue.ToString(), ddlSupervisor.SelectedValue.ToString(), int.Parse(ddlCaja.SelectedValue)) : ((ddlCaja.SelectedIndex != 0 && ddlSupervisor.SelectedIndex != 0) ? new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin, ddlSupervisor.SelectedValue.ToString(), int.Parse(ddlCaja.SelectedValue)) : ((ddlCaja.SelectedIndex != 0 && ddlCajero.SelectedIndex != 0) ? new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin, int.Parse(ddlCaja.SelectedValue), ddlCajero.SelectedValue.ToString()) : ((ddlSupervisor.SelectedIndex != 0 && ddlCajero.SelectedIndex != 0) ? new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin, ddlCajero.SelectedValue.ToString(), ddlSupervisor.SelectedValue.ToString()) : ((ddlCaja.SelectedIndex != 0) ? new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin, int.Parse(ddlCaja.SelectedValue)) : ((ddlSupervisor.SelectedIndex != 0) ? new RepositorioCajas().getMovimientos(ddlSupervisor.SelectedValue.ToString(), fecha_ini, fecha_fin) : ((ddlCajero.SelectedIndex == 0) ? new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin) : new RepositorioCajas().getMovimientos(fecha_ini, fecha_fin, ddlCajero.SelectedValue.ToString()))))))));
                }
                else
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "validacionFechas()", addScriptTags: true);
                }
                if (list.Count > 0)
                {
                    Session["listaMov"] = list;
                    DataBindGrid();
                }
                else
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "sinResultados()", addScriptTags: true);
                }
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
                gvMovimiento.DataSource = (List<VentaMovimientoExtended>)Session["listaMov"];
                gvMovimiento.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public bool validarDropDown()
        {
            bool result = false;
            try
            {
                if (!ddlSupervisor.SelectedItem.ToString().Equals("--SELECCIONAR--") && !ddlCajero.SelectedItem.ToString().Equals("--SELECCIONAR--"))
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public bool validarFechas()
        {
            bool result = false;
            try
            {
                if (!txtFechaIni.Text.Equals("") && !txtFechaFin.Text.Equals(""))
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                crearPDF(txtFechaIni.Text, txtFechaFin.Text, ddlCajero.SelectedItem);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                base.Response.Clear();
                base.Response.Buffer = true;
                base.Response.AddHeader("content-disposition", "attachment;filename=ArticulosVisor.xls");
                base.Response.Charset = "";
                base.Response.ContentType = "application/vnd.ms-excel";
                using StringWriter stringWriter = new StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                gvMovimiento.AllowPaging = false;
                DataBindGrid();
                gvMovimiento.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvMovimiento.HeaderRow.Cells)
                {
                    cell.BackColor = gvMovimiento.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvMovimiento.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell2 in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell2.BackColor = gvMovimiento.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell2.BackColor = gvMovimiento.RowStyle.BackColor;
                        }
                        cell2.CssClass = "textmode";
                    }
                }
                gvMovimiento.RenderControl(writer);
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
    }
}