using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.Membership.OpenAuth;

namespace WebAppPOSAdmin.Account
{
    public partial class Manage : System.Web.UI.Page
    {

		protected string SuccessMessage { get; private set; }

		protected bool CanRemoveExternalLogins { get; private set; }

		protected void Page_Load(object sender, EventArgs e)
        {
			if (!base.IsPostBack)
			{
				bool flag = OpenAuth.HasLocalPassword(base.User.Identity.Name);
				setPassword.Visible = !flag;
				changePassword.Visible = flag;
				CanRemoveExternalLogins = flag;
				string text = base.Request.QueryString["m"];
				if (text != null)
				{
					base.Form.Action = ResolveUrl("~/Account/Manage");
					SuccessMessage = text switch
					{
						"RemoveLoginSuccess" => "El inicio de sesión externo se ha quitado.",
						"SetPwdSuccess" => "Se estableció la contraseña.",
						"ChangePwdSuccess" => "Se cambió la contraseña.",
						_ => string.Empty,
					};
					successMessage.Visible = !string.IsNullOrEmpty(SuccessMessage);
				}
			}
		}

		protected void setPassword_Click(object sender, EventArgs e)
		{
			if (base.IsValid)
			{
				SetPasswordResult setPasswordResult = OpenAuth.AddLocalPassword(base.User.Identity.Name, password.Text);
				if (setPasswordResult.IsSuccessful)
				{
					base.Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
				}
				else
				{
					base.ModelState.AddModelError("NewPassword", setPasswordResult.ErrorMessage);
				}
			}
		}

		public IEnumerable<OpenAuthAccountData> GetExternalLogins()
		{
			IEnumerable<OpenAuthAccountData> accountsForUser = OpenAuth.GetAccountsForUser(base.User.Identity.Name);
			CanRemoveExternalLogins = CanRemoveExternalLogins || accountsForUser.Count() > 1;
			return accountsForUser;
		}

		public void RemoveExternalLogin(string providerName, string providerUserId)
		{
			string text = (OpenAuth.DeleteAccount(base.User.Identity.Name, providerName, providerUserId) ? "?m=RemoveLoginSuccess" : string.Empty);
			base.Response.Redirect("~/Account/Manage" + text);
		}

		protected static string ConvertToDisplayDateTime(DateTime? utcDateTime)
		{
			if (!utcDateTime.HasValue)
			{
				return "[nunca]";
			}
			return utcDateTime.Value.ToLocalTime().ToString("G");
		}
	}
}