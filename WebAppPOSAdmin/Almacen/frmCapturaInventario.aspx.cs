using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.DropDownListExtender;
using WebAppPOSAdmin.Controles;

namespace WebAppPOSAdmin.Almacen
{
    public partial class frmCapturaInventario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                cargaInicial();

        }

        public void cargaInicial()
        {
            try
            {
                ddlProveedor.getListaInventariosAbierto();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void ddlProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                gvCaptura.DataSource = (from i in dcContextoSuPlazaDataContext.sp_reporte_inventario_fisico(Guid.Parse(ddlProveedor.SelectedValue))
                                        select new
                                        {
                                            cod_barras = i.cod_barras,
                                            descripcion_larga = i.articulo,
                                            descripcion = i.unidad,
                                            stock_estimado = i.stock_estimado,
                                            stock_fisico = i.stock_fisico
                                        }).ToList();
                gvCaptura.DataBind();
                btnCreatePDF.Enabled = gvCaptura.Rows.Count > 0;
                Session["InventoryID"] = ((gvCaptura.Rows.Count > 0) ? Guid.Parse(ddlProveedor.SelectedValue) : default(Guid));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvCaptura_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowState == DataControlRowState.Edit)
                {
                    e.Row.BackColor = Color.Aqua;
                    e.Row.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void btnCreatePDF_Click(object sender, EventArgs e)
        {
            try
            {
                string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/CapturaInventario.aspx";
                string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }
    }
}