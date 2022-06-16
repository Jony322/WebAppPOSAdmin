using System;
using System.Linq;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class TicketsFactura
	{
		public Guid id_venta { get; set; }

		public int id_pos { get; set; }

		public long folio { get; set; }

		public bool isInvoicedTicket(TicketsFactura ticket)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.factura_venta.FirstOrDefault((factura_venta f) => f.id_venta.Equals(ticket.id_venta) && f.id_pos.Equals(ticket.id_pos)) != null;
		}

		public bool validateClient(Guid ClientID, string rfc)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.cliente.FirstOrDefault((cliente c) => c.id_cliente.Equals(ClientID) && c.rfc.Equals(rfc)) == null;
		}
	}
}
