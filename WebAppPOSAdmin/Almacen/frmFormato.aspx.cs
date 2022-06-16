using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;

using WebAppPOSAdmin.DropDownListExtender;
using WebAppPOSAdmin.Common;
using NLog;

namespace WebAppPOSAdmin.Almacen
{
    public partial class frmFormato : System.Web.UI.Page
    {

        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private string directorio = "";

        private string nombre = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            eliminarReporte();
            if (!IsPostBack)
                ddlProveedor.getListaInventariosAbierto();
        }

        public void eliminarReporte()
        {
            try
            {
                directorio = AppDomain.CurrentDomain.BaseDirectory + "/ReportesPdf/";
                string[] files = Directory.GetFiles(directorio);
                foreach (string path in files)
                {
                    nombre = Path.GetFileName(path);
                    File.Delete(directorio + nombre);
                    Session.Clear();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmFormato" + "Acción: eliminarReporte " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void generarReporte(Guid id_inventario)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                if ((from i in dcContextoSuPlazaDataContext.inventario_fisico_articulo
                     orderby i.articulo.descripcion
                     where i.id_inventario_fisico == id_inventario && i.articulo.tipo_articulo == "principal"
                     select new formato
                     {
                         cod_barras = i.cod_barras,
                         descripcion_larga = i.articulo.descripcion,
                         razon_social = i.inventario_fisico.proveedor.razon_social,
                         unidad_medida = i.articulo.unidad_medida.descripcion,
                         stock = i.articulo.stock
                     }).ToList().Count <= 0)
                {
                    throw new Exception("El inventario abierto seleccionado no contiene articulos.");
                }
                nombre = "reporteFormato.pdf";
                directorio = AppDomain.CurrentDomain.BaseDirectory + "/ReportesPdf/";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmFormato" + "Acción: generarReporte " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ddlProveedor.SelectedValue.Equals("00000000-0000-0000-0000-000000000000"))
                {
                    string arg = string.Concat("http://", base.Request["HTTP_HOST"], "/PdfReports/FormatoInventario.aspx?id=" + Guid.Parse(ddlProveedor.SelectedValue));
                    string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                    return;
                }
                throw new Exception("Debe elegir un Proveedor");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: frmFormato" + "Acción: btnVer_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}