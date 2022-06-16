using System;
using System.Web.Security;
using Microsoft.AspNet.Membership.OpenAuth;

namespace WebAppPOSAdmin.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = base.Request.QueryString["ReturnUrl"];
        }

		protected void RegisterUser_CreatedUser(object sender, EventArgs e)
		{
			FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);
			string url = RegisterUser.ContinueDestinationPageUrl;
			if (!OpenAuth.IsLocalUrl(url))
			{
				url = "~/";
			}
			base.Response.Redirect(url);
		}
	}
}