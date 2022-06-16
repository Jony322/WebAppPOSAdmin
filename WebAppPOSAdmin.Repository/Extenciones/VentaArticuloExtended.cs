using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
    public class VentaArticuloExtended
    {
        public string cod_barras { get; set; }

        public string descripcion { get; set; }

        public decimal cantidad { get; set; }

        public decimal total { get; set; }
    }

}
