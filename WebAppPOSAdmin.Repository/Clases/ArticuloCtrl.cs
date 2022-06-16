using System;
using System.Collections.Generic;
using System.Linq;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Clases
{
    public class ArticuloCtrl
    {
        public enum FindArticuloBy
        {
            BarCode,
            InternalCode,
            Descripcion
        }

        public enum TipoCodigo
        {
            Normal,
            Pesable,
            NoPesable
        }

        private articulo item { get; set; }

        public ArticuloCtrl()
        {
            item = null;
        }

        public void Articulos()
        {
        }

        public void createAsociado(articulo p)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            if (dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(p.cod_barras)) != null)
            {
                throw new Exception("El código a asociar ya existe.");
            }
            new RepositorioArticulos().CreateAsociado(p);
        }

        public void update(articulo Articulo)
        {
            ((IArticulos)new RepositorioArticulos()).actualizarArticulo(Articulo);
        }

        public articulo getArticulo(object barCode)
        {
            findArticulo(barCode, 2);
            return item;
        }

        public object getArticuloAnexo(string barCode)
        {
            return from a in new dcContextoSuPlazaDataContext().articulo
                   where a.cod_barras.Equals(barCode)
                   select new { a.cod_barras, a.cod_interno, a.descripcion, a.descripcion_corta, a.id_unidad, a.cantidad_um, a.precio_compra, a.utilidad, a.precio_venta, a.iva };
        }

        private void findArticulo(object barCode, int nivelBusqueda)
        {
            if (item == null && nivelBusqueda == 0)
            {
                return;
            }
            if (item != null)
            {
                if (item.cod_barras.Equals(barCode))
                {
                    return;
                }
                item = null;
            }
            item = new RepositorioArticulos().getArticuloById(barCode);
            if (item != null && !item.tipo_articulo.Equals("principal"))
            {
                findArticulo(item.cod_asociado, nivelBusqueda - 1);
            }
        }

        public void saveItemToKit(kit_articulos a)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            if (dcContextoSuPlazaDataContext.kit_articulos.FirstOrDefault((kit_articulos k) => k.cod_barras_kit.Equals(a.cod_barras_kit) && k.cod_barras_pro.Equals(a.cod_barras_pro)) == null)
            {
                dcContextoSuPlazaDataContext.kit_articulos.InsertOnSubmit(a);
                dcContextoSuPlazaDataContext.SubmitChanges();
                return;
            }
            dcContextoSuPlazaDataContext.kit_articulos.FirstOrDefault((kit_articulos k) => k.cod_barras_kit.Equals(a.cod_barras_kit) && k.cod_barras_pro.Equals(a.cod_barras_pro)).cantidad = a.cantidad;
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public List<articulo> findBy(string findText, FindArticuloBy findOption, bool kit)
        {
            return findOption switch
            {
                FindArticuloBy.BarCode => new RepositorioArticulos().findArticulosByBarCode(findText, kit),
                FindArticuloBy.InternalCode => new RepositorioArticulos().findArticulosByInternalCode(findText, kit),
                FindArticuloBy.Descripcion => new RepositorioArticulos().findArticulosByDescription(findText, kit),
                _ => null,
            };
        }

        public List<articulo> getArticulosPrincipales()
        {
            return new RepositorioArticulos().getArticulosPrincipales().ToList();
        }

        public void deleteItemAnexo(string barCode)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            articulo entity = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(barCode));
            dcContextoSuPlazaDataContext.articulo.DeleteOnSubmit(entity);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void deleteItemToKit(string barCodeKit, string barCodeItem)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            kit_articulos entity = dcContextoSuPlazaDataContext.kit_articulos.FirstOrDefault((kit_articulos k) => k.cod_barras_kit.Equals(barCodeKit) && k.cod_barras_pro.Equals(barCodeItem));
            dcContextoSuPlazaDataContext.kit_articulos.DeleteOnSubmit(entity);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public bool existInternalCode(string internalCode)
        {
            return new dcContextoSuPlazaDataContext().articulo.FirstOrDefault((articulo a) => a.cod_interno.Equals(internalCode)) != null;
        }

        public bool existArticuloKit(string barCode)
        {
            return new dcContextoSuPlazaDataContext().articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(barCode) && a.kit) != null;
        }

        public void deleteItemAsociado(string barCode)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            articulo entity = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(barCode));
            articulos entity2 = dcContextoSuPlazaDataContext.articulos.FirstOrDefault((articulos a) => a.cod_barras.Equals(barCode));
            dcContextoSuPlazaDataContext.articulos.DeleteOnSubmit(entity2);
            dcContextoSuPlazaDataContext.articulo.DeleteOnSubmit(entity);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }
    }
}
