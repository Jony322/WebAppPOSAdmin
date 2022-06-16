using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

using WebAppPOSAdmin.Funcionalidades;
using NLog;

namespace WebAppPOSAdmin.Articulos
{
    public partial class VisorArticulos : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                cargarDepartamentoPrincipal();
                lblMensaje.Visible = false;
            }
        }

        public void cargarDepartamentoPrincipal()
        {
            try
            {
                Clasificacion clasificacion = new Clasificacion();
                new List<clasificacion>();
                foreach (clasificacion item in clasificacion.getDepartamentosByNivel(0))
                {
                    TreeNode treeNode = new TreeNode(item.descripcion, item.id_clasificacion.ToString());
                    treeNode.PopulateOnDemand = true;
                    treeViewArticulos.Nodes.Add(treeNode);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: cargarDepartamentoPrincipal " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarNodos(int id_clasificacion_depto, TreeNode e)
        {
            DataContext dataContext = new dcContextoSuPlazaDataContext();
            try
            {
                foreach (clasificacion item in (from r in dataContext.GetTable<clasificacion>()
                                                where r.id_clasificacion_dep == (long?)(long)id_clasificacion_depto
                                                select r into c
                                                orderby c.descripcion
                                                select c).ToList())
                {
                    TreeNode treeNode = new TreeNode(item.descripcion, item.id_clasificacion.ToString());
                    e.ChildNodes.Add(treeNode);
                    List<clasificacion> nodoById = getNodoById((int)item.id_clasificacion);
                    if (nodoById == null)
                    {
                        continue;
                    }
                    foreach (clasificacion item2 in nodoById)
                    {
                        TreeNode treeNode2 = new TreeNode(item2.descripcion, item2.id_clasificacion.ToString());
                        treeNode.ChildNodes.Add(treeNode2);
                        List<clasificacion> nodoById2 = getNodoById((int)item2.id_clasificacion);
                        if (nodoById2 == null)
                        {
                            continue;
                        }
                        foreach (clasificacion item3 in nodoById2)
                        {
                            TreeNode treeNode3 = new TreeNode(item3.descripcion, item3.id_clasificacion.ToString());
                            treeNode2.ChildNodes.Add(treeNode3);
                            List<clasificacion> nodoById3 = getNodoById((int)item3.id_clasificacion);
                            if (nodoById3 == null)
                            {
                                continue;
                            }
                            foreach (clasificacion item4 in nodoById3)
                            {
                                TreeNode child = new TreeNode(item4.descripcion, item4.id_clasificacion.ToString());
                                treeNode3.ChildNodes.Add(child);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: llenarNodos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public List<clasificacion> getNodoById(int id)
        {
            try
            {
                return (from r in new dcContextoSuPlazaDataContext().GetTable<clasificacion>()
                        where r.id_clasificacion_dep == (long?)(long)id
                        select r into e
                        orderby e.descripcion
                        select e).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: getNodoById " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
                return null;
            }
        }

        protected void treeViewArticulos_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            try
            {
                TreeNode node = e.Node;
                int id_clasificacion_depto = Convert.ToInt32(node.Value);
                llenarNodos(id_clasificacion_depto, node);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: treeViewArticulos_TreeNodePopulate " + ex.Message);
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
                gvLista.AllowPaging = false;
                gvLista.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvLista.HeaderRow.Cells)
                {
                    cell.BackColor = gvLista.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvLista.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell2 in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell2.BackColor = gvLista.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell2.BackColor = gvLista.RowStyle.BackColor;
                        }
                        cell2.CssClass = "textmode";
                    }
                }
                gvLista.RenderControl(writer);
                string s = "<style> .textmode { } </style>";
                base.Response.Write(s);
                base.Response.Output.Write(stringWriter.ToString());
                base.Response.Flush();
                base.Response.End();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: btnExportarExcel_Click " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarGridArticulos(int id)
        {
            try
            {
                Funciones funciones = new Funciones();
                List<VisorArticuloExtended> list = new List<VisorArticuloExtended>();
                list = (from e in funciones.getListagetArticuloVisor(id)
                        orderby e.descripcion_larga
                        select e).ToList();
                if (list.Count > 0)
                {
                    gvLista.DataSource = list;
                    gvLista.DataBind();
                    lblMensaje.Visible = false;
                }
                else
                {
                    lblMensaje.Text = "No se encontrarón coincidencias";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: llenarGridArticulos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected void treeViewArticulos_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    llenarGridArticulos(Convert.ToInt32(treeViewArticulos.SelectedValue));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Excepción Generada en: VisorArticulos " + "Acción: treeViewArticulos_SelectedNodeChanged " + ex.Message);
                    loggerdb.Error(ex);
                    _ = ex.Message;
                }
            }
            catch (Exception ex2)
            {
                Log.Error(ex2, "Excepción Generada en: VisorArticulos " + "Acción: treeViewArticulos_SelectedNodeChanged Anidada " + ex2.Message);
                loggerdb.Error(ex2);
                _ = ex2.Message;
            }
        }
    }
}