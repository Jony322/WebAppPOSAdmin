using NLog;
using System;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatClientes : System.Web.UI.Page
    {

        #region
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private enum operations
        {
            CREATE,
            UPDATE
        }

        private cliente client;

        private operations operation;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.Request.QueryString["id"] != null)
                {
                    client = new Clientes().getClienteById(Guid.Parse(base.Request.QueryString["id"]));
                    if (client != null)
                    {
                        operation = operations.UPDATE;
                    }
                }
                if (!IsPostBack)
                {
                    LlenarDropDownList();
                    if (operation.Equals(operations.UPDATE))
                    {
                        llenarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatClientes " + "Acción: Page_Load " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Clientes','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        public void LlenarDropDownList()
        {
        }

        public void llenarDatos()
        {
            try
            {
                using (new dcContextoSuPlazaDataContext())
                {
                    Clientes clientes = new Clientes();
                    cliente cliente = new cliente();
                    cliente = clientes.getClienteById(client.id_cliente);
                    txtRazonSocial.Text = cliente.razon_social;
                    txtRFC.Text = cliente.rfc;
                    txtCorreo.Text = cliente.e_mail;
                    txtCorreoAlt.Text = cliente.e_mail2;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatClientes " + "Acción: llenarDatos " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Clientes','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRFC.Text.Trim().Length == 0)
                {
                    throw new Exception("El RFC es requerido");
                }
                if (txtRazonSocial.Text.Trim().Length == 0)
                {
                    throw new Exception("La Razón Social es requerida");
                }
                switch (operation)
                {
                    case operations.CREATE:
                        new Clientes().create(getDatos());
                        break;
                    case operations.UPDATE:
                        new Clientes().update(getDatos());
                        break;
                }
                base.Response.Redirect("~/Catalogos/CatClientes.aspx", endResponse: false);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: CatClientes " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }

        protected cliente getDatos()
        {
            cliente cliente;
            if (operation.Equals(operations.CREATE))
            {
                cliente = new cliente();
                cliente.id_cliente = Guid.NewGuid();
            }
            else
            {
                cliente = client;
            }
            cliente.contacto = null;
            cliente.rfc = txtRFC.Text.Trim();
            cliente.razon_social = txtRazonSocial.Text.Trim();
            cliente.e_mail = txtCorreo.Text.Trim();
            cliente.e_mail2 = txtCorreoAlt.Text.Trim();
            return cliente;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
        }

        public void setItem(ref DropDownList _control, int valor)
        {
            string text = Convert.ToString(valor);
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
                Log.Error(ex, "Excepción Generada en: CatClientes " + "Acción: setItem " + ex.Message);
                loggerdb.Error(ex);
                throw ex;
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            loadResultsFind();
        }

        private void loadResultsFind()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                gvCliente.DataSource = null;
                if (rbtAll.Checked)
                {
                    gvCliente.DataSource = from e in dcContextoSuPlazaDataContext.cliente
                                           orderby e.razon_social
                                           select new
                                           {
                                               id = e.id_cliente,
                                               rfc = e.rfc,
                                               razon_social = e.razon_social,
                                               contacto = e.contacto,
                                               e_mail = e.e_mail
                                           };
                }
                else if (rbtRazonSocial.Checked)
                {
                    gvCliente.DataSource = from e in dcContextoSuPlazaDataContext.cliente
                                           where SqlMethods.Like(e.razon_social, $"%{txtFind.Text.Trim()}%")
                                           orderby e.razon_social
                                           select new
                                           {
                                               id = e.id_cliente,
                                               rfc = e.rfc,
                                               razon_social = e.razon_social,
                                               contacto = e.contacto,
                                               e_mail = e.e_mail
                                           };
                }
                else if (rbtRFC.Checked)
                {
                    gvCliente.DataSource = from e in dcContextoSuPlazaDataContext.cliente
                                           where SqlMethods.Like(e.rfc, $"%{txtFind.Text.Trim()}%")
                                           orderby e.rfc
                                           select new
                                           {
                                               id = e.id_cliente,
                                               rfc = e.rfc,
                                               razon_social = e.razon_social,
                                               contacto = e.contacto,
                                               e_mail = e.e_mail
                                           };
                }
                gvCliente.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatClientes " + "Acción: loadResultsFind " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Clientes','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        protected void gvCliente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandName = e.CommandName;
                if (!(commandName == "update"))
                {
                    if (commandName == "delete")
                    {
                        new Clientes().delete(Guid.Parse(e.CommandArgument.ToString()));
                        loadResultsFind();
                    }
                }
                else
                {
                    base.Response.Redirect($"~/Catalogos/CatClientes.aspx?id={e.CommandArgument.ToString()}", endResponse: false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: CatClientes " + "Acción: gvCliente_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('CLIENTES','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }
    }
}