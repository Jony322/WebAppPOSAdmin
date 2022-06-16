using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class CompraRelacionExtended
	{
		public string Fecha { get; set; }

		public long Pedido { get; set; }

		public string Proveedor { get; set; }

		public string Factura { get; set; }

		public decimal Total { get; set; }

		public string Fecha_Entrega { get; set; }
	}
}
