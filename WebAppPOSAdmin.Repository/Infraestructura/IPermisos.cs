using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IPermisos
	{
		List<permiso> getFiltroListBoxPermiso(Guid id_empleado, string filtro);

		List<permiso> getAllPermisos(string tipo_sistema);

		List<permiso> getPermisoFiltro(usuario usuTemp);

		List<permiso> getAllPermisosUsuarios();

		bool insertarPermiso(permiso permisos);

		bool modificarPermiso(permiso permisos);

		bool eliminarPermiso(string idPermiso);

		permiso getPermisoById(string idPermiso);

		bool accesoVista(string user_name, string id_permiso);
	}

}
