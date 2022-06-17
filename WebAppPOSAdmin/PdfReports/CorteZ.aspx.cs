using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.PdfReports
{
    public partial class CorteZ : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion
        private enum Align
        {
            toLeft,
            toRight,
            toMiddle
        }

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
                if (Session["corte_x"] == null)
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                sp_corte_cajaResult c = (sp_corte_cajaResult)Session["corte_x"];
                int num = c.no_transacciones ?? 0;
                decimal num2 = c.efectivo ?? 0.0m;
                decimal num3 = c.vales ?? 0.0m;
                decimal num4 = c.cheques ?? 0.0m;
                decimal num5 = c.tc ?? 0.0m;
                decimal num6 = c.total_vendido ?? 0.0m;
                decimal num7 = c.total_devuelto ?? 0.0m;
                decimal num8 = c.total_desglosado ?? 0.0m;
                decimal num9 = c.iva ?? 0.0m;
                decimal num10 = c.total_exentos ?? 0.0m;
                Document document = new Document(PageSize.LETTER, 36f, 36f, 36f, 36f);
                PdfWriter.GetInstance(document, base.Response.OutputStream);
                document.Open();
                Font font = new Font(Font.FontFamily.COURIER, 9f);
                new Paragraph();
                empresa empresa = dcContextoSuPlazaDataContext.empresa.FirstOrDefault();
                document.Add(new Paragraph(setLineAlignLn(empresa.razon_social, Align.toMiddle), font));
                document.Add(new Paragraph(setLineAlignLn(empresa.calle, Align.toMiddle), font));
                document.Add(new Paragraph(setLineAlignLn("COLONIA " + empresa.colonia + "," + empresa.municipio + "," + empresa.estado + " CP: " + empresa.codigo_postal, Align.toMiddle), font));
                document.Add(new Paragraph(setLineAlignLn(empresa.rfc, Align.toMiddle), font));
                document.Add(new Paragraph(Chunk.NEWLINE));
                document.Add(new Paragraph("CORTE Z", font));
                document.Add(new Paragraph(Chunk.NEWLINE));
                document.Add(new Paragraph(setLineAlignLn("Fecha Inicial  : " + c.fecha_ini, Align.toLeft), font));
                document.Add(new Paragraph(setLineAlignLn("Fecha Final    : " + c.fecha_fin, Align.toLeft), font));
                document.Add(new Paragraph(setLineAlignLn("Folio Inicial  : " + c.folio_ini, Align.toLeft), font));
                document.Add(new Paragraph(setLineAlignLn("Folio Final    : " + c.folio_fin, Align.toLeft), font));
                document.Add(new Paragraph(setLineAlignLn("Caja           : " + c.id_pos, Align.toLeft), font));
                document.Add(new Paragraph(setLineAlignLn("Cajero         : " + dcContextoSuPlazaDataContext.empleado.FirstOrDefault((empleado em) => em.user_name.Equals(c.vendedor)).shortName(), Align.toLeft), font));
                document.Add(new Paragraph(setLineAlignLn("Fecha impresion: " + DateTime.Now, Align.toLeft), font));
                document.Add(new Paragraph(Chunk.NEWLINE));
                document.Add(new Paragraph(setDotLine(), font));
                document.Add(new Paragraph("Efectivo: $" + new string(' ', 33 - num2.ToString("F2").Length) + num2.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph("Vales   : $" + new string(' ', 33 - num3.ToString("F2").Length) + num3.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph("Cheques : $" + new string(' ', 33 - num4.ToString("F2").Length) + num4.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph("T.C.    : $" + new string(' ', 33 - num5.ToString("F2").Length) + num5.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph("# Transacciones:" + new string(' ', 28 - num.ToString().Length) + num + "\r\n", font));
                document.Add(new Paragraph(setDotLine(), font));
                document.Add(new Paragraph("Total de Venta :" + new string(' ', 28 - num6.ToString("F2").Length) + num6.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph(Chunk.NEWLINE));
                document.Add(new Paragraph("Venta IVA 16%  :" + new string(' ', 28 - num8.ToString("F2").Length) + num8.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph("Impuesto  16%  :" + new string(' ', 28 - num9.ToString("F2").Length) + num9.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph("Venta  exenta  :" + new string(' ', 28 - num10.ToString("F2").Length) + num10.ToString("F2") + "\r\n", font));
                document.Add(new Paragraph(setDotLine(), font));
                document.Add(new Paragraph(Chunk.NEWLINE));
                document.Add(new Paragraph(setLineAlignLn("TOTAL: " + (num6 - num7).ToString("C2"), Align.toMiddle), font));
                List<venta_devolucion> list = (from d in dcContextoSuPlazaDataContext.venta_devolucion
                                               where d.fecha_dev >= c.fecha_ini && d.fecha_dev <= c.fecha_fin && d.id_pos.Equals(c.id_pos) && d.vendedor.Equals(c.vendedor)
                                               orderby d.folio
                                               select d).ToList();
                if (list != null)
                {
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(new Paragraph(setDotLine(), font));
                    document.Add(new Paragraph(setLineAlignLn("***DEVOLUCIONES***", Align.toMiddle), font));
                    foreach (venta_devolucion item in list)
                    {
                        document.Add(new Paragraph(string.Concat(new string(' ', 5 - item.folio.ToString().Length), item.folio.ToString(), " | ", item.fecha_dev, " | ", new string(' ', 7 - item.cant_dev.ToString("F2").Length), "-", item.cant_dev.ToString("F2")), font));
                    }
                }
                document.Close();
                base.Response.ContentType = "application/pdf";
                base.Response.AppendHeader("content-disposition", "attachment; filename=CorteZ.pdf");
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.Write(document);
                base.Response.Flush();
                base.Response.Close();
                Session.Remove("corte_x");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CorteZ " + "Acción: createPDF " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        private string setLineAlignLn(string str, Align align)
        {
            str = str.Trim();
            return align switch
            {
                Align.toLeft => str + "\r\n",
                Align.toRight => new string(' ', 44 - str.Length) + str + "\r\n",
                Align.toMiddle => new string(' ', (44 - str.Length) / 2) + str + "\r\n",
                _ => "",
            };
        }

        private string setDotLine()
        {
            return new string('-', 44) + "\r\n";
        }
    }
}