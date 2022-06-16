using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Util.RecursoPedidos
{
    public class Fechas
    {
        public DateTime getFechaByMes(string mes)
        {
            DateTime dateTime = default(DateTime);
            try
            {
                return mes switch
                {
                    "Enero" => Convert.ToDateTime("01/01/" + DateTime.Now.Year),
                    _ => dateTime,
                };
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return DateTime.Now;
            }
        }
    }
}
