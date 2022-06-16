using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IProveedores
	{
		bool insertarProveedor(proveedor proTemp);

		bool eliminarProveedor(proveedor proTemp);

		bool actualizarProveedor(proveedor proTemp);

		proveedor getProveedorById(Guid idTemp);

		List<PedidosSuspendidoExtended> getProveedorByInventarioAbierto();
	}
}
