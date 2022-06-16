using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VisorOfertasExtended
	{
		public string cod_barras { get; set; }

		public string descripcion_larga { get; set; }

		public string fecha_emision { get; set; }

		public decimal precio_oferta { get; set; }

		public string usuario { get; set; }
	}
}
