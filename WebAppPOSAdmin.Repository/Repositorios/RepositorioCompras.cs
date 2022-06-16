using System;
using System.Collections.Generic;
using System.Linq;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioCompras : ICompras
	{
		public List<compra> listaVisorListaCompraByFechas(DateTime fecha_ini, DateTime fecha_fin)
		{
			new List<compra>();
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from c in dcContextoSuPlazaDataContext.compra
					join ca in dcContextoSuPlazaDataContext.compra_articulo on c.id_compra equals ca.id_compra
					where c.fecha_compra >= fecha_ini.Date && c.fecha_compra.Date <= fecha_fin
					select c).Distinct().ToList();
		}

		public List<compra> listaVisorListaCompraByFechasProveedor(DateTime fecha_ini, DateTime fecha_fin, Guid id_proveedor)
		{
			new List<compra>();
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from c in dcContextoSuPlazaDataContext.compra
					join ca in dcContextoSuPlazaDataContext.compra_articulo on c.id_compra equals ca.id_compra
					where c.fecha_compra >= fecha_ini && c.fecha_compra <= fecha_fin && c.id_proveedor == id_proveedor
					select c).Distinct().ToList();
		}

		public List<compra> listaComprasPorIngresarId()
		{
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			new List<compra>();
			return dcContextoSuPlazaDataContext.compra.ToList();
		}

		public CompraRelacionExtended getCompraById(Guid id_compra)
		{
			CompraRelacionExtended compraRelacionExtended = new CompraRelacionExtended();
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (var item in (from c in dcContextoSuPlazaDataContext.compra
								  join p in dcContextoSuPlazaDataContext.proveedor on c.id_proveedor equals p.id_proveedor
								  where c.id_compra == id_compra
								  select new
								  {
									  proveedores = p.razon_social,
									  fecha = c.fecha_compra.Date
								  }).ToList())
			{
				compraRelacionExtended.Fecha = item.fecha.ToString();
				compraRelacionExtended.Proveedor = item.proveedores;
			}
			return compraRelacionExtended;
		}

		public List<CompraExtended> getPurchases(Guid OrderID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from c in dcContextoSuPlazaDataContext.compra
					where ((object)c.id_pedido).Equals((object)OrderID)
					select new CompraExtended
					{
						id_compra = c.id_compra,
						id_pedido = (c.id_pedido ?? new Guid()),
						fecha_compra = c.fecha_compra
					} into c
					orderby c.fecha_compra
					select c).ToList();
		}

		public List<ComprasPorPedidoExtended> getComprasPorPedido(DateTime fecha_ini, DateTime fecha_fin)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from c in dcContextoSuPlazaDataContext.compra
					where fecha_ini <= c.fecha_compra && fecha_fin >= c.fecha_compra && c.id_pedido != null && c.compra_articulo.FirstOrDefault() != null
					group c by new
					{
						c.pedido.id_pedido,
						c.pedido.num_pedido,
						c.pedido.fecha_pedido,
						c.pedido.proveedor.razon_social
					} into gc
					select new ComprasPorPedidoExtended
					{
						id_pedido = gc.Key.id_pedido,
						folio = gc.Key.num_pedido,
						fecha_pedido = gc.Key.fecha_pedido,
						proveedor = gc.Key.razon_social,
						no_entradas = gc.Count()
					} into c
					orderby c.fecha_pedido descending
					select c).ToList();
		}

		public List<ComprasPorPedidoExtended> getComprasPorPedido(DateTime fecha_ini, DateTime fecha_fin, Guid ProviderID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from c in dcContextoSuPlazaDataContext.compra
					where c.fecha_compra >= fecha_ini && c.fecha_compra <= fecha_fin && c.id_pedido != null && c.compra_articulo.FirstOrDefault() != null && c.pedido.id_proveedor.Equals(ProviderID)
					group c by new
					{
						c.pedido.id_pedido,
						c.pedido.num_pedido,
						c.pedido.fecha_pedido,
						c.pedido.proveedor.razon_social
					} into gc
					select new ComprasPorPedidoExtended
					{
						id_pedido = gc.Key.id_pedido,
						folio = gc.Key.num_pedido,
						fecha_pedido = gc.Key.fecha_pedido,
						proveedor = gc.Key.razon_social,
						no_entradas = gc.Count()
					} into c
					orderby c.fecha_pedido descending
					select c).ToList();
		}

		public void GuardarFactura(Guid PurchaseID, string NumFactura, string observaciones)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			compra obj = dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => e.id_compra.Equals(PurchaseID)) ?? throw new Exception("La compra no existe");
			obj.no_factura = NumFactura;
			obj.observaciones = observaciones;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void GuardarFactura(Guid OrderID, Guid PurchaseID, string NumFactura)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			(dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => ((object)e.id_pedido).Equals((object)OrderID) && e.id_compra.Equals(PurchaseID)) ?? throw new Exception("La compra no existe")).no_factura = NumFactura;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public string getNumFactura(Guid PurchaseID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => e.id_compra.Equals(PurchaseID)).no_factura;
		}

		public string getObservaciones(Guid PurchaseID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => e.id_compra.Equals(PurchaseID)).observaciones;
		}

		public string getNumFactura(Guid OrderID, Guid PurchaseID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.compra.FirstOrDefault((compra e) => ((object)e.id_pedido).Equals((object)OrderID) && e.id_compra.Equals(PurchaseID)).no_factura;
		}
	}
}
