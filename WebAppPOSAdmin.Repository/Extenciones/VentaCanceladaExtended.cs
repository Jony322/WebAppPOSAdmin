using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VentaCanceladaExtended
	{
		public Guid id_venta { get; set; }

		public int id_pos { get; set; }

		public DateTime fecha_cancel { get; set; }

		public string cajero { get; set; }

		public string supervisor { get; set; }

		public string status { get; set; }
	}
}
