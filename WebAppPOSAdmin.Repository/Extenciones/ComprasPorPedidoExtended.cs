using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class ComprasPorPedidoExtended
	{
		public Guid id_pedido { get; set; }

		public long folio { get; set; }

		public DateTime fecha_pedido { get; set; }

		public string proveedor { get; set; }

		public int no_entradas { get; set; }
	}

}
