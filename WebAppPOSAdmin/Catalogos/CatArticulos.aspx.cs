using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.DropDownListExtender;
using WebAppPOSAdmin.Recursos;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatArticulos : System.Web.UI.Page
    {

        #region
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private enum operations
        {
            CREATE,
            UPDATE
        }

        private articulo item;

        private string barCode;

        private operations operation;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.Request.QueryString["barCode"] != null)
                {
                    item = new ArticuloCtrl().getArticulo(base.Request.QueryString["barCode"].ToString());
                    if (item != null)
                    {
                        Session["status"] = "update";
                        operation = operations.UPDATE;
                        barCode = base.Request.QueryString["barCode"];
                    }
                }
                else
                {
                    Session["barCode"] = null;
                }
                if (!IsPostBack)
                {
                    getDropDownList();
                    iniciarTextbox();
                    if (operation.Equals(operations.UPDATE))
                    {
                        llenarCampos(barCode);
                    }
                }
                enableControls(operation.Equals(operations.UPDATE));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos "+"Acción: Load " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Artículo asociado','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        private void enableControls(bool status)
        {
            txtCodBarrasAnexo.Enabled = status;
            txtCodInternoAnexo.Enabled = status;
            txtDescripcionAnexo.Enabled = status;
            txtDescripcionCortaAnexo.Enabled = status;
            ddlUnidadAnexo.Enabled = status;
            txtPiezasAnexo.Enabled = status;
            txtCostoAnexo.Enabled = status;
            txtUtilidadAnexo.Enabled = status;
            txtPrecioVentaAnexo.Enabled = status;
            btnAnexar.Enabled = status;
            txtCodAsociado.Enabled = status;
            btnAsociar.Enabled = status;
        }

        public void loadDetailAnexos()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                gvDetailAnexos.DataSource = from a in dcContextoSuPlazaDataContext.articulo
                                            where a.cod_asociado.Equals(item.cod_barras) && a.tipo_articulo.Equals("anexo")
                                            select new
                                            {
                                                cod_barras = a.cod_barras,
                                                descripcion = a.descripcion,
                                                um = a.unidad_medida.descripcion,
                                                cantidad_um = a.cantidad_um,
                                                precio_compra = a.precio_compra,
                                                utilidad = a.utilidad,
                                                precio_venta = a.precio_venta
                                            };
                gvDetailAnexos.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: loadDetailAnexos: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Artículo asociado','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        public void loadDetailAsociados()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                gvAsociados.DataSource = from a in dcContextoSuPlazaDataContext.articulo
                                         where a.cod_asociado.Equals(item.cod_barras) && a.tipo_articulo.Equals("asociado")
                                         select new { a.cod_barras };
                gvAsociados.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: loadDetailAsociados: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Artículo asociado','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        public void llenarCampos(object cod_barras)
        {
            try
            {
                txtCodBarras.Text = item.cod_barras;
                txtCodInterno.Text = item.cod_interno;
                txtDescripcion.Text = item.descripcion;
                txtDescripcionCorta.Text = item.descripcion_corta;
                txtStock.Text = item.stock.ToString();
                txtStockMaximo.Text = item.stock_max.ToString();
                txtStockMinimo.Text = item.stock_min.ToString();
                txtCosto.Text = item.precio_compra.ToString();
                txtUtilidad.Text = item.utilidad.ToString();
                txtPiezas.Text = item.cantidad_um.ToString();
                chkIVA.Checked = item.iva > 0.0m;
                txtIva.Text = (item.iva * 100.0m).ToString("F3");
                txtPrecioVenta.Text = item.precio_venta.ToString();
                txtCveProducto.Text = item.cve_producto;
                clasificacion clasificacion = llenarDepartamentos(item.id_clasificacion);
                clasificacion clasificacion2 = llenarDepartamentos(clasificacion.id_clasificacion_dep.Value);
                clasificacion clasificacion3 = llenarDepartamentos(clasificacion2.id_clasificacion_dep.Value);
                clasificacion clasificacion4 = llenarDepartamentos(clasificacion3.id_clasificacion_dep.Value);
                ddlProveedor.SelectedValue = item.id_proveedor.ToString();
                setItem(ref ddlDepartamento, clasificacion4.id_clasificacion);
                getLiniaUno(Convert.ToInt32(ddlDepartamento.SelectedValue));
                setItem(ref ddlPrimeraLinia, clasificacion3.id_clasificacion);
                getLiniaDos(Convert.ToInt32(ddlPrimeraLinia.SelectedValue));
                setItem(ref ddlSegundaLinia, clasificacion2.id_clasificacion);
                getLiniaTres(Convert.ToInt32(ddlSegundaLinia.SelectedValue));
                setItem(ref ddlTerceraLinia, clasificacion.id_clasificacion);
                ddlUnidad.SelectedValue = item.id_unidad.ToString();
                loadDetailAnexos();
                loadDetailAsociados();
                Session["barCode"] = item.cod_barras;
                Session["internalCode"] = item.cod_interno;
                txtDescripcion.Focus();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: llenarCampos: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{ex.Message}\");", addScriptTags: true);
            }
        }

        public clasificacion llenarDepartamentos(long id_clasificacion)
        {
            try
            {
                return new Clasificacion().Departamentos(id_clasificacion);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: llenarDepartamentos: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{ex.Message}\");", addScriptTags: true);
                return null;
            }
        }

        public void getDropDownList()
        {
            ddlProveedor.getProveedores();
            ddlDepartamento.GetClasificacionDepartamentos();
            ddlUnidad.getUnidad();
            ddlUnidadAnexo.getUnidad();
        }

        public articulo getArticuloPrincipal()
        {
            return new articulo
            {
                tipo_articulo = Resourses.Principal,
                cod_barras = txtCodBarras.Text.Trim(),
                cod_interno = ((txtCodInterno.Text.Trim().Length > 0) ? txtCodInterno.Text.Trim() : null),
                descripcion = ((txtDescripcion.Text.Trim().Length > 0) ? txtDescripcion.Text.Trim() : null),
                descripcion_corta = ((txtDescripcionCorta.Text.Trim().Length > 0) ? txtDescripcionCorta.Text.Trim() : null),
                stock = decimal.Parse(txtStock.Text.Trim()),
                stock_max = decimal.Parse(txtStockMaximo.Text.Trim()),
                stock_min = decimal.Parse(txtStockMinimo.Text.Trim()),
                precio_venta = decimal.Parse(txtPrecioVenta.Text.Trim()),
                utilidad = decimal.Parse(txtUtilidad.Text.Trim()),
                id_proveedor = Guid.Parse(ddlProveedor.SelectedValue),
                id_clasificacion = int.Parse(ddlTerceraLinia.SelectedValue),
                id_unidad = Guid.Parse(ddlUnidad.SelectedValue),
                precio_compra = decimal.Parse(txtCosto.Text.Trim()),
                iva = decimal.Parse(txtIva.Text.Trim()) / 100.0m,
                articulo_disponible = true,
                cantidad_um = decimal.Parse(txtPiezas.Text.Trim()),
                kit = false,
                cve_producto = txtCveProducto.Text
            };
        }

        public articulo getArticuloAnexo()
        {
            return new articulo
            {
                cod_barras = ((txtCodBarrasAnexo.Text.Trim().Length > 0) ? txtCodBarrasAnexo.Text.Trim() : null),
                cod_interno = ((txtCodInternoAnexo.Text.Trim().Length > 0) ? txtCodInternoAnexo.Text.Trim() : null),
                cod_asociado = item.cod_barras,
                cantidad_um = decimal.Parse(txtPiezasAnexo.Text),
                id_unidad = Guid.Parse(ddlUnidadAnexo.SelectedValue.ToString()),
                id_clasificacion = item.id_clasificacion,
                id_proveedor = item.id_proveedor,
                precio_compra = decimal.Parse(txtCostoAnexo.Text.Trim()),
                precio_venta = decimal.Parse(txtPrecioVentaAnexo.Text.Trim()),
                utilidad = decimal.Parse(txtUtilidadAnexo.Text.Trim()),
                iva = item.iva,
                descripcion_corta = ((txtDescripcionCortaAnexo.Text.Trim().Length > 0) ? txtDescripcionCortaAnexo.Text.Trim() : null),
                descripcion = ((txtDescripcionAnexo.Text.Trim().Length > 0) ? txtDescripcionAnexo.Text.Trim() : null),
                tipo_articulo = Resourses.Anexo,
                articulo_disponible = true,
                fecha_registro = DateTime.Now,
                kit = false,
                cve_producto = txtCveProducto.Text
            };
        }

        public void setItem(ref DropDownList _control, long value)
        {
            string text = Convert.ToString(value);
            try
            {
                foreach (ListItem item in _control.Items)
                {
                    if (item.Value == text)
                    {
                        _control.SelectedValue = item.Value.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: setItem: " + ex.Message);
                loggerdb.Error(ex);
                throw ex;
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getLiniaUno(Convert.ToInt32(ddlDepartamento.SelectedValue));
                ddlSegundaLinia.Items.Clear();
                ddlTerceraLinia.Items.Clear();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: ddlDepartamento: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void ddlPrimeraLinia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getLiniaDos(Convert.ToInt32(ddlPrimeraLinia.SelectedValue));
                ddlTerceraLinia.Items.Clear();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: ddlPrimeraLinia: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void ddlSegundaLinia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getLiniaTres(Convert.ToInt32(ddlSegundaLinia.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: ddlSegundaLinia: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void getLiniaUno(int id_clasificacion)
        {
            try
            {
                ddlPrimeraLinia.GetPrimeraLiniaCategoria(id_clasificacion);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: getLiniaUno: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void getLiniaDos(int id_clasificacion)
        {
            try
            {
                ddlSegundaLinia.GetSegundaLiniaCategoria(id_clasificacion);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: getLiniaDos: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void getLiniaTres(int id_clasificacion)
        {
            try
            {
                ddlTerceraLinia.GetTerceraLiniaCategoria(id_clasificacion);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: getLiniaTres: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void iniciarTextbox()
        {
            try
            {
                txtStockMaximo.Text = "0";
                txtStockMinimo.Text = "0";
                txtCosto.Text = "0";
                txtIva.Text = "0";
                txtUtilidad.Text = "0";
                txtPrecioVenta.Text = "0";
                txtPrecioVentaAnexo.Text = "0";
                txtCostoAnexo.Text = "0";
                txtUtilidadAnexo.Text = "0";
                txtCveProducto.Text = "";
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: iniciarTextbox: " + ex.Message);
                loggerdb.Error(ex);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioArticulos().saveFirstItem(getArticuloPrincipal());
                Session["barCode"] = null;
                Session["internalCode"] = null;
                base.Response.Redirect($"~/Catalogos/CatArticulos.aspx?barCode={txtCodBarras.Text.Trim()}", endResponse: false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: btnGuardar: " + ex.Message);
                loggerdb.Error(ex);
                new RepositorioCtrlErrores().InsertError(ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", string.Format("alert('{0}');", ex.Message.Replace("'", "").Replace("\"", "")), addScriptTags: true);
            }
        }

        protected void btnAnexar_Click(object sender, EventArgs e)
        {
            try
            {
                new RepositorioArticulos().saveFirstItem(getArticuloAnexo());
                loadDetailAnexos();
                limpiarAnexos();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: btnAnexar: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{ex.Message}\");", addScriptTags: true);
            }
        }

        protected void btnAsociar_Click(object sender, EventArgs e)
        {
            try
            {
                new ArticuloCtrl().createAsociado(getArticuloAsociado());
                loadDetailAsociados();
                txtCodAsociado.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: btnAsociar: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{ex.Message}\");", addScriptTags: true);
            }
        }

        public articulo getArticuloAsociado()
        {
            return new articulo
            {
                cod_barras = txtCodAsociado.Text.Trim(),
                cod_asociado = item.cod_barras,
                tipo_articulo = Resourses.Asociado,
                descripcion = item.descripcion,
                descripcion_corta = item.descripcion_corta,
                stock_max = item.stock_max,
                stock_min = item.stock_min,
                precio_venta = item.precio_venta,
                utilidad = item.utilidad,
                id_clasificacion = item.id_clasificacion,
                id_proveedor = item.id_proveedor,
                id_unidad = item.id_unidad,
                precio_compra = item.precio_compra,
                iva = item.iva,
                articulo_disponible = item.articulo_disponible,
                cantidad_um = item.cantidad_um,
                cve_producto = item.cve_producto
            };
        }

        protected void gvAsociados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (!(commandName == "convert"))
                {
                    if (commandName == "delete")
                    {
                        new ArticuloCtrl().deleteItemAsociado(e.CommandArgument.ToString());
                        loadDetailAsociados();
                    }
                }
                else
                {
                    new RepositorioArticulos().fromAsociadoToPrincipal(e.CommandArgument.ToString());
                    base.Response.Redirect($"~/Catalogos/CatArticulos.aspx?barCode={e.CommandArgument.ToString()}", endResponse: false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: gvAsociados_RowCommand: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{ex.Message}\");", addScriptTags: true);
            }
        }

        public void limpiarPrincipal()
        {
            txtCodBarras.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtDescripcionCorta.Text = string.Empty;
            txtCodInterno.Text = string.Empty;
            txtCosto.Text = "0.000";
            txtPrecioVenta.Text = "0.000";
            txtPiezas.Text = "1";
            txtUtilidad.Text = "0.000";
            txtStock.Text = "0";
            txtStockMinimo.Text = "0";
            txtStockMaximo.Text = "0";
            ddlUnidad.ClearSelection();
            txtCveProducto.Text = "";
            gvDetailAnexos.DataSource = null;
            gvDetailAnexos.DataBind();
            gvAsociados.DataSource = null;
            gvAsociados.DataBind();
        }

        public void limpiarAnexos()
        {
            txtCodBarrasAnexo.Text = string.Empty;
            txtDescripcionAnexo.Text = string.Empty;
            txtDescripcionCortaAnexo.Text = string.Empty;
            txtCodInternoAnexo.Text = string.Empty;
            txtCostoAnexo.Text = "0.000";
            txtPrecioVentaAnexo.Text = "0.000";
            txtPiezasAnexo.Text = "1";
            txtUtilidadAnexo.Text = "0.000";
            ddlUnidadAnexo.ClearSelection();
        }

        protected void gvDetailAnexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                articulo articulo = new dcContextoSuPlazaDataContext().articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(e.CommandArgument.ToString()));
                if (articulo == null)
                {
                    return;
                }
                string commandName = e.CommandName;
                if (!(commandName == "update"))
                {
                    if (commandName == "delete")
                    {
                        new ArticuloCtrl().deleteItemAnexo(articulo.cod_barras);
                        loadDetailAnexos();
                    }
                    return;
                }
                txtCodBarrasAnexo.Text = articulo.cod_barras;
                txtDescripcionAnexo.Text = articulo.descripcion;
                txtDescripcionCortaAnexo.Text = articulo.descripcion_corta;
                txtCodInternoAnexo.Text = articulo.cod_interno;
                txtCostoAnexo.Text = articulo.precio_compra.ToString("F3");
                txtPrecioVentaAnexo.Text = articulo.precio_venta.ToString("F3");
                txtPiezasAnexo.Text = articulo.cantidad_um.ToString("F3");
                txtUtilidadAnexo.Text = articulo.utilidad.ToString("F3");
                ddlUnidadAnexo.SelectedValue = articulo.id_unidad.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatArticulos " + "Acción: gvDetailAnexos: " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{ex.Message}\");", addScriptTags: true);
            }
        }
    }
}