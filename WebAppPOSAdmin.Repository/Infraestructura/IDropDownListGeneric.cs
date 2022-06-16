using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
    public interface IDropDownListGeneric
    {
        List<T> getDropDownList<T>() where T : class;

        List<municipio> getDropDownListMunicipioByID(long id);

        List<vw_clasificacion> getDropDownListVWClasificacion(int id);

        List<vw_clasificacion> GetAllDepartamentos();

        List<vw_clasificacion> GetPrimeraLiniaCategoria(int value);

        List<vw_clasificacion> GetSegundaLiniaCategoria(int value);

        List<vw_clasificacion> GetTerceraLiniaCategoria(int value);

        List<empleado> getListaCajeros();

        List<empleado> getSupervisoresCancelaVentas();

        List<empleado> getSupervisoresMovimientos();
    }
}
