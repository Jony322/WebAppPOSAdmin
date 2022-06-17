using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class Devoluciones : System.Web.UI.Page
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
                if (Session["devoluciones"] == null)
                {
                    return;
                }
                using (new dcContextoSuPlazaDataContext())
                {
                    DateTime dateTime = (DateTime)Session["dateIni"];
                    DateTime dateTime2 = (DateTime)Session["dateFin"];
                    string text = ((Session["barCode"] != null) ? Session["barCode"].ToString() : null);
                    short num = (short)((Session["pos"] != null) ? short.Parse(Session["pos"].ToString()) : 0);
                    List<VentaDevolucionExtended> list = (List<VentaDevolucionExtended>)Session["devoluciones"];
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
                    PdfPCell pdfPCell2 = new PdfPCell(new Phrase(string.Format("RELACIÓN DE DEVOLUCIONES\n\nDEL: {0}  AL  {1}\n\n{2}  {3}", dateTime, dateTime2, (num != 0) ? ("CAJA: " + num + " ") : "", (text != null) ? (" CÓDIGO BARRAS: " + text) : ""), font));
                    pdfPCell2.HorizontalAlignment = 1;
                    pdfPCell2.Border = 0;
                    pdfPTable.AddCell(pdfPCell2);
                    document.Add(pdfPTable);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    PdfPTable pdfPTable2 = new PdfPTable(6);
                    pdfPTable2.WidthPercentage = 100f;
                    pdfPTable2.SetWidths(new float[6] { 5f, 10f, 15f, 30f, 30f, 10f });
                    string[] array = new string[6] { "Caja", "Folio", "Fecha/Hora", "Cajero", "Supervisor", "Total" };
                    for (int i = 0; i < array.Length; i++)
                    {
                        PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array[i], FontFactory.GetFont("Arial", 8f, 1)));
                        pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                        pdfPCell3.HorizontalAlignment = 1;
                        pdfPTable2.AddCell(pdfPCell3);
                    }
                    int num2 = 1;
                    foreach (VentaDevolucionExtended item in list)
                    {
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.id_pos.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.folio.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.fecha.ToString("dd/MM/yyyy HH:mm:ss"), FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cajero, FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        pdfPTable2.AddCell(new PdfPCell(new Phrase(item.supervisor, FontFactory.GetFont("Arial", 8f, 0)))
                        {
                            BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                            HorizontalAlignment = 1
                        });
                        PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.total_devuelto.ToString("C2"), FontFactory.GetFont("Arial", 8f, 0)));
                        pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num2 % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                        pdfPCell4.HorizontalAlignment = 2;
                        pdfPTable2.AddCell(pdfPCell4);
                        num2++;
                    }
                    document.Add(pdfPTable2);
                    document.Close();
                    base.Response.ContentType = "application/pdf";
                    base.Response.AppendHeader("content-disposition", "attachment; filename=Devolucion.pdf");
                    base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    base.Response.Write(document);
                    base.Response.Flush();
                    base.Response.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Devoluciones " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}