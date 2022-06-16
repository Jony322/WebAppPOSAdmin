using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.DocsXLS
{
    public partial class VentaSuspendida : System.Web.UI.Page
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
                if (Session["SalesSuspended"] != null)
                {
                    DateTime dateTime = (DateTime)Session["dateIni"];
                    DateTime dateTime2 = (DateTime)Session["dateFin"];
                    string text = ((Session["cajero"] != null) ? Session["cajero"].ToString() : null);
                    short num = (short)((Session["caja"] != null) ? short.Parse(Session["caja"].ToString()) : 0);
                    List<VentaSuspendidaExtended> list = (List<VentaSuspendidaExtended>)Session["SalesSuspended"];
                    using StringWriter stringWriter = new StringWriter();
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        base.Response.ContentType = "application/vnd.ms-excel";
                        base.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}{1}.xls", "VentasSuspendidas", DateTime.Now.ToString("yyyyMMddHHmm")));
                        Table table = new Table();
                        TableRow tableRow = new TableRow();
                        tableRow.Cells.Add(new TableCell
                        {
                            Text = string.Format("<img src='http://{0}/Images/plaza.png' border='0' width='165' height='70' />", base.Request["HTTP_HOST"])
                        });
                        tableRow.Cells.Add(new TableCell());
                        tableRow.Cells.Add(new TableCell());
                        tableRow.Cells.Add(new TableCell
                        {
                            Text = string.Format("<h2>RELACION  DE VENTAS SUSPENDIDAS<br />DEL: {0}  AL  {1}<br />{2}  {3}</h2>", dateTime, dateTime2, (num != 0) ? ("CAJA: " + num + " ") : "", (text != null) ? (" CAJERO: " + text) : "")
                        });
                        table.Rows.Add(tableRow);
                        table.Rows.Add(new TableRow());
                        table.Rows.Add(new TableRow());
                        TableRow tableRow2 = new TableRow();
                        foreach (string item in new List<string> { "[Caja]", "[Fecha/Hora]", "[Cajero]", "[Status]", "[Codigo Barras]" })
                        {
                            tableRow2.Cells.Add(new TableCell
                            {
                                Text = item
                            });
                        }
                        table.Rows.Add(tableRow2);
                        foreach (VentaSuspendidaExtended row in list)
                        {
                            tableRow2 = new TableRow();
                            tableRow2.Cells.Add(new TableCell
                            {
                                Text = row.id_pos.ToString()
                            });
                            tableRow2.Cells.Add(new TableCell
                            {
                                Text = row.fecha_suspencion.ToString("dd/MM/yyyy HH:mm:ss")
                            });
                            tableRow2.Cells.Add(new TableCell
                            {
                                Text = row.cajero
                            });
                            tableRow2.Cells.Add(new TableCell
                            {
                                Text = row.status
                            });
                            List<venta_cancelada_articulo> list2 = new dcContextoSuPlazaDataContext().venta_cancelada_articulo.Where((venta_cancelada_articulo vca) => vca.id_venta_cancel.Equals(row.id_venta)).ToList();
                            string codigos = string.Empty;
                            list2.ForEach(delegate (venta_cancelada_articulo item)
                            {
                                codigos += $"'{item.cod_barras}, ";
                            });
                            tableRow2.Cells.Add(new TableCell
                            {
                                Text = codigos
                            });
                            table.Rows.Add(tableRow2);
                        }
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