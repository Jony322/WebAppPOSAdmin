using System;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioLogueo : ILogueo
	{
		public empleado accederLogueo(usuario _usuario)
		{
			empleado result = null;
			Expression<Func<usuario, bool>> expression = (usuario p) => p.user_name.Equals(_usuario.user_name) && p.password.Equals(_usuario.password) && p.enable == true;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			usuario usuario = dcContextoSuPlazaDataContext.GetTable<usuario>().FirstOrDefault(expression.Compile());
			if (usuario != null)
			{
				if (usuario.tipo_usuario.Equals(RecursosObjects.admin))
				{
					foreach (empleado item in dcContextoSuPlazaDataContext.GetTable<empleado>())
					{
						if (item.usuario == usuario)
						{
							return item;
						}
					}
					return result;
				}
				return result;
			}
			return result;
		}
	}
}
