using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class Statistics
    {
        public string cod_barras { get; set; }

        public string descripcion { get; set; }

        public string medida { get; set; }

        public DateTime fecha { get; set; }

        public decimal cantidad { get; set; }

        public decimal total { get; set; }
    }

}
