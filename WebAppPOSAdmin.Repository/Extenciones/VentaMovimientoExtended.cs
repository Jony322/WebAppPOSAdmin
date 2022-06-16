using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VentaMovimientoExtended
	{
		public long id_pos { get; set; }

		public string status { get; set; }

		public DateTime fecha { get; set; }

		public string cajero { get; set; }

		public string supervisor { get; set; }

		public string transaccion { get; set; }

		public string cod_barras { get; set; }

		public string descripcion { get; set; }

		public decimal precio_regular { get; set; }

		public decimal precio_venta { get; set; }

		public decimal descuento { get; set; }

		public long ticket { get; set; }
	}
}
