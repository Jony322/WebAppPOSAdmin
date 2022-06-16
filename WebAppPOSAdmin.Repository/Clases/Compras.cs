using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class Compras
    {
        public DateTime fecha_pedido { get; set; }

        public long num_pedido { get; set; }

        public string razon_social { get; set; }

        public string no_factura { get; set; }

        public DateTime fecha_compra { get; set; }
    }
}
