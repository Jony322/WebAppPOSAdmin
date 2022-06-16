using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Facturacion
{
    public partial class Facturar : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["TicketsFactura"] = null;
                Session["ItemsSales"] = null;
                Session["OthersItemsSales"] = null;
                ddlCaja.getListPOS();
                ddlUnidad.getUnidad();
                ddlUsoCFDI.getUsoCFDI();
                ddlUsoCFDI.SelectedIndex = 11;
                ddlTipoComprobante.getTipoComprobante();
                ddlTipoComprobante.SelectedValue = "FA";
                ddlCondicionPago.getCondicionPago();
                ddlCondicionPago.SelectedValue = "PUE";
                ddlUnidad.Text = "Pza";
                checkPathServer();
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                lstMetodoPago.DataSource = dcContextoSuPlazaDataContext.metodo_pago.ToList();
                lstMetodoPago.DataValueField = "id_metodo";
                lstMetodoPago.DataTextField = "descripcion";
                lstMetodoPago.SelectedIndex = 0;
                lstMetodoPago.DataBind();
            }
        }

        public void checkPathServer()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            string cfdi_path_txt = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().cfdi_path_txt;
            if (cfdi_path_txt == null || cfdi_path_txt.Length <= 5)
            {
                lblWarning.Text = "ADVERTENCIA: El directorio para generar el TXT no se ha especificado.";
            }
            if (!Directory.Exists(cfdi_path_txt))
            {
                lblWarning.Text = "ADVERTENCIA: No es accesible el directorio para facturar";
            }
        }

        protected void ibtFindItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                var dataSource = (from c in dcContextoSuPlazaDataContext.cliente
                                  orderby c.razon_social
                                  select new { c.id_cliente, c.rfc, c.razon_social }).ToList();
                gvResultsFind.DataSource = dataSource;
                gvResultsFind.DataBind();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: ibtFindItem_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected void btnRecovery_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRFC.Text.Trim().Length == 0)
                {
                    throw new Exception("Debe ingresar el RFC del cliente");
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                cliente cliente = dcContextoSuPlazaDataContext.cliente.FirstOrDefault((cliente t) => t.rfc.Equals(txtRFC.Text.Trim()));
                if (cliente == null)
                {
                    RecoveryClient(default(Guid));
                }
                else
                {
                    RecoveryClient(cliente.id_cliente);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: btnRecovery_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        public void RecoveryClient(Guid id)
        {
            try
            {
                txtIdCliente.Value = "";
                txtRFC.Text = "";
                txtRazonSocial.Text = "";
                txtCorreo.Text = "";
                txtCorreo2.Text = "";
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                cliente cliente = dcContextoSuPlazaDataContext.cliente.FirstOrDefault((cliente t) => t.id_cliente.Equals(id));
                if (cliente == null)
                {
                    throw new Exception("El cliente no está registrado");
                }
                txtIdCliente.Value = cliente.id_cliente.ToString();
                txtRFC.Text = cliente.rfc;
                txtRazonSocial.Text = cliente.razon_social;
                txtCorreo.Text = cliente.e_mail;
                txtCorreo2.Text = cliente.e_mail2;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: RecoveryClient " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
            finally
            {
                txtRFC.Focus();
            }
        }

        protected void gvResultsFind_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] array = e.CommandArgument.ToString().Split(',');
            string commandName = e.CommandName;
            if (commandName == "selectedItem")
            {
                RecoveryClient(Guid.Parse(array[0]));
            }
        }

        protected void btnAddTicket_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCaja.SelectedValue.Equals("0") || txtFolio.Text.Trim().Length == 0)
                {
                    throw new Exception("Seleccione una caja e ingrese un número de ticket");
                }
                List<TicketsFactura> list = new List<TicketsFactura>();
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                if (Session["TicketsFactura"] == null)
                {
                    TicketsFactura ticketsFactura = (from v in dcContextoSuPlazaDataContext.venta
                                                     where v.id_pos.Equals(int.Parse(ddlCaja.SelectedValue)) && v.folio.Equals(long.Parse(txtFolio.Text))
                                                     select new TicketsFactura
                                                     {
                                                         id_venta = v.id_venta,
                                                         id_pos = v.id_pos,
                                                         folio = v.folio
                                                     }).FirstOrDefault();
                    if (ticketsFactura == null)
                    {
                        throw new Exception("No se encontró el Ticket");
                    }
                    if (new TicketsFactura().isInvoicedTicket(ticketsFactura))
                    {
                        throw new Exception("El Ticket ya fue Facturado");
                    }
                    list.Add(ticketsFactura);
                    Session["TicketsFactura"] = list;
                }
                else
                {
                    list = (List<TicketsFactura>)Session["TicketsFactura"];
                    if (list.FirstOrDefault((TicketsFactura t) => t.id_pos.Equals(int.Parse(ddlCaja.SelectedValue)) && t.folio.Equals(long.Parse(txtFolio.Text))) != null)
                    {
                        throw new Exception("El Ticket que ingresó, ya está asociado a la factura");
                    }
                    TicketsFactura ticketsFactura2 = (from v in dcContextoSuPlazaDataContext.venta
                                                      where v.id_pos.Equals(int.Parse(ddlCaja.SelectedValue)) && v.folio.Equals(long.Parse(txtFolio.Text))
                                                      select new TicketsFactura
                                                      {
                                                          id_venta = v.id_venta,
                                                          id_pos = v.id_pos,
                                                          folio = v.folio
                                                      }).FirstOrDefault();
                    if (ticketsFactura2 == null)
                    {
                        throw new Exception("No se encontró el Ticket");
                    }
                    if (new TicketsFactura().isInvoicedTicket(ticketsFactura2))
                    {
                        throw new Exception("El Ticket ya fue Facturado");
                    }
                    list.Add(ticketsFactura2);
                    Session["TicketsFactura"] = list;
                }
                loadDetailInvoice(list);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: btnAddTicket_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
            finally
            {
                txtFolio.Text = "";
                txtFolio.Focus();
            }
        }

        protected void btnAddItemInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBarCode.Text.Trim().Length == 0 && txtDescripcion.Text.Trim().Length == 0 && txtCantidad.Text.Trim().Length == 0 && txtPrecioVta.Text.Trim().Length == 0 && txtItemIVA.Text.Trim().Length == 0)
                {
                    throw new Exception("Debe llenar todos los campos del artículo");
                }
                List<venta_articulo> list = ((Session["OthersItemsSales"] != null) ? ((List<venta_articulo>)Session["OthersItemsSales"]) : new List<venta_articulo>());
                using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
                {
                    list.Add(new venta_articulo
                    {
                        cod_barras = txtBarCode.Text.Trim(),
                        articulo = new articulo
                        {
                            cod_barras = txtBarCode.Text.Trim(),
                            cve_producto = txtBarCode.Text.Trim(),
                            descripcion = txtDescripcion.Text.Trim(),
                            unidad_medida = dcContextoSuPlazaDataContext.unidad_medida.FirstOrDefault((unidad_medida u) => ((object)u.id_unidad).Equals((object)ddlUnidad.SelectedValue)),
                            iva = decimal.Parse(txtItemIVA.Text.Trim()) / 100m
                        },
                        precio_vta = decimal.Parse(txtPrecioVta.Text.Trim()),
                        iva = decimal.Parse(txtItemIVA.Text.Trim()) / 100m,
                        cantidad = decimal.Parse(txtCantidad.Text.Trim()),
                        register = false
                    });
                }
                Session["OthersItemsSales"] = list;
                loadDetailInvoice((List<TicketsFactura>)Session["TicketsFactura"]);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: btnAddItemInvoice_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
            finally
            {
                gvItems.DataBind();
                btnFacturar.Enabled = gvItems.Rows.Count > 0;
                ResetNewItemPanel();
            }
        }

        public void ResetNewItemPanel()
        {
            txtBarCode.Text = "";
            txtDescripcion.Text = "";
            txtCantidad.Text = "0";
            txtPrecioVta.Text = "0.00";
            chkIVA.Checked = false;
            txtItemIVA.Text = "0.00";
            txtTotalItem.Text = "0.00";
        }

        public void loadDetailInvoice(List<TicketsFactura> tickets)
        {
            try
            {
                gvItems.DataSource = null;
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                decimal num = default(decimal);
                decimal num2 = default(decimal);
                decimal num3 = default(decimal);
                decimal num4 = default(decimal);
                List<venta_articulo> list = new List<venta_articulo>();
                if (tickets != null)
                {
                    foreach (TicketsFactura ticket in tickets)
                    {
                        List<venta_articulo> list2 = dcContextoSuPlazaDataContext.venta_articulo.Where((venta_articulo va) => va.id_venta.Equals(ticket.id_venta) && va.id_pos.Equals(ticket.id_pos)).ToList();
                        venta_devolucion devolution = dcContextoSuPlazaDataContext.venta_devolucion.FirstOrDefault((venta_devolucion vd) => vd.id_venta.Equals(ticket.id_venta) && vd.id_pos.Equals(ticket.id_pos));
                        if (devolution != null)
                        {
                            foreach (venta_devolucion_articulo vda in dcContextoSuPlazaDataContext.venta_devolucion_articulo.Where((venta_devolucion_articulo e) => e.venta_devolucion.Equals(devolution)).ToList())
                            {
                                if (list2.Find((venta_articulo va) => va.cod_barras.Equals(vda.cod_barras) && va.no_articulo.Equals(vda.no_articulo)) == null)
                                {
                                    continue;
                                }
                                for (int j = 0; j < list2.Count; j++)
                                {
                                    if (list2[j].cod_barras.Equals(vda.cod_barras) && list2[j].no_articulo.Equals(vda.no_articulo))
                                    {
                                        if (list2[j].cantidad >= vda.cantidad)
                                        {
                                            list2[j].cantidad -= vda.cantidad;
                                        }
                                        if (list2[j].cantidad <= 0m)
                                        {
                                            list2.RemoveAt(j);
                                        }
                                    }
                                }
                            }
                        }
                        list.AddRange(list2);
                    }
                }
                if (Session["OthersItemsSales"] != null)
                {
                    foreach (venta_articulo item in (List<venta_articulo>)Session["OthersItemsSales"])
                    {
                        list.Add(item);
                    }
                }
                foreach (venta_articulo item2 in list)
                {
                    num += item2.subTotalConDescuento();
                    num2 += item2.descuento();
                    num3 += item2.getIVAd3();
                    num4 += item2.total();
                }
                var dataSource = list.Select((venta_articulo i) => new
                {
                    cod_barras = i.cod_barras,
                    descripcion = i.articulo.descripcion,
                    unidad = i.articulo.unidad_medida.descripcion,
                    cantidad = i.cantidad,
                    total = i.total()
                }).ToList();
                gvItems.DataSource = dataSource;
                txtSubTotal.Text = num.ToString("F2");
                txtDescuento.Text = num2.ToString("F2");
                txtIVA.Text = num3.ToString("F2");
                txtTotal.Text = num4.ToString("F2");
                Session["ItemsSales"] = list;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: loadDetailInvoice " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
            finally
            {
                gvItems.DataBind();
                btnFacturar.Enabled = gvItems.Rows.Count > 0;
            }
        }

        protected void btnResetDetail_Click(object sender, EventArgs e)
        {
            gvItems.DataSource = null;
            gvItems.DataBind();
            btnFacturar.Enabled = false;
            Session["TicketsFactura"] = null;
            Session["ItemsSales"] = null;
            Session["OthersItemsSales"] = null;
        }

        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (ListItem item in from ListItem item in lstMetodoPago.Items
                                          where item.Selected
                                          select item)
                {
                    if (item.Selected)
                    {
                        list.Add(item.Value);
                    }
                }
                if (txtRFC.Text.Trim().Length == 0 || txtIdCliente.Value.Equals(""))
                {
                    throw new Exception("Ingrese el RFC del Cliente y presione Enter");
                }
                if (new TicketsFactura().validateClient(Guid.Parse(txtIdCliente.Value), txtRFC.Text.Trim()))
                {
                    throw new Exception("Verifique que el RFC del Cliente sea correcto");
                }
                if (lstMetodoPago.GetSelectedIndices().Count() == 0)
                {
                    throw new Exception("Debe elegír al menos un método de pago");
                }
                if (ddlUsoCFDI.SelectedIndex == 0)
                {
                    throw new Exception("Debe seleccionar el Uo del CFDI");
                }
                generateInvoice(new facturacion().facturar(Guid.Parse(txtIdCliente.Value), list, ddlTipoComprobante.SelectedValue, ddlCondicionPago.SelectedValue, ddlUsoCFDI.SelectedValue, (List<TicketsFactura>)Session["TicketsFactura"], (List<venta_articulo>)Session["ItemsSales"]), list);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: btnFacturar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        public void generateInvoice(long InvoiceID, List<string> payMethods)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                factura factura = dcContextoSuPlazaDataContext.factura.FirstOrDefault((factura i) => i.id_factura.Equals(InvoiceID));
                dcContextoSuPlazaDataContext.empresa.FirstOrDefault();
                string arg = string.Format("{0}{1}{2}.txt", factura.cliente.rfc, DateTime.Now.ToString("ddMMyyyy"), factura.id_factura);
                string cfdi_path_txt = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().cfdi_path_txt;
                if (cfdi_path_txt == null || cfdi_path_txt.Length <= 5)
                {
                    throw new Exception("El directorio para generar el TXT no se ha especificado.");
                }
                if (!Directory.Exists(cfdi_path_txt))
                {
                    throw new Exception("El directorio para generar el TXT no existe.");
                }
                string arg2 = string.Join(",", payMethods);
                using StreamWriter streamWriter = new StreamWriter($"{cfdi_path_txt}\\{arg}");
                streamWriter.WriteLine($"[DATOS_ACCESO]");
                streamWriter.WriteLine(string.Format("USUARIO;{0}", dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().cfdi_user ?? ""));
                streamWriter.WriteLine(string.Format("PASSWORD;{0}", dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().cfdi_pwsd ?? ""));
                streamWriter.WriteLine("");
                streamWriter.WriteLine($"[CFD33]");
                streamWriter.WriteLine(string.Format("SERIE;{0}", "A"));
                streamWriter.WriteLine($"FOLIO;{InvoiceID}");
                streamWriter.WriteLine(string.Format("FECHA;{0}", DateTime.Now.ToString("yyyy-MM-dd")));
                streamWriter.WriteLine($"FORMAPAGO;{arg2}");
                streamWriter.WriteLine($"CONDICIONESDEPAGO;{ddlCondicionPago.SelectedItem}");
                decimal num = default(decimal);
                decimal num2 = default(decimal);
                decimal num3 = default(decimal);
                decimal num4 = default(decimal);
                List<venta_articulo> list = (List<venta_articulo>)Session["ItemsSales"];
                foreach (venta_articulo item in list)
                {
                    decimal twoDecimal = getTwoDecimal(item.subTotalUnitario());
                    decimal twoDecimal2 = getTwoDecimal(twoDecimal * item.cantidad);
                    decimal twoDecimal3 = getTwoDecimal(twoDecimal2 * item.porcent_desc);
                    decimal twoDecimal4 = getTwoDecimal(twoDecimal * item.cantidad - twoDecimal3);
                    decimal threeDecimal = getThreeDecimal(twoDecimal4 * item.iva);
                    num4 += twoDecimal3;
                    num += decimal.Parse(twoDecimal2.ToString("F2"));
                    num3 += threeDecimal;
                    num2 += getTwoDecimal(twoDecimal2 - twoDecimal3);
                }
                streamWriter.WriteLine(string.Format("SUBTOTAL;{0}", num.ToString("F3")));
                streamWriter.WriteLine(string.Format("DESCUENTO;{0}", num4.ToString("F2")));
                streamWriter.WriteLine("MONEDA;MXN");
                streamWriter.WriteLine("TIPOCAMBIO;0.00");
                streamWriter.WriteLine(string.Format("TOTAL;{0}", (num2 + num3).ToString("F3")));
                streamWriter.WriteLine($"TIPODECOMPROBANTE;{ddlTipoComprobante.SelectedValue}");
                streamWriter.WriteLine($"METODOPAGO;{ddlCondicionPago.SelectedValue}");
                streamWriter.WriteLine($"LUGAREXPEDICION;{dcContextoSuPlazaDataContext.empresa.FirstOrDefault().codigo_postal}");
                streamWriter.WriteLine($"[CFDIRELACIONADOS]");
                streamWriter.WriteLine($"TIPORELACION;{txtCFDIrelacionado.Text}");
                streamWriter.WriteLine($"UUID;{txtUUIDrelacionado.Text}");
                List<string> list2 = new List<string>();
                if (factura.cliente.e_mail != null && factura.cliente.e_mail.Length > 0)
                {
                    list2.Add(txtCorreo.Text);
                }
                if (factura.cliente.e_mail2 != null && factura.cliente.e_mail2.Length > 0)
                {
                    list2.Add(txtCorreo2.Text);
                }
                streamWriter.WriteLine("[RECEPTOR]");
                streamWriter.WriteLine($"RFCR;{factura.cliente.rfc}");
                streamWriter.WriteLine($"NOMBRER;{factura.cliente.razon_social}");
                streamWriter.WriteLine(string.Format("NUMREGIDTRIB;{0}", ""));
                streamWriter.WriteLine($"USOCFDI;{ddlUsoCFDI.SelectedValue}");
                streamWriter.WriteLine(string.Format("EMAIL;{0}", string.Join(":", list2)));
                foreach (venta_articulo item2 in list)
                {
                    decimal twoDecimal5 = getTwoDecimal(item2.subTotalUnitario());
                    decimal twoDecimal6 = getTwoDecimal(twoDecimal5 * item2.cantidad);
                    decimal twoDecimal7 = getTwoDecimal(twoDecimal6 * item2.porcent_desc);
                    decimal twoDecimal8 = getTwoDecimal(twoDecimal5 * item2.cantidad - twoDecimal7);
                    decimal threeDecimal2 = getThreeDecimal(twoDecimal8 * item2.iva);
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine("[CONCEPTO]");
                    streamWriter.WriteLine($"CLAVEPRODSERV;{item2.articulo.cve_producto}");
                    streamWriter.WriteLine("NOIDENTIFICACION;");
                    streamWriter.WriteLine($"CANTIDAD;{item2.cantidad}");
                    streamWriter.WriteLine($"CLAVEUNIDAD;{item2.articulo.unidad_medida.cve_sat}");
                    streamWriter.WriteLine($"UNIDAD;{item2.articulo.unidad_medida.descripcion}");
                    streamWriter.WriteLine($"DESCRIPCION;{item2.articulo.descripcion}");
                    streamWriter.WriteLine(string.Format("VALORUNITARIO;{0}", twoDecimal5.ToString("F2")));
                    streamWriter.WriteLine(string.Format("IMPORTE;{0}", twoDecimal6.ToString("F2")));
                    streamWriter.WriteLine(string.Format("DESCUENTOC;{0}", twoDecimal7.ToString("F2")));
                    streamWriter.WriteLine("NUMEROCUENTAPREDIAL;1");
                    streamWriter.WriteLine("[TRASLADOC]");
                    streamWriter.WriteLine(string.Format("BASETC;{0}", twoDecimal8.ToString("F2")));
                    streamWriter.WriteLine(string.Format("IMPUESTOTC;{0}", "002"));
                    streamWriter.WriteLine(string.Format("TIPOFACTORTC;{0}", "Tasa"));
                    streamWriter.WriteLine(string.Format("TASAOCUOTATC;{0}", item2.iva.ToString("F6")));
                    streamWriter.WriteLine(string.Format("IMPORTETRTC;{0}", threeDecimal2.ToString("F3")));
                }
                streamWriter.Close();
                string arg3 = string.Format("http://{0}/Facturacion/Show.aspx?file=A{1}", base.Request["HTTP_HOST"], factura.id_factura);
                string script = $"window.open('{arg3}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Facturar " + "Acción: generateInvoice " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        protected decimal getThreeDecimal(decimal number)
        {
            return Math.Ceiling(decimal.Parse(number.ToString("F4")) * 1000.0m) / 1000.0m;
        }

        protected decimal getTwoDecimal(decimal number)
        {
            return Math.Ceiling(decimal.Parse(number.ToString("F3")) * 100.0m) / 100.0m;
        }

        protected void chkIVA_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                txtItemIVA.Text = (new MySettings().getIVA() * 100m).ToString("F2");
            }
            else
            {
                txtItemIVA.Text = 0.ToString("F2");
            }
            txtTotalItem.Text = (decimal.Parse(txtCantidad.Text) * decimal.Parse(txtPrecioVta.Text)).ToString("F2");
        }

        protected void btnFindClient_Click(object sender, EventArgs e)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            var dataSource = (from c in dcContextoSuPlazaDataContext.cliente
                              orderby c.razon_social
                              where SqlMethods.Like(c.razon_social, $"%{txtFindClient.Text.Trim()}%")
                              select new { c.id_cliente, c.rfc, c.razon_social }).ToList();
            gvResultsFind.DataSource = dataSource;
            gvResultsFind.DataBind();
        }
    }
}