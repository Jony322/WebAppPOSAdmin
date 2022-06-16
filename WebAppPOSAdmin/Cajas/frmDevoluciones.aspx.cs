using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.DropDownListExtender;
using NLog;

namespace WebAppPOSAdmin.Cajas
{
    public partial class frmDevoluciones : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private Guid SaleID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove("devoluciones");
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                btnPdf.Enabled = false;
                ddlCaja.getListPOS();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    getVentas();
                    List<VentaDevolucionExtended> list = (List<VentaDevolucionExtended>)Session["devoluciones"];
                    if (list.Count > 0)
                    {
                        gvVentas.DataSource = list;
                        gvVentas.DataBind();
                        btnPdf.Enabled = true;
                        return;
                    }
                    gvVentas.DataSource = null;
                    gvVentas.DataBind();
                    btnPdf.Enabled = false;
                    throw new Exception("No se encontraron resultados");
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    Log.Error(ex, "Excepción Generada en: frmDevoluciones " + "Acción: btnVer_Click " + ex.Message);
                    loggerdb.Error(ex);
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}')", addScriptTags: true);
                }
            }
            catch (Exception ex2)
            {
                Log.Error(ex2, "Excepción Generada en: frmDevoluciones " + "Acción: btnVer_Click " + ex2.Message);
                loggerdb.Error(ex2);
                _ = ex2.Message;
            }
        }

        public void getVentas()
        {
            try
            {
                DateTime dateTime = DateTime.Parse(txtFechaIni.Text + " " + txtHoraIni.Text);
                DateTime dateTime2 = DateTime.Parse(txtFechaFin.Text + " " + txtHoraFin.Text);
                if (txtBarras.Text.Trim().Length == 0 && ddlCaja.SelectedValue.Equals("0"))
                {
                    Session["dateIni"] = dateTime;
                    Session["dateFin"] = dateTime2;
                    Session["barCode"] = null;
                    Session["pos"] = null;
                    Session["devoluciones"] = new RepositorioCajas().getDevolucionesBy(dateTime, dateTime2);
                    return;
                }
                if (txtBarras.Text.Trim().Length > 0 && !ddlCaja.SelectedValue.Equals("0"))
                {
                    Session["dateIni"] = dateTime;
                    Session["dateFin"] = dateTime2;
                    Session["barCode"] = txtBarras.Text.Trim();
                    Session["pos"] = short.Parse(ddlCaja.SelectedValue);
                    Session["devoluciones"] = new RepositorioCajas().getDevolucionesBy(dateTime, dateTime2, txtBarras.Text.Trim(), int.Parse(ddlCaja.SelectedValue));
                    return;
                }
                if (txtBarras.Text.Trim().Length > 0)
                {
                    Session["dateIni"] = dateTime;
                    Session["dateFin"] = dateTime2;
                    Session["barCode"] = txtBarras.Text.Trim();
                    Session["pos"] = null;
                    Session["devoluciones"] = new RepositorioCajas().getDevolucionesBy(dateTime, dateTime2, txtBarras.Text.Trim());
                    return;
                }
                if (!ddlCaja.SelectedValue.Equals("0"))
                {
                    Session["dateIni"] = dateTime;
                    Session["dateFin"] = dateTime2;
                    Session["barCode"] = null;
                    Session["pos"] = short.Parse(ddlCaja.SelectedValue);
                    Session["devoluciones"] = new RepositorioCajas().getDevolucionesBy(dateTime, dateTime2, int.Parse(ddlCaja.SelectedValue));
                    return;
                }
                throw new Exception("Verifique que los datos requeridos hayan sido seleccionados.");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmDevoluciones " + "Acción: getVentas " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Devoluciones.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: frmDevoluciones " + "Acción: btnPdf_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }

        protected void gvVentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (!(commandName == "view"))
                {
                    return;
                }
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                SaleID = Guid.Parse(e.CommandArgument.ToString());
                var dataSource = (from vda in dcContextoSuPlazaDataContext.venta_devolucion_articulo
                                  join art in dcContextoSuPlazaDataContext.articulo on vda.cod_barras equals art.cod_barras
                                  orderby vda.no_articulo
                                  where vda.id_devolucion.Equals(SaleID)
                                  select new
                                  {
                                      cod_barras = vda.cod_barras,
                                      descripcion = art.descripcion,
                                      cantidad = vda.cantidad,
                                      total = vda.total().ToString("F2")
                                  }).ToList();
                gvVentaDetail.DataSource = dataSource;
                gvVentaDetail.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmDevoluciones " + "Acción: gvVentas_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}