using System;
using System.Linq;
using System.Linq.Expressions;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
    public class RepositorioEmpresa : IEmpresa
    {
        public bool insertarEmpresa(empresa _empresa)
        {
            bool result = false;
            try
            {
                using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
                {
                    dcContextoSuPlazaDataContext.empresa.InsertOnSubmit(_empresa);
                    dcContextoSuPlazaDataContext.SubmitChanges();
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public empresa getEmpresaById()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.empresa.FirstOrDefault();
        }

        public bool existEnterprise()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.empresa.FirstOrDefault() != null;
        }

        public bool actualizarEmpresa(empresa _empresa)
        {
            bool result = false;
            new empresa();
            try
            {
                using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
                {
                    Expression<Func<empresa, bool>> expression = (empresa p) => p.rfc == _empresa.rfc;
                    empresa empresa = dcContextoSuPlazaDataContext.empresa.Where(expression.Compile()).FirstOrDefault();
                    empresa.razon_social = _empresa.razon_social;
                    empresa.representante = _empresa.representante;
                    empresa.pais = _empresa.pais;
                    empresa.colonia = _empresa.colonia;
                    empresa.estado = _empresa.estado;
                    empresa.municipio = _empresa.municipio;
                    empresa.no_ext = _empresa.no_ext;
                    empresa.no_int = _empresa.no_int;
                    empresa.tel_principal = _empresa.tel_principal;
                    empresa.codigo_postal = _empresa.codigo_postal;
                    empresa.calle = _empresa.calle;
                    empresa.e_mail = _empresa.e_mail;
                    empresa.logo = _empresa.logo;
                    empresa.fecha_registro = DateTime.Now;
                    dcContextoSuPlazaDataContext.SubmitChanges();
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }
    }
}
