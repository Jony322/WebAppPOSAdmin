using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VentaSuspendidaExtended
	{
		public long id_pos { get; set; }

		public Guid id_venta { get; set; }

		public DateTime fecha_suspencion { get; set; }

		public string cajero { get; set; }

		public string status { get; set; }
	}

}
