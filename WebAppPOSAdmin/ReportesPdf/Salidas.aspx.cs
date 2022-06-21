using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Scripts;

namespace WebAppPOSAdmin.ReportesPdf
{
    public partial class Salidas : System.Web.UI.Page
    {
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
                if (Session["SalidaID"] == null)
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                salida salida = dcContextoSuPlazaDataContext.salida.FirstOrDefault((salida e) => e.id_salida.Equals(Guid.Parse(Session["SalidaID"].ToString())));
                List<ArticuloExtended> salidas = new Procedures().getSalidas(Guid.Parse(Session["SalidaID"].ToString()));
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
                PdfPCell pdfPCell2 = new PdfPCell(new Phrase($"SALIDA DE ALMACEN\nSALIDA #: {salida.num_salida}, \t FECHA: {salida.fecha_salida}\nRESPONSABLE: {salida.user_name}\n\nOBSERVACIONES: {salida.observacion}", font));
                pdfPCell2.HorizontalAlignment = 1;
                pdfPCell2.Border = 0;
                pdfPTable.AddCell(pdfPCell2);
                document.Add(pdfPTable);
                document.Add(new Paragraph(Chunk.NEWLINE));
                PdfPTable pdfPTable2 = new PdfPTable(9);
                pdfPTable2.WidthPercentage = 100f;
                pdfPTable2.SetWidths(new float[9] { 15f, 7f, 24f, 9f, 9f, 9f, 9f, 9f, 9f });
                string[] array = new string[9] { "Cod. Barras", "Cod. Interno", "Descripcion", "UM", "Piezas", "Can. Cja.", "Cant. Pza.", "Costo", "Total" };
                for (int i = 0; i < array.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num = 1;
                foreach (ArticuloExtended item in salidas)
                {
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_barras, FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_interno, FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.descripcion, FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 0
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.unidad, FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.umc.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cant_cja.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cant_pza.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.precio_compra.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 2
                    });
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.total.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell4.HorizontalAlignment = 2;
                    pdfPTable2.AddCell(pdfPCell4);
                    num++;
                }
                document.Add(pdfPTable2);
                decimal num2 = salidas.Sum((ArticuloExtended a) => a.total);
                Paragraph paragraph = new Paragraph(num2.ToString("C2"), FontFactory.GetFont("Arial", 12f, 1));
                paragraph.Alignment = 2;
                document.Add(paragraph);
                PdfPTable pdfPTable3 = new PdfPTable(3);
                pdfPTable3.HorizontalAlignment = 0;
                pdfPTable3.WidthPercentage = 50f;
                pdfPTable3.SetWidths(new float[3] { 33f, 33f, 34f });
                PdfPCell pdfPCell5 = new PdfPCell(new Phrase("CONCENTRADO DE PAQUETERÍA", FontFactory.GetFont("Arial", 12f, 1)));
                pdfPCell5.HorizontalAlignment = 1;
                pdfPCell5.Colspan = 3;
                pdfPTable3.AddCell(pdfPCell5);
                foreach (var item2 in from a in salidas
                                      group a by a.unidad into ga
                                      select new
                                      {
                                          unidad = ga.Key,
                                          cant_cja = ga.Sum((ArticuloExtended q) => q.cant_cja),
                                          cant_pza = ga.Sum((ArticuloExtended q) => q.cant_pza),
                                          costos = ga.Sum((ArticuloExtended c) => c.total)
                                      })
                {
                    pdfPCell5 = new PdfPCell(new Phrase(item2.unidad, FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell5.HorizontalAlignment = 1;
                    pdfPTable3.AddCell(pdfPCell5);
                    pdfPCell5 = new PdfPCell(new Phrase(item2.unidad.Equals("Cja") ? item2.cant_cja.ToString("G9") : item2.cant_pza.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell5.HorizontalAlignment = 1;
                    pdfPTable3.AddCell(pdfPCell5);
                    pdfPCell5 = new PdfPCell(new Phrase(item2.costos.ToString("C2"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell5.HorizontalAlignment = 2;
                    pdfPTable3.AddCell(pdfPCell5);
                }
                pdfPCell5 = new PdfPCell(new Phrase("TOTAL GENERAL:", FontFactory.GetFont("Arial", 10f, 1)));
                pdfPCell5.HorizontalAlignment = 0;
                pdfPCell5.Colspan = 2;
                pdfPTable3.AddCell(pdfPCell5);
                pdfPCell5 = new PdfPCell(new Phrase(num2.ToString("C2"), FontFactory.GetFont("Arial", 10f, 1)));
                pdfPCell5.HorizontalAlignment = 2;
                pdfPTable3.AddCell(pdfPCell5);
                document.Add(pdfPTable3);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", $"filename=Entrada_{salida.num_salida}.pdf");
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