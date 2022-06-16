using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
    public class PedidoArticulosExtended
    {
        public string cod_barras { get; set; }

        public string descripcion { get; set; }

        public decimal umc { get; set; }

        public string unidad { get; set; }

        public decimal costo { get; set; }

        public decimal existencia_caja { get; set; }

        public decimal existencias_pieza { get; set; }

        public decimal sugerido { get; set; }

        public decimal num_articulos { get; set; }

        public decimal a_pedir { get; set; }
    }
}
