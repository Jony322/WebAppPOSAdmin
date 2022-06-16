using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Clases;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatKitArticulo : System.Web.UI.Page
    {

        #region  logger
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
            Session["status"] = "create";
            if (base.Request.QueryString["barCode"] != null && new ArticuloCtrl().existArticuloKit(base.Request.QueryString["barCode"]))
            {
                Session["status"] = "update";
                operation = operations.UPDATE;
                barCode = base.Request.QueryString["barCode"];
                item = new dcContextoSuPlazaDataContext().articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(barCode));
            }
            if (!IsPostBack)
            {
                txtFecha_ini.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFecha_fin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                getDropDownList();
                if (operation.Equals(operations.UPDATE))
                {
                    fillFields(barCode);
                }
            }
            enableControlsKit(operation.Equals(operations.UPDATE));
        }

        private void fillFields(string barCode)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                articulo articulo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo i) => i.cod_barras.Equals(barCode));
                if (item == null)
                {
                    throw new Exception("El artículo no existe.");
                }
                txtCodBarras.Text = articulo.cod_barras;
                txtCodInterno.Text = articulo.cod_interno;
                txtDescripcion.Text = articulo.descripcion;
                txtDescripcioCorta.Text = articulo.descripcion_corta;
                txtPiezas.Text = articulo.cantidad_um.ToString();
                ddlUnidad.SelectedValue = articulo.id_unidad.ToString();
                txtCosto.Text = articulo.precio_compra.ToString("F3");
                txtUtilidad.Text = articulo.utilidad.ToString("F3");
                txtPrecioVenta.Text = articulo.precio_venta.ToString("F3");
                chkIVA.Checked = articulo.iva > 0.0m;
                txtIva.Text = (articulo.iva * 100.0m).ToString("F3");
                txtFecha_ini.Text = (articulo.kit_fecha_ini ?? DateTime.Now).ToString("dd/MM/yyyy");
                txtFecha_fin.Text = (articulo.kit_fecha_fin ?? DateTime.Now).ToString("dd/MM/yyyy");
                chkDisponible.Checked = articulo.articulo_disponible;
                articulo.articulo_disponible = true;
                loadKitDetail();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatKitArticulos " + "Acción: fillFields " + ex.Message);
                loggerdb.Error(ex);
                string arg = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('ERROR: {arg}');", addScriptTags: true);
            }
        }

        private void loadKitDetail()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            var queryable = from k in dcContextoSuPlazaDataContext.kit_articulos
                            where k.cod_barras_kit.Equals(item.cod_barras)
                            select new
                            {
                                cod_barras = k.cod_barras_pro,
                                descripcion = k.articulo1.descripcion,
                                pco = k.articulo1.precio_compra,
                                precio_venta = k.articulo1.precio_venta,
                                cantidad = k.cantidad
                            };
            decimal num = 0.0m;
            foreach (var item in queryable)
            {
                num += item.pco * item.cantidad;
            }
            gvKitDetail.DataSource = queryable;
            gvKitDetail.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"$('#MainContent_txtCosto').val(({num}).toFixed(3)); calcularPrecioVentaArticulo();", addScriptTags: true);
        }

        private void enableControlsKit(bool status)
        {
            txtCosto.Enabled = status;
            txtUtilidad.Enabled = status;
            txtPrecioVenta.Enabled = status;
            txtIva.Enabled = status;
            txtBarCodeToKit.Enabled = status;
            txtQuantityToKit.Enabled = status;
            btnAddItemToKit.Enabled = status;
        }

        private void getDropDownList()
        {
            ddlUnidad.getUnidad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                articulo articulo = new articulo();
                articulo.cod_barras = txtCodBarras.Text.Trim();
                articulo.cod_interno = ((txtCodInterno.Text.Trim().Length > 0) ? txtCodInterno.Text.Trim() : null);
                articulo.descripcion = txtDescripcion.Text.Trim();
                articulo.descripcion_corta = txtDescripcioCorta.Text.Trim();
                articulo.cantidad_um = decimal.Parse(txtPiezas.Text);
                articulo.id_unidad = Guid.Parse(ddlUnidad.SelectedValue.ToString());
                articulo.precio_compra = decimal.Parse(txtCosto.Text.Trim());
                articulo.utilidad = decimal.Parse(txtUtilidad.Text.Trim());
                articulo.precio_venta = decimal.Parse(txtPrecioVenta.Text.Trim());
                articulo.tipo_articulo = "principal";
                articulo.cve_producto = "0";
                articulo.iva = decimal.Parse(txtIva.Text) / 100.0m;
                articulo.kit_fecha_ini = Convert.ToDateTime(txtFecha_ini.Text);
                articulo.kit_fecha_fin = Convert.ToDateTime(txtFecha_fin.Text);
                articulo.articulo_disponible = chkDisponible.Checked;
                articulo.kit = true;
                switch (operation)
                {
                    case operations.CREATE:
                        new RepositorioArticulos().insertarArticulo(articulo);
                        break;
                    case operations.UPDATE:
                        new RepositorioArticulos().save(articulo);
                        break;
                }
                base.Response.Redirect($"~/Catalogos/CatKitArticulo.aspx?barCode={articulo.cod_barras}", endResponse: false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatKitArticulos " + "Acción: btnSave_click " + ex.Message);
                loggerdb.Error(ex);
                string arg = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('ERROR: {arg}');", addScriptTags: true);
            }
        }

        protected void btnAddItemToKit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBarCodeToKit.Text.Trim().Equals("") || decimal.Parse(txtQuantityToKit.Text) == 0m)
                {
                    throw new Exception("Favor de ingresar un código de barras existente y la cantidad");
                }
                kit_articulos kit_articulos = new kit_articulos();
                kit_articulos.cod_barras_kit = item.cod_barras;
                kit_articulos.cod_barras_pro = txtBarCodeToKit.Text.Trim();
                kit_articulos.cantidad = decimal.Parse(txtQuantityToKit.Text);
                new ArticuloCtrl().saveItemToKit(kit_articulos);
                loadKitDetail();
                txtBarCodeToKit.Text = "";
                txtQuantityToKit.Text = "1";
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "MyMessageBox('KIT de articulos','Registro almacenado correctamente')", addScriptTags: true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatKitArticulos " + "Acción: btnAddItemToKit_Click " + ex.Message);
                loggerdb.Error(ex);
                string arg = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('ERROR: {arg}');", addScriptTags: true);
            }
        }

        protected void gvKitDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;
            if (!(commandName == "update"))
            {
                if (commandName == "delete")
                {
                    new ArticuloCtrl().deleteItemToKit(item.cod_barras, e.CommandArgument.ToString());
                    loadKitDetail();
                }
            }
            else
            {
                kit_articulos kit_articulos = new dcContextoSuPlazaDataContext().kit_articulos.FirstOrDefault((kit_articulos k) => k.cod_barras_kit.Equals(item.cod_barras) && k.cod_barras_pro.Equals(e.CommandArgument.ToString()));
                txtBarCodeToKit.Text = kit_articulos.cod_barras_pro;
                txtQuantityToKit.Text = kit_articulos.cantidad.ToString();
            }
        }
    }
}