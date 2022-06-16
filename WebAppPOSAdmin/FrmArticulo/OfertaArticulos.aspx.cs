using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Security.SeguridadSession;

using WebAppPOSAdmin.Common;
using WebAppPOSAdmin.DropDownListExtender;
using WebAppPOSAdmin.Funcionalidades;

namespace WebAppPOSAdmin.FrmArticulo
{
    public partial class OfertaArticulos : System.Web.UI.Page
    {

        private enum operations
        {
            CREATE,
            UPDATE
        }

        private operations operation;

        private oferta offer;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["id"] != null && new RepositorioOfertas().existOffer(Guid.Parse(base.Request.QueryString["id"].ToString())))
            {
                operation = operations.UPDATE;
                offer = new RepositorioOfertas().getOffer(Guid.Parse(base.Request.QueryString["id"].ToString()));
            }
            if (!base.IsPostBack)
            {
                txtFecha_ini.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFecha_fin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                llenarDropIniciales();
                if (operation.Equals(operations.UPDATE))
                {
                    loadSuspendedOffer(offer.id_oferta);
                }
            }
        }

        private void loadSuspendedOffer(Guid offerId)
        {
            try
            {
                txtIdOferta.Value = offer.id_oferta.ToString();
                txtDescOffer.Text = offer.descripcion;
                txtFecha_ini.Text = offer.fecha_ini.ToString("dd/MM/yyyy");
                txtFecha_fin.Text = offer.fecha_fin.ToString("dd/MM/yyyy");
                DataTable dataTable = new DataTable();
                ViewState["OfferItemsTable"] = null;
                dataTable.Columns.Add(new DataColumn("cod_barras", typeof(string)));
                dataTable.Columns.Add(new DataColumn("descripcion_larga", typeof(string)));
                dataTable.Columns.Add(new DataColumn("unidad", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precio_venta", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precio_oferta", typeof(string)));
                DataRow dataRow = dataTable.NewRow();
                foreach (OfertaArticuloExtended item in new Funciones().recoveryOfferDetail(offer.id_oferta))
                {
                    dataRow["cod_barras"] = item.cod_barras;
                    dataRow["descripcion_larga"] = item.descripcion_larga;
                    dataRow["unidad"] = item.unidad;
                    dataRow["precio_venta"] = item.precio_venta.ToString("F2");
                    dataRow["precio_oferta"] = item.precio_oferta.ToString("F2");
                    dataTable.Rows.Add(dataRow);
                    dataRow = dataTable.NewRow();
                }
                ViewState["OfferItemsTable"] = dataTable;
                gvOfferDetail.DataSource = dataTable;
                gvOfferDetail.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void llenarDropIniciales()
        {
            try
            {
                ddlProveedores.getProveedores();
                ddlOfertas.getOfertasSuspendidas();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public oferta getDatosOferta(string status)
        {
            try
            {
                oferta oferta = new oferta();
                if (txtDescOffer.Text.Trim().Equals(""))
                {
                    throw new Exception("Describa la oferta");
                }
                if (operation.Equals(operations.UPDATE))
                {
                    oferta.id_oferta = Guid.Parse(txtIdOferta.Value);
                }
                oferta.descripcion = txtDescOffer.Text.Trim();
                oferta.fecha_ini = DateTime.Parse($"{txtFecha_ini.Text} 00:00:00");
                oferta.fecha_fin = DateTime.Parse($"{txtFecha_fin.Text} 23:59:00");
                oferta.fecha_oferta = DateTime.Now;
                oferta.user_name = ((empleado)((SessionManager)Session["usuarioSession"]).Parametros["usuarioSession"]).user_name;
                oferta.status_oferta = status;
                return oferta;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
                return null;
            }
        }

        public oferta getDatosSuspendidos()
        {
            try
            {
                if (txtDescOffer.Text.Trim().Equals(""))
                {
                    throw new Exception("Describa la oferta");
                }
                oferta oferta = new oferta();
                oferta.fecha_ini = DateTime.Parse($"{txtFecha_ini.Text} 00:00:00");
                oferta.fecha_fin = DateTime.Parse($"{txtFecha_fin.Text} 23:59:00");
                oferta.fecha_oferta = DateTime.Now;
                oferta.descripcion = txtDescOffer.Text.Trim();
                oferta.status_oferta = "suspendida";
                if (Session["esSuspendido"].Equals(true))
                {
                    oferta.id_oferta = (Guid)Session["idOferta"];
                }
                return oferta;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
                return null;
            }
        }

        public bool verificarFechas()
        {
            bool result = false;
            try
            {
                if (!txtFecha_ini.Text.Equals("") && !txtFecha_fin.Text.Equals(""))
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
                return result;
            }
        }

        protected void AddItemsToOffer(object sender, EventArgs e)
        {
            try
            {
                cargarGridView();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void cargarGridView()
        {
            try
            {
                if (ViewState["OfferItemsTable"] != null)
                {
                    DataTable dataTable = (DataTable)ViewState["OfferItemsTable"];
                    List<OfertaArticuloExtended> list = findAndGetItems();
                    if (list != null && list.Count == 0)
                    {
                        throw new Exception("No hubo coincidencias de busqueda.");
                    }
                    if (dataTable.Rows.Count <= 0)
                    {
                        return;
                    }
                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        string barCode = gvOfferDetail.Rows[j].Cells[0].Text;
                        dataTable.Rows[j]["cod_barras"] = barCode;
                        dataTable.Rows[j]["descripcion_larga"] = gvOfferDetail.Rows[j].Cells[1].Text;
                        dataTable.Rows[j]["unidad"] = gvOfferDetail.Rows[j].Cells[2].Text;
                        dataTable.Rows[j]["precio_venta"] = gvOfferDetail.Rows[j].Cells[3].Text;
                        dataTable.Rows[j]["precio_oferta"] = ((TextBox)gvOfferDetail.Rows[j].Cells[4].FindControl("txtPrecioOferta")).Text;
                        if (list.Exists((OfertaArticuloExtended i) => i.cod_barras.Equals(barCode)))
                        {
                            list.Remove(list.First((OfertaArticuloExtended i) => i.cod_barras.Equals(barCode)));
                        }
                    }
                    DataRow dataRow = dataTable.NewRow();
                    foreach (OfertaArticuloExtended item in list)
                    {
                        dataRow["cod_barras"] = item.cod_barras;
                        dataRow["descripcion_larga"] = item.descripcion_larga;
                        dataRow["unidad"] = item.unidad;
                        dataRow["precio_venta"] = item.precio_venta.ToString("F2");
                        dataRow["precio_oferta"] = item.precio_oferta.ToString("F2");
                        dataTable.Rows.Add(dataRow);
                        dataRow = dataTable.NewRow();
                    }
                    ViewState["OfferItemsTable"] = dataTable;
                    gvOfferDetail.DataSource = dataTable;
                    gvOfferDetail.DataBind();
                    return;
                }
                DataTable dataTable2 = new DataTable();
                dataTable2.Columns.Add(new DataColumn("cod_barras", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("descripcion_larga", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("unidad", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("precio_venta", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("precio_oferta", typeof(string)));
                DataRow dataRow2 = dataTable2.NewRow();
                List<OfertaArticuloExtended> list2 = findAndGetItems();
                if (list2 != null && list2.Count == 0)
                {
                    throw new Exception("No hubo coincidencias de busqueda.");
                }
                foreach (OfertaArticuloExtended item2 in list2)
                {
                    dataRow2["cod_barras"] = item2.cod_barras;
                    dataRow2["descripcion_larga"] = item2.descripcion_larga;
                    dataRow2["unidad"] = item2.unidad;
                    dataRow2["precio_venta"] = item2.precio_venta.ToString("F2");
                    dataRow2["precio_oferta"] = item2.precio_oferta.ToString("F2");
                    dataTable2.Rows.Add(dataRow2);
                    dataRow2 = dataTable2.NewRow();
                }
                ViewState["OfferItemsTable"] = dataTable2;
                gvOfferDetail.DataSource = dataTable2;
                gvOfferDetail.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        private List<OfertaArticuloExtended> findAndGetItems()
        {
            try
            {
                if (ddlProveedores.SelectedValue.Equals("00000000-0000-0000-0000-000000000000") && txtDescripcion.Text.Trim().Equals(""))
                {
                    throw new Exception("Debe elegir un Proveedor y/o descripción del artículo");
                }
                return (!ddlProveedores.SelectedValue.Equals("00000000-0000-0000-0000-000000000000") && txtDescripcion.Text.Trim() != "") ? new Funciones().findAndGetItems(Guid.Parse(ddlProveedores.SelectedValue), txtDescripcion.Text.Trim()) : ((!ddlProveedores.SelectedValue.Equals("00000000-0000-0000-0000-000000000000")) ? new Funciones().findAndGetItems(Guid.Parse(ddlProveedores.SelectedValue)) : ((txtDescripcion.Text.Trim() != "") ? new Funciones().findAndGetItems(txtDescripcion.Text.Trim()) : null));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
                return null;
            }
        }

        public void actualizarSession(string cod_barras)
        {
            try
            {
                List<OfertaArticuloExtended> list = (List<OfertaArticuloExtended>)Session["listaOfertas"];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].cod_barras == cod_barras)
                    {
                        list.Remove(list[i]);
                    }
                }
                Session["listaOfertas"] = list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void addItems(object sender, EventArgs e)
        {
            try
            {
                cargarGridView();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void createOffer(object sender, EventArgs e)
        {
            saveOffer("disponible");
        }

        protected void suspendedOffer(object sender, EventArgs e)
        {
            switch (short.Parse(ddlSuspender.SelectedValue))
            {
                case 1:
                    offer = new RepositorioOfertas().getLastOffer();
                    operation = ((offer != null) ? operations.UPDATE : operations.CREATE);
                    if (offer != null)
                    {
                        txtIdOferta.Value = offer.id_oferta.ToString();
                        txtDescOffer.Text = offer.descripcion;
                        txtFecha_ini.Text = offer.fecha_ini.ToString("dd/MM/yyyy");
                        txtFecha_fin.Text = offer.fecha_fin.ToString("dd/MM/yyyy");
                    }
                    break;
                case 2:
                    operation = operations.CREATE;
                    break;
            }
            saveOffer("suspendida");
        }

        private void saveOffer(string status)
        {
            try
            {
                RepositorioOfertas repositorioOfertas = new RepositorioOfertas();
                if (gvOfferDetail.Rows.Count > 0)
                {
                    if (operation.Equals(operations.CREATE))
                    {
                        Guid guid = repositorioOfertas.insertarOferta(getDatosOferta(status));
                        repositorioOfertas.insertaListaArticulosOferta(listaDatosGrid(guid, status), guid);
                    }
                    else if (operation.Equals(operations.UPDATE))
                    {
                        Guid id_oferta = offer.id_oferta;
                        repositorioOfertas.updateOffer(getDatosOferta(status));
                        repositorioOfertas.updateOfferDetail(listaDatosGrid(id_oferta, status), id_oferta);
                    }
                    base.Response.Redirect("~/FrmArticulo/OfertaArticulos.aspx", endResponse: false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", string.Format("alert('{0}');", "Aún no se agrega artículos a la oferta"), addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        public List<oferta_articulo> listaDatosGrid(Guid OfferID, string status)
        {
            List<oferta_articulo> list = new List<oferta_articulo>();
            foreach (GridViewRow row in gvOfferDetail.Rows)
            {
                TextBox textBox = (TextBox)gvOfferDetail.Rows[row.RowIndex].Cells[4].FindControl("txtPrecioOferta");
                if (!(decimal.Parse(textBox.Text.Trim()) == 0.0m))
                {
                    oferta_articulo oferta_articulo = new oferta_articulo();
                    oferta_articulo.id_oferta = OfferID;
                    oferta_articulo.cod_barras = gvOfferDetail.Rows[row.RowIndex].Cells[0].Text;
                    oferta_articulo.precio_oferta = decimal.Parse(textBox.Text.Trim());
                    oferta_articulo.status_oferta = status;
                    list.Add(oferta_articulo);
                }
            }
            return list;
        }

        protected void cleanOffer(object sender, EventArgs e)
        {
            ViewState["OfferItemsTable"] = null;
            gvOfferDetail.DataSource = null;
            gvOfferDetail.DataBind();
        }

        protected void btnNewOffer_Click(object sender, EventArgs e)
        {
            ViewState["OfferItemsTable"] = null;
            base.Response.Redirect("~/FrmArticulo/OfertaArticulos.aspx", endResponse: false);
        }

        protected void btnRecoveryOffer_Click(object sender, EventArgs e)
        {
            try
            {
                base.Response.Redirect($"~/FrmArticulo/OfertaArticulos.aspx?id={ddlOfertas.SelectedValue}", endResponse: false);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnPrinterLabels_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < gvOfferDetail.Rows.Count; i++)
                {
                    TextBox textBox = (TextBox)gvOfferDetail.Rows[i].Cells[4].FindControl("txtPrecioOferta");
                    if (ddlPrinterType.SelectedValue.Equals("1"))
                    {
                        new ZebraPrinterController().printLblAdherible(gvOfferDetail.Rows[i].Cells[0].Text, 1, normalPrice: false, decimal.Parse(textBox.Text));
                    }
                    else
                    {
                        new ZebraPrinterController().printLblAnaquel(gvOfferDetail.Rows[i].Cells[0].Text, 1, normalPrice: false, decimal.Parse(textBox.Text));
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }
    }
}