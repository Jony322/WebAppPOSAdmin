using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Recursos;
using WebAppPOSAdmin.ListBoxExtender;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class Usuario : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private enum operations
        {
            CREATE,
            UPDATE
        }

        private ArrayList ListaUno = new ArrayList();

        private ArrayList ListaDos = new ArrayList();

        private empleado employee;

        private usuario user;

        private operations operation;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["id"] != null && new RepositorioEmpleado().existEmployee(Guid.Parse(base.Request.QueryString["id"].ToString())))
            {
                operation = operations.UPDATE;
                employee = new RepositorioEmpleado().getEmployee(Guid.Parse(base.Request.QueryString["id"].ToString()));
                user = new RepositorioEmpleado().getUser(employee.user_name);
            }
            if (!base.IsPostBack)
            {
                loadInicial();
                if (operation.Equals(operations.UPDATE))
                {
                    llenarCampos();
                }
            }
        }

        private void loadInicial()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            gvUsers.DataSource = from u in dcContextoSuPlazaDataContext.usuario
                                 join e in dcContextoSuPlazaDataContext.empleado on u.user_name equals e.user_name
                                 select new
                                 {
                                     user_name = u.user_name,
                                     nombre = e.nombre + " " + e.a_paterno + " " + e.a_materno,
                                     enable = u.enable,
                                     id_empleado = e.id_empleado
                                 };
            gvUsers.DataBind();
        }

        public void llenarCampos()
        {
            try
            {
                txtNombre.Text = employee.nombre;
                txtApaterno.Text = employee.a_paterno;
                txtAmaterno.Text = employee.a_materno;
                txtUser.Text = user.user_name;
                txtPswd.Text = user.password;
                llenarListas(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: llenarCampos " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        public void llenarListas(usuario Usuario)
        {
            try
            {
                lb_permiso_usuario.GetListBoxFiltro(Usuario);
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                decimal num = dcContextoSuPlazaDataContext.usuario_permiso.FirstOrDefault((usuario_permiso u) => u.user_name.Equals(Usuario.user_name) && u.id_permiso.Equals("cambiar_precio")).valor_num ?? 0m;
                decimal num2 = dcContextoSuPlazaDataContext.usuario_permiso.FirstOrDefault((usuario_permiso u) => u.user_name.Equals(Usuario.user_name) && u.id_permiso.Equals("desc_online")).valor_num ?? 0m;
                decimal num3 = dcContextoSuPlazaDataContext.usuario_permiso.FirstOrDefault((usuario_permiso u) => u.user_name.Equals(Usuario.user_name) && u.id_permiso.Equals("desc_global")).valor_num ?? 0m;
                ddlChangePrice.SelectedValue = ((int)num).ToString();
                txtDescuentoLinea.Text = ((num2 != 0m) ? (num2 * 100m).ToString("G4") : null);
                txtDescuentoGlobal.Text = ((num3 != 0m) ? (num2 * 100m).ToString("G4") : null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: llenarListas " + ex.Message);
                loggerdb.Error(ex);
                _ = ex.Message;
            }
        }

        protected empleado getDatos()
        {
            if (txtNombre.Text.Trim().Length <= 0)
            {
                throw new Exception("El campo Nombre es requerido");
            }
            if (txtApaterno.Text.Trim().Length <= 0)
            {
                throw new Exception("El campo Apellido Paterno es requerido");
            }
            if (txtAmaterno.Text.Trim().Length <= 0)
            {
                throw new Exception("El campo Apellido Materno es requerido");
            }
            if (txtUser.Text.Trim().Length <= 0)
            {
                throw new Exception("El campo Usuario es requerido");
            }
            if (txtPswd.Text.Trim().Length <= 0)
            {
                throw new Exception("El campo Contraseña es requerido");
            }
            if (operation.Equals(operations.CREATE) && new RepositorioEmpleado().ExistUser(txtUser.Text.Trim()))
            {
                throw new Exception("El usuario ya existe");
            }
            if (operation.Equals(operations.CREATE) && new RepositorioEmpleado().IsValidPassword(txtPswd.Text.Trim()))
            {
                throw new Exception("La constraseña ya está asignada");
            }
            empleado empleado = new empleado();
            usuario usuario = new usuario();
            empleado.id_empleado = (operation.Equals(operations.UPDATE) ? employee.id_empleado : Guid.NewGuid());
            empleado.nombre = txtNombre.Text.Trim();
            empleado.a_paterno = txtApaterno.Text.Trim();
            empleado.a_materno = txtAmaterno.Text.Trim();
            empleado.fecha_registro = DateTime.Now;
            usuario.user_name = txtUser.Text.Trim();
            usuario.password = txtPswd.Text.Trim();
            usuario.enable = true;
            usuario.tipo_usuario = Resourses.administrador;
            usuario.fecha_registro = DateTime.Now;
            empleado.usuario = usuario;
            return empleado;
        }

        public List<usuario_permiso> asignarPermisos()
        {
            List<usuario_permiso> list = new List<usuario_permiso>();
            for (int i = 0; i < lb_permiso_usuario.Items.Count; i++)
            {
                usuario_permiso usuario_permiso = new usuario_permiso();
                lb_permiso_usuario.Items[i].Selected = true;
                usuario_permiso.user_name = txtUser.Text.Trim();
                usuario_permiso.id_permiso = lb_permiso_usuario.SelectedValue.ToString();
                if (usuario_permiso.id_permiso.Equals("cambiar_precio"))
                {
                    usuario_permiso.valor_num = decimal.Parse(ddlChangePrice.SelectedValue);
                }
                else if (usuario_permiso.id_permiso.Equals("desc_online"))
                {
                    if (txtDescuentoLinea.Text.Trim().Length == 0)
                    {
                        throw new Exception("Debe ingresar el porcentaje de descuento en línea");
                    }
                    usuario_permiso.valor_num = decimal.Parse(txtDescuentoLinea.Text.Trim()) / 100m;
                }
                else if (usuario_permiso.id_permiso.Equals("desc_global"))
                {
                    if (txtDescuentoGlobal.Text.Trim().Length == 0)
                    {
                        throw new Exception("Debe ingresar el porcentaje de descuento global");
                    }
                    usuario_permiso.valor_num = decimal.Parse(txtDescuentoGlobal.Text.Trim()) / 100m;
                }
                else
                {
                    usuario_permiso.valor_num = null;
                }
                lb_permiso_usuario.Items[i].Selected = false;
                list.Add(usuario_permiso);
            }
            return list;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                switch (operation)
                {
                    case operations.CREATE:
                        new RepositorioEmpleado().insertarEmpleado(getDatos(), asignarPermisos());
                        break;
                    case operations.UPDATE:
                        new RepositorioEmpleado().actualizarEmpleado(getDatos(), asignarPermisos());
                        break;
                }
                base.Response.Redirect("~/Catalogos/Usuario.aspx", endResponse: false);
            }
            catch (Exception ex)
            {
                string arg = ex.Message.Replace("'", "");
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{arg}\");", addScriptTags: true);
            }
        }

        protected void btnMoverDerecha_Click(object sender, EventArgs e)
        {
            try
            {
                if (lb_permisos.SelectedIndex < 0)
                {
                    return;
                }
                for (int i = 0; i < lb_permisos.Items.Count; i++)
                {
                    if (lb_permisos.Items[i].Selected && !ListaUno.Contains(lb_permisos.Items[i]))
                    {
                        ListaUno.Add(lb_permisos.Items[i]);
                    }
                }
                for (int j = 0; j < ListaUno.Count; j++)
                {
                    if (!lb_permiso_usuario.Items.Contains((ListItem)ListaUno[j]))
                    {
                        lb_permiso_usuario.Items.Add((ListItem)ListaUno[j]);
                    }
                    lb_permisos.Items.Remove((ListItem)ListaUno[j]);
                }
                lb_permiso_usuario.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: btnMoverDerecha_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnMoverTodoDerecha_Click(object sender, EventArgs e)
        {
            try
            {
                while (lb_permisos.Items.Count != 0)
                {
                    for (int i = 0; i < lb_permisos.Items.Count; i++)
                    {
                        lb_permiso_usuario.Items.Add(lb_permisos.Items[i]);
                        lb_permisos.Items.Remove(lb_permisos.Items[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: btnMoverTodoDerecha_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnMoverIzquierda_Click(object sender, EventArgs e)
        {
            try
            {
                if (lb_permiso_usuario.SelectedIndex < 0)
                {
                    return;
                }
                for (int i = 0; i < lb_permiso_usuario.Items.Count; i++)
                {
                    if (lb_permiso_usuario.Items[i].Selected && !ListaDos.Contains(lb_permiso_usuario.Items[i]))
                    {
                        ListaDos.Add(lb_permiso_usuario.Items[i]);
                    }
                }
                for (int j = 0; j < ListaDos.Count; j++)
                {
                    if (!lb_permisos.Items.Contains((ListItem)ListaDos[j]))
                    {
                        lb_permisos.Items.Add((ListItem)ListaDos[j]);
                    }
                    lb_permiso_usuario.Items.Remove((ListItem)ListaDos[j]);
                }
                lb_permisos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: btnMoverIzquierda_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void btnMoverTodoIzquierda_Click(object sender, EventArgs e)
        {
            try
            {
                while (lb_permiso_usuario.Items.Count != 0)
                {
                    for (int i = 0; i < lb_permiso_usuario.Items.Count; i++)
                    {
                        lb_permisos.Items.Add(lb_permiso_usuario.Items[i]);
                        lb_permiso_usuario.Items.Remove(lb_permiso_usuario.Items[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: btnMoverTodoIzquierda_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        protected void rbtPermisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (employee == null)
                {
                    lb_permisos.getListBox(ref rbtPermisos);
                }
                else
                {
                    lb_permisos.GetListBoxFiltroPermiso(employee.id_empleado, rbtPermisos.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Usuario " + "Acción: rbtPermisos_SelectedIndexChanged " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\");", addScriptTags: true);
            }
        }

        public void limpiarCajeros()
        {
            txtNombre.Text = string.Empty;
            txtApaterno.Text = string.Empty;
            txtAmaterno.Text = string.Empty;
            txtUser.Text = string.Empty;
            txtPswd.Text = string.Empty;
            txtDescuentoLinea.Text = string.Empty;
            txtDescuentoGlobal.Text = string.Empty;
            rbtPermisos.ClearSelection();
            lb_permiso_usuario.Items.Clear();
            lb_permisos.Items.Clear();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;
            if (commandName == "edit")
            {
                base.Response.Redirect($"~/Catalogos/Usuario.aspx?id={e.CommandArgument.ToString()}", endResponse: false);
            }
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("~/Catalogos/Usuario.aspx", endResponse: false);
        }
    }
}