using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Membership.OpenAuth;

namespace WebAppPOSAdmin.Account
{
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        public string ReturnUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!base.IsPostBack)
			{
				return;
			}
			string text = base.Request.Form["provider"];
			if (text != null)
			{
				string text2 = "~/Account/RegisterExternalLogin";
				if (!string.IsNullOrEmpty(ReturnUrl))
				{
					string str = ResolveUrl(ReturnUrl);
					text2 = text2 + "?ReturnUrl=" + HttpUtility.UrlEncode(str);
				}
				OpenAuth.RequestAuthentication(text, text2);
			}
		}

        public IEnumerable<ProviderDetails> GetProviderNames()
        {
            return OpenAuth.AuthenticationClients.GetAll();
        }
    }
}