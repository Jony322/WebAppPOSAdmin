using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Common;

namespace WebAppPOSAdmin.FrmArticulo
{
    public partial class frmEtiquetas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["ItemsToPrinted"] = null;
                txtCantidad.Text = "1";
            }
            txtCantidad.Focus();
        }

        protected void btnAdherible_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < gvImpresion.Rows.Count; i++)
                {
                    new ZebraPrinterController().printLblAdherible(gvImpresion.Rows[i].Cells[0].Text, int.Parse(gvImpresion.Rows[i].Cells[3].Text), chkPrecioNormal.Checked);
                }
            }
            catch (Exception ex)
            {
                string s = $"<script>alert('ERROR: {ex.Message}')</script>";
                base.Response.Write(s);
            }
        }

        protected void btnAnaquel_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < gvImpresion.Rows.Count; i++)
                {
                    new ZebraPrinterController().printLblAnaquel(gvImpresion.Rows[i].Cells[0].Text, int.Parse(gvImpresion.Rows[i].Cells[3].Text), chkPrecioNormal.Checked);
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
                ViewState["ItemsToPrinted"] = null;
                gvImpresion.DataSource = null;
                gvImpresion.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            etMensaje.Text = string.Empty;
            try
            {
                Session.Remove("item");
                if (txtDescripcion.Text.Trim().Length > 0)
                {
                    Session["item"] = new RepositorioArticulos().findItem(txtDescripcion.Text.Trim());
                    if (Session["item"] != null)
                    {
                        btnBuscar_Modal.Show();
                    }
                    else
                    {
                        findInModal();
                    }
                }
                else
                {
                    findInModal();
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void findInModal()
        {
            modalFindItem.Show();
            txtFindItem.Text = txtDescripcion.Text;
            txtFindItem.Focus();
            gvFindItem.DataSource = null;
            gvFindItem.DataBind();
        }

        protected void btnVisible_Click(object sender, EventArgs e)
        {
            try
            {
                btnBuscar_Modal.Hide();
                int num = int.Parse(txtCantidad.Text);
                if (num > 0)
                {
                    addItemToListToPrinter((articulo)Session["item"], num);
                }
                ResetControls();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void addItemToListToPrinter(articulo item, int printsNumber)
        {
            try
            {
                if (item == null)
                {
                    return;
                }
                if (ViewState["ItemsToPrinted"] != null)
                {
                    DataTable dataTable = (DataTable)ViewState["ItemsToPrinted"];
                    if (dataTable.Rows.Count <= 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string text = gvImpresion.Rows[i].Cells[0].Text;
                        if (item.cod_barras.Equals(text))
                        {
                            break;
                        }
                        dataTable.Rows[i]["cod_barras"] = text;
                        dataTable.Rows[i]["descripcion"] = HttpUtility.HtmlDecode(gvImpresion.Rows[i].Cells[1].Text);
                        dataTable.Rows[i]["precio_venta"] = gvImpresion.Rows[i].Cells[2].Text;
                        dataTable.Rows[i]["cantidad"] = gvImpresion.Rows[i].Cells[3].Text;
                    }
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["cod_barras"] = item.cod_barras;
                    dataRow["descripcion"] = item.descripcion;
                    dataRow["precio_venta"] = ((item.itemInOffer() && !chkPrecioNormal.Checked) ? item.getOfferPrice().ToString("F2") : item.precio_venta.ToString("F2"));
                    dataRow["cantidad"] = printsNumber.ToString();
                    dataTable.Rows.Add(dataRow);
                    dataRow = dataTable.NewRow();
                    ViewState["ItemsToPrinted"] = dataTable;
                    gvImpresion.DataSource = dataTable;
                    gvImpresion.DataBind();
                }
                else
                {
                    DataTable dataTable2 = new DataTable();
                    dataTable2.Columns.Add(new DataColumn("cod_barras", typeof(string)));
                    dataTable2.Columns.Add(new DataColumn("descripcion", typeof(string)));
                    dataTable2.Columns.Add(new DataColumn("precio_venta", typeof(string)));
                    dataTable2.Columns.Add(new DataColumn("cantidad", typeof(string)));
                    DataRow dataRow2 = dataTable2.NewRow();
                    dataRow2["cod_barras"] = item.cod_barras;
                    dataRow2["descripcion"] = item.descripcion;
                    dataRow2["precio_venta"] = ((item.itemInOffer() && !chkPrecioNormal.Checked) ? item.getOfferPrice().ToString("F2") : item.precio_venta.ToString("F2"));
                    dataRow2["cantidad"] = printsNumber.ToString();
                    dataTable2.Rows.Add(dataRow2);
                    dataRow2 = dataTable2.NewRow();
                    ViewState["ItemsToPrinted"] = dataTable2;
                    gvImpresion.DataSource = dataTable2;
                    gvImpresion.DataBind();
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void ResetControls()
        {
            txtDescripcion.Text = string.Empty;
            txtCantidad.Text = "1";
            Session.Remove("item");
            txtDescripcion.Focus();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            modalFindItem.Hide();
        }

        protected void btnFindItem_Click(object sender, EventArgs e)
        {
            if (txtFindItem.Text.Trim().Length <= 0)
            {
                return;
            }
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            List<articulo> dataSource = dcContextoSuPlazaDataContext.articulo.Where((articulo a) => (a.tipo_articulo.Equals("principal") || a.tipo_articulo.Equals("anexo")) && SqlMethods.Like(a.descripcion, $"%{txtFindItem.Text.Trim()}%")).ToList();
            gvFindItem.DataSource = dataSource;
            gvFindItem.DataBind();
        }

        protected void gvFindItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string text = e.CommandName.ToString();
            if (text == "selected")
            {
                modalFindItem.Hide();
                Session.Remove("item");
                Session["item"] = new RepositorioArticulos().findItem(e.CommandArgument.ToString());
                btnBuscar_Modal.Show();
            }
        }
    }
}