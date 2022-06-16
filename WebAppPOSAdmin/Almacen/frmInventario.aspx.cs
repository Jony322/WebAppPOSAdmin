using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using WebAppPOSAdmin.DropDownListExtender;

namespace WebAppPOSAdmin.Almacen
{
    public partial class frmInventario : System.Web.UI.Page
    {
        private string directorio = "";

        private string nombre = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            eliminarReporte();
            if (!IsPostBack) { }
        }

        protected void rbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedValue = ((RadioButtonList)sender).SelectedValue;
                if (!(selectedValue == "fisico"))
                {
                    if (selectedValue == "actual")
                    {
                        ddlProveedor.getProveedores();
                    }
                }
                else
                {
                    ddlProveedor.getListaInventariosAbierto();
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
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
                _ = ex.Message;
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlProveedor.SelectedValue.Equals("00000000-0000-0000-0000-000000000000"))
                {
                    throw new Exception("Debe seleccionar un proveedor.");
                }
                string text = rbEstado.SelectedValue.ToString();
                if (!(text == "actual"))
                {
                    if (text == "fisico")
                    {
                        Session["InventoryFisicalID"] = Guid.Parse(ddlProveedor.SelectedValue);
                        string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/InventarioFisico.aspx";
                        string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                    }
                }
                else
                {
                    Session["ProviderID"] = Guid.Parse(ddlProveedor.SelectedValue);
                    string arg2 = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/InventarioActual.aspx";
                    string script2 = $"window.open('{arg2}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "modal", script2, addScriptTags: true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}