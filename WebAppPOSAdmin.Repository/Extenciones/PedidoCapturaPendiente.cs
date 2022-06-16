using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class PedidoCapturaPendiente
	{
		public string cod_barras { get; set; }

		public string descripcion { get; set; }

		public string unidad { get; set; }

		public decimal umc { get; set; }

		public decimal precio_costo { get; set; }

		public decimal existencia_cja { get; set; }

		public decimal existencia_pza { get; set; }

		public decimal total { get; set; }

		public decimal pedido_real { get; set; }

		public decimal cantidad { get; set; }

		public decimal cant_original { get; set; }
	}

}
