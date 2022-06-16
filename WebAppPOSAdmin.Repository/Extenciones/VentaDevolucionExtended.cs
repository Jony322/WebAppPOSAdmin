using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VentaDevolucionExtended
	{
		public int id_pos { get; set; }

		public Guid id_devolucion { get; set; }

		public long folio { get; set; }

		public DateTime fecha { get; set; }

		public string cajero { get; set; }

		public string supervisor { get; set; }

		public decimal total_devuelto { get; set; }
	}

}
