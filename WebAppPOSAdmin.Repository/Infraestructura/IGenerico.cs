using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
    public interface IGenerico<T> where T : class
    {
        List<T> getListaByTipoEmpleado(int valor);
    }

}
