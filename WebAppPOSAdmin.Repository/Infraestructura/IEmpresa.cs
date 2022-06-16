using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IEmpresa
	{
		bool insertarEmpresa(empresa _empresa);

		empresa getEmpresaById();

		bool actualizarEmpresa(empresa _empresa);
	}

}
