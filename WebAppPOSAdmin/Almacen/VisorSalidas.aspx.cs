using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using NLog;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Security.SeguridadSession;

namespace WebAppPOSAdmin.Almacen
{
    public partial class VisorSalidas : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private SessionFormularios formulario = new SessionFormularios();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["value"] = 0;
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        public void consultarSalidas()
        {
            try
            {
                if (validarFechas().Equals(obj: true))
                {
                    List<salida> list = null;
                    list = ((txtCodBarras.Text.Trim() != "" && txtObservaciones.Text.Trim() != "") ? new RepositorioAlmacen().findSalidasBy(txtCodBarras.Text.Trim(), txtObservaciones.Text.Trim(), DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text)) : ((txtObservaciones.Text.Trim() != "") ? new RepositorioAlmacen().findSalidasBy(DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text), txtObservaciones.Text.Trim()) : ((!(txtCodBarras.Text.Trim() != "")) ? new RepositorioAlmacen().findSalidasBy(DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text)) : new RepositorioAlmacen().findSalidasBy(txtCodBarras.Text.Trim(), DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text)))));
                    if (list.Count == 0)
                    {
                        throw new Exception("No se encontraron Salidas");
                    }
                    gvSalidas.DataSource = list;
                    gvSalidas.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorSalidas " + "Acción: consultarSalidas " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public bool validarFechas()
        {
            bool result = false;
            try
            {
                if (!txtFechaIni.Text.Equals("") && !txtFechaFin.Text.Equals(""))
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorSalidas " + "Acción: validarFechas " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
                return result;
            }
        }

        public void LimpiarCampos()
        {
            try
            {
                txtFechaFin.Text = string.Empty;
                txtFechaIni.Text = string.Empty;
                txtCodBarras.Text = string.Empty;
                Session["value"] = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorSalidas " + "Acción: LimpiarCampos " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvSalidas_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void gvSalidas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Guid guid = Guid.Parse(e.CommandArgument.ToString());
            string commandName = e.CommandName;
            if (commandName == "Reporte")
            {
                Session["SalidaID"] = guid;
                string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Salidas.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                consultarSalidas();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorSalidas " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}