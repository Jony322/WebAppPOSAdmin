using System;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;

using WebAppPOSAdmin.Security.SeguridadSession;

using WebAppPOSAdmin.Recursos;
using NLog;

namespace WebAppPOSAdmin.Catalogos
{
    public partial class CatPermisos : System.Web.UI.Page
    {
		#region
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
		private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
		#endregion


		private SessionFormularios sesion = new SessionFormularios();

        private string idPermiso;

        private string Accion;

		protected void Page_Load(object sender, EventArgs e)
		{
			sesion = (SessionFormularios)Session["SessionPermisos"];
			if (sesion != null)
			{
				if (Session["usuarioSession"] != null)
				{
					idPermiso = sesion.Parametros["idPermiso"].ToString();
				}
				if (idPermiso == null || idPermiso == "")
				{
					Accion = Resourses.Insertar;
					txtIdpermiso.Enabled = true;
				}
				else
				{
					Accion = Resourses.Actualizar;
				}
			}
			if (!IsPostBack && !string.IsNullOrEmpty(idPermiso))
			{
				llenarDatos();
			}
		}

		public void llenarDatos()
		{
			try
			{
				Permisos permisos = new Permisos();
				permiso permiso = new permiso();
				txtIdpermiso.Enabled = false;
				permiso = permisos.getPermisoById(idPermiso);
				txtIdpermiso.Text = permiso.id_permiso;
				txtDescripcion.Text = permiso.descripcion;
				for (int i = 0; i < rbtPermisos.Items.Count; i++)
				{
					if (rbtPermisos.Items[i].Value == permiso.tipo_sistema)
					{
						rbtPermisos.Items[i].Selected = true;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: CatPermisos " + "Acción: llenarDatos " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}

		public permiso getDatosPermiso()
		{
			return new permiso
			{
				id_permiso = txtIdpermiso.Text.Trim(),
				descripcion = txtDescripcion.Text.Trim(),
				tipo_sistema = rbtPermisos.SelectedValue.ToString()
			};
		}

		protected void btnGuardar_Click(object sender, EventArgs e)
		{
			try
			{
				Permisos permisos = new Permisos();
				string accion = Accion;
				if (!(accion == "insertar"))
				{
					if (accion == "actualizar")
					{
						permisos.actualizarPermiso(getDatosPermiso());
					}
				}
				else
				{
					permisos.insertarPermiso(getDatosPermiso());
				}
				Session.Remove("SessionPermisos");
				base.Response.Redirect("~/Catalogos/CatPermisos.aspx");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Excepción Generada en: CatPermisos " + "Acción: btnGuardar_Click " + ex.Message);
				loggerdb.Error(ex);
				_ = ex.Message;
			}
		}
	}
}