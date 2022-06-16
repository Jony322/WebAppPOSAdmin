
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class Empresas
    {
        public static Empresas myEmpresa { get; set; }

        public void insertarEmpresa(empresa _empresa)
        {
            new RepositorioEmpresa().insertarEmpresa(_empresa);
        }

        public empresa getEmpresaById()
        {
            return new RepositorioEmpresa().getEmpresaById();
        }

        public void actualizarEmpresa(empresa _empresa)
        {
            new RepositorioEmpresa().actualizarEmpresa(_empresa);
        }
    }
}
