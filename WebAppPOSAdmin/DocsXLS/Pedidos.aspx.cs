using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.DocsXLS
{
    public partial class Pedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                createXLS();
            }
        }

        public void createXLS()
        {
            try
            {
                if (Session["Order"] != null)
                {
                    using StringWriter stringWriter = new StringWriter();
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        using (new dcContextoSuPlazaDataContext())
                        {
                            OrderExtended orderExtended = (OrderExtended)Session["Order"];
                            base.Response.Clear();
                            base.Response.ContentType = "application/vnd.ms-excel";
                            base.Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Pedido_{0}.xls", DateTime.Now.ToString("yyyyMMdd")));
                            Table tableXLS = new Table();
                            TableRow tableRow = new TableRow();
                            tableRow.Cells.Add(new TableCell
                            {
                                Text = string.Format("<img src='http://{0}/Images/plaza.png' border='0' width='165' height='70' />", base.Request["HTTP_HOST"])
                            });
                            tableRow.Cells.Add(new TableCell());
                            tableRow.Cells.Add(new TableCell
                            {
                                Text = string.Format("<h1>{0}<br />Pedido: {1}</h1>", orderExtended.proveedor, "---")
                            });
                            tableXLS.Rows.Add(tableRow);
                            tableXLS.Rows.Add(new TableRow());
                            tableXLS.Rows.Add(new TableRow());
                            TableRow fila = new TableRow();
                            foreach (string item in new List<string> { "Codigo barras", "Descripcion", "UM", "UMC", "Costo", "Exis. Cja", "Exis. Pza", "Sugerido", "A pedir", "Devolver" })
                            {
                                fila.Cells.Add(new TableCell
                                {
                                    Text = item.ToUpper()
                                });
                            }
                            tableXLS.Rows.Add(fila);
                            orderExtended.items.ToList().ForEach(delegate (PedidoArticulosExtended row)
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
                                    Text = row.umc.ToString("G9")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.costo.ToString("F2")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.existencia_caja.ToString("G9")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.existencias_pieza.ToString("G9")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.sugerido.ToString("G9")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = row.a_pedir.ToString("G9")
                                });
                                fila.Cells.Add(new TableCell
                                {
                                    Text = "0"
                                });
                                tableXLS.Rows.Add(fila);
                            });
                            tableXLS.RenderControl(writer);
                        }
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