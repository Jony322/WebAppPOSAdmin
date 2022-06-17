using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;
using NLog;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class EstadisticaOferta : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                createPDF();
            }
        }

        public void createPDF()
        {
            try
            {
                if (Session["oferta"] == null || Session["OfferItems"] == null)
                {
                    return;
                }
                using (new dcContextoSuPlazaDataContext())
                {
                    oferta oferta = (oferta)Session["oferta"];
                    List<OfferItems> list = (List<OfferItems>)Session["OfferItems"];
                    string str = string.Format("REPORTE DE ESTADÍSTICA DE OFERTAS\n\n{0}\n\nDEL: {1}  AL: {2}", oferta.descripcion.ToUpper(), oferta.fecha_ini.ToString("dd/MM/yyyy"), oferta.fecha_fin.ToString("dd/MM/yyyy"));
                    iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, 1);
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0);
                    Document document = new Document(PageSize.LETTER, 36f, 36f, 36f, 36f);
                    PdfWriter.GetInstance(document, base.Response.OutputStream).PageEvent = new PDFFooter();
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
                    PdfPCell pdfPCell2 = new PdfPCell(new Phrase(str, font));
                    pdfPCell2.HorizontalAlignment = 1;
                    pdfPCell2.Border = 0;
                    pdfPTable.AddCell(pdfPCell2);
                    document.Add(pdfPTable);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    PdfPTable pdfPTable2 = new PdfPTable(5);
                    pdfPTable2.WidthPercentage = 100f;
                    pdfPTable2.SetWidths(new float[5] { 24f, 40f, 12f, 12f, 12f });
                    string[] array = new string[5] { "Código barras", "Descripción", "UM", "UMC", "Total" };
                    for (int i = 0; i < array.Length; i++)
                    {
                        PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array[i], FontFactory.GetFont("Arial", 8f, 1)));
                        pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                        pdfPCell3.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell3);
                    }
                    int num = 1;
                    foreach (OfferItems item in list)
                    {
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_barras.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.descripcion.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 0
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.unidad.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cantidad_um.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.total.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell4.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell4);
                        num++;
                    }
                    document.Add(pdfPTable2);
                    document.Close();
                    base.Response.ContentType = "application/pdf";
                    base.Response.AppendHeader("content-disposition", "attachment; filename=EstadisticaOferta.pdf");
                    base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    base.Response.Write(document);
                    base.Response.Flush();
                    base.Response.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: EstadisticaOferta " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}