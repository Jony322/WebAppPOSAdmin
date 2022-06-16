using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface ICompras
	{
		List<compra> listaComprasPorIngresarId();

		CompraRelacionExtended getCompraById(Guid id_compra);

		List<compra> listaVisorListaCompraByFechas(DateTime fecha_ini, DateTime fecha_fin);

		List<compra> listaVisorListaCompraByFechasProveedor(DateTime fecha_ini, DateTime fecha_fin, Guid id_proveedor);
	}
}
