using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class Permisos
    {
        public bool insertarPermiso(permiso Permiso)
        {
            bool result = false;
            try
            {
                result = ((IPermisos)new RepositorioPermisos()).insertarPermiso(Permiso);
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public bool actualizarPermiso(permiso Permiso)
        {
            bool result = false;
            try
            {
                result = ((IPermisos)new RepositorioPermisos()).modificarPermiso(Permiso);
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public permiso getPermisoById(string idPermiso)
        {
            permiso result = new permiso();
            try
            {
                result = ((IPermisos)new RepositorioPermisos()).getPermisoById(idPermiso);
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }
    }
}
