using NLog;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Scripts;

namespace WebAppPOSAdmin.Pedidos
{
    public partial class FreePurchase : System.Web.UI.Page
    {
		#region  logger
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
		private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
		#endregion
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				txtFecha_ini.Text = DateTime.Now.ToString("dd/MM/yyyy");
				txtFecha_fin.Text = DateTime.Now.ToString("dd/MM/yyyy");
				Session.Remove("id_pedido");
				Session.Remove("id_compra");
				Session.Remove("purchases");
			}
		}

		public void limpiarCampos()
		{
			try
			{
				txtFindBarCode.Text = string.Empty;
				txtFindObservaciones.Text = string.Empty;
				txtFecha_ini.Text = string.Empty;
				txtFecha_fin.Text = string.Empty;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: FreePurchase " + "Acción: limpiarCampos " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		public bool validarFechas()
		{
			bool result = false;
			try
			{
				if (!txtFecha_ini.Text.Equals("") && !txtFecha_fin.Text.Equals(""))
				{
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: FreePurchase " + "Acción: validarFechas " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
				return result;
			}
		}

		public void BindDataGrid()
		{
			try
			{
				gvRelacion.DataSource = (List<CompraRelacionExtended>)Session["relacionCompra"];
				gvRelacion.DataBind();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: FreePurchase " + "Acción: BindDataGrid " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		protected void btnVer_Click(object sender, EventArgs e)
		{
			new RepositorioCompras();
			List<CompraRelacionExtended> list = new List<CompraRelacionExtended>();
			try
			{
				if (validarFechas())
				{
					DateTime fecha_ini = Convert.ToDateTime(txtFecha_ini.Text + " 00:01:00");
					DateTime fecha_fin = Convert.ToDateTime(txtFecha_fin.Text + " 23:59:59");
					dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
					try
					{
						if (txtFindObservaciones.Text.Trim().Equals("") && txtFindBarCode.Text.Trim().Equals(""))
						{
							gvRelacion.DataSource = (from c in dc.compra
													 where c.fecha_compra >= fecha_ini && c.fecha_compra <= fecha_fin && c.compra_articulo.FirstOrDefault() != null && ((object)c.id_pedido).Equals((object)null)
													 select new { c.id_compra, c.fecha_compra, c.no_factura, c.observaciones } into c
													 orderby c.fecha_compra descending
													 select c).ToList();
						}
						else if (!txtFindObservaciones.Text.Trim().Equals("") && !txtFindBarCode.Text.Trim().Equals(""))
						{
							gvRelacion.DataSource = (from c in dc.compra
													 where c.fecha_compra >= fecha_ini && c.fecha_compra <= fecha_fin && SqlMethods.Like(c.observaciones, $"%{txtFindObservaciones.Text.Trim()}%") && dc.compra_articulo.FirstOrDefault((compra_articulo a) => a.cod_barras.Equals(txtFindBarCode.Text.Trim())) != null && ((object)c.id_pedido).Equals((object)null)
													 select new { c.id_compra, c.fecha_compra, c.no_factura, c.observaciones } into c
													 orderby c.fecha_compra descending
													 select c).ToList();
						}
						else if (!txtFindObservaciones.Text.Trim().Equals(""))
						{
							gvRelacion.DataSource = (from c in dc.compra
													 where c.fecha_compra >= fecha_ini && c.fecha_compra <= fecha_fin && SqlMethods.Like(c.observaciones, $"%{txtFindObservaciones.Text.Trim()}%") && ((object)c.id_pedido).Equals((object)null)
													 select new { c.id_compra, c.fecha_compra, c.no_factura, c.observaciones } into c
													 orderby c.fecha_compra descending
													 select c).ToList();
						}
						else if (!txtFindBarCode.Text.Trim().Equals(""))
						{
							gvRelacion.DataSource = (from c in dc.compra
													 where c.fecha_compra >= fecha_ini && c.fecha_compra <= fecha_fin && c.compra_articulo.FirstOrDefault((compra_articulo a) => a.cod_barras.Equals(txtFindBarCode.Text.Trim())) != null && ((object)c.id_pedido).Equals((object)null)
													 select new { c.id_compra, c.fecha_compra, c.no_factura, c.observaciones } into c
													 orderby c.fecha_compra descending
													 select c).ToList();
						}
						gvRelacion.DataBind();
					}
					finally
					{
						if (dc != null)
						{
							((IDisposable)dc).Dispose();
						}
					}
				}
				else
				{
					base.ClientScript.RegisterStartupScript(GetType(), "modal", "validacionFechas()", addScriptTags: true);
				}
				if (list.Count > 0)
				{
					Session["relacionCompra"] = list;
				}
				else
				{
					base.ClientScript.RegisterStartupScript(GetType(), "modal", "sinResultados()", addScriptTags: true);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: FreePurchase " + "Acción: btnVer_Click " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		protected void gvRelacion_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			string commandName = e.CommandName;
			if (!(commandName == "view"))
			{
				return;
			}
			txtNumFactura.Text = "";
			txtNumFactura.Enabled = false;
			gvDetailPurchase.DataSource = null;
			gvDetailPurchase.DataBind();
			List<CompraArticuloExtended> purchaseByOrderDetail = new Procedures().getPurchaseByOrderDetail(Guid.Parse(e.CommandArgument.ToString()));
			gvDetailPurchase.DataSource = purchaseByOrderDetail;
			gvDetailPurchase.DataBind();
			if (purchaseByOrderDetail != null)
			{
				if (purchaseByOrderDetail.Count > 0)
				{
					lblTotal.Text = purchaseByOrderDetail.Sum((CompraArticuloExtended i) => i.costo_total).ToString("C2");
					Session["id_compra"] = Guid.Parse(e.CommandArgument.ToString());
					Session["purchases"] = purchaseByOrderDetail;
					txtNumFactura.Text = new RepositorioCompras().getNumFactura(Guid.Parse(e.CommandArgument.ToString()));
					txtObservaciones.Text = new RepositorioCompras().getObservaciones(Guid.Parse(e.CommandArgument.ToString()));
					txtNumFactura.Enabled = true;
				}
				else
				{
					Session.Remove("id_compra");
					Session.Remove("purchases");
				}
			}
			btnGuardarFactura.Enabled = txtNumFactura.Enabled;
		}

		protected void btnPDF_Click(object sender, EventArgs e)
		{
			string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/Compras.aspx";
			string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
			ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
		}

		protected void btnGuardarFactura_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtNumFactura.Text.Trim().Length < 1)
				{
					throw new Exception("Debe ingresar el número de factura.");
				}
				new RepositorioCompras().GuardarFactura(Guid.Parse(Session["id_compra"].ToString()), txtNumFactura.Text.Trim(), txtObservaciones.Text.Trim());
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				Log.Error(ex, "Excepción Generada en: FreePurchase " + "Acción: btnGuardarFactura_Click " + ex.Message);
				loggerdb.Error(ex);
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{message}');", addScriptTags: true);
			}
		}
	}
}