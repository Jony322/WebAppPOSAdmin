using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class BackOrderExtended
	{
		public string cod_barras { get; set; }

		public string descripcion { get; set; }

		public string unidad { get; set; }

		public decimal umc { get; set; }

		public decimal cant_pedido { get; set; }

		public decimal cant_compra { get; set; }

		public decimal diferencia { get; set; }

		public decimal getDiferencia()
		{
			return cant_pedido - cant_compra;
		}
	}
}
