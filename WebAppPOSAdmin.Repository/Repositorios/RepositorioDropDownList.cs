using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
    public class RepositorioDropDownList : IDropDownListGeneric
    {
        public List<T> getDropDownList<T>() where T : class
        {
            List<T> result = null;
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                result = dcContextoSuPlazaDataContext.GetTable<T>().ToList();
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public List<municipio> getDropDownListMunicipioByID(long id)
        {
            List<municipio> result = new List<municipio>();
            Expression<Func<municipio, bool>> expression = (municipio p) => (long)p.id_entidad == id;
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                result = dcContextoSuPlazaDataContext.municipio.Where(expression.Compile()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public List<vw_clasificacion> getDropDownListVWClasificacion(int id)
        {
            return new List<vw_clasificacion>();
        }

        public List<vw_clasificacion> GetAllDepartamentos()
        {
            List<vw_clasificacion> result = new List<vw_clasificacion>();
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                result = dcContextoSuPlazaDataContext.vw_clasificacion.Select((vw_clasificacion vw) => new vw_clasificacion
                {
                    id_dpto = vw.id_dpto,
                    departamento = vw.departamento
                }).Distinct().ToList();
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public List<vw_clasificacion> GetPrimeraLiniaCategoria(int value)
        {
            List<vw_clasificacion> result = new List<vw_clasificacion>();
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                result = (from vw in dcContextoSuPlazaDataContext.vw_clasificacion
                          where vw.id_dpto == (long)value
                          select new vw_clasificacion
                          {
                              id_cat = vw.id_cat,
                              categoria = vw.categoria
                          }).Distinct().ToList();
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public List<vw_clasificacion> GetSegundaLiniaCategoria(int value)
        {
            List<vw_clasificacion> result = new List<vw_clasificacion>();
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                result = (from vw in dcContextoSuPlazaDataContext.vw_clasificacion
                          where vw.id_cat == (long)value
                          select new vw_clasificacion
                          {
                              id_subcat = vw.id_subcat,
                              subcategoria = vw.subcategoria
                          }).Distinct().ToList();
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public List<vw_clasificacion> GetTerceraLiniaCategoria(int value)
        {
            List<vw_clasificacion> result = new List<vw_clasificacion>();
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                result = (from vw in dcContextoSuPlazaDataContext.vw_clasificacion
                          where vw.id_subcat == (long)value
                          select new vw_clasificacion
                          {
                              id_linea = vw.id_linea,
                              linea = vw.linea
                          }).Distinct().ToList();
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return result;
            }
        }

        public List<empleado> getListaCajeros()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.empleado
                    join u in dcContextoSuPlazaDataContext.usuario on e.user_name equals u.user_name
                    join up in dcContextoSuPlazaDataContext.usuario_permiso on u.user_name equals up.user_name
                    where up.id_permiso.Equals("pos_caja")
                    orderby e.nombre, e.a_paterno, e.a_materno
                    select e).ToList();
        }

        public List<empleado> getSupervisoresCancelaVentas()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.empleado
                    join u in dcContextoSuPlazaDataContext.usuario on e.user_name equals u.user_name
                    join up in dcContextoSuPlazaDataContext.usuario_permiso on u.user_name equals up.user_name
                    where up.id_permiso.Equals("cancel_vta")
                    orderby e.nombre, e.a_paterno, e.a_materno
                    select e).ToList();
        }

        public List<empleado> getSupervisoresMovimientos()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.empleado
                    join u in dcContextoSuPlazaDataContext.usuario on e.user_name equals u.user_name
                    join up in dcContextoSuPlazaDataContext.usuario_permiso on u.user_name equals up.user_name
                    where up.id_permiso.Equals("cambiar_precio") || up.id_permiso.Equals("desc_online") || up.id_permiso.Equals("desc_global")
                    orderby e.nombre, e.a_paterno, e.a_materno
                    select e).Distinct().ToList();
        }
    }
}
