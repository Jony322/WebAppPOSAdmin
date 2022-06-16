using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IAlmacen
	{
		List<movimiento_almacen> getMvtoAlmacenByTipo(string tipo_mvo);

		void insertarEntradaOSalidaAlmacen<T>(T _clase) where T : class;

		bool insertarListaArticulosEntradaSalida<T>(List<T> _clase) where T : class;

		void cambiarEstadiSalida(Guid id_salida);

		List<proveedor> listaProveedoresByStatus();
	}
}
