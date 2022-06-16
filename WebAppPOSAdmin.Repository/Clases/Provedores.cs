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
	public class Provedores
	{
		public void create(proveedor Proveedor)
		{
			((IProveedores)new RepositorioProveedores()).insertarProveedor(Proveedor);
		}

		public void update(proveedor Proveedor)
		{
			((IProveedores)new RepositorioProveedores()).actualizarProveedor(Proveedor);
		}

		public proveedor buscarProvedor(Guid id_proveedor)
		{
			new proveedor();
			return ((IProveedores)new RepositorioProveedores()).getProveedorById(id_proveedor);
		}

		public void delete(long id_proveedor)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			proveedor entity = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor e) => ((object)e.id_proveedor).Equals((object)id_proveedor));
			dcContextoSuPlazaDataContext.proveedor.DeleteOnSubmit(entity);
			dcContextoSuPlazaDataContext.SubmitChanges();
		}
	}
}
