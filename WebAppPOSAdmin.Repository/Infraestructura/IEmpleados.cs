using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IEmpleados
	{
		void insertarEmpleado(empleado empTemp, List<usuario_permiso> permisos);

		void eliminarEmpleado(Guid idUsuario);

		void actualizarEmpleado(empleado empTemp, List<usuario_permiso> permisos);

		empleado getEmpleadoById(Guid id);

		List<empleado> getAllEmpleados();
	}

}
