using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
    public class RepositorioOfertas : IOfertas
    {
        public Guid insertarOferta(oferta offer)
        {
            oferta oferta = new oferta();
            if (offer.fecha_ini > offer.fecha_fin)
            {
                throw new Exception("El rango de fechas no es válido.");
            }
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            Expression<Func<oferta, bool>> expression = (oferta p) => p.id_oferta == offer.id_oferta;
            oferta = dcContextoSuPlazaDataContext.oferta.Where(expression.Compile()).FirstOrDefault();
            if (oferta != null)
            {
                oferta.status_oferta = RecursosObjects.Disponible;
                oferta.descripcion = RecursosObjects.Disponible;
            }
            else
            {
                offer.id_oferta = Guid.NewGuid();
                long num = ((dcContextoSuPlazaDataContext.oferta.FirstOrDefault() != null) ? dcContextoSuPlazaDataContext.oferta.Max((oferta o) => o.num_oferta) : 0);
                num = (offer.num_oferta = num + 1);
                dcContextoSuPlazaDataContext.oferta.InsertOnSubmit(offer);
            }
            dcContextoSuPlazaDataContext.SubmitChanges();
            return offer.id_oferta;
        }

        public void insertaListaArticulosOferta(List<oferta_articulo> lista, Guid id_oferta)
        {
            List<oferta_articulo> list = new List<oferta_articulo>();
            new oferta();
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            Expression<Func<oferta_articulo, bool>> expression = (oferta_articulo p) => p.id_oferta == id_oferta;
            Expression<Func<oferta, bool>> expression2 = (oferta p) => p.id_oferta == id_oferta;
            list = dcContextoSuPlazaDataContext.oferta_articulo.Where(expression.Compile()).ToList();
            if (list.Count == 0)
            {
                foreach (oferta_articulo listum in lista)
                {
                    listum.fecha_registro = DateTime.Now;
                    dcContextoSuPlazaDataContext.oferta_articulo.InsertOnSubmit(listum);
                    Thread.Sleep(50);
                }
            }
            else
            {
                dcContextoSuPlazaDataContext.oferta.Where(expression2.Compile()).FirstOrDefault().status_oferta = "disponible";
                foreach (oferta_articulo listum2 in lista)
                {
                    foreach (oferta_articulo item in list)
                    {
                        if (listum2.id_oferta.Equals(item.id_oferta))
                        {
                            item.fecha_registro = DateTime.Now;
                            item.precio_oferta = listum2.precio_oferta;
                            Thread.Sleep(50);
                            break;
                        }
                    }
                }
            }
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void updateOffer(oferta p)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            if (p.fecha_ini > p.fecha_fin)
            {
                throw new Exception("El rango de fechas no es válido.");
            }
            oferta oferta = dcContextoSuPlazaDataContext.oferta.FirstOrDefault((oferta o) => o.id_oferta.Equals(p.id_oferta));
            if (oferta != null)
            {
                oferta.descripcion = p.descripcion;
                oferta.fecha_ini = p.fecha_ini;
                oferta.fecha_fin = p.fecha_fin;
                oferta.status_oferta = p.status_oferta;
                oferta.user_name = p.user_name;
                dcContextoSuPlazaDataContext.SubmitChanges();
            }
        }

        public void updateOfferDetail(List<oferta_articulo> NewOfferDetail, Guid OfferID)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            foreach (oferta_articulo item in NewOfferDetail)
            {
                oferta_articulo oferta_articulo = dcContextoSuPlazaDataContext.oferta_articulo.FirstOrDefault((oferta_articulo i) => i.id_oferta.Equals(OfferID) && i.cod_barras.Equals(item.cod_barras));
                if (oferta_articulo != null)
                {
                    oferta_articulo.precio_oferta = item.precio_oferta;
                    oferta_articulo.status_oferta = item.status_oferta;
                    oferta_articulo.fecha_cancelacion = item.fecha_cancelacion;
                    oferta_articulo.fecha_registro = DateTime.Now;
                }
                else
                {
                    oferta_articulo = new oferta_articulo
                    {
                        id_oferta = OfferID,
                        cod_barras = item.cod_barras,
                        precio_oferta = item.precio_oferta,
                        status_oferta = item.status_oferta,
                        fecha_cancelacion = item.fecha_cancelacion,
                        fecha_registro = DateTime.Now
                    };
                    dcContextoSuPlazaDataContext.oferta_articulo.InsertOnSubmit(oferta_articulo);
                }
                dcContextoSuPlazaDataContext.SubmitChanges();
                Thread.Sleep(50);
            }
        }

        public List<VisorOfertasExtended> listaArticulosVisorOfertaById(Guid id)
        {
            try
            {
                List<VisorOfertasExtended> list = new List<VisorOfertasExtended>();
                dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                foreach (var item in (from o in dcContextoSuPlazaDataContext.oferta
                                      join oa in dcContextoSuPlazaDataContext.oferta_articulo on o.id_oferta equals oa.id_oferta
                                      join a in dcContextoSuPlazaDataContext.articulo on oa.cod_barras equals a.cod_barras
                                      join u in dcContextoSuPlazaDataContext.unidad_medida on a.id_unidad equals u.id_unidad
                                      where o.id_oferta == id
                                      select new
                                      {
                                          cod_barras = a.cod_barras,
                                          descripcion_larga = a.descripcion,
                                          precio_oferta = oa.precio_oferta,
                                          fecha_emision = o.fecha_ini,
                                          user = o.user_name
                                      }).ToList())
                {
                    VisorOfertasExtended visorOfertasExtended = new VisorOfertasExtended();
                    visorOfertasExtended.cod_barras = item.cod_barras;
                    visorOfertasExtended.descripcion_larga = item.descripcion_larga;
                    visorOfertasExtended.fecha_emision = item.fecha_emision.ToString("dd/MM/yyyy");
                    visorOfertasExtended.usuario = item.user;
                    visorOfertasExtended.precio_oferta = item.precio_oferta;
                    list.Add(visorOfertasExtended);
                }
                return list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }
        }

        public void eliminarArticuloOfertaByCodBarras(string cod_barras, Guid id_oferta)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            new oferta_articulo();
            Expression<Func<oferta_articulo, bool>> expression = (oferta_articulo p) => p.cod_barras.Equals(cod_barras) & p.id_oferta.Equals(id_oferta);
            oferta_articulo oferta_articulo = dcContextoSuPlazaDataContext.oferta_articulo.Where(expression.Compile()).FirstOrDefault();
            oferta_articulo.status_oferta = "cancelada";
            oferta_articulo.fecha_registro = DateTime.Now;
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void cancelarOferta(Guid id_oferta)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            new oferta();
            Expression<Func<oferta, bool>> expression = (oferta p) => p.id_oferta.Equals(id_oferta);
            oferta oferta = dcContextoSuPlazaDataContext.oferta.Where(expression.Compile()).FirstOrDefault();
            oferta.status_oferta = RecursosObjects.Cancelado;
            oferta.fecha_cancelacion = DateTime.Now;
            dcContextoSuPlazaDataContext.SubmitChanges();
            foreach (oferta_articulo item in dcContextoSuPlazaDataContext.oferta_articulo.Where((oferta_articulo i) => i.id_oferta.Equals(id_oferta)).ToList())
            {
                item.status_oferta = "cancelada";
                item.fecha_registro = DateTime.Now;
                item.fecha_cancelacion = DateTime.Now;
                Thread.Sleep(10);
            }
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void deleteItemOffer(Guid offerId, string barCode)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            oferta_articulo oferta_articulo = dcContextoSuPlazaDataContext.oferta_articulo.FirstOrDefault((oferta_articulo e) => e.id_oferta.Equals(offerId) && e.cod_barras.Equals(barCode));
            oferta_articulo.fecha_registro = DateTime.Now;
            oferta_articulo.fecha_cancelacion = DateTime.Now;
            oferta_articulo.status_oferta = "cancelada";
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public void printLabel(Guid offerId, string barCode)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            oferta_articulo entity = dcContextoSuPlazaDataContext.oferta_articulo.FirstOrDefault((oferta_articulo e) => e.id_oferta.Equals(offerId) && e.cod_barras.Equals(barCode));
            dcContextoSuPlazaDataContext.oferta_articulo.DeleteOnSubmit(entity);
            dcContextoSuPlazaDataContext.SubmitChanges();
        }

        public List<oferta> listaOfertaSuspendida()
        {
            try
            {
                new List<oferta>();
                List<oferta> list = new List<oferta>();
                Expression<Func<oferta, bool>> expression = (oferta p) => p.status_oferta == "suspendida";
                using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
                {
                    foreach (oferta item in dcContextoSuPlazaDataContext.oferta.Where(expression.Compile()).ToList())
                    {
                        oferta oferta = new oferta();
                        oferta.id_oferta = item.id_oferta;
                        oferta.descripcion = item.descripcion + " => " + item.fecha_ini.ToString("dd/MM/yyyy") + " - " + item.fecha_fin.ToString("dd/MM/yyyy");
                        list.Add(oferta);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }
        }

        public oferta getOffer(Guid p)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            oferta offer = dcContextoSuPlazaDataContext.oferta.FirstOrDefault((oferta o) => o.id_oferta.Equals(p));
            if (offer != null)
            {
                offer.oferta_articulo.AddRange(dcContextoSuPlazaDataContext.oferta_articulo.Where((oferta_articulo oa) => oa.oferta.Equals(offer)));
            }
            return offer;
        }

        public oferta getLastOffer()
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.oferta.OrderByDescending((oferta o) => o.fecha_oferta).FirstOrDefault((oferta f) => f.status_oferta.Equals("suspendida"));
        }

        public bool existOffer(Guid id)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.oferta.FirstOrDefault((oferta o) => o.id_oferta.Equals(id)) != null;
        }

        public bool isSuspendedOffer(Guid id)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            return dcContextoSuPlazaDataContext.oferta.FirstOrDefault((oferta o) => o.id_oferta.Equals(id) && o.status_oferta.Equals("suspendida")) != null;
        }
    }
}
