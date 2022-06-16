using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IDepartamentos
	{
		clasificacion getDepartamento(long id_clasificacion);

		void insertarClasificacion(clasificacion Clasificacion);

		void modificarClasificacion(clasificacion Clasificacion);

		List<clasificacion> getDepartamentoByNivel(int nivel_clasificacion);

		List<clasificacion> getCategoriasById(int id_categoria_depto);

		clasificacion getCategoriaByParametros(string descripcion);

		bool exitenciaClasificacion(int id);
	}
}
