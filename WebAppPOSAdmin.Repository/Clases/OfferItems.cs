using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class OfferItems
    {
        public Guid id_oferta { get; set; }

        public string cod_barras { get; set; }

        public string descripcion { get; set; }

        public string unidad { get; set; }

        public decimal cantidad_um { get; set; }

        public decimal cantidad_vta { get; set; }

        public decimal cantidad_dev { get; set; }

        public decimal total => cantidad_vta - cantidad_dev;
    }
}
