using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	[Serializable]
	public class OfertaArticuloExtended
	{
		public long id_oferta { get; set; }

		public string cod_barras { get; set; }

		public string descripcion_larga { get; set; }

		public string unidad { get; set; }

		public decimal precio_venta { get; set; }

		public decimal precio_oferta { get; set; }

		public bool kit { get; set; }
	}
}
