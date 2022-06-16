using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class Compras : System.Web.UI.Page
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
                if (Session["id_compra"] == null)
                {
                    return;
                }
                Guid id_compra = Guid.Parse(Session["id_compra"].ToString());
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                List<CompraArticuloExtended> list = (List<CompraArticuloExtended>)Session["purchases"];
                string empty = string.Empty;
                if (Session["id_pedido"] != null && id_compra.Equals(default(Guid)))
                {
                    pedido pedido = dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido e) => e.id_pedido.Equals(Guid.Parse(Session["id_pedido"].ToString())));
                    empty = "REPORTE DE COMPRAS POR PEDIDO\n\n";
                    empty += string.Format("PEDIDO: {0}   FECHA PEDIDO: {1}", pedido.num_pedido, pedido.fecha_pedido.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                else if (Session["id_pedido"] != null && !id_compra.Equals(default(Guid)))
                {
                    compra compra = dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => e.id_compra.Equals(id_compra));
                    empty = "REPORTE DE COMPRAS POR PEDIDO\n";
                    empty += $"PROVEEDOR: {compra.proveedor.razon_social.ToUpper()} ";
                    empty += string.Format("PEDIDO: {0}   FACTURA: {1}   FECHA COMPRA: {2}", compra.pedido.num_pedido, compra.no_factura, compra.fecha_compra.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                else
                {
                    if (id_compra.Equals(default(Guid)))
                    {
                        return;
                    }
                    compra compra2 = dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => e.id_compra.Equals(Guid.Parse(Session["id_compra"].ToString())));
                    empty = "REPORTE DE COMPRAS ABIERTAS\n\n";
                    empty += string.Format("OBSERVACION: {0}\nFACTURA: {1}   FECHA COMPRA: {2}", compra2.observaciones, compra2.no_factura, compra2.fecha_compra.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, 1);
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0);
                Document document = new Document(PageSize.LETTER.Rotate(), 36f, 36f, 36f, 36f);
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
                PdfPCell pdfPCell2 = new PdfPCell(new Phrase(empty, font));
                pdfPCell2.HorizontalAlignment = 1;
                pdfPCell2.Border = 0;
                pdfPTable.AddCell(pdfPCell2);
                document.Add(pdfPTable);
                document.Add(new Paragraph(Chunk.NEWLINE));
                PdfPTable pdfPTable2 = new PdfPTable(9);
                pdfPTable2.WidthPercentage = 100f;
                string[] array = new string[9] { "Código barras", "Código Int", "Descripción", "UM", "UMC", "Cant cja", "Cant pza", "Costo", "Total" };
                pdfPTable2.SetWidths(new float[9] { 16f, 6f, 36f, 7f, 7f, 7f, 7f, 7f, 7f });
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num = 1;
                foreach (CompraArticuloExtended item in list)
                {
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_barras.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_interno.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
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
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.costo.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 2
                    });
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.costo_total.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell4.HorizontalAlignment = 2;
                    pdfPTable2.AddCell(pdfPCell4);
                    num++;
                }
                document.Add(pdfPTable2);
                decimal num2 = list.Sum((CompraArticuloExtended a) => a.cant_pza * a.costo);
                Paragraph paragraph = new Paragraph(num2.ToString("C2"), FontFactory.GetFont("Arial", 12f, 1));
                paragraph.Alignment = 2;
                document.Add(paragraph);
                PdfPTable pdfPTable3 = new PdfPTable(3);
                pdfPTable3.HorizontalAlignment = 0;
                pdfPTable3.WidthPercentage = 50f;
                pdfPTable3.SetWidths(new float[3] { 33f, 33f, 34f });
                PdfPCell pdfPCell5 = new PdfPCell(new Phrase("CONCENTRADO DE PRODUCTOS", FontFactory.GetFont("Arial", 12f, 1)));
                pdfPCell5.HorizontalAlignment = 1;
                pdfPCell5.Colspan = 3;
                pdfPTable3.AddCell(pdfPCell5);
                foreach (var item2 in from a in list
                                      group a by a.unidad into ga
                                      select new
                                      {
                                          unidad = ga.Key,
                                          cantidad = ga.Sum((CompraArticuloExtended q) => q.cant_pza / q.umc),
                                          costos = ga.Sum((CompraArticuloExtended c) => c.cant_pza * c.costo)
                                      })
                {
                    pdfPTable3.AddCell(new PdfPCell(new Phrase(item2.unidad, FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        HorizontalAlignment = 1
                    });
                    pdfPTable3.AddCell(new PdfPCell(new Phrase(item2.cantidad.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        HorizontalAlignment = 1
                    });
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
                base.Response.AppendHeader("content-disposition", "attachment; filename=Relacion.pdf");
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.Write(document);
                base.Response.Flush();
                base.Response.Close();
            }
            catch (Exception ex)
            {
                base.Response.Write(ex.Message);
            }
        }
    }
}