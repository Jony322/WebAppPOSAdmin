using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class CompraExtended
	{
		public Guid id_pedido { get; set; }

		public Guid id_compra { get; set; }

		public string num_captura { get; set; }

		public DateTime fecha_compra { get; set; }
	}

}
