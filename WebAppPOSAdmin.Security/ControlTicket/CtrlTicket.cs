using System;
using System.Web;
using System.Web.Security;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Security.ControlTicket
{
    public class CtrlTicket
    {
        private DateTime dateExpiration { get; set; }

        public HttpCookie validarSessionTicket(object _object)
        {
            dateExpiration = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy 22:00:00"));
            try
            {
                empleado empleado = (empleado)_object;
                FormsAuthentication.Initialize();
                FormsAuthenticationTicket formsAuthenticationTicket = new FormsAuthenticationTicket(1, empleado.user_name, DateTime.Now, dateExpiration, isPersistent: true, FormsAuthentication.FormsCookiePath);
                string value = FormsAuthentication.Encrypt(formsAuthenticationTicket);
                HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);
                if (formsAuthenticationTicket.IsPersistent)
                {
                    httpCookie.Expires = formsAuthenticationTicket.Expiration;
                }
                return httpCookie;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }
        }
    }
}
