using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
    public interface ICliente
    {
        void create(cliente cliTemp);

        void delete(Guid id);

        void update(cliente cliTemp);

        cliente getClienteById(Guid idTemp);

        List<cliente> getAllClientes();
    }

}
