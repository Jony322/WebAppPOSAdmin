using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class ArticuloAnexoExtended
	{
		public string cod_barras { get; set; }

		public string cod_anexo { get; set; }

		public string cod_interno { get; set; }

		public string descripcion_larga { get; set; }

		public string descripcion { get; set; }

		public string articulo { get; set; }

		public string unidad { get; set; }

		public decimal cantidad_um { get; set; }

		public decimal precio_compra { get; set; }

		public decimal costo { get; set; }

		public decimal utilidad { get; set; }

		public decimal precio_venta { get; set; }

		public decimal stock_cja { get; set; }

		public decimal stock_pza { get; set; }

		public decimal sugerido { get; set; }

		public decimal pedir { get; set; }
	}
}
