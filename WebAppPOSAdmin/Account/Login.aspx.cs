using System;
using System.Web;

using System.Web.UI.WebControls;

namespace WebAppPOSAdmin.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            OpenAuthLogin.ReturnUrl = base.Request.QueryString["ReturnUrl"];
            string text = HttpUtility.UrlEncode(base.Request.QueryString["ReturnUrl"]);
            if (!string.IsNullOrEmpty(text))
            {
                HyperLink registerHyperLink = RegisterHyperLink;
                registerHyperLink.NavigateUrl = registerHyperLink.NavigateUrl + "?ReturnUrl=" + text;
            }
        }
    }
}