using NLog;
using System;
using System.Linq;
using System.Web.UI;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Facturacion
{
    public partial class Settings : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                loadSettings();
            }
        }

        public void loadSettings()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                pos_admin_settings pos_admin_settings = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
                if (pos_admin_settings != null)
                {
                    txtCfdiUser.Text = pos_admin_settings.cfdi_user;
                    txtCfdiPassword.Text = pos_admin_settings.cfdi_pwsd;
                    txtPathTXT.Text = pos_admin_settings.cfdi_path_txt;
                    txtPathPDF.Text = pos_admin_settings.cfdi_path_pdf;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Settings " + "Acción: loadSettings " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                pos_admin_settings pos_admin_settings = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
                if (pos_admin_settings == null)
                {
                    pos_admin_settings pos_admin_settings2 = new pos_admin_settings();
                    pos_admin_settings2.id_setting = Guid.NewGuid();
                    pos_admin_settings2.cfdi_user = txtCfdiUser.Text.Trim();
                    pos_admin_settings2.cfdi_pwsd = txtCfdiPassword.Text.Trim();
                    pos_admin_settings2.cfdi_path_txt = txtPathTXT.Text.Trim();
                    pos_admin_settings2.cfdi_path_pdf = txtPathPDF.Text.Trim();
                    dcContextoSuPlazaDataContext.pos_admin_settings.InsertOnSubmit(pos_admin_settings2);
                }
                else
                {
                    pos_admin_settings.cfdi_user = txtCfdiUser.Text.Trim();
                    pos_admin_settings.cfdi_pwsd = txtCfdiPassword.Text.Trim();
                    pos_admin_settings.cfdi_path_txt = txtPathTXT.Text.Trim();
                    pos_admin_settings.cfdi_path_pdf = txtPathPDF.Text.Trim();
                }
                dcContextoSuPlazaDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Log.Error(ex, "Excepción Generada en: Settings " + "Acción: btnGuardar_Click " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert(\"{message}\")", addScriptTags: true);
            }
        }
    }
}