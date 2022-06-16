using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
    public class RepositorioClientes : ICliente
    {
        public void create(cliente c)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            dcContextoSuPlazaDataContext.cliente.InsertOnSubmit(c);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void delete(Guid id)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            cliente entity = dcContextoSuPlazaDataContext.cliente.FirstOrDefault((cliente e) => e.id_cliente.Equals(id));
            dcContextoSuPlazaDataContext.cliente.DeleteOnSubmit(entity);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void update(cliente cliTemp)
        {
            new cliente();
            Expression<Func<cliente, bool>> expression = (cliente p) => p.id_cliente == cliTemp.id_cliente;
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            cliente cliente = dcContextoSuPlazaDataContext.cliente.Where(expression.Compile()).FirstOrDefault();
            cliente.contacto = cliTemp.contacto;
            cliente.e_mail = cliTemp.e_mail;
            cliente.e_mail2 = cliTemp.e_mail2;
            cliente.razon_social = cliTemp.razon_social;
            cliente.rfc = cliTemp.rfc;
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public cliente getClienteById(Guid id)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.cliente.FirstOrDefault((cliente ec) => ec.id_cliente.Equals(id));
        }

        public List<cliente> getAllClientes()
        {
            throw new NotImplementedException();
        }
    }
}
