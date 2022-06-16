using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.DocsXLS
{
    public partial class CambioPrecios : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

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
                if (Session["itemsAffected"] != null)
                {
                    List<ArticuloExtended> list = (List<ArticuloExtended>)Session["itemsAffected"];
                    if (list.Count > 0)
                    {
                        using StringWriter stringWriter = new StringWriter();
                        using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                        {
                            base.Response.ContentType = "application/vnd.ms-excel";
                            base.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}{1}.xls", "CambioPrecios", DateTime.Now.ToString("yyyyMMddHHmm")));
                            Table tableXLS = new Table();
                            TableRow tableRow = new TableRow();
                            tableRow.Cells.Add(new TableCell
                            {
                                Text = string.Format("<img src='http://{0}/Images/plaza.png' border='0' width='165' height='70' />", base.Request["HTTP_HOST"])
                            });
                            tableRow.Cells.Add(new TableCell());
                            tableRow.Cells.Add(new TableCell
                            {
                                Text = "<h1>Cambios de Precios</h1>"
                            });
                            tableXLS.Rows.Add(tableRow);
                            tableXLS.Rows.Add(new TableRow());
                            tableXLS.Rows.Add(new TableRow());
                            TableRow fila = new TableRow();
                            foreach (string item in new List<string> { "[Codigo Barras]", "[Descripcion]", "[Unidad]", "[Precio Compra]", "[Utilidad]", "[Precio Venta]", "[IVA]" })
                            {
                                fila.Cells.Add(new TableCell
                                {
                                    Text = item
                                });
                            }
                            tableXLS.Rows.Add(fila);
                            list.ToList().ForEach(delegate (ArticuloExtended row)
                            {
                                fila = new TableRow();
                                fila.Cells.Add(new TableCell
                                {
                                    Text = "'" + row.cod_barras
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.descripcion
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.unidad
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.precio_compra.ToString("F2")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.utilidad.ToString("F2")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.precio_venta.ToString("F2")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.iva.ToString("F2")
                                });
                                tableXLS.Rows.Add(fila);
                            });
                            tableXLS.RenderControl(writer);
                        }
                        HttpContext.Current.Response.Write(stringWriter.ToString());
                    }
                }
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {
                try
                {
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Thread.Sleep(1);
                }
                catch
                {

                }
            }
        }
    }
}