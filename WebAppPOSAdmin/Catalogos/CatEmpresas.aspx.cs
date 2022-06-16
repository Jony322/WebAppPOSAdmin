using System;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Util.Validadores;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatEmpresas : System.Web.UI.Page
    {
        #region
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    llenarCampos();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatEmpresas " + "Acción: Page_Load " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarCampos()
        {
            Empresas empresas = new Empresas();
            empresa empresa = new empresa();
            empresa = empresas.getEmpresaById();
            if (empresa != null)
            {
                txtrfc.Text = empresa.rfc;
                txtRazonSocial.Text = empresa.razon_social;
                txtRepresentante.Text = empresa.representante;
                txtEstado.Text = empresa.estado;
                txtMunicipio.Text = empresa.municipio;
                txtNuExterior.Text = empresa.no_ext;
                txtNuInteriro.Text = empresa.no_int;
                txtCalle.Text = empresa.calle;
                txtCodPostal.Text = empresa.codigo_postal;
                txtColonia.Text = empresa.colonia;
                txtEmail.Text = empresa.e_mail;
                txtTelefono.Text = empresa.tel_principal;
                txtPais.Text = empresa.pais;
            }
        }

        public empresa getDatosEmpresa()
        {
            return new empresa
            {
                rfc = txtrfc.Text.Trim(),
                razon_social = txtRazonSocial.Text.Trim(),
                representante = txtRepresentante.Text.Trim(),
                calle = txtCalle.Text.Trim(),
                municipio = txtMunicipio.Text.Trim(),
                no_ext = txtNuExterior.Text.Trim(),
                no_int = txtNuInteriro.Text.Trim(),
                tel_principal = txtTelefono.Text.Trim(),
                codigo_postal = txtCodPostal.Text.Trim(),
                colonia = txtColonia.Text.Trim(),
                estado = txtEstado.Text.Trim(),
                pais = txtPais.Text.Trim(),
                e_mail = txtEmail.Text.Trim(),
                fecha_registro = DateTime.Now
            };
        }

        public empresa getDatosEmpresa(byte[] imagen)
        {
            return new empresa
            {
                rfc = txtrfc.Text.Trim(),
                razon_social = txtRazonSocial.Text.Trim(),
                representante = txtRepresentante.Text.Trim(),
                calle = txtCalle.Text.Trim(),
                municipio = txtMunicipio.Text.Trim(),
                no_ext = txtNuExterior.Text.Trim(),
                no_int = txtNuInteriro.Text.Trim(),
                tel_principal = txtTelefono.Text.Trim(),
                codigo_postal = txtCodPostal.Text.Trim(),
                colonia = txtColonia.Text.Trim(),
                pais = txtPais.Text.Trim(),
                estado = txtEstado.Text.Trim(),
                logo = imagen,
                fecha_registro = DateTime.Now
            };
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Empresas empresas = new Empresas();
                string text = (new RepositorioEmpresa().existEnterprise() ? "actualizar" : "insertar");
                if (!(text == "insertar"))
                {
                    if (text == "actualizar")
                    {
                        if (FileUpload.HasFile)
                        {
                            if (new Validaciones().validarFormatoImagen(FileUpload.PostedFile.FileName).Equals(obj: true))
                            {
                                byte[] fileBytes = FileUpload.FileBytes;
                                empresas.actualizarEmpresa(getDatosEmpresa(fileBytes));
                            }
                            else
                            {
                                base.ClientScript.RegisterStartupScript(GetType(), "modal", "validacionImagenes();", addScriptTags: true);
                            }
                        }
                        else
                        {
                            empresas.actualizarEmpresa(getDatosEmpresa());
                        }
                    }
                }
                else if (FileUpload.HasFile)
                {
                    if (new Validaciones().validarFormatoImagen(FileUpload.PostedFile.FileName).Equals(obj: true))
                    {
                        byte[] fileBytes2 = FileUpload.FileBytes;
                        empresas.insertarEmpresa(getDatosEmpresa(fileBytes2));
                    }
                    else
                    {
                        base.ClientScript.RegisterStartupScript(GetType(), "modal", "validacionImagenes();", addScriptTags: true);
                    }
                }
                else
                {
                    empresas.insertarEmpresa(getDatosEmpresa());
                }
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "insercionCorrecta();", addScriptTags: true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatEmpresas " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "insercionErronea();", addScriptTags: true);
            }
        }
    }
}