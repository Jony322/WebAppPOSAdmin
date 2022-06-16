using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IOfertas
	{
		Guid insertarOferta(oferta _oferta);

		void insertaListaArticulosOferta(List<oferta_articulo> lista, Guid id_oferta);

		List<VisorOfertasExtended> listaArticulosVisorOfertaById(Guid id);

		List<oferta> listaOfertaSuspendida();
	}

}
