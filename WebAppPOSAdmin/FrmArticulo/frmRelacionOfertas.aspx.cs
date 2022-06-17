using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Funcionalidades;
using WebAppPOSAdmin.Recursos;
using NLog;

namespace WebAppPOSAdmin.FrmArticulo
{
    public partial class frmRelacionOfertas : System.Web.UI.Page
    {
		#region  logger
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
		private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
		#endregion
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				cargarNodoPrincipal();
			}
		}

		public void cargarNodoPrincipal()
		{
			try
			{
				TreeNode treeNode = new TreeNode("Ofertas", "1");
				treeNode.PopulateOnDemand = true;
				treeViewOfertas.Nodes.Add(treeNode);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: cargarNodoPrincipal " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		public void llenarNodos(TreeNode e)
		{
			try
			{
				List<oferta> list = (from r in new dcContextoSuPlazaDataContext().GetTable<oferta>()
									 where r.status_oferta != "suspendida"
									 select r).ToList();
				if (list == null)
				{
					return;
				}
				foreach (oferta item in list)
				{
					string text = ((!item.status_oferta.Equals(Resourses.Cancelado)) ? (item.fecha_ini.ToString("dd/MM/yyyy") + " -- " + item.fecha_fin.ToString("dd/MM/yyyy")) : (item.fecha_ini.ToString("dd/MM/yyyy") + " -- " + item.fecha_fin.ToString("dd/MM/yyyy") + " -- " + item.user_name));
					TreeNode child = new TreeNode(text, item.id_oferta.ToString());
					e.ChildNodes.Add(child);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: llenarNodos " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		protected void treeViewOfertas_TreeNodePopulate(object sender, TreeNodeEventArgs e)
		{
			try
			{
				TreeNode node = e.Node;
				llenarNodos(node);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: treeViewOfertas_TreeNodePopulate " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		public void cargarGridView(Guid id)
		{
			try
			{
				Funciones funciones = new Funciones();
				List<VisorOfertasExtended> list = new List<VisorOfertasExtended>();
				list = funciones.listaVisorOfertaById(id);
				gvOfertas.DataSource = list;
				gvOfertas.DataBind();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: cargarGridView " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		protected void treeViewOfertas_SelectedNodeChanged(object sender, EventArgs e)
		{
			try
			{
				Session["idOferta"] = treeViewOfertas.SelectedValue.ToString();
				cargarGridView(Guid.Parse(Session["idOferta"].ToString()));
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: treeViewOfertas_SelectedNodeChanged " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		public override void VerifyRenderingInServerForm(Control control)
		{
		}

		protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
		{
			try
			{
				base.Response.Clear();
				base.Response.Buffer = true;
				base.Response.AddHeader("content-disposition", "attachment;filename=ArticulosVisor.xls");
				base.Response.Charset = "";
				base.Response.ContentType = "application/vnd.ms-excel";
				using StringWriter stringWriter = new StringWriter();
				HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
				gvOfertas.AllowPaging = false;
				gvOfertas.HeaderRow.BackColor = Color.White;
				foreach (TableCell cell in gvOfertas.HeaderRow.Cells)
				{
					cell.BackColor = gvOfertas.HeaderStyle.BackColor;
				}
				foreach (GridViewRow row in gvOfertas.Rows)
				{
					row.BackColor = Color.White;
					foreach (TableCell cell2 in row.Cells)
					{
						if (row.RowIndex % 2 == 0)
						{
							cell2.BackColor = gvOfertas.AlternatingRowStyle.BackColor;
						}
						else
						{
							cell2.BackColor = gvOfertas.RowStyle.BackColor;
						}
						cell2.CssClass = "textmode";
					}
				}
				gvOfertas.RenderControl(writer);
				string s = "<style> .textmode { } </style>";
				base.Response.Write(s);
				base.Response.Output.Write(stringWriter.ToString());
				base.Response.Flush();
				base.Response.End();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: btnExportarExcel_Click " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		protected void gvOfertas_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				string cod_barras = e.CommandArgument.ToString();
				string commandName = e.CommandName;
				if (commandName == "Eliminar")
				{
					eliminarArticulo(cod_barras, Guid.Parse(Session["idOferta"].ToString()));
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: gvOfertas_RowCommand " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		public void eliminarArticulo(string cod_barras, Guid id_oferta)
		{
			try
			{
				new RepositorioOfertas().eliminarArticuloOfertaByCodBarras(cod_barras, id_oferta);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: eliminarArticulo " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		protected void btnEliminarOferta_Click(object sender, EventArgs e)
		{
			try
			{
				if (Session["idOferta"] == null)
				{
					base.Response.Write("<script>alert('Seleccione una oferta');</script>");
				}
				else
				{
					new RepositorioOfertas().cancelarOferta(Guid.Parse(Session["idOferta"].ToString()));
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: frmRelacionOfertas " + "Acción: btnEliminarOferta_Click " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}
	}
}