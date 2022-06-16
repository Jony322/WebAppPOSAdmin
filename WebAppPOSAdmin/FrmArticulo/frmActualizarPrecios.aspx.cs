using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Scripts;

using WebAppPOSAdmin.DropDownListExtender;
using WebAppPOSAdmin.Funcionalidades;
using WebAppPOSAdmin.Common;

namespace WebAppPOSAdmin.FrmArticulo
{
    public partial class frmActualizarPrecios : System.Web.UI.Page
    {
        private enum operations
        {
            UpdatePrices,
            RecoveryPrices
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                Session["ListaFiltro"] = null;
                Session["esSuspendido"] = false;
                Session["cont"] = 0;
                llenarDropIniciales();
            }
        }

        public void llenarDropIniciales()
        {
            try
            {
                ddlProveedor.getProveedores();
                enableControls();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvArticulos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvArticulos.EditIndex = e.NewEditIndex;
                gvArticulos.DataSource = (List<ArticuloExtended>)Session["listaDatos"];
                gvArticulos.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void gvArticulos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvArticulos.EditIndex = -1;
                gvArticulos.DataSource = (List<ArticuloExtended>)Session["listaDatos"];
                gvArticulos.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void gvArticulos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string cod_barrasN = gvArticulos.DataKeys[e.RowIndex].Values[0].ToString();
                Convert.ToDecimal(gvArticulos.DataKeys[e.RowIndex].Values[1]);
                Convert.ToDecimal(gvArticulos.DataKeys[e.RowIndex].Values[2]);
                Convert.ToDecimal(gvArticulos.DataKeys[e.RowIndex].Values[3]);
                Convert.ToDecimal(gvArticulos.DataKeys[e.RowIndex].Values[4]);
                TextBox textBox = (TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txtPrecioCompra");
                TextBox textBox2 = (TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txtUtilidad");
                TextBox textBox3 = (TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txtPrecioVenta");
                TextBox textBox4 = (TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txtIva");
                actualizarLista(cod_barrasN, Convert.ToDecimal(textBox.Text), Convert.ToDecimal(textBox3.Text), Convert.ToDecimal(textBox2.Text), Convert.ToDecimal(textBox4.Text));
                gvArticulos.EditIndex = -1;
                gvArticulos.DataSource = (List<ArticuloExtended>)Session["listaDatos"];
                gvArticulos.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void actualizarLista(string cod_barrasN, decimal precio_compraN, decimal precio_ventaN, decimal utilidadN, decimal ivaN)
        {
            try
            {
                List<ArticuloExtended> list = (List<ArticuloExtended>)Session["listaDatos"];
                List<ArticuloExtended> list2 = new List<ArticuloExtended>();
                if ((int)Session["cont"] > 0)
                {
                    list2 = (List<ArticuloExtended>)Session["ListaFiltro"];
                }
                foreach (ArticuloExtended item in list)
                {
                    if (item.cod_barras.Equals(cod_barrasN))
                    {
                        item.precio_compra = precio_compraN;
                        item.utilidad = utilidadN;
                        item.precio_venta = precio_ventaN;
                        item.iva = ivaN;
                        ArticuloExtended articuloExtended = new ArticuloExtended();
                        articuloExtended.cod_barras = cod_barrasN;
                        articuloExtended.precio_compra = precio_compraN;
                        articuloExtended.precio_venta = precio_ventaN;
                        articuloExtended.iva = ivaN;
                        articuloExtended.utilidad = utilidadN;
                        list2.Add(articuloExtended);
                        Session["cont"] = 1;
                        break;
                    }
                }
                Session["listaDatos"] = list;
                Session["ListaFiltro"] = list2;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlProveedor.SelectedValue.Equals("00000000-0000-0000-0000-000000000000") && txtDescripcion.Text.Trim().Equals(""))
                {
                    throw new Exception("Debe elegir un Proveedor y/o descripción del artículo");
                }
                cargarGridView();
                enableControls();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void cargarGridView()
        {
            Session["listaDatos"] = null;
            List<ArticuloExtended> list = obtenerListaArticulos();
            if (list == null)
            {
                throw new Exception("No hubo resultados");
            }
            if (list.Count <= 0)
            {
                throw new Exception("No hubo resultados");
            }
            gvArticulos.DataSource = list.OrderBy((ArticuloExtended i) => i.descripcion);
            gvArticulos.DataBind();
        }

        public List<ArticuloExtended> obtenerListaArticulos()
        {
            List<ArticuloExtended> list = ((!ddlProveedor.SelectedValue.Equals("00000000-0000-0000-0000-000000000000") && txtDescripcion.Text.Trim() != "") ? new Procedures().findAndGetItems(Guid.Parse(ddlProveedor.SelectedValue), txtDescripcion.Text.Trim(), ddlOrderBy.SelectedValue, "ASC") : ((!ddlProveedor.SelectedValue.Equals("00000000-0000-0000-0000-000000000000")) ? new Procedures().findAndGetItems(Guid.Parse(ddlProveedor.SelectedValue), ddlOrderBy.SelectedValue, "ASC") : ((txtDescripcion.Text.Trim() != "") ? new Procedures().findAndGetItems(txtDescripcion.Text.Trim(), ddlOrderBy.SelectedValue, "ASC") : null)));
            return new RepositorioArticulos().mergeList(list);
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvArticulos.Rows.Count == 0)
                {
                    throw new Exception("No hay artículos por actualizar su precio.");
                }
                Funciones funciones = new Funciones();
                if (Session["esSuspendido"].Equals(true))
                {
                    actualizarPrecio();
                    funciones.eliminarArticulosSuspendidos((List<ArticuloExtended>)Session["ListaFiltro"]);
                }
                else
                {
                    actualizarPrecio();
                }
                limpiarCampos();
                llenarDropIniciales();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnLlamarSuspenciones_Click(object sender, EventArgs e)
        {
            try
            {
                if (!new RepositorioArticulos().existsItemsSuspended())
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", string.Format("alert('{0}');", "No hay artículos suspendidos por recuperar."), addScriptTags: true);
                }
                Funciones funciones = new Funciones();
                Session["listaDatos"] = funciones.getArticulosSuspendidos();
                if (Session["listaDatos"] != null)
                {
                    Session["ListaFiltro"] = Session["listaDatos"];
                    gvArticulos.DataSource = (List<ArticuloExtended>)Session["listaDatos"];
                    gvArticulos.DataBind();
                    Session["esSuspendido"] = true;
                }
                else
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "sinResultados();", addScriptTags: true);
                }
                enableControls();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void limpiarCampos()
        {
            txtDescripcion.Text = string.Empty;
            gvArticulos.DataSource = null;
            gvArticulos.DataBind();
        }

        public void actualizarPrecio()
        {
            List<ArticuloExtended> list = null;
            try
            {
                RepositorioArticulos repositorioArticulos = new RepositorioArticulos();
                List<ArticuloExtended> list2 = new List<ArticuloExtended>();
                foreach (GridViewRow row in gvArticulos.Rows)
                {
                    ArticuloExtended articuloExtended = new ArticuloExtended();
                    TextBox textBox = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[3].FindControl("txtPrecioCompra");
                    TextBox textBox2 = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[4].FindControl("txtUtilidad");
                    TextBox textBox3 = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[5].FindControl("txtPrecioVenta");
                    TextBox obj = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[6].FindControl("txtIva");
                    string text = gvArticulos.Rows[row.RowIndex].Cells[0].Text;
                    decimal precio_compra = decimal.Parse(textBox.Text.Trim());
                    decimal utilidad = decimal.Parse(textBox2.Text.Trim());
                    decimal precio_venta = decimal.Parse(textBox3.Text.Trim());
                    decimal iva = decimal.Parse(obj.Text.Trim());
                    articuloExtended.cod_barras = text;
                    articuloExtended.precio_compra = precio_compra;
                    articuloExtended.utilidad = utilidad;
                    articuloExtended.precio_venta = precio_venta;
                    articuloExtended.iva = iva;
                    list2.Add(articuloExtended);
                }
                list = repositorioArticulos.changePriceItem(list2);
                Session["itemsAffected"] = list;
                if (list != null)
                {
                    showKitsAffected(list);
                }
                string arg = "http://" + base.Request["HTTP_HOST"] + "/DocsXLS/CambioPrecios.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void showKitsAffected(List<ArticuloExtended> itemsAffected)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            gvKITs.DataSource = null;
            List<kit_articulos> list = new List<kit_articulos>();
            foreach (ArticuloExtended item in itemsAffected)
            {
                kit_articulos kit_articulos = dcContextoSuPlazaDataContext.kit_articulos.FirstOrDefault((kit_articulos k) => k.cod_barras_pro.Equals(item.cod_barras) && DateTime.Now >= k.articulo1.kit_fecha_ini && DateTime.Now <= k.articulo1.kit_fecha_fin);
                if (kit_articulos != null)
                {
                    list.Add(kit_articulos);
                }
            }
            if (list.Count > 0)
            {
                gvKITs.DataSource = list.Select((kit_articulos k) => new
                {
                    k.cod_barras_kit,
                    k.articulo1.descripcion,
                    k.cod_barras_pro
                }).ToList();
                gvKITs.DataBind();
            }
        }

        public void suspenderCambioPrecio(bool addItems)
        {
            IArticulos articulos = new RepositorioArticulos();
            List<ArticuloExtended> list = new List<ArticuloExtended>();
            foreach (GridViewRow row in gvArticulos.Rows)
            {
                ArticuloExtended articuloExtended = new ArticuloExtended();
                TextBox textBox = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[3].FindControl("txtPrecioCompra");
                TextBox textBox2 = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[4].FindControl("txtUtilidad");
                TextBox textBox3 = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[5].FindControl("txtPrecioVenta");
                TextBox obj = (TextBox)gvArticulos.Rows[row.RowIndex].Cells[6].FindControl("txtIva");
                string text = gvArticulos.Rows[row.RowIndex].Cells[0].Text;
                decimal precio_compra = decimal.Parse(textBox.Text.Trim());
                decimal utilidad = decimal.Parse(textBox2.Text.Trim());
                decimal precio_venta = decimal.Parse(textBox3.Text.Trim());
                decimal iva = decimal.Parse(obj.Text.Trim());
                articuloExtended.cod_barras = text;
                articuloExtended.precio_compra = precio_compra;
                articuloExtended.utilidad = utilidad;
                articuloExtended.precio_venta = precio_venta;
                articuloExtended.iva = iva;
                list.Add(articuloExtended);
            }
            articulos.suspenderfrmActualizarPrecio(list, addItems);
        }

        protected void btnSuspender_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvArticulos.Rows.Count == 0)
                {
                    throw new Exception("No hay artículos por actualizar su precio.");
                }
                if (new RepositorioArticulos().existsItemsSuspended())
                {
                    base.ClientScript.RegisterStartupScript(GetType(), "modal", "WhatDoIDoChangePrice()", addScriptTags: true);
                    return;
                }
                suspenderCambioPrecio(addItems: true);
                base.ClientScript.RegisterStartupScript(GetType(), "modal", string.Format("alert('{0}')", "Los cambios de precios se suspendieron correctamente."), addScriptTags: true);
                limpiarCampos();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnNewItems_Click(object sender, EventArgs e)
        {
            try
            {
                suspenderCambioPrecio(addItems: false);
                base.ClientScript.RegisterStartupScript(GetType(), "modal", string.Format("alert('{0}')", "Los cambios de precios se suspendieron correctamente."), addScriptTags: true);
                limpiarCampos();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnAddItems_Click(object sender, EventArgs e)
        {
            try
            {
                suspenderCambioPrecio(addItems: true);
                base.ClientScript.RegisterStartupScript(GetType(), "modal", string.Format("alert('{0}')", "Los cambios de precios se suspendieron correctamente."), addScriptTags: true);
                limpiarCampos();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            try
            { 
                for (int i = 0; i < gvArticulos.Rows.Count; i++)
                {
                    TextBox textBox = (TextBox)gvArticulos.Rows[i].Cells[5].FindControl("txtPrecioVenta");
                    if (new RepositorioArticulos().IsChangedPrice(gvArticulos.Rows[i].Cells[0].Text, decimal.Parse(textBox.Text)))
                    {
                        new ZebraPrinterController().printLblAnaquel(gvArticulos.Rows[i].Cells[0].Text, 1, normalPrice: false, decimal.Parse(textBox.Text));
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        private void enableControls()
        {
            bool enabled = true;
            btnActualizar.Enabled = enabled;
            btnSuspender.Enabled = enabled;
            btnImprimirEtiqueta.Enabled = enabled;
        }
    }
}