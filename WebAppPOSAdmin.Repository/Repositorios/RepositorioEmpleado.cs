using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioEmpleado : IEmpleados, IGenerico<empleado>
	{
		public void insertarEmpleado(empleado empTemp, List<usuario_permiso> permisos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			empTemp.fecha_registro = DateTime.Now;
			dcContextoSuPlazaDataContext.empleado.InsertOnSubmit(empTemp);
			dcContextoSuPlazaDataContext.SubmitChanges();
			foreach (usuario_permiso permiso in permisos)
			{
				Thread.Sleep(500);
				permiso.fecha_registro = DateTime.Now;
				dcContextoSuPlazaDataContext.usuario_permiso.InsertOnSubmit(permiso);
				dcContextoSuPlazaDataContext.SubmitChanges();
			}
		}

		public void eliminarEmpleado(Guid idUsuario)
		{
			empleado empleado = new empleado();
			Expression<Func<empleado, bool>> expression = (empleado p) => p.id_empleado == idUsuario;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			empleado = dcContextoSuPlazaDataContext.empleado.Where(expression.Compile()).FirstOrDefault();
			string user = empleado.user_name;
			Expression<Func<usuario, bool>> expression2 = (usuario c) => c.user_name == user;
			dcContextoSuPlazaDataContext.usuario.Where(expression2.Compile()).FirstOrDefault().enable = false;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void actualizarEmpleado(empleado empTemp, List<usuario_permiso> permisos)
		{
			empleado oldEmpleado = new empleado();
			new usuario();
			List<usuario_permiso> list = new List<usuario_permiso>();
			Expression<Func<empleado, bool>> expression = (empleado p) => p.id_empleado == empTemp.id_empleado;
			Expression<Func<usuario_permiso, bool>> expression2 = (usuario_permiso p) => p.user_name == empTemp.user_name;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			oldEmpleado = dcContextoSuPlazaDataContext.empleado.Where(expression.Compile()).FirstOrDefault();
			usuario usuario = dcContextoSuPlazaDataContext.usuario.Where((usuario u) => u.user_name.Equals(oldEmpleado.user_name)).FirstOrDefault();
			if (!oldEmpleado.usuario.user_name.Equals(empTemp.usuario.user_name))
			{
				throw new Exception("El nombre usuario no puede ser modificado");
			}
			oldEmpleado.nombre = empTemp.nombre;
			oldEmpleado.a_paterno = empTemp.a_paterno;
			oldEmpleado.a_materno = empTemp.a_materno;
			usuario.password = empTemp.usuario.password;
			usuario.fecha_registro = DateTime.Now;
			dcContextoSuPlazaDataContext.SubmitChanges();
			list = dcContextoSuPlazaDataContext.usuario_permiso.Where(expression2.Compile()).ToList();
			if (list != null)
			{
				foreach (usuario_permiso item in list)
				{
					dcContextoSuPlazaDataContext.usuario_permiso.DeleteOnSubmit(item);
				}
			}
			foreach (usuario_permiso permiso in permisos)
			{
				permiso.fecha_registro = DateTime.Now;
				dcContextoSuPlazaDataContext.usuario_permiso.InsertOnSubmit(permiso);
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public empleado getEmpleadoById(Guid id)
		{
			empleado empTemp = new empleado();
			usuario usuario = new usuario();
			try
			{
				Expression<Func<empleado, bool>> expression = (empleado p) => p.id_empleado == id;
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				empTemp = dcContextoSuPlazaDataContext.empleado.Where(expression.Compile()).FirstOrDefault();
				if (empTemp != null)
				{
					usuario = dcContextoSuPlazaDataContext.usuario.Where((usuario p) => p.user_name == empTemp.user_name).FirstOrDefault();
					empTemp.usuario = usuario;
				}
			}
			catch (Exception ex)
			{
				_ = ex.Message;
			}
			return empTemp;
		}

		public List<empleado> getAllEmpleados()
		{
			throw new NotImplementedException();
		}

		public List<empleado> getListaByTipoEmpleado(int valor)
		{
			List<empleado> result = new List<empleado>();
			new usuario();
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				switch (valor)
				{
					case 0:
						result = (from e in dcContextoSuPlazaDataContext.empleado
								  join u in dcContextoSuPlazaDataContext.usuario on e.user_name equals u.user_name
								  where u.tipo_usuario == RecursosObjects.supervisor
								  select new empleado
								  {
									  nombre = e.nombre,
									  a_paterno = e.a_paterno,
									  a_materno = e.a_materno
								  }).ToList();
						return result;
					case 1:
						result = (from e in dcContextoSuPlazaDataContext.empleado
								  join u in dcContextoSuPlazaDataContext.usuario on e.user_name equals u.user_name
								  where u.tipo_usuario == RecursosObjects.cajero
								  select new empleado
								  {
									  nombre = e.nombre,
									  a_paterno = e.a_paterno,
									  a_materno = e.a_materno
								  }).ToList();
						return result;
					default:
						return result;
				}
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public bool existEmployee(Guid id)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.empleado.FirstOrDefault((empleado e) => e.id_empleado.Equals(id)) != null;
		}

		public empleado getEmployee(Guid id)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.empleado.FirstOrDefault((empleado e) => e.id_empleado.Equals(id));
		}

		public usuario getUser(string userName)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.usuario.FirstOrDefault((usuario u) => u.user_name.Equals(userName));
		}

		public bool ExistUser(string userName)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.usuario.FirstOrDefault((usuario u) => u.user_name.Equals(userName)) != null;
		}

		public bool IsValidPassword(string pwsd)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.usuario.FirstOrDefault((usuario u) => u.password.Equals(pwsd)) != null;
		}
	}
}
