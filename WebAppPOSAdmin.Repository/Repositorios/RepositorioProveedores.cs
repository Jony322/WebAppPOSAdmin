using System;
using System.Collections.Generic;
using System.Linq;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioProveedores : IProveedores
	{
		public bool insertarProveedor(proveedor proTemp)
		{
			bool result = false;
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				dcContextoSuPlazaDataContext.proveedor.InsertOnSubmit(proTemp);
				dcContextoSuPlazaDataContext.SubmitChanges();
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public bool eliminarProveedor(proveedor proTemp)
		{
			throw new NotImplementedException();
		}

		public bool actualizarProveedor(proveedor p)
		{
			bool result = false;
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					proveedor proveedor = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor e) => e.id_proveedor.Equals(p.id_proveedor));
					proveedor.nombre_contacto = p.nombre_contacto;
					proveedor.razon_social = p.razon_social;
					proveedor.rfc = p.rfc;
					proveedor.tel_movil = p.tel_movil;
					proveedor.tel_principal = p.tel_principal;
					proveedor.estatus = p.estatus;
					proveedor.direccion_proveedor.calle = p.direccion_proveedor.calle;
					proveedor.direccion_proveedor.colonia = p.direccion_proveedor.colonia;
					proveedor.direccion_proveedor.cod_postal = p.direccion_proveedor.cod_postal;
					proveedor.direccion_proveedor.id_entidad = p.direccion_proveedor.id_entidad;
					proveedor.direccion_proveedor.id_municipio = p.direccion_proveedor.id_municipio;
					proveedor.direccion_proveedor.localidad = p.direccion_proveedor.localidad;
					proveedor.direccion_proveedor.pais = p.direccion_proveedor.pais;
					proveedor.direccion_proveedor.no_ext = p.direccion_proveedor.no_ext;
					proveedor.direccion_proveedor.no_int = p.direccion_proveedor.no_int;
					dcContextoSuPlazaDataContext.SubmitChanges();
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return false;
			}
		}

		public proveedor getProveedorById(Guid ProviderID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			proveedor proveedor = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor e) => e.id_proveedor.Equals(ProviderID));
			if (proveedor != null)
			{
				direccion_proveedor direccion_proveedor = dcContextoSuPlazaDataContext.direccion_proveedor.FirstOrDefault((direccion_proveedor e) => e.id_proveedor.Equals(ProviderID));
				proveedor.direccion_proveedor = ((direccion_proveedor != null) ? direccion_proveedor : new direccion_proveedor());
			}
			return proveedor;
		}

		public List<PedidosSuspendidoExtended> getProveedorByInventarioAbierto()
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from i in dcContextoSuPlazaDataContext.inventario_fisico
					where i.fecha_fin == null
					select new PedidosSuspendidoExtended
					{
						id = i.id_inventario_fisico,
						razon_social = i.proveedor.razon_social
					}).ToList();
		}
	}
}
