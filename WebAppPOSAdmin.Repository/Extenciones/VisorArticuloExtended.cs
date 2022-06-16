using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class VisorArticuloExtended
	{
		public string cod_barras { get; set; }

		public string cod_interno { get; set; }

		public string descripcion_larga { get; set; }

		public string unidad { get; set; }

		public string proveedor { get; set; }

		public string tipo_articulo { get; set; }
	}
}
