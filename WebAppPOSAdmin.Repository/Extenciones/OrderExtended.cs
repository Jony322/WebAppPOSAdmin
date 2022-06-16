using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class OrderExtended
	{
		public List<PedidoArticulosExtended> items = new List<PedidoArticulosExtended>();

		public Guid id_pedido { get; set; }

		public string proveedor { get; set; }

		public short no_dias { get; set; }
	}
}
