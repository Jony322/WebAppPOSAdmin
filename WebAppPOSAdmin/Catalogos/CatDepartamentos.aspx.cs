using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;

using WebAppPOSAdmin.DropDownListExtender;

using WebAppPOSAdmin.Controles;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatDepartamentos : System.Web.UI.Page
    {
        #region
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            loadCategories();
            if (IsPostBack)
            {
                getDepartamentos();
            }
            btnDepartamento.Visible = true;
            btnCategoria.Visible = true;
            btnSubcategoria.Visible = true;
            btnLinea.Visible = true;
            btnEditarDepartamento.Visible = false;
            btnEditarCategoria.Visible = false;
            btnEditarSubcategoria.Visible = false;
            btnEditarLinea.Visible = false;
            lblId.Visible = false;
        }

        private void loadCategories()
        {
            StringBuilder stringBuilder = new StringBuilder("<script>var availableCategories = [");
            int num = 1;
            string[] categoriesList = getCategoriesList();
            foreach (string arg in categoriesList)
            {
                if (num++ > 1)
                {
                    stringBuilder.Append(",");
                }
                stringBuilder.Append($"\"{arg}\"");
            }
            stringBuilder.Append("] </script>");
            ltlCategoryList.Text = stringBuilder.ToString();
        }

        private string[] getCategoriesList()
        {
            return (from c in new dcContextoSuPlazaDataContext().clasificacion
                    select c.descripcion into c
                    orderby c
                    select c).ToArray();
        }

        public void getDepartamentos()
        {
            try
            {
                ddlDepartamento.GetDepartamentoByNivel(1);
                ddlDepartamento_SubCategoria.GetDepartamentoByNivel(1);
                ddlDepartamento_Linea.GetDepartamentoByNivel(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: getDepartamentos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void getClasificacion(ref DropDownList _control, int id_clasificacion_depto)
        {
            try
            {
                _control.GetClasificacionById(id_clasificacion_depto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: getClasificacion " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public clasificacion getDatosDepartamento()
        {
            clasificacion clasificacion = new clasificacion();
            if (lblId.Text != "")
            {
                clasificacion.id_clasificacion = int.Parse(lblId.Text);
            }
            clasificacion.descripcion = txtDepartamento.Text.Trim();
            clasificacion.nivel_clasificacion = 1;
            return clasificacion;
        }

        public clasificacion getDatosClasificacion()
        {
            clasificacion clasificacion = new clasificacion();
            if (lblId.Text != "")
            {
                clasificacion.id_clasificacion = int.Parse(lblId.Text);
            }
            clasificacion.id_clasificacion_dep = int.Parse(ddlDepartamento.SelectedValue);
            clasificacion.descripcion = txtCategoria.Text.Trim();
            clasificacion.nivel_clasificacion = 2;
            return clasificacion;
        }

        public clasificacion getDatosSubCategoria()
        {
            clasificacion clasificacion = new clasificacion();
            if (lblId.Text != "")
            {
                clasificacion.id_clasificacion = int.Parse(lblId.Text);
            }
            clasificacion.id_clasificacion_dep = int.Parse(ddlCategoria_SubCategoria.SelectedValue);
            clasificacion.descripcion = txtSubcategoria.Text.Trim();
            clasificacion.nivel_clasificacion = 3;
            return clasificacion;
        }

        public clasificacion getDatosLinea()
        {
            clasificacion clasificacion = new clasificacion();
            if (lblId.Text != "")
            {
                clasificacion.id_clasificacion = int.Parse(lblId.Text);
            }
            clasificacion.id_clasificacion_dep = int.Parse(ddlSubcategoria_linea.SelectedValue);
            clasificacion.descripcion = txtLinea.Text.Trim();
            clasificacion.nivel_clasificacion = 4;
            return clasificacion;
        }

        protected void btnDepartamento_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().insertarClasificacion(getDatosDepartamento());
                getDepartamentos();
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "insercionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnDepartamento_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        public void LimpiarCampos()
        {
            txtDepartamento.Text = string.Empty;
            txtCategoria.Text = string.Empty;
            txtSubcategoria.Text = string.Empty;
            txtLinea.Text = string.Empty;
        }

        public void llenarDatosConsulta(clasificacion Clasificacion)
        {
            int nivel_clasificacion = Clasificacion.nivel_clasificacion;
            lblMensaje.Text = nivel_clasificacion.ToString();
            switch (nivel_clasificacion)
            {
                case 1:
                    txtDepartamento.Text = Clasificacion.descripcion;
                    lblId.Text = Clasificacion.id_clasificacion.ToString();
                    btnDepartamento.Visible = false;
                    btnEditarDepartamento.Visible = true;
                    break;
                case 2:
                    txtCategoria.Text = Clasificacion.descripcion;
                    lblId.Text = Clasificacion.id_clasificacion.ToString();
                    setItem(ref ddlDepartamento, Clasificacion.id_clasificacion_dep.Value);
                    btnCategoria.Visible = false;
                    btnEditarCategoria.Visible = true;
                    break;
                case 3:
                    {
                        txtSubcategoria.Text = Clasificacion.descripcion;
                        lblId.Text = Clasificacion.id_clasificacion.ToString();
                        btnSubcategoria.Visible = false;
                        btnEditarSubcategoria.Visible = true;
                        clasificacion clasificacion4 = llenarDepartamentos(Clasificacion.id_clasificacion_dep.Value);
                        clasificacion clasificacion5 = llenarDepartamentos(clasificacion4.id_clasificacion_dep.Value);
                        setItem(ref ddlDepartamento_SubCategoria, clasificacion5.id_clasificacion);
                        getClasificacion(ref ddlCategoria_SubCategoria, (int)clasificacion5.id_clasificacion);
                        setItem(ref ddlCategoria_SubCategoria, clasificacion4.id_clasificacion);
                        break;
                    }
                case 4:
                    {
                        txtLinea.Text = Clasificacion.descripcion;
                        lblId.Text = Clasificacion.id_clasificacion.ToString();
                        btnEditarLinea.Visible = true;
                        btnLinea.Visible = false;
                        clasificacion clasificacion = llenarDepartamentos(Clasificacion.id_clasificacion_dep.Value);
                        clasificacion clasificacion2 = llenarDepartamentos(clasificacion.id_clasificacion_dep.Value);
                        clasificacion clasificacion3 = llenarDepartamentos(clasificacion2.id_clasificacion_dep.Value);
                        setItem(ref ddlDepartamento_Linea, clasificacion3.id_clasificacion);
                        getClasificacion(ref ddlCategoria_Linea, (int)clasificacion3.id_clasificacion);
                        setItem(ref ddlCategoria_Linea, clasificacion2.id_clasificacion);
                        getClasificacion(ref ddlSubcategoria_linea, (int)clasificacion2.id_clasificacion);
                        setItem(ref ddlSubcategoria_linea, clasificacion.id_clasificacion);
                        break;
                    }
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
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: llenarDepartamentos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
                return null;
            }
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
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: setItem " + ex.Message);
                loggerdb.Error(ex);
                throw ex;
            }
        }

        protected void ddlDepartamento_SubCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getClasificacion(ref ddlCategoria_SubCategoria, Convert.ToInt32(ddlDepartamento_SubCategoria.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: ddlDepartamento_SubCategoria_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlDepartamento_Linea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getClasificacion(ref ddlCategoria_Linea, Convert.ToInt32(ddlDepartamento_Linea.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: ddlDepartamento_Linea_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlCategoria_Linea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getClasificacion(ref ddlSubcategoria_linea, Convert.ToInt32(ddlCategoria_Linea.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: ddlCategoria_Linea_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().insertarClasificacion(getDatosClasificacion());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "insercionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnCategoria_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnSubcategoria_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().insertarClasificacion(getDatosSubCategoria());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "insercionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnSubcategoria_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnLinea_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().insertarClasificacion(getDatosLinea());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "insercionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnLinea_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Clasificacion clasificacion = new Clasificacion();
                clasificacion clasificacion2 = new clasificacion();
                clasificacion2 = clasificacion.getClasificacioByDescripcion(txtBuscador.Text.Trim());
                if (clasificacion2 != null)
                {
                    llenarDatosConsulta(clasificacion2);
                    return;
                }
                base.ClientScript.RegisterStartupScript(GetType(), "modal", " MyMessageBox('No se encontraron resultados', 'Información');", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnBuscar_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnEditarDepartamento_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().actualizarDepartamento(getDatosDepartamento());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "actualizacionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnEditarDepartamento_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnEditarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().actualizarDepartamento(getDatosClasificacion());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "actualizacionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnEditarCategoria_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnEditarSubcategoria_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().actualizarDepartamento(getDatosSubCategoria());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "actualizacionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnEditarSubcategoria_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnEditarLinea_Click(object sender, EventArgs e)
        {
            try
            {
                new Clasificacion().actualizarDepartamento(getDatosLinea());
                base.ClientScript.RegisterStartupScript(GetType(), "modal", "actualizacionCorrecta();", addScriptTags: true);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnEditarLinea_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                base.Response.Redirect("~/ReportesViews/CatReporteDepartamentos.aspx", endResponse: false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatDepartamentos " + "Acción: btnReporte_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }
    }
}