using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Clases;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioDepartamentos : IDepartamentos
	{
		public clasificacion getDepartamento(long id_clasificacion)
		{
			new clasificacion();
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				Expression<Func<clasificacion, bool>> expression = (clasificacion p) => p.id_clasificacion == id_clasificacion;
				return dcContextoSuPlazaDataContext.clasificacion.Where(expression.Compile()).FirstOrDefault();
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public void insertarClasificacion(clasificacion c)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			exitenciaClasificacion(c.descripcion);
			if (c.nivel_clasificacion == 1)
			{
				clasificacion clasificacion = dcContextoSuPlazaDataContext.clasificacion.Where((clasificacion r) => r.nivel_clasificacion == 0).FirstOrDefault();
				c.id_clasificacion_dep = clasificacion.id_clasificacion;
			}
			dcContextoSuPlazaDataContext.clasificacion.InsertOnSubmit(c);
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void modificarClasificacion(clasificacion Clasificacion)
		{
			clasificacion clasificacion = new clasificacion();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			Expression<Func<clasificacion, bool>> expression = (clasificacion p) => p.id_clasificacion == Clasificacion.id_clasificacion;
			clasificacion = dcContextoSuPlazaDataContext.clasificacion.Where(expression.Compile()).FirstOrDefault();
			if (clasificacion.nivel_clasificacion == 1)
			{
				Expression<Func<clasificacion, bool>> expression2 = (clasificacion p) => p.nivel_clasificacion == 0;
				clasificacion clasificacion2 = dcContextoSuPlazaDataContext.clasificacion.Where(expression2.Compile()).FirstOrDefault();
				clasificacion.id_clasificacion_dep = clasificacion2.id_clasificacion;
			}
			else
			{
				clasificacion.id_clasificacion_dep = Clasificacion.id_clasificacion_dep;
			}
			clasificacion.descripcion = Clasificacion.descripcion;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public List<clasificacion> getDepartamentoByNivel(int nivel_clasificacion)
		{
			new List<clasificacion>();
			Expression<Func<clasificacion, bool>> expression = (clasificacion p) => p.nivel_clasificacion == nivel_clasificacion;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from e in dcContextoSuPlazaDataContext.clasificacion.Where(expression.Compile())
					orderby e.descripcion
					select e).ToList();
		}

		public List<clasificacion> getCategoriasById(int id_categoria_depto)
		{
			new List<clasificacion>();
			Expression<Func<clasificacion, bool>> expression = (clasificacion p) => p.id_clasificacion_dep == (long?)(long)id_categoria_depto;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.clasificacion.Where(expression.Compile()).ToList();
		}

		public List<vw_clasificacion> buscarClasificacionByDescripcion(clasificador tipo, string desc)
		{
			List<vw_clasificacion> result = null;
			switch (tipo)
			{
				case clasificador.Todo:
					result = new dcContextoSuPlazaDataContext().vw_clasificacion.Where((vw_clasificacion v) => SqlMethods.Like(v.departamento, string.Concat("%" + desc, "%")) || SqlMethods.Like(v.categoria, string.Concat("%" + desc, "%")) || SqlMethods.Like(v.subcategoria, string.Concat("%" + desc, "%")) || SqlMethods.Like(v.linea, string.Concat("%" + desc, "%"))).ToList();
					break;
				case clasificador.Departamento:
					result = new dcContextoSuPlazaDataContext().vw_clasificacion.Where((vw_clasificacion v) => SqlMethods.Like(v.departamento, string.Concat("%" + desc, "%"))).ToList();
					break;
				case clasificador.Categoria:
					result = new dcContextoSuPlazaDataContext().vw_clasificacion.Where((vw_clasificacion v) => SqlMethods.Like(v.categoria, string.Concat("%" + desc, "%"))).ToList();
					break;
				case clasificador.Subcategoria:
					result = new dcContextoSuPlazaDataContext().vw_clasificacion.Where((vw_clasificacion v) => SqlMethods.Like(v.subcategoria, string.Concat("%" + desc, "%"))).ToList();
					break;
				case clasificador.Linea:
					result = new dcContextoSuPlazaDataContext().vw_clasificacion.Where((vw_clasificacion v) => SqlMethods.Like(v.linea, string.Concat("%" + desc, "%"))).ToList();
					break;
			}
			return result;
		}

		public clasificacion getCategoriaByParametros(string descripcion)
		{
			new clasificacion();
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			Expression<Func<clasificacion, bool>> expression = (clasificacion d) => d.descripcion.Equals(descripcion);
			return dcContextoSuPlazaDataContext.clasificacion.Where(expression.Compile()).FirstOrDefault();
		}

		public bool exitenciaClasificacion(int id)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			Expression<Func<clasificacion, bool>> expression = (clasificacion p) => p.id_clasificacion == (long)id;
			return dcContextoSuPlazaDataContext.clasificacion.Where(expression.Compile()).FirstOrDefault().nivel_clasificacion == 4;
		}

		public bool exitenciaClasificacion(string desc)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			if (dcContextoSuPlazaDataContext.clasificacion.Where((clasificacion c) => c.descripcion.Equals(desc)).FirstOrDefault() != null)
			{
				throw new Exception("La Clasificación ya existe");
			}
			return false;
		}
	}
}
