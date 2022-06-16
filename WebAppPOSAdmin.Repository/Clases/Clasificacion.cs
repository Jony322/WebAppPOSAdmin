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
	public class Clasificacion
	{
		public clasificacion Departamentos(long id_clasificacion)
		{
			new clasificacion();
			return ((IDepartamentos)new RepositorioDepartamentos()).getDepartamento(id_clasificacion);
		}

		public void insertarClasificacion(clasificacion Clasificacion)
		{
			((IDepartamentos)new RepositorioDepartamentos()).insertarClasificacion(Clasificacion);
		}

		public clasificacion getClasificacioByDescripcion(string descripcion)
		{
			new clasificacion();
			return ((IDepartamentos)new RepositorioDepartamentos()).getCategoriaByParametros(descripcion);
		}

		public void actualizarDepartamento(clasificacion Clasificacion)
		{
			((IDepartamentos)new RepositorioDepartamentos()).modificarClasificacion(Clasificacion);
		}

		public List<clasificacion> getDepartamentosByNivel(int nivel_clasificacion)
		{
			return ((IDepartamentos)new RepositorioDepartamentos()).getDepartamentoByNivel(nivel_clasificacion);
		}

		public List<vw_clasificacion> clasificacionPorDescripcion(clasificador tipo, string descripcion)
		{
			return new RepositorioDepartamentos().buscarClasificacionByDescripcion(tipo, descripcion);
		}

		public bool exitenciaClasificacion(int id)
		{
			return new Clasificacion().exitenciaClasificacion(id);
		}
	}
}
