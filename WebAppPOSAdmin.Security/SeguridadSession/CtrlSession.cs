using System.Web.SessionState;
using System.Web.UI;

namespace WebAppPOSAdmin.Security.SeguridadSession
{
    public class CtrlSession
    {
        public void validaSession(Page _page)
        {
            HttpSessionState session = _page.Session;
            if (session == null)
            {
                _page.Response.Redirect("/Principal/Login.aspx");
            }
            else if (session["usuarioSession"] == null)
            {
                _page.Response.Redirect("/Principal/Login.aspx");
            }
        }
    }
}
