using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Scripts;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Cajas
{
    public partial class frmEstadisticaArticulos : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool validarFechas()
        {
            try
            {
                if (!txtFechaIni.Text.Equals("") && !txtHoraFin.Text.Equals(""))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: validarFechas " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
                return false;
            }
        }

        public void cargaDropInicial()
        {
            try
            {
                ddlProveedores.getProveedores();
                ddlDepartamento.GetClasificacionDepartamentos();
                ddlAnio.setYear();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: cargaDropInicial " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarDropCategoria()
        {
            try
            {
                ddlCategoria.GetPrimeraLiniaCategoria(Convert.ToInt32(ddlDepartamento.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: llenarDropCategoria " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarDropSubCategoria()
        {
            try
            {
                ddlSubCategoria.GetSegundaLiniaCategoria(Convert.ToInt32(ddlCategoria.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: llenarDropSubCategoria " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarDropLinea()
        {
            try
            {
                ddlLinea.GetTerceraLiniaCategoria(Convert.ToInt32(ddlSubCategoria.SelectedValue));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: llenarDropLinea " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                llenarDropSubCategoria();
                ddlSubCategoria.ClearSelection();
                ddlLinea.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: ddlCategoria_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlSubCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                llenarDropLinea();
                ddlLinea.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: ddlSubCategoria_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                llenarDropCategoria();
                ddlCategoria.ClearSelection();
                ddlSubCategoria.ClearSelection();
                ddlLinea.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: ddlDepartamento_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                validarFechas().Equals(obj: true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnShowStaticAcum_Click(object sender, EventArgs e)
        {
            try
            {
                gvEstadisticaDiaria.DataSource = null;
                gvEstadisticaDiaria.DataBind();
                List<Statistics> statistics = new Procedures().GetStatistics(txtFechaIni.Text + " " + txtHoraIni.Text, txtFechaFin.Text + " " + txtHoraFin.Text, (!ddlProveedores.SelectedValue.Equals("00000000-0000-0000-0000-000000000000")) ? ddlProveedores.SelectedValue : null, (ddlLinea.SelectedIndex != 0) ? int.Parse(ddlLinea.SelectedValue) : 0, txtCodBarras.Text.Trim(), OrderBy.SelectedValue, txtFiltrarDescripcion.Text.Trim());
                GridView gridView = gvEstadistica;
                IEnumerable<Statistics> dataSource;
                if (short.Parse(txtTop.Text.Trim()) <= 0)
                {
                    IEnumerable<Statistics> enumerable = statistics;
                    dataSource = enumerable;
                }
                else
                {
                    dataSource = statistics.Take(short.Parse(txtTop.Text.Trim()));
                }
                gridView.DataSource = dataSource;
                gvEstadistica.DataBind();
                btnPrintReport.Enabled = gvEstadistica.Rows.Count > 0;
                Session["reporte"] = "acumulada";
                Session["dateIni"] = txtFechaIni.Text + " " + txtHoraIni.Text;
                Session["dateEnd"] = txtFechaFin.Text + " " + txtHoraFin.Text;
                Session["statistics"] = statistics;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: btnShowStaticAcum_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}')", addScriptTags: true);
            }
        }

        protected void btnEstadisticaMes_Click(object sender, EventArgs e)
        {
            try
            {
                gvEstadisticaDiaria.DataSource = null;
                gvEstadisticaDiaria.DataBind();
                if (txtBarCodeMes.Text.Trim().Length == 0)
                {
                    throw new Exception("Debe ingresar un código de barras");
                }
                List<Statistics> statistics = new Procedures().GetStatistics(int.Parse(ddlMes.SelectedValue), int.Parse(ddlAnio.SelectedValue), txtBarCodeMes.Text.Trim());
                gvEstadistica.DataSource = statistics;
                gvEstadistica.DataBind();
                btnPrintReport.Enabled = gvEstadistica.Rows.Count > 0;
                Session["reporte"] = "mes";
                Session["month"] = ddlMes.SelectedItem.Text;
                Session["year"] = ddlAnio.SelectedValue;
                Session["statistics"] = statistics;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: btnEstadisticaMes_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}')", addScriptTags: true);
            }
        }

        protected void btnEstadisticaDiaria_Click(object sender, EventArgs e)
        {
            try
            {
                gvEstadistica.DataSource = null;
                gvEstadistica.DataBind();
                if (txtBarCode2.Text.Trim().Length == 0)
                {
                    throw new Exception("Debe ingresar un código de barras");
                }
                List<Statistics> statistics = new Procedures().GetStatistics(txtFechaIni2.Text + " " + txtHoraIni2.Text, txtFechaFin2.Text + " " + txtHoraFin2.Text, txtBarCode2.Text.Trim());
                gvEstadisticaDiaria.DataSource = statistics;
                gvEstadisticaDiaria.DataBind();
                btnPrintReport.Enabled = gvEstadisticaDiaria.Rows.Count > 0;
                Session["reporte"] = "dia";
                Session["dateIni"] = txtFechaIni2.Text + " " + txtHoraIni2.Text;
                Session["dateEnd"] = txtFechaFin2.Text + " " + txtHoraFin2.Text;
                Session["statistics"] = statistics;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: btnEstadisticaDiaria_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}')", addScriptTags: true);
            }
        }

        protected void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Estadistica.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmEstadisticasArticulos " + "Acción: btnPrintReport_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }
    }
}