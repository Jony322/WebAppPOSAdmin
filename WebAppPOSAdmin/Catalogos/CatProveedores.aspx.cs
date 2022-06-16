using System;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;

using WebAppPOSAdmin.DropDownListExtender;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatProveedores : System.Web.UI.Page
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

        private proveedor provider;

        private operations operation;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.Request.QueryString["id"] != null)
                {
                    provider = new Provedores().buscarProvedor(Guid.Parse(base.Request.QueryString["id"]));
                    if (provider != null)
                    {
                        operation = operations.UPDATE;
                    }
                }
                if (!IsPostBack)
                {
                    llenarDropDownList();
                    if (operation.Equals(operations.UPDATE))
                    {
                        llenarCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: Page_Load " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void getMunicipios()
        {
            try
            {
                ddlMunicipios.getMunicipios(int.Parse(ddlEntidades.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: getMunicipios " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void llenarCampos()
        {
            try
            {
                txtContacto.Text = provider.nombre_contacto;
                txtRazonSocial.Text = provider.razon_social;
                txtRFC.Text = provider.rfc;
                txtTelefono.Text = provider.tel_principal;
                txtEmail.Text = provider.e_mail;
                ddlStatus.Text = provider.estatus;
                txtPais.Text = provider.direccion_proveedor.pais;
                txtNumExterior.Text = provider.direccion_proveedor.no_ext;
                txtNumInterior.Text = provider.direccion_proveedor.no_int;
                txtCalle.Text = provider.direccion_proveedor.calle;
                txtCodPostal.Text = provider.direccion_proveedor.cod_postal;
                txtLocalidad.Text = provider.direccion_proveedor.localidad;
                txtColonia.Text = provider.direccion_proveedor.colonia;
                setItem(ref ddlEntidades, provider.direccion_proveedor.id_entidad ?? 0);
                getMunicipios();
                setItem(ref ddlMunicipios, provider.direccion_proveedor.id_municipio ?? 0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: llenarCampos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void llenarDropDownList()
        {
            ddlEntidades.getEntidades();
        }

        protected proveedor getDatos(bool isNew)
        {
            proveedor proveedor = new proveedor();
            proveedor.id_proveedor = (isNew ? Guid.NewGuid() : provider.id_proveedor);
            proveedor.nombre_contacto = txtContacto.Text.Trim();
            proveedor.razon_social = txtRazonSocial.Text.Trim();
            proveedor.rfc = txtRFC.Text.Trim();
            proveedor.tel_principal = txtTelefono.Text.Trim();
            proveedor.e_mail = txtEmail.Text.Trim();
            proveedor.estatus = ddlStatus.SelectedValue;
            proveedor.fecha_registro = DateTime.Now;
            proveedor.direccion_proveedor = new direccion_proveedor();
            proveedor.direccion_proveedor.calle = txtCalle.Text.Trim();
            proveedor.direccion_proveedor.cod_postal = txtCodPostal.Text.Trim();
            proveedor.direccion_proveedor.colonia = txtColonia.Text.Trim();
            proveedor.direccion_proveedor.pais = txtPais.Text.Trim();
            proveedor.direccion_proveedor.id_entidad = short.Parse(ddlEntidades.SelectedValue);
            proveedor.direccion_proveedor.id_municipio = short.Parse(ddlMunicipios.SelectedValue);
            proveedor.direccion_proveedor.localidad = txtLocalidad.Text.Trim();
            proveedor.direccion_proveedor.no_ext = txtNumExterior.Text.Trim();
            proveedor.direccion_proveedor.no_int = txtNumInterior.Text.Trim();
            return proveedor;
        }

        protected void setItem(ref DropDownList _control, int valor)
        {
            string text = Convert.ToString(valor);
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
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: setItem " + ex.Message);
                loggerdb.Error(ex);
                throw ex;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                switch (operation)
                {
                    case operations.CREATE:
                        new Provedores().create(getDatos(isNew: true));
                        base.Response.Redirect("~/Catalogos/CatProveedores.aspx", endResponse: false);
                        break;
                    case operations.UPDATE:
                        new Provedores().update(getDatos(isNew: false));
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlEntidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMunicipios.getMunicipios(long.Parse(ddlEntidades.SelectedValue.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: ddlEntidades_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void gvProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "update":
                        base.Response.Redirect($"~/Catalogos/CatProveedores.aspx?id={e.CommandArgument.ToString()}", endResponse: false);
                        break;
                    case "delete":
                        new Provedores().delete(long.Parse(e.CommandArgument.ToString()));
                        loadResultsFind();
                        break;
                    case "report":
                        base.Response.Redirect($"~/ReportesViews/CatReporteProveedor.aspx?id={e.CommandArgument.ToString()}", endResponse: false);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: gvProveedores_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('PROVEEDORES','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            loadResultsFind();
        }

        private void loadResultsFind()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                gvProveedores.DataSource = null;
                if (rbtAll.Checked)
                {
                    gvProveedores.DataSource = from p in dcContextoSuPlazaDataContext.proveedor
                                               orderby p.razon_social
                                               select new { p.id_proveedor, p.rfc, p.razon_social, p.nombre_contacto, p.tel_principal };
                }
                else if (rbtRazonSocial.Checked)
                {
                    gvProveedores.DataSource = from p in dcContextoSuPlazaDataContext.proveedor
                                               where SqlMethods.Like(p.razon_social, $"%{txtFind.Text.Trim()}%")
                                               orderby p.razon_social
                                               select new { p.id_proveedor, p.rfc, p.razon_social, p.nombre_contacto, p.tel_principal };
                }
                else if (rbtRFC.Checked)
                {
                    gvProveedores.DataSource = from p in dcContextoSuPlazaDataContext.proveedor
                                               where SqlMethods.Like(p.rfc, $"%{txtFind.Text.Trim()}%")
                                               orderby p.rfc
                                               select new { p.id_proveedor, p.rfc, p.razon_social, p.nombre_contacto, p.tel_principal };
                }
                gvProveedores.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatProveedores " + "Acción: loadResultsFind " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Artículo asociado','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }
    }
}