using System;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioCtrlErrores
	{
		public void InsertError(string msg)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			dcContextoSuPlazaDataContext.ctrl_errores.InsertOnSubmit(new ctrl_errores
			{
				id_error = Guid.NewGuid(),
				fecha_log = DateTime.Now,
				descripcion = msg
			});
			dcContextoSuPlazaDataContext.SubmitChanges();
		}
	}
}
