using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class Usuarios
    {
        public void insertarUsuario(empleado Empleado, List<usuario_permiso> permisos)
        {
            new RepositorioEmpleado().insertarEmpleado(Empleado, permisos);
        }

        public void modificarUsuario(empleado Empleado, List<usuario_permiso> permisos)
        {
            new RepositorioEmpleado().actualizarEmpleado(Empleado, permisos);
        }

        public empleado getEmpleadoById(Guid idUsuario)
        {
            return new RepositorioEmpleado().getEmpleadoById(idUsuario);
        }

        public void eliminarCajero(Guid idUsuario)
        {
            new RepositorioEmpleado().eliminarEmpleado(idUsuario);
        }
    }
}
