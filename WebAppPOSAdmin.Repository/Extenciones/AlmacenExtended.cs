using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class AlmacenExtended
	{
		public string cod_barras { get; set; }

		public string descripcion { get; set; }

		public string unidad { get; set; }

		public decimal UMC { get; set; }

		public decimal can_cja { get; set; }

		public decimal can_pza { get; set; }

		public decimal regalo { get; set; }
	}
}
