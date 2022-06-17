using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

using WebAppPOSAdmin.Repository.Entidad;

using WebAppPOSAdmin.Common;
using NLog;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class InventarioActual : System.Web.UI.Page
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
                Guid ProviderID = Guid.Parse(Session["ProviderID"].ToString());
                if (ProviderID.Equals(default(Guid)))
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                proveedor proveedor = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor e) => e.id_proveedor.Equals(ProviderID));
                List<inventario_actual> list = (from item in dcContextoSuPlazaDataContext.sp_reporte_inventario_actual(ProviderID)
                                                select new inventario_actual
                                                {
                                                    cod_barras = item.cod_barras,
                                                    descripcion_larga = item.articulo,
                                                    unidad_medida = item.unidad,
                                                    stock_cja = Convert.ToDecimal(item.stock_cja),
                                                    stock_pza = Convert.ToDecimal(item.stock_pza),
                                                    total = Convert.ToDecimal((!item.total.HasValue) ? new decimal?(default(decimal)) : item.total),
                                                    precio_compra = Convert.ToDecimal(item.precio_compra),
                                                    cantidad_um = Convert.ToDecimal(item.cantidad_um),
                                                    stock = item.stock
                                                } into l
                                                orderby l.descripcion_larga
                                                select l).ToList();
                string str = string.Format("INVENTARIO ACTUAL\n\nPROVEEDOR: {0}\n\nFECHA EMISIÓN: {1}", proveedor.razon_social, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
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
                PdfPTable pdfPTable2 = new PdfPTable(8);
                pdfPTable2.WidthPercentage = 100f;
                string[] array = new string[8] { "Código barras", "Descripción", "UM", "Exis. Cja", "Exis. Pza", "Precio", "Total", "Cant. UM" };
                pdfPTable2.SetWidths(new float[8] { 16f, 30f, 9f, 9f, 9f, 9f, 9f, 9f });
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
                    pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
                    pdfPCell3.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell3);
                }
                int num = 1;
                foreach (inventario_actual item in list)
                {
                    PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.cod_barras.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell4.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell4.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell4);
                    PdfPCell pdfPCell5 = new PdfPCell(new Phrase(item.descripcion_larga.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell5.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell5.HorizontalAlignment = 0;
                    pdfPTable2.AddCell(pdfPCell5);
                    PdfPCell pdfPCell6 = new PdfPCell(new Phrase(item.unidad_medida.ToString(), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell6.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell6.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell6);
                    PdfPCell pdfPCell7 = new PdfPCell(new Phrase(item.stock_cja.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell7.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell7.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell7);
                    PdfPCell pdfPCell8 = new PdfPCell(new Phrase(item.unidad_medida.Equals("Kg") ? item.stock.ToString("F3") : item.stock_pza.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell8.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell8.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell8);
                    PdfPCell pdfPCell9 = new PdfPCell(new Phrase(item.precio_compra.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell9.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell9.HorizontalAlignment = 2;
                    pdfPTable2.AddCell(pdfPCell9);
                    PdfPCell pdfPCell10 = new PdfPCell(new Phrase(item.total.ToString("F2"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell10.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell10.HorizontalAlignment = 2;
                    pdfPTable2.AddCell(pdfPCell10);
                    PdfPCell pdfPCell11 = new PdfPCell(new Phrase(item.unidad_medida.Equals("Cja") ? item.cantidad_um.ToString("G9") : 0.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
                    pdfPCell11.BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF"));
                    pdfPCell11.HorizontalAlignment = 1;
                    pdfPTable2.AddCell(pdfPCell11);
                    num++;
                }
                document.Add(pdfPTable2);
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", $"attachment; filename=InventarioActual_{proveedor.rfc}.pdf");
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.Write(document);
                base.Response.Flush();
                base.Response.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: InventarioActual " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}