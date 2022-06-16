using System.Linq;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class MySettings
    {
        public decimal getIVA()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().iva;
        }
    }
}
