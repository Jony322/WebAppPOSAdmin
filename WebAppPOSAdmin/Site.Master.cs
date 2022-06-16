using System;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Recursos;

namespace WebAppPOSAdmin
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";

        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";

        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            HttpCookie httpCookie = base.Request.Cookies["__AntiXsrfToken"];
            if (httpCookie != null && Guid.TryParse(httpCookie.Value, out var _))
            {
                _antiXsrfTokenValue = httpCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;
                HttpCookie httpCookie2 = new HttpCookie("__AntiXsrfToken")
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && base.Request.IsSecureConnection)
                {
                    httpCookie2.Secure = true;
                }
                base.Response.Cookies.Set(httpCookie2);
            }
            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (!base.IsPostBack)
                {
                    ViewState["__AntiXsrfToken"] = Page.ViewStateUserKey;
                    ViewState["__AntiXsrfUserName"] = Context.User.Identity.Name ?? string.Empty;
                    HttpSessionState session = base.Session;
                    _ = base.Request.Path;
                    if (session == null)
                    {
                        throw new Exception();
                    }
                    if (session["usuarioSession"] == null)
                    {
                        throw new Exception();
                    }
                }
                else if ((string)ViewState["__AntiXsrfToken"] != _antiXsrfTokenValue || (string)ViewState["__AntiXsrfUserName"] != (Context.User.Identity.Name ?? string.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
            catch (Exception)
            {
                base.Response.Redirect("~/Principal/Login.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _ = base.IsPostBack;
        }

        public void validarVista(empleado vista)
        {
            try
            {
                if (!((IPermisos)new RepositorioPermisos()).accesoVista(vista.user_name, Lista_Permisos.Articulos))
                {
                    base.Response.Redirect("~/Default.aspx", endResponse: false);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('Artículo asociado','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                base.Session.Remove("usuarioSession");
                base.Response.Redirect("~/Principal/Login.aspx", endResponse: false);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
    }
}