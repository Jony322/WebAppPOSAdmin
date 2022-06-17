using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Scripts;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class ConcentradoCompras : System.Web.UI.Page
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
                if (Session["id_pedido"] == null)
                {
                    return;
                }
                Guid id_pedido = Guid.Parse(Session["id_pedido"].ToString());
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                List<ConcentradoOrderExtended> list = (from e in new Procedures().getConcentradoCompras(id_pedido)
                                                       orderby e.descripcion
                                                       select e).ToList();
                string empty = string.Empty;
                pedido pedido = dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido e) => e.id_pedido.Equals(id_pedido));
                empty = "CAPTURA DEL PEDIDO\n\n";
                empty += string.Format("PEDIDO: {0}   FECHA PEDIDO: {1}\n\nPROVEEDOR: {2}", pedido.num_pedido, pedido.fecha_pedido.ToString("dd/MM/yyyy HH:mm:ss"), pedido.proveedor.razon_social);
                iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, 1);
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0);
                Document document = new Document(PageSize.LETTER.Rotate(), 36f, 36f, 36f, 36f);
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
                PdfPCell pdfPCell2 = new PdfPCell(new Phrase(empty, font));
                pdfPCell2.HorizontalAlignment = 1;
                pdfPCell2.Border = 0;
                pdfPTable.AddCell(pdfPCell2);
                document.Add(pdfPTable);
                document.Add(new Paragraph(Chunk.NEWLINE));
                PdfPTable pdfPTable2 = new PdfPTable(15);
                pdfPTable2.WidthPercentage = 100f;
                string[] array = new string[15]
                {
                "Código barras", "Descripción", "UM", "UMC", "Costo", "Pedido", "Cap-1", "Cap-2", "Cap-3", "Cap-4",
                "Cap-5", "Cap-6", "Cap-7", "Cap-8", "Total"
                };
                pdfPTable2.SetWidths(new float[15]
                {
                10f, 25f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
                5f, 5f, 5f, 5f, 5f
                });
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num = 1;
                foreach (ConcentradoOrderExtended item in list)
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
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.umc.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.costo.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 2
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cant_pedido.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[0].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[1].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[2].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[3].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[4].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[5].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[6].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.entradas[7].ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
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
                base.Response.AppendHeader("content-disposition", "attachment; filename=Relacion.pdf");
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.Write(document);
                base.Response.Flush();
                base.Response.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: ConcentradoCompras " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                base.Response.Write(ex.Message);
            }
        }
    }
}