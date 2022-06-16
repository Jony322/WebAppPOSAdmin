using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

namespace WebAppPOSAdmin.Almacen
{
    public partial class VisorEntradas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["value"] = 0;
                inicializarFechas();
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        public bool inicializarFechas()
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
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void gvEntrada_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Guid guid = Guid.Parse(e.CommandArgument.ToString());
                string commandName = e.CommandName;
                if (commandName == "Reporte")
                {
                    Session["EntradaID"] = guid;
                    string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Entradas.aspx";
                    string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                consultarEntradas();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        public void consultarEntradas()
        {
            try
            {
                if (inicializarFechas())
                {
                    List<entrada> list = null;
                    list = ((txtCodBarras.Text.Trim() != "" && txtObservaciones.Text.Trim() != "") ? new RepositorioAlmacen().findEntradasBy(txtCodBarras.Text.Trim(), txtObservaciones.Text.Trim(), DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text)) : ((txtObservaciones.Text.Trim() != "") ? new RepositorioAlmacen().findEntradasBy(DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text), txtObservaciones.Text.Trim()) : ((!(txtCodBarras.Text.Trim() != "")) ? new RepositorioAlmacen().findEntradasBy(DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text)) : new RepositorioAlmacen().findEntradasBy(txtCodBarras.Text.Trim(), DateTime.Parse(txtFechaIni.Text), DateTime.Parse(txtFechaFin.Text)))));
                    if (list.Count == 0)
                    {
                        throw new Exception("No se encontraron Entradas");
                    }
                    gvEntrada.DataSource = list;
                    gvEntrada.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}