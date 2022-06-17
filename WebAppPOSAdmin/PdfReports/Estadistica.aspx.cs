using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;
using NLog;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class Estadistica : System.Web.UI.Page
    {

        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

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
                if (Session["reporte"] == null || Session["statistics"] == null)
                {
                    return;
                }
                using (new dcContextoSuPlazaDataContext())
                {
                    string text = Session["reporte"].ToString();
                    List<Statistics> list = (List<Statistics>)Session["statistics"];
                    string text2 = "REPORTE DE ESTADÍSTICA ";
                    short num = 5;
                    switch (text)
                    {
                        case "acumulada":
                            text2 += string.Format("ACUMULADA\n\nDEL: {0}  AL: {1}", Session["dateIni"].ToString(), Session["dateEnd"].ToString());
                            break;
                        case "mes":
                            text2 += string.Format("POR MES\n\nMES: {0}  AÑO: {1}", Session["month"].ToString(), Session["year"].ToString());
                            break;
                        case "dia":
                            text2 += string.Format("POR DÍA\n\nDEL: {0}  AL: {1}", Session["dateIni"].ToString(), Session["dateEnd"].ToString());
                            num = 6;
                            break;
                    }
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
                    PdfPCell pdfPCell2 = new PdfPCell(new Phrase(text2, font));
                    pdfPCell2.HorizontalAlignment = 1;
                    pdfPCell2.Border = 0;
                    pdfPTable.AddCell(pdfPCell2);
                    document.Add(pdfPTable);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    PdfPTable pdfPTable2 = new PdfPTable(num);
                    pdfPTable2.WidthPercentage = 100f;
                    string[] array;
                    if (num == 5)
                    {
                        array = new string[5] { "Código barras", "Descripción", "Unidad", "Cantidad", "Total" };
                        pdfPTable2.SetWidths(new float[5] { 20f, 50f, 10f, 10f, 10f });
                    }
                    else
                    {
                        array = new string[6] { "Código barras", "Descripción", "Unidad", "Fecha", "Cantidad", "Total" };
                        pdfPTable2.SetWidths(new float[6] { 20f, 40f, 10f, 10f, 10f, 10f });
                    }
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
                        pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                        pdfPCell3.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell3);
                    }
                    int num2 = 1;
                    decimal num3 = 0.0m;
                    foreach (Statistics item in list)
                    {
                        PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.cod_barras.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell4.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell4);
                        PdfPCell pdfPCell5 = new PdfPCell(new Phrase(item.descripcion.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell5.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell5.HorizontalAlignment = 0;
                        pdfPTable2.AddCell(pdfPCell5);
                        PdfPCell pdfPCell6 = new PdfPCell(new Phrase(item.medida.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell6.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell6.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell6);
                        if (num == 6)
                        {
                            PdfPCell pdfPCell7 = new PdfPCell(new Phrase(item.fecha.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 8f, 0)));
                            pdfPCell7.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                            pdfPCell7.HorizontalAlignment = 1;
                            pdfPTable2.AddCell(pdfPCell7);
                        }
                        PdfPCell pdfPCell8 = new PdfPCell(new Phrase(item.cantidad.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell8.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell8.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell8);
                        PdfPCell pdfPCell9 = new PdfPCell(new Phrase(item.total.ToString("C2"), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell9.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell9.HorizontalAlignment = 2;
                        pdfPTable2.AddCell(pdfPCell9);
                        num2++;
                        num3 += item.total;
                    }
                    document.Add(pdfPTable2);
                    Paragraph paragraph = new Paragraph(num3.ToString("C2"), FontFactory.GetFont("Arial", 12f, 1));
                    paragraph.Alignment = 2;
                    document.Add(paragraph);
                    document.Close();
                    base.Response.ContentType = "application/pdf";
                    base.Response.AppendHeader("content-disposition", "attachment; filename=Relacion.pdf");
                    base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    base.Response.Write(document);
                    base.Response.Flush();
                    base.Response.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Estadistica " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}