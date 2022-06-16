using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class Clientes
    {
        public void create(cliente Cliente)
        {
            ((ICliente)new RepositorioClientes()).create(Cliente);
        }

        public void update(cliente Cliente)
        {
            ((ICliente)new RepositorioClientes()).update(Cliente);
        }

        public void delete(Guid id)
        {
            ((ICliente)new RepositorioClientes()).delete(id);
        }

        public cliente getClienteById(Guid idCliente)
        {
            return new RepositorioClientes().getClienteById(idCliente);
        }
    }

}
