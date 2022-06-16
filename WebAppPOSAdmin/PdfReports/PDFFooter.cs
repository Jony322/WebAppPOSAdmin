using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WebAppPOSAdmin.PdfReports
{
    public class PDFFooter : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            Font font = new Font(Font.FontFamily.HELVETICA, 8f, 2);
            PdfPTable pdfPTable = new PdfPTable(new float[1] { 1f });
            pdfPTable.TotalWidth = 378f;
            PdfPCell pdfPCell = new PdfPCell(new Phrase(string.Format("Fecha de impresión: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")), font));
            pdfPCell.Border = 1;
            pdfPTable.AddCell(pdfPCell);
            new Font(Font.FontFamily.HELVETICA, 8f, 2);
            PdfPTable pdfPTable2 = new PdfPTable(new float[1] { 1f });
            pdfPTable2.TotalWidth = 378f;
            pdfPTable2.AddCell(new PdfPCell(new Phrase($"Página: {writer.PageNumber}", font))
            {
                Border = 1,
                HorizontalAlignment = 2
            });
            pdfPTable.WriteSelectedRows(0, -1, 36f, document.Bottom, writer.DirectContent);
            pdfPTable2.WriteSelectedRows(0, -1, 378f, document.Bottom, writer.DirectContent);
        }
    }
}