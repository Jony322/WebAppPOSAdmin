using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
    public interface ICajas
    {
        List<VentaArticuloExtended> listaArticulosByid(Guid id);
    }
}
