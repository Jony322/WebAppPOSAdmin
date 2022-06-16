using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioPermisos : IPermisos
	{
		public List<permiso> getAllPermisos(string tipo_sistema)
		{
			List<permiso> result = new List<permiso>();
			try
			{
				Expression<Func<permiso, bool>> expression = (permiso p) => p.tipo_sistema == tipo_sistema;
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				result = dcContextoSuPlazaDataContext.permiso.Where(expression.Compile()).ToList();
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public List<permiso> getPermisoFiltro(usuario usuTemp)
		{
			new List<usuario_permiso>();
			List<permiso> list = new List<permiso>();
			usuario usuario = new usuario();
			permiso permiso = new permiso();
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				Expression<Func<usuario, bool>> expression = (usuario p) => p.user_name == usuTemp.user_name;
				usuario = dcContextoSuPlazaDataContext.usuario.Where(expression.Compile()).FirstOrDefault();
				if (usuario != null)
				{
					foreach (usuario_permiso item in dcContextoSuPlazaDataContext.usuario_permiso.ToList())
					{
						if (usuario.user_name == item.user_name)
						{
							permiso = dcContextoSuPlazaDataContext.permiso.Where((permiso p) => p.id_permiso.Equals(item.id_permiso)).FirstOrDefault();
							list.Add(permiso);
						}
						permiso = null;
					}
					return list;
				}
				return list;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return list;
			}
		}

		public List<permiso> getFiltroListBoxPermiso(Guid id_empleado, string filtro)
		{
			new List<usuario_permiso>();
			List<permiso> result = new List<permiso>();
			empleado empTemp = new empleado();
			usuario userTemp = new usuario();
			new permiso();
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				Expression<Func<empleado, bool>> expression = (empleado c) => c.id_empleado == id_empleado;
				empTemp = dcContextoSuPlazaDataContext.empleado.Where(expression.Compile()).FirstOrDefault();
				Expression<Func<usuario, bool>> expression2 = (usuario p) => p.user_name == empTemp.user_name;
				userTemp = dcContextoSuPlazaDataContext.usuario.Where(expression2.Compile()).FirstOrDefault();
				if (userTemp != null)
				{
					List<usuario_permiso> list = dcContextoSuPlazaDataContext.usuario_permiso.Where((usuario_permiso up) => up.user_name == userTemp.user_name).ToList();
					List<permiso> list2 = dcContextoSuPlazaDataContext.permiso.Where((permiso r) => r.tipo_sistema == filtro).ToList();
					for (int i = 0; i < list.Count; i++)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							if (list2[j].id_permiso.Equals(list[i].id_permiso))
							{
								list2.Remove(list2[j]);
								break;
							}
						}
					}
					result = list2;
					return result;
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public List<permiso> getAllPermisosUsuarios()
		{
			List<permiso> result = new List<permiso>();
			try
			{
				Expression<Func<permiso, bool>> expression = (permiso p) => p.tipo_sistema.Equals(RecursosObjects.pos_admin);
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				result = dcContextoSuPlazaDataContext.permiso.Where(expression.Compile()).ToList();
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public bool insertarPermiso(permiso permisos)
		{
			bool result = false;
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					dcContextoSuPlazaDataContext.permiso.InsertOnSubmit(permisos);
					dcContextoSuPlazaDataContext.SubmitChanges();
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public bool modificarPermiso(permiso permisos)
		{
			bool result = false;
			new permiso();
			try
			{
				Expression<Func<permiso, bool>> expression = (permiso p) => p.id_permiso == permisos.id_permiso;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					permiso permiso = dcContextoSuPlazaDataContext.permiso.Where(expression.Compile()).FirstOrDefault();
					permiso.descripcion = permisos.descripcion;
					permiso.tipo_sistema = permisos.tipo_sistema;
					dcContextoSuPlazaDataContext.SubmitChanges();
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public bool eliminarPermiso(string idPermiso)
		{
			bool result = false;
			permiso permiso = new permiso();
			try
			{
				Expression<Func<permiso, bool>> expression = (permiso p) => p.id_permiso == idPermiso;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					permiso = dcContextoSuPlazaDataContext.permiso.Where(expression.Compile()).FirstOrDefault();
					dcContextoSuPlazaDataContext.permiso.DeleteOnSubmit(permiso);
					dcContextoSuPlazaDataContext.SubmitChanges();
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public permiso getPermisoById(string idPermiso)
		{
			permiso result = new permiso();
			try
			{
				Expression<Func<permiso, bool>> expression = (permiso p) => p.id_permiso == idPermiso;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					result = dcContextoSuPlazaDataContext.permiso.Where(expression.Compile()).FirstOrDefault();
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public bool accesoVista(string user_name, string id_permiso)
		{
			bool result = false;
			try
			{
				dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				if ((from u in dcContextoSuPlazaDataContext.usuario
					 join up in dcContextoSuPlazaDataContext.usuario_permiso on u.user_name equals up.user_name
					 where (up.user_name == user_name) & (up.id_permiso == id_permiso)
					 select new
					 {
						 cont = u.user_name
					 }).ToList().Count > 0)
				{
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}
	}
}
