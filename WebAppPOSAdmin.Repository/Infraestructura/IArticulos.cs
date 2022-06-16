using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IArticulos
	{
		bool eliminarArticulo(articulo artTemp);

		void actualizarArticulo(articulo artTemp);

		articulo getArticuloById(object idTemp);

		bool actualizarfrmActualizarPrecio(List<ArticuloExtended> lista);

		void suspenderfrmActualizarPrecio(List<ArticuloExtended> lista, bool addItems);

		bool eliminarSuspencionesfrmActualizarPrecio(List<ArticuloExtended> lista);

		List<ArticuloExtended> getArticulosSuspendidos();

		List<VisorArticuloExtended> getArticuloVisor(int id);

		List<ArticuloAnexoExtended> listaArticulosAnexos(string cod_barras);

		List<articulo> listaArticulosAsociados(string cod_barras);

		bool eliminarArticuloAsociado(string cod_barras);

		bool insertarKitArticulo(kit_articulos kit, DateTime fecha_ini, DateTime fecha_fin);

		List<kitArticuloExtended> listaArticulosKit(string cod_barras);

		List<articulo> getArticuloByCodBarrasPrincipal(string cod_barras, bool ofertado);

		articulo getArticuloByCodInterno(string cod_interno);

		articulo getArticuloByCodAsociado(string cod_asociado);

		articulo getArticuloByCodBarras(string cod_barras);

		List<articulo> getArticuloByDescripcion(string descripcion_larga);
	}
}
