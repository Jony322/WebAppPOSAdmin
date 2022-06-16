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
    public partial class BackOrder : System.Web.UI.Page
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
				if (Session["id_pedido"] == null)
				{
					return;
				}
				Guid.Parse(Session["id_pedido"].ToString());
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				List<BackOrderExtended> list = (List<BackOrderExtended>)Session["backorder"];
				string empty = string.Empty;
				pedido pedido = dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido e) => e.id_pedido.Equals(Guid.Parse(Session["id_pedido"].ToString())));
				empty = "BACKORDER DEL PEDIDO\n\n";
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
				PdfPTable pdfPTable2 = new PdfPTable(7);
				pdfPTable2.WidthPercentage = 100f;
				string[] array = new string[7] { "Código barras", "Descripción", "UM", "UMC", "Se pidió (cja)", "Llegó (cja)", "Diferencia" };
				pdfPTable2.SetWidths(new float[7] { 18f, 38f, 8f, 8f, 8f, 8f, 8f });
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					PdfPCell pdfPCell3 = new PdfPCell(new Phrase(array2[i], FontFactory.GetFont("Arial", 8f, 1)));
					pdfPCell3.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F9F9F9"));
					pdfPCell3.HorizontalAlignment = 1;
					pdfPTable2.AddCell(pdfPCell3);
				}
				int num = 1;
				foreach (BackOrderExtended item in list)
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
					pdfPTable2.AddCell(new PdfPCell(new Phrase(item.umc.ToString(), FontFactory.GetFont("Arial", 8f, 0)))
					{
						BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
						HorizontalAlignment = 1
					});
					pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cant_pedido.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
					{
						BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
						HorizontalAlignment = 1
					});
					pdfPTable2.AddCell(new PdfPCell(new Phrase(item.cant_compra.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)))
					{
						BackgroundColor = new BaseColor(ColorTranslator.FromHtml((num % 2 == 0) ? "#F9F9F9" : "#FFFFFF")),
						HorizontalAlignment = 1
					});
					PdfPCell pdfPCell4 = new PdfPCell(new Phrase(item.diferencia.ToString("G9"), FontFactory.GetFont("Arial", 8f, 0)));
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
				base.Response.Write(ex.Message);
			}
		}
	}
}