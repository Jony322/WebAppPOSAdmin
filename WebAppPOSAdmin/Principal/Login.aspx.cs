using NLog;
using System;
using System.Linq;
using System.Web;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Security.ControlTicket;
using WebAppPOSAdmin.Security.SeguridadSession;


namespace WebAppPOSAdmin.Principal
{
    public partial class Login : System.Web.UI.Page
    {
        #region
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion
        private SessionManager session = new SessionManager();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAcceder_Click(object sender, EventArgs e)
        {
            try
            {
                RepositorioLogueo repositorioLogueo = new RepositorioLogueo();
                CtrlTicket ctrlTicket = new CtrlTicket();
                empleado empleado = ((ILogueo)repositorioLogueo).accederLogueo(getDatos());
                if (empleado != null)
                {
                    HttpCookie cookie = ctrlTicket.validarSessionTicket(empleado);
                    session.IdUsuario = empleado.id_empleado;
                    session.Parametros["usuarioSession"] = empleado;
                    Session["usuarioSession"] = session;
                    Session["idEmpleado"] = session.IdUsuario;
                    //Session["iva"] = new dcContextoSuPlazaDataContext().pos_admin_settings.FirstOrDefault().iva;
                    Session.Timeout = 600;
                    base.Response.Cookies.Add(cookie);
                    base.Response.Redirect("/Default.aspx", endResponse: false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Login " + "Acción: btnAcceder_Click " + ex.Message);
                loggerdb.Error(ex);
                CtrlException.SetError(ex.Message);
            }
        }

        protected usuario getDatos()
        {
            return new usuario
            {
                user_name = txtUsuario.Text.Trim(),
                password = txtPassword.Text.Trim()
            };
        }
    }
}