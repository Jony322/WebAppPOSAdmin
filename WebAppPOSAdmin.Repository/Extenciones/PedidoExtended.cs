using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class PedidoExtended
	{
		public string cod_barras { get; set; }

		public string descripcion_larga { get; set; }

		public decimal no_piezas { get; set; }

		public string unidad { get; set; }
	}
}
