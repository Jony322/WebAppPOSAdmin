using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Infraestructura;

using WebAppPOSAdmin.Security.SeguridadSession;

using WebAppPOSAdmin.DropDownListExtender;
using WebAppPOSAdmin.Funcionalidades;
using WebAppPOSAdmin.Recursos;

namespace WebAppPOSAdmin.Almacen
{
    public partial class frmEntrada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["cont"] = 0;
                Session["almacen"] = null;
                cargarDropInicial();
                txtCantidad.Text = "1";
                txtRegalo.Text = "0";
            }
        }

		public void LimpiarCampos(bool cleanBarCode)
		{
			if (cleanBarCode)
			{
				txtCodBarras.Text = string.Empty;
			}
			txtObservaciones.Text = string.Empty;
			txtDescripcion.Text = string.Empty;
			txtRegalo.Text = string.Empty;
			txtCantidad.Text = string.Empty;
			txtUnidad.Text = string.Empty;
			txtUMC.Text = string.Empty;
			ddlPiezas.Items.Clear();
			ddlMvtoAlmacen.ClearSelection();
			txtCantidad.Text = "1";
			txtRegalo.Text = "0";
		}

		public void cargarDropInicial()
		{
			try
			{
				ddlMvtoAlmacen.getMvtoAlmacen(Resourses.Tipo_mvto);
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		public void BindDataGrid()
		{
			try
			{
				gvAnexos.DataSource = (List<AlmacenExtended>)Session["almacen"];
				gvAnexos.DataBind();
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		public void setItem(ref DropDownList _control, decimal value)
		{
			string text = Convert.ToString(value);
			try
			{
				foreach (ListItem item in _control.Items)
				{
					if (item.Value == text)
					{
						_control.SelectedValue = item.Value.ToString();
						break;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected void btnBuscar_Click(object sender, EventArgs e)
		{
			try
			{
				string text = txtCodBarras.Text.Trim().ToUpper();
				IArticulos articulos = new RepositorioArticulos();
				articulo articulo = new articulo();
				articulo = articulos.getArticuloByCodBarras(text);
				LimpiarCampos(cleanBarCode: false);
				if (articulo != null)
				{
					if (articulo.cod_asociado != null)
					{
						txtCodBarras.Text = articulo.cod_asociado;
					}
					txtDescripcion.Text = articulo.descripcion;
					txtUnidad.Text = articulo.unidad_medida.descripcion;
					txtUMC.Text = articulo.cantidad_um.ToString();
					ddlPiezas.Items.Insert(0, "1.000");
					ddlPiezas.Items.Insert(1, articulo.cantidad_um.ToString());
					setItem(ref ddlPiezas, articulo.cantidad_um);
					txtCantidad.Focus();
					return;
				}
				articulo = null;
				articulo = articulos.getArticuloByCodInterno(text);
				if (articulo != null)
				{
					if (articulo.cod_asociado != null)
					{
						txtCodBarras.Text = articulo.cod_asociado;
					}
					else
					{
						txtCodBarras.Text = articulo.cod_barras;
					}
					txtDescripcion.Text = articulo.descripcion;
					txtUnidad.Text = articulo.unidad_medida.descripcion;
					txtUMC.Text = articulo.cantidad_um.ToString();
					ddlPiezas.Items.Insert(0, "1.000");
					ddlPiezas.Items.Insert(1, articulo.cantidad_um.ToString());
					setItem(ref ddlPiezas, articulo.cantidad_um);
					txtCantidad.Focus();
				}
				else
				{
					base.ClientScript.RegisterStartupScript(GetType(), "modal", "sinResultados()", addScriptTags: true);
					LimpiarCampos(cleanBarCode: true);
				}
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		protected void btnAnexar_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtObservaciones.Text.Trim().Length < 3)
				{
					throw new Exception("Debe ingresar una descripción a la Entrada");
				}
				List<AlmacenExtended> list = new List<AlmacenExtended>();
				if ((int)Session["cont"] > 0)
				{
					list = (List<AlmacenExtended>)Session["almacen"];
				}
				if (list.Count == 0)
				{
					Session["cont"] = 1;
					AlmacenExtended almacenExtended = new AlmacenExtended();
					almacenExtended.cod_barras = txtCodBarras.Text.Trim();
					almacenExtended.descripcion = txtDescripcion.Text.Trim();
					almacenExtended.unidad = txtUnidad.Text.Trim();
					almacenExtended.regalo = decimal.Parse(txtRegalo.Text);
					if (txtUnidad.Text == "Cja" || txtUnidad.Text == "Paq")
					{
						if (ddlPiezas.SelectedItem.Text == "1.000")
						{
							almacenExtended.UMC = 1m;
							almacenExtended.can_cja = 0m;
							almacenExtended.can_pza = decimal.Parse(txtCantidad.Text);
						}
						else
						{
							almacenExtended.UMC = decimal.Parse(txtUMC.Text);
							almacenExtended.can_cja = decimal.Parse(txtCantidad.Text);
							almacenExtended.can_pza = decimal.Parse(txtUMC.Text) * decimal.Parse(txtCantidad.Text);
						}
					}
					else
					{
						almacenExtended.UMC = decimal.Parse(txtUMC.Text);
						almacenExtended.can_cja = 0m;
						almacenExtended.can_pza = decimal.Parse(txtCantidad.Text);
					}
					list.Add(almacenExtended);
				}
				else if (verificarExistenciTabla().Equals(obj: false))
				{
					AlmacenExtended almacenExtended2 = new AlmacenExtended();
					almacenExtended2.cod_barras = txtCodBarras.Text.Trim();
					almacenExtended2.descripcion = txtDescripcion.Text.Trim();
					almacenExtended2.unidad = txtUnidad.Text.Trim();
					almacenExtended2.regalo = decimal.Parse(txtRegalo.Text);
					if (txtUnidad.Text == "Cja" || txtUnidad.Text == "Paq")
					{
						if (ddlPiezas.SelectedItem.Equals("1.000"))
						{
							almacenExtended2.UMC = 1m;
							almacenExtended2.can_cja = 0m;
							almacenExtended2.can_pza = decimal.Parse(txtCantidad.Text);
						}
						else
						{
							almacenExtended2.UMC = decimal.Parse(txtUMC.Text);
							almacenExtended2.can_cja = decimal.Parse(txtCantidad.Text);
							almacenExtended2.can_pza = decimal.Parse(txtUMC.Text) * decimal.Parse(txtCantidad.Text);
						}
					}
					else
					{
						almacenExtended2.UMC = decimal.Parse(txtUMC.Text);
						almacenExtended2.can_cja = 0m;
						almacenExtended2.can_pza = decimal.Parse(txtCantidad.Text);
					}
					list.Add(almacenExtended2);
				}
				Session["almacen"] = list;
				BindDataGrid();
				LimpiarCampos(cleanBarCode: true);
				txtCodBarras.Focus();
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		protected void btnGuardar_Click(object sender, EventArgs e)
		{
			try
			{
				new Funciones();
				new RepositorioAlmacen();
				if (Session["almacen"] != null)
				{
					new RepositorioAlmacen().CreateEntradaAlmacen(getDatosEntrada(), convertirSessionALista());
				}
				Session["almacen"] = null;
				BindDataGrid();
				LimpiarCampos(cleanBarCode: true);
				base.Response.Redirect("~/Almacen/frmEntrada.aspx", endResponse: false);
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		public bool verificarExistenciTabla()
		{
			bool result = false;
			try
			{
				new List<AlmacenExtended>();
				foreach (AlmacenExtended item in (List<AlmacenExtended>)Session["almacen"])
				{
					if (!item.cod_barras.Equals(txtCodBarras.Text))
					{
						continue;
					}
					result = true;
					item.regalo += decimal.Parse(txtRegalo.Text);
					if (txtUnidad.Text == "Cja" || txtUnidad.Text == "Paq")
					{
						if (ddlPiezas.SelectedItem.Equals("1.000"))
						{
							item.can_pza += decimal.Parse(txtCantidad.Text);
							break;
						}
						item.can_cja += decimal.Parse(txtCantidad.Text);
						item.can_pza += decimal.Parse(txtUMC.Text) * decimal.Parse(txtCantidad.Text);
					}
					else
					{
						item.can_cja = 0m;
						item.can_pza += decimal.Parse(txtCantidad.Text);
					}
					break;
				}
				return result;
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
				return result;
			}
		}

		public entrada getDatosEntrada()
		{
			return new entrada
			{
				id_movto = Convert.ToInt32(ddlMvtoAlmacen.SelectedValue),
				observacion = ((txtObservaciones.Text.Trim() != "") ? txtObservaciones.Text.Trim() : null),
				fecha_entrada = DateTime.Now,
				user_name = ((empleado)((SessionManager)Session["usuarioSession"]).Parametros["usuarioSession"]).user_name,
				cancelada = false
			};
		}

		public List<entrada_articulo> convertirSessionALista()
		{
			List<entrada_articulo> list = new List<entrada_articulo>();
			try
			{
				foreach (AlmacenExtended item in (List<AlmacenExtended>)Session["almacen"])
				{
					list.Add(new entrada_articulo
					{
						cod_barras = item.cod_barras,
						cant_ent = item.can_cja,
						cant_pza = item.can_pza,
						cant_reg = item.regalo
					});
				}
				return list;
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
				return null;
			}
		}

		protected void gvResultsFind_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				string commandName = e.CommandName;
				if (commandName == "selectedItem")
				{
					txtCodBarras.Text = e.CommandArgument.ToString();
					txtCodBarras.Focus();
				}
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		protected void btnFindItemDesc_Click(object sender, EventArgs e)
		{
			try
			{
				gvResultsFind.Dispose();
				if (txtFindItem.Text.Trim().Equals(""))
				{
					throw new Exception("Debe ingresar una descripción");
				}
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				gvResultsFind.DataSource = from a in dcContextoSuPlazaDataContext.articulo
										   where SqlMethods.Like(a.descripcion, $"%{txtFindItem.Text.Trim()}%")
										   select new { a.cod_barras, a.descripcion };
				gvResultsFind.DataBind();
			}
			catch (Exception ex)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
			}
		}

		protected void ibtFindItem_Click(object sender, ImageClickEventArgs e)
		{
			txtFindItem.Text = "";
			txtFindItem.Focus();
			gvResultsFind.Dispose();
			gvResultsFind.DataSource = null;
			gvResultsFind.DataBind();
		}
	}
}