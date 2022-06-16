using System;
using System.Web;
using System.Web.Security;

using DotNetOpenAuth.AspNet;
using Microsoft.AspNet.Membership.OpenAuth;

namespace WebAppPOSAdmin.Account
{
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
		protected string ProviderName
		{
			get
			{
				return ((string)ViewState["ProviderName"]) ?? string.Empty;
			}
			private set
			{
				ViewState["ProviderName"] = value;
			}
		}

		protected string ProviderDisplayName
		{
			get
			{
				return ((string)ViewState["ProviderDisplayName"]) ?? string.Empty;
			}
			private set
			{
				ViewState["ProviderDisplayName"] = value;
			}
		}

		protected string ProviderUserId
		{
			get
			{
				return ((string)ViewState["ProviderUserId"]) ?? string.Empty;
			}
			private set
			{
				ViewState["ProviderUserId"] = value;
			}
		}

		protected string ProviderUserName
		{
			get
			{
				return ((string)ViewState["ProviderUserName"]) ?? string.Empty;
			}
			private set
			{
				ViewState["ProviderUserName"] = value;
			}
		}

		protected void Page_Load()
		{
			if (!base.IsPostBack)
			{
				ProcessProviderResult();
			}
		}

		protected void logIn_Click(object sender, EventArgs e)
		{
			CreateAndLoginUser();
		}

		protected void cancel_Click(object sender, EventArgs e)
		{
			RedirectToReturnUrl();
		}

		private void ProcessProviderResult()
		{
			ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();
			if (string.IsNullOrEmpty(ProviderName))
			{
				base.Response.Redirect(FormsAuthentication.LoginUrl);
			}
			string text = "~/Account/RegisterExternalLogin";
			string text2 = base.Request.QueryString["ReturnUrl"];
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "?ReturnUrl=" + HttpUtility.UrlEncode(text2);
			}
			AuthenticationResult authenticationResult = OpenAuth.VerifyAuthentication(text);
			ProviderDisplayName = OpenAuth.GetProviderDisplayName(ProviderName);
			if (!authenticationResult.IsSuccessful)
			{
				base.Title = "Error de inicio de sesión externo";
				userNameForm.Visible = false;
				base.ModelState.AddModelError("Provider", $"Error de inicio de sesión externo {ProviderDisplayName}.");
				base.Trace.Warn("OpenAuth", $"Error al comprobar la autenticación con {ProviderDisplayName})", authenticationResult.Error);
				return;
			}
			if (OpenAuth.Login(authenticationResult.Provider, authenticationResult.ProviderUserId, createPersistentCookie: false))
			{
				RedirectToReturnUrl();
			}
			ProviderName = authenticationResult.Provider;
			ProviderUserId = authenticationResult.ProviderUserId;
			ProviderUserName = authenticationResult.UserName;
			base.Form.Action = ResolveUrl(text);
			if (base.User.Identity.IsAuthenticated)
			{
				OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, base.User.Identity.Name);
				RedirectToReturnUrl();
			}
			else
			{
				userName.Text = authenticationResult.UserName;
			}
		}

		private void CreateAndLoginUser()
		{
			if (base.IsValid)
			{
				CreateResult createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, userName.Text);
				if (!createResult.IsSuccessful)
				{
					base.ModelState.AddModelError("UserName", createResult.ErrorMessage);
				}
				else if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false))
				{
					RedirectToReturnUrl();
				}
			}
		}

		private void RedirectToReturnUrl()
		{
			string text = base.Request.QueryString["ReturnUrl"];
			if (!string.IsNullOrEmpty(text) && OpenAuth.IsLocalUrl(text))
			{
				base.Response.Redirect(text);
			}
			else
			{
				base.Response.Redirect("~/");
			}
		}
	}
}