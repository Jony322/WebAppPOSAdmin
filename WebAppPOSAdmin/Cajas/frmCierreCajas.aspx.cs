using System;
using System.Linq;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Entidad;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Cajas
{
    public partial class frmCierreCajas : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                cargarDropInicial();
            }
        }

        public void cargarDropInicial()
        {
            try
            {
                ddlCaja.getListPOS();
                ddlCajeros.getListaCajeros();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmCierreCajas " + "Acción: cargarDropInicial " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ddlCajeros.SelectedValue.Equals("") && !ddlCaja.SelectedValue.Equals("0"))
                {
                    DateTime value = DateTime.Parse(txtFechaIni.Text + " " + txtHoraIni.Text);
                    DateTime value2 = DateTime.Parse(txtFechaFin.Text + " " + txtHoraFin.Text);
                    sp_corte_cajaResult sp_corte_cajaResult = new dcContextoSuPlazaDataContext().sp_corte_caja(value, value2, ddlCajeros.SelectedValue, int.Parse(ddlCaja.SelectedValue)).FirstOrDefault();
                    if (sp_corte_cajaResult != null)
                    {
                        Session["corte_x"] = sp_corte_cajaResult;
                        string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/CorteZ.aspx";
                        string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                        return;
                    }
                    throw new Exception("Se encontraron registros");
                }
                throw new Exception("Verifique que todos los datos hayan sido seleccionados.");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmCierreCajas " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}')", addScriptTags: true);
            }
        }
    }
}