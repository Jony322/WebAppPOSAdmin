using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class CapturaInventarioExtended
	{
		public string cod_barras { get; set; }

		public string descripcion_larga { get; set; }

		public string descripcion { get; set; }

		public decimal? existencias { get; set; }

		public decimal? fisica { get; set; }
	}

}
