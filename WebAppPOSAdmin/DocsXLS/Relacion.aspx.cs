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
    public partial class Relacion : System.Web.UI.Page
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
                if (Session["ventas"] != null)
                {
                    DateTime dateTime = (DateTime)Session["dateIni"];
                    DateTime dateTime2 = (DateTime)Session["dateFin"];
                    string text = ((Session["barCode"] != null) ? Session["barCode"].ToString() : null);
                    short num = (short)((Session["pos"] != null) ? short.Parse(Session["pos"].ToString()) : 0);
                    List<VentaRelacionExtended> source = (List<VentaRelacionExtended>)Session["ventas"];
                    using StringWriter stringWriter = new StringWriter();
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        base.Response.ContentType = "application/vnd.ms-excel";
                        base.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}{1}.xls", "RelacionVentas", DateTime.Now.ToString("yyyyMMddHHmm")));
                        Table tableXLS = new Table();
                        TableRow tableRow = new TableRow();
                        tableRow.Cells.Add(new TableCell
                        {
                            Text = string.Format("<img src='http://{0}/Images/plaza.png' border='0' width='165' height='70' />", base.Request["HTTP_HOST"])
                        });
                        tableRow.Cells.Add(new TableCell());
                        tableRow.Cells.Add(new TableCell());
                        tableRow.Cells.Add(new TableCell
                        {
                            Text = string.Format("<h2>RELACION  DE VENTAS<br />DEL: {0}  AL  {1}<br />{2}  {3}</h2>", dateTime, dateTime2, (num != 0) ? ("CAJA: " + num + " ") : "", (text != null) ? (" CÓDIGO BARRAS: " + text) : "")
                        });
                        tableXLS.Rows.Add(tableRow);
                        tableXLS.Rows.Add(new TableRow());
                        tableXLS.Rows.Add(new TableRow());
                        TableRow fila = new TableRow();
                        foreach (string item in new List<string> { "[Caja]", "[Folio venta]", "[Fecha/Hora]", "[Cajero]", "[Total]" })
                        {
                            fila.Cells.Add(new TableCell
                            {
                                Text = item
                            });
                        }
                        tableXLS.Rows.Add(fila);
                        source.ToList().ForEach(delegate (VentaRelacionExtended row)
                        {
                            fila = new TableRow();
                            fila.Cells.Add(new TableCell
                            {
                                Text = row.id_pos.ToString()
                            });
                            fila.Cells.Add(new TableCell
                            {
                                Text = row.folio.ToString()
                            });
                            fila.Cells.Add(new TableCell
                            {
                                Text = row.fecha_venta.ToString("dd/MM/yyyy HH:mm:ss")
                            });
                            fila.Cells.Add(new TableCell
                            {
                                Text = row.cajero
                            });
                            fila.Cells.Add(new TableCell
                            {
                                Text = row.total_venta.ToString("F2")
                            });
                            tableXLS.Rows.Add(fila);
                        });
                        tableXLS.RenderControl(writer);
                    }
                    HttpContext.Current.Response.Write(stringWriter.ToString());
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