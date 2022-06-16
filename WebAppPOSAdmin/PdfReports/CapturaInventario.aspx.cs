using System;
using System.Drawing;
using System.Linq;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class CapturaInventario : System.Web.UI.Page
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
                Guid InventoryID = Guid.Parse(Session["InventoryID"].ToString());
                if (InventoryID.Equals(default(Guid)))
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                inventario_fisico inventario_fisico = dcContextoSuPlazaDataContext.inventario_fisico.FirstOrDefault((inventario_fisico e) => e.id_inventario_fisico.Equals(InventoryID));
                var list = (from e in dcContextoSuPlazaDataContext.sp_reporte_inventario_fisico(InventoryID)
                            select new
                            {
                                cod_barras = e.cod_barras,
                                descripcion_larga = e.articulo,
                                descripcion = e.unidad,
                                stock_estimado = e.stock_estimado,
                                stock_fisico = e.stock_fisico
                            }).ToList();
                string str = string.Format("INVENTARIO FÍSICO\n\n{0}\n\nDEL: {1}  AL: {2}", inventario_fisico.proveedor.razon_social, inventario_fisico.fecha_ini.ToString("dd/MM/yyyy HH:mm:ss"), DateTime.Now.ToString("dd/MM/yyyy HH/mm/ss"));
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
                PdfPCell pdfPCell2 = new PdfPCell(new Phrase(str, font));
                pdfPCell2.HorizontalAlignment = 1;
                pdfPCell2.Border = 0;
                pdfPTable.AddCell(pdfPCell2);
                document.Add(pdfPTable);
                document.Add(new Paragraph(Chunk.NEWLINE));
                PdfPTable pdfPTable2 = new PdfPTable(5);
                pdfPTable2.WidthPercentage = 100f;
                string[] array = new string[5] { "Código barras", "Descripción", "Unidad", "Existencia", "Física" };
                pdfPTable2.SetWidths(new float[5] { 20f, 50f, 10f, 10f, 10f });
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num = 1;
                foreach (var item in list)
                {
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cod_barras.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.descripcion_larga.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 0
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.descripcion.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    pdfPTable2.AddCell(new PdfPCell(new Phrase(item.stock_estimado.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
                    {
                        BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
                        HorizontalAlignment = 1
                    });
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase((item.stock_fisico ?? 0.0m).ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell4.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell4);
                    num++;
                }
                document.Add(pdfPTable2);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}{1}.pdf", inventario_fisico.proveedor.rfc, DateTime.Now.ToString("yyyyMMddHHmmss")));
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