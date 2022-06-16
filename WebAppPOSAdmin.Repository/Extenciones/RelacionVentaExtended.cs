using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class RelacionVentaExtended
	{
		public long Caja { get; set; }

		public long Numero { get; set; }

		public string Fecha { get; set; }

		public string Hora { get; set; }

		public decimal Total { get; set; }

		public string Cajero { get; set; }
	}
}
