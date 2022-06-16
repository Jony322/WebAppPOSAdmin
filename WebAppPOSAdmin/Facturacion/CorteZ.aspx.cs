using System;
using System.Threading;
using System.Linq;
using System.Net;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Facturacion
{
    public partial class CorteZ : System.Web.UI.Page
    {
        public enum Aline
        {
            toLeft,
            toRight,
            toMiddle
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["file"] != null && !base.IsPostBack)
            {
                showInvoice(base.Request.QueryString["file"].ToString());
            }
        }

        public void showInvoice(string fileName)
        {
            try
            {
                Thread.Sleep(10000);
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                byte[] array = new WebClient().DownloadData($"{dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().cfdi_path_pdf}{fileName}.pdf");
                if (array != null)
                {
                    base.Response.ContentType = "application/pdf";
                    base.Response.AddHeader("content-length", array.Length.ToString());
                    base.Response.AddHeader("Content-Disposition", $"filename={fileName}");
                    base.Response.BinaryWrite(array);
                }
            }
            catch
            {
                base.Response.Write("<h1>Factura a&uacute;n no generada.<br />En 10 segundos se recargar&aacute; autom&aacute;ticamente &eacute;sta p&aacute;gina. </h1>");
            }
        }
    }
}