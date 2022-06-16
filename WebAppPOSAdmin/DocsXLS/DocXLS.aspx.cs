using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppPOSAdmin.DocsXLS
{
    public partial class DocXLS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                createXLS();
            }
        }

        public void createXLS()
        {
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        base.Response.ContentType = "application/vnd.ms-excel";
                        base.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}{1}.xls", "CambioPrecios", DateTime.Now.ToString("yyyyMMddHHmm")));
                        List<string> obj = new List<string> { "[Codigo Barras]", "[Descripción]", "[Unidad]", "[Precio Compra]", "[Utilidad]", "[Precio Venta]", "[IVA]" };
                        Table table = new Table();
                        TableRow row = new TableRow
                        {
                            Cells =
                        {
                            new TableCell
                            {
                                Text = string.Format("<img src='http://{0}/Images/plaza.png' border='0' width='165' height='70' />", base.Request["HTTP_HOST"])
                            }
                        }
                        };
                        table.Rows.Add(row);
                        table.Rows.Add(new TableRow());
                        table.Rows.Add(new TableRow());
                        table.Rows.Add(new TableRow());
                        table.Rows.Add(new TableRow());
                        TableRow tableRow = new TableRow();
                        foreach (string item in obj)
                        {
                            tableRow.Cells.Add(new TableCell
                            {
                                Text = item
                            });
                        }
                        table.Rows.Add(tableRow);
                        table.RenderControl(writer);
                    }
                    HttpContext.Current.Response.Write(stringWriter.ToString());
                }
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {
            }
        }
    }
}