using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Scripts;

namespace WebAppPOSAdmin.Repository.Repositorios
{
    public class RepositorioAlmacen : IAlmacen
    {
        public List<movimiento_almacen> getMvtoAlmacenByTipo(string tipo_mvo)
        {
            List<movimiento_almacen> result = new List<movimiento_almacen>();
            try
            {
                Expression<Func<movimiento_almacen, bool>> expression = (movimiento_almacen p) => p.tipo_movto == tipo_mvo;
                using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
                {
                    result = dcContextoSuPlazaDataContext.movimiento_almacen.Where(expression.Compile()).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }
        }

        public void insertarEntradaOSalidaAlmacen<T>(T _clase) where T : class
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            dcContextoSuPlazaDataContext.GetTable<T>().InsertOnSubmit(_clase);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public bool insertarListaArticulosEntradaSalida<T>(List<T> _clase) where T : class
        {
            bool result = false;
            try
            {
                using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
                {
                    foreach (T item in _clase)
                    {
                        dcContextoSuPlazaDataContext.GetTable<T>().InsertOnSubmit(item);
                    }
                    dcContextoSuPlazaDataContext.SubmitChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        public void cambiarEstadiSalida(Guid id_salida)
        {
            Expression<Func<salida, bool>> expression = (salida p) => p.id_salida == id_salida;
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            salida salida = dcContextoSuPlazaDataContext.salida.Where(expression.Compile()).FirstOrDefault();
            salida.cancelada = !salida.cancelada;
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public List<proveedor> listaProveedoresByStatus()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from i in dcContextoSuPlazaDataContext.inventario_fisico
                    where i.fecha_fin == null
                    select new proveedor
                    {
                        id_proveedor = i.proveedor.id_proveedor,
                        razon_social = i.proveedor.razon_social
                    }).ToList();
        }

        public void UpdateRealStock(List<inventario_fisico_articulo> listRealStock)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            foreach (inventario_fisico_articulo item in listRealStock)
            {
                inventario_fisico_articulo inventario_fisico_articulo = dcContextoSuPlazaDataContext.inventario_fisico_articulo.FirstOrDefault((inventario_fisico_articulo e) => e.id_inventario_fisico.Equals(item.id_inventario_fisico) && e.cod_barras.Equals(item.cod_barras));
                if (inventario_fisico_articulo != null)
                {
                    inventario_fisico_articulo.stock_fisico = item.stock_fisico;
                }
            }
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void UpdateStock(long id_inventario)
        {
            throw new Exception("Definir tarea en éste método.");
        }

        public void CreateEntradaAlmacen(entrada e, List<entrada_articulo> listEA)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            e.id_entrada = Guid.NewGuid();
            long num = ((dcContextoSuPlazaDataContext.entrada.FirstOrDefault() != null) ? dcContextoSuPlazaDataContext.entrada.Max((entrada ent) => ent.num_entrada) : 0);
            num = (e.num_entrada = num + 1);
            dcContextoSuPlazaDataContext.entrada.InsertOnSubmit(e);
            dcContextoSuPlazaDataContext.SubmitChanges();
            foreach (entrada_articulo ea in listEA)
            {
                ea.id_entrada = e.id_entrada;
                ea.costo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(ea.cod_barras)).precio_compra;
                dcContextoSuPlazaDataContext.entrada_articulo.InsertOnSubmit(ea);
                dcContextoSuPlazaDataContext.SubmitChanges();
            }
        }

        public void CreateSalidaAlmacen(salida s, List<salida_articulo> listSA)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            s.id_salida = Guid.NewGuid();
            long num = ((dcContextoSuPlazaDataContext.salida.FirstOrDefault() != null) ? dcContextoSuPlazaDataContext.salida.Max((salida sal) => sal.num_salida) : 0);
            num = (s.num_salida = num + 1);
            dcContextoSuPlazaDataContext.salida.InsertOnSubmit(s);
            dcContextoSuPlazaDataContext.SubmitChanges();
            foreach (salida_articulo sa in listSA)
            {
                sa.id_salida = s.id_salida;
                sa.costo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(sa.cod_barras)).precio_compra;
                dcContextoSuPlazaDataContext.salida_articulo.InsertOnSubmit(sa);
                dcContextoSuPlazaDataContext.SubmitChanges();
            }
        }

        public List<entrada> findEntradasBy(DateTime iniDate, DateTime endDate)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.entrada
                    where e.fecha_entrada.Date >= iniDate && e.fecha_entrada.Date <= endDate
                    orderby e.num_entrada descending
                    select e).ToList();
        }

        public List<entrada> findEntradasBy(DateTime iniDate, DateTime endDate, string observacion)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.entrada
                    where e.fecha_entrada.Date >= iniDate && e.fecha_entrada.Date <= endDate && SqlMethods.Like(e.observacion, $"%{observacion}%")
                    orderby e.num_entrada descending
                    select e).ToList();
        }

        public List<entrada> findEntradasBy(string barCode, DateTime iniDate, DateTime endDate)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.entrada
                    join ea in dcContextoSuPlazaDataContext.entrada_articulo on e equals ea.entrada
                    where e.fecha_entrada >= iniDate && e.fecha_entrada <= endDate && ea.cod_barras == barCode
                    orderby e.num_entrada descending
                    select e).ToList();
        }

        public List<entrada> findEntradasBy(string barCode, string observacion, DateTime iniDate, DateTime endDate)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.entrada
                    join ea in dcContextoSuPlazaDataContext.entrada_articulo on e equals ea.entrada
                    where e.fecha_entrada >= iniDate && e.fecha_entrada <= endDate && SqlMethods.Like(e.observacion, $"%{observacion}%") && ea.cod_barras == barCode
                    orderby e.num_entrada descending
                    select e).ToList();
        }

        public List<salida> findSalidasBy(DateTime iniDate, DateTime endDate)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.salida
                    where e.fecha_salida.Date >= iniDate && e.fecha_salida.Date <= endDate
                    orderby e.num_salida descending
                    select e).ToList();
        }

        public List<salida> findSalidasBy(DateTime iniDate, DateTime endDate, string observacion)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from e in dcContextoSuPlazaDataContext.salida
                    where e.fecha_salida.Date >= iniDate && e.fecha_salida.Date <= endDate && SqlMethods.Like(e.observacion, $"%{observacion}%")
                    orderby e.num_salida descending
                    select e).ToList();
        }

        public List<salida> findSalidasBy(string barCode, DateTime iniDate, DateTime endDate)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from s in dcContextoSuPlazaDataContext.salida
                    join sa in dcContextoSuPlazaDataContext.salida_articulo on s equals sa.salida
                    where s.fecha_salida >= iniDate && s.fecha_salida <= endDate && sa.cod_barras == barCode
                    orderby s.num_salida descending
                    select s).ToList();
        }

        public List<salida> findSalidasBy(string barCode, string observacion, DateTime iniDate, DateTime endDate)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return (from s in dcContextoSuPlazaDataContext.salida
                    join sa in dcContextoSuPlazaDataContext.salida_articulo on s equals sa.salida
                    where s.fecha_salida >= iniDate && s.fecha_salida <= endDate && s.observacion.Equals(observacion) && sa.cod_barras == barCode
                    orderby s.num_salida descending
                    select s).ToList();
        }

        public void lookStoreHouse(inventario_fisico p)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            if (dcContextoSuPlazaDataContext.inventario_fisico.FirstOrDefault((inventario_fisico i) => i.id_proveedor.Equals(p.id_proveedor) && ((object)i.fecha_fin).Equals((object)null)) != null)
            {
                throw new Exception("Existe ya un inventario abierto de éste proveedor");
            }
            p.id_inventario_fisico = Guid.NewGuid();
            p.fecha_ini = DateTime.Now;
            p.fecha_registro = DateTime.Now;
            dcContextoSuPlazaDataContext.inventario_fisico.InsertOnSubmit(p);
            dcContextoSuPlazaDataContext.SubmitChanges();
            foreach (articulo item in dcContextoSuPlazaDataContext.articulo.Where((articulo a) => a.id_proveedor.Equals(p.id_proveedor) && a.tipo_articulo.Equals("principal")).ToList())
            {
                inventario_fisico_articulo inventario_fisico_articulo = dcContextoSuPlazaDataContext.inventario_fisico_articulo.FirstOrDefault((inventario_fisico_articulo i) => i.articulo.cod_barras.Equals(item.cod_barras));
                if (inventario_fisico_articulo != null)
                {
                    inventario_fisico_articulo.inventario_fisico = p;
                    inventario_fisico_articulo.stock_estimado = item.stock;
                    inventario_fisico_articulo.stock_fisico = 0.0m;
                    dcContextoSuPlazaDataContext.SubmitChanges();
                }
                else
                {
                    dcContextoSuPlazaDataContext.inventario_fisico_articulo.InsertOnSubmit(new inventario_fisico_articulo
                    {
                        inventario_fisico = p,
                        cod_barras = item.cod_barras,
                        stock_estimado = item.stock
                    });
                    dcContextoSuPlazaDataContext.SubmitChanges();
                }
            }
        }

        public void UpdateAndLock(Guid id_inventario_fisico)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            dcContextoSuPlazaDataContext.inventario_fisico.FirstOrDefault((inventario_fisico i) => i.id_inventario_fisico.Equals(id_inventario_fisico)).fecha_fin = DateTime.Now;
            dcContextoSuPlazaDataContext.SubmitChanges();
            new Procedures().CerrarInventario(id_inventario_fisico);
        }
    }
}
