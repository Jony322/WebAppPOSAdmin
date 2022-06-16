using System;
using System.Collections.Generic;
using System.Linq;

using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.Repository.Entidad
{
	public class facturacion
	{
		public long facturar(Guid ClientId, List<string> payMethods, string comprobante, string condicion, string uso, List<TicketsFactura> tickets, List<venta_articulo> items)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext2 = new dcContextoSuPlazaDataContext();
			checkTickets(tickets);
			long id_factura = ((dcContextoSuPlazaDataContext2.factura.FirstOrDefault() != null) ? dcContextoSuPlazaDataContext2.factura.Max((factura i) => i.id_factura) : 0) + 1;
			factura factura2 = new factura
			{
				id_factura = id_factura,
				fecha_remision = DateTime.Now,
				id_cliente = ClientId,
				id_comprobante = comprobante,
				id_condicion = condicion,
				id_uso = uso
			};
			dcContextoSuPlazaDataContext2.factura.InsertOnSubmit(factura2);
			dcContextoSuPlazaDataContext2.SubmitChanges();
			foreach (string payMethod in payMethods)
			{
				factura_metodo_pago factura_metodo_pago2 = new factura_metodo_pago();
				factura_metodo_pago2.id_factura = factura2.id_factura;
				factura_metodo_pago2.id_metodo = payMethod;
				dcContextoSuPlazaDataContext2.factura_metodo_pago.InsertOnSubmit(factura_metodo_pago2);
				dcContextoSuPlazaDataContext2.SubmitChanges();
			}
			if (tickets != null && tickets.Count > 0)
			{
				foreach (TicketsFactura ticket in tickets)
				{
					factura_venta entity = new factura_venta
					{
						id_factura = factura2.id_factura,
						id_pos = ticket.id_pos,
						id_venta = ticket.id_venta,
						fecha_registro = factura2.fecha_remision
					};
					dcContextoSuPlazaDataContext2.factura_venta.InsertOnSubmit(entity);
					dcContextoSuPlazaDataContext2.SubmitChanges();
				}
			}
			if (items.Count > 0)
			{
				foreach (venta_articulo item in items)
				{
					if (!item.register)
					{
						factura_articulo entity2 = new factura_articulo
						{
							id_factura_articulo = Guid.NewGuid(),
							id_factura = factura2.id_factura,
							cod_barras = item.cod_barras,
							descripcion = item.articulo.descripcion,
							id_unidad = item.articulo.id_unidad,
							cantidad = item.cantidad,
							precio_vta = item.precio_vta,
							iva = item.articulo.iva
						};
						dcContextoSuPlazaDataContext2.factura_articulo.InsertOnSubmit(entity2);
						dcContextoSuPlazaDataContext2.SubmitChanges();
					}
				}
			}
			return factura2.id_factura;
		}

		public void checkTickets(List<TicketsFactura> tickets)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext2 = new dcContextoSuPlazaDataContext();
			if (tickets == null)
			{
				return;
			}
			foreach (TicketsFactura ticket in tickets)
			{
				if (dcContextoSuPlazaDataContext2.factura_venta.FirstOrDefault((factura_venta f) => f.id_venta.Equals(ticket.id_venta) && f.id_pos.Equals(ticket.id_pos)) != null)
				{
					venta venta2 = dcContextoSuPlazaDataContext2.venta.FirstOrDefault((venta e) => e.id_venta.Equals(ticket.id_venta) && e.id_pos.Equals(ticket.id_pos));
					throw new Exception($"El ticket {venta2.folio} de la caja {venta2.id_pos} ya está registrado");
				}
			}
		}
	}
}
