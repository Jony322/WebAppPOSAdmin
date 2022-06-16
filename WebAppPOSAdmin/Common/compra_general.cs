using System;

namespace WebAppPOSAdmin.Common
{
	public class compra_general
	{
		public string cod_barras { get; set; }

		public string descripcion_larga { get; set; }

		public string descripcion { get; set; }

		public decimal cantidad_um { get; set; }

		public decimal precio_compra { get; set; }

		public decimal cantidad_piezas { get; set; }

		public string factura { get; set; }

		public string razon_social { get; set; }

		public Guid id_compra { get; set; }

		public decimal total { get; set; }

		public string fecha { get; set; }
	}
}