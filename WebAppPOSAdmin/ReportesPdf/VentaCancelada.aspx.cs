using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.ReportesPdf
{
    public partial class VentaCancelada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                createPDF();
            }
        }

        public void createPDF()
        {
            try
            {
                if (Session["SalesCancel"] == null)
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                DateTime dateTime = (DateTime)Session["dateIni"];
                DateTime dateTime2 = (DateTime)Session["dateFin"];
                string text = ((Session["cajero"] != null) ? Session["cajero"].ToString() : null);
                string text2 = ((Session["supervisor"] != null) ? Session["supervisor"].ToString() : null);
                short num = (short)((Session["caja"] != null) ? short.Parse(Session["caja"].ToString()) : 0);
                List<VentaCanceladaExtended> list = (List<VentaCanceladaExtended>)Session["SalesCancel"];
                iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, 1);
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0);
                Document document = new Document(PageSize.LETTER, 36f, 36f, 36f, 36f);
                PdfWriter.GetInstance(document, base.Response.OutputStream);
                document.Open();
                PdfPTable pdfPTable = new PdfPTable(2);
                pdfPTable.WidthPercentage = 100f;
                pdfPTable.SetWidths(new float[2] { 30f, 70f });
                iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "/Images/plaza.png");
                instance.SetAbsolutePosition(50f, 530f);
                instance.ScaleAbsoluteWidth(165f);
                instance.ScaleAbsoluteHeight(70f);
                PdfPCell pdfPCell = new PdfPCell(instance);
                pdfPCell.Border = 0;
                pdfPTable.AddCell(pdfPCell);
                PdfPCell pdfPCell2 = new PdfPCell(new Phrase(string.Format("RELACIÓN  DE VENTAS CANCELADAS\n\nDEL: {0}  AL  {1}\n\n{2}  {3}  {4}", dateTime, dateTime2, (num != 0) ? ("CAJA: " + num + " ") : "", (text != null) ? (" CAJERO: " + text) : "", (text2 != null) ? (" SUPERVISOR: " + text2) : ""), font));
                pdfPCell2.HorizontalAlignment = 1;
                pdfPCell2.Border = 0;
                pdfPTable.AddCell(pdfPCell2);
                document.Add(pdfPTable);
                document.Add(new Paragraph(Chunk.NEWLINE));
                PdfPTable pdfPTable2 = new PdfPTable(6);
                pdfPTable2.WidthPercentage = 100f;
                pdfPTable2.SetWidths(new float[6] { 8f, 22f, 22f, 22f, 10f, 16f });
                string[] array = new string[6] { "Caja", "Fecha/Hora", "Cajero", "Supervisor", "Status", "Códigos Barras" };
                for (int i = 0; i < array.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num2 = 1;
                foreach (VentaCanceladaExtended venta in list)
                {
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase(venta.id_pos.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell4.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell4);
                    PdfPCell pdfPCell5 = new PdfPCell(new Phrase(venta.fecha_cancel.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell5.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell5.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell5);
                    PdfPCell pdfPCell6 = new PdfPCell(new Phrase(venta.cajero.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell6.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell6.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell6);
                    PdfPCell pdfPCell7 = new PdfPCell(new Phrase(venta.supervisor.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell7.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell7.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell7);
                    PdfPCell pdfPCell8 = new PdfPCell(new Phrase(venta.status, FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell8.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell8.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell8);
                    string text3 = string.Empty;
                    foreach (venta_cancelada_articulo item in dcContextoSuPlazaDataContext.venta_cancelada_articulo.Where((venta_cancelada_articulo vca) => vca.id_venta_cancel.Equals(venta.id_venta)).ToList())
                    {
                        text3 += $"{item.cod_barras}\n";
                    }
                    PdfPCell pdfPCell9 = new PdfPCell(new Phrase(text3, FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell9.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell9.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell9);
                    num2++;
                }
                document.Add(pdfPTable2);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", "attachment; filename=VentasCanceladas.pdf");
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