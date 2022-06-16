using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
    public class CompraArticuloExtended
    {
        public string cod_barras { get; set; }

        public string cod_interno { get; set; }

        public string descripcion { get; set; }

        public string unidad { get; set; }

        public decimal umc { get; set; }

        public decimal costo { get; set; }

        public decimal cant_cja { get; set; }

        public decimal cant_pza { get; set; }

        public decimal total { get; set; }

        public decimal costo_total => cant_pza * costo;
    }
}
