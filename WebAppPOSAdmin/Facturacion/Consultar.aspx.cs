using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Infraestructura;
using NLog;

namespace WebAppPOSAdmin.Facturacion
{
    public partial class Consultar : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack && !base.IsPostBack)
            {
                txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        public void llenarSiguienteGrid(Guid id)
        {
            try
            {
                List<VentaArticuloExtended> dataSource = ((ICajas)new RepositorioCajas()).listaArticulosByid(id);
                gvCancelacionArticulo.DataSource = dataSource;
                gvCancelacionArticulo.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Consultar " + "Acción: llenarSiguienteGrid " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void DataBindGrid()
        {
            try
            {
                gvFacturaciones.DataSource = (List<VentaCanceladaExtended>)Session["cancelaciones"];
                gvFacturaciones.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Consultar " + "Acción: DataBindGrid " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTime = Convert.ToDateTime($"{txtFechaIni.Text} 00:00:00");
                DateTime dateTime2 = Convert.ToDateTime($"{txtFechaFin.Text} 23:59:59");
                _ = dateTime > dateTime2;
                if (((List<VentaSuspendidaExtended>)Session["SalesSuspended"]).Count == 0)
                {
                    throw new Exception("No se encontraron coincidencias.");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Consultar " + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/VentaSuspendida.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Consultar " + "Acción: btnExportarPdf_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/DocsXLS/VentaSuspendida.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Consultar " + "Acción: btnExportarExcel_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void gvCancelaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (!(commandName == "view"))
                {
                    return;
                }
                Guid VentaID = Guid.Parse(e.CommandArgument.ToString());
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                var dataSource = (from va in dcContextoSuPlazaDataContext.venta_cancelada_articulo
                                  where va.id_venta_cancel.Equals(VentaID)
                                  select new
                                  {
                                      cod_barras = va.cod_barras,
                                      descripcion = va.articulo.descripcion,
                                      cantidad = va.cantidad,
                                      total = va.total().ToString("F2")
                                  }).ToList();
                gvCancelacionArticulo.DataSource = dataSource;
                gvCancelacionArticulo.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Consultar " + "Acción: gvCancelaciones_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}