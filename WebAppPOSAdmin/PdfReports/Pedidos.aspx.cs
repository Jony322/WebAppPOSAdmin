using System;
using System.Drawing;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class Pedidos : System.Web.UI.Page
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
                if (Session["Order"] == null)
                {
                    return;
                }
                OrderExtended order = (OrderExtended)Session["Order"];
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                pedido pedido = null;
                if (!order.id_pedido.Equals(default(Guid)))
                {
                    pedido = dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido o) => o.id_pedido.Equals(order.id_pedido));
                }
                else
                {
                    pedido = new pedido();
                    pedido.id_pedido = order.id_pedido;
                    pedido.fecha_pedido = DateTime.Now;
                    pedido.num_pedido = 0L;
                }
                string str = string.Format("{0}\n\nPEDIDO: {1}   FECHA PEDIDO: {2}", order.proveedor.ToUpper(), pedido.num_pedido, pedido.fecha_pedido.ToString("dd/MM/yyyy HH:mm:ss"));
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
                PdfPCell pdfPCell2 = new PdfPCell(new Phrase(str, font));
                pdfPCell2.HorizontalAlignment = 1;
                pdfPCell2.Border = 0;
                pdfPTable.AddCell(pdfPCell2);
                document.Add(pdfPTable);
                document.Add(new Paragraph(Chunk.NEWLINE));
                PdfPTable pdfPTable2 = new PdfPTable(9);
                pdfPTable2.WidthPercentage = 100f;
                string[] array = new string[9] { "Código barras", "Descripción", "UM", "UMC", "Costo", "Exis. Cja", "Exis. Pza", "Sugerido", "A pedir" };
                pdfPTable2.SetWidths(new float[9] { 14f, 38f, 8f, 8f, 8f, 8f, 8f, 8f, 8f });
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num = 1;
                foreach (PedidoArticulosExtended item in order.items)
                {
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_barras.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.descripcion.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 0
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.unidad.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.umc.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.costo.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 2
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.existencia_caja.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.existencias_pieza.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.sugerido.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.a_pedir.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#C2D69B" : "#FFFFFF"));
                    pdfPCell4.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell4);
                    num++;
                }
                document.Add(pdfPTable2);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", $"attachment; filename=Pedido_{pedido.num_pedido}.pdf");
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.Write(document);
                base.Response.Flush();
                base.Response.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Pedidos " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                base.Response.Write(ex.Message);
            }
        }
    }
}