using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VentaRelacionExtended
	{
		public Guid id_venta { get; set; }

		public int id_pos { get; set; }

		public long folio { get; set; }

		public DateTime fecha_venta { get; set; }

		public string cajero { get; set; }

		public decimal total_venta { get; set; }
	}

}
