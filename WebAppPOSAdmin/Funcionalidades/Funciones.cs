using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Util.RecursoPedidos;

namespace WebAppPOSAdmin.Funcionalidades
{
	public class Funciones
	{
		public List<ArticuloExtended> getArticulosSuspendidos()
		{
			new List<ArticuloExtended>();
			return ((IArticulos)new RepositorioArticulos()).getArticulosSuspendidos();
		}

		public bool eliminarArticulosSuspendidos(List<ArticuloExtended> lista)
		{
			return ((IArticulos)new RepositorioArticulos()).eliminarSuspencionesfrmActualizarPrecio(lista);
		}

		public List<VisorArticuloExtended> getListagetArticuloVisor(int id)
		{
			new List<VisorArticuloExtended>();
			return ((IArticulos)new RepositorioArticulos()).getArticuloVisor(id);
		}

		public List<OfertaArticuloExtended> findAndGetItems(Guid id_proveedor)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			List<OfertaArticuloExtended> results = (from a in dcContextoSuPlazaDataContext.articulo
													where a.id_proveedor.Equals(id_proveedor) && a.tipo_articulo.Equals("principal")
													orderby a.descripcion
													select new OfertaArticuloExtended
													{
														cod_barras = a.cod_barras,
														descripcion_larga = a.descripcion,
														unidad = a.unidad_medida.descripcion,
														precio_venta = a.precio_venta,
														precio_oferta = 0.0m,
														kit = a.kit
													} into a
													orderby a.descripcion_larga
													select a).ToList();
			return findOtherOffers(results);
		}

		public List<OfertaArticuloExtended> findAndGetItems(string desc)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			List<OfertaArticuloExtended> results = (from a in dcContextoSuPlazaDataContext.articulo
													where SqlMethods.Like(a.descripcion, $"%{desc}%") && a.tipo_articulo.Equals("principal")
													orderby a.descripcion
													select new OfertaArticuloExtended
													{
														cod_barras = a.cod_barras,
														descripcion_larga = a.descripcion,
														unidad = a.unidad_medida.descripcion,
														precio_venta = a.precio_venta,
														precio_oferta = 0.0m,
														kit = a.kit
													} into a
													orderby a.descripcion_larga
													select a).ToList();
			return findOtherOffers(results);
		}

		public List<OfertaArticuloExtended> findAndGetItems(Guid id_proveedor, string desc)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			List<OfertaArticuloExtended> results = (from a in dcContextoSuPlazaDataContext.articulo
													where a.id_proveedor.Equals(id_proveedor) && SqlMethods.Like(a.descripcion, $"%{desc}%") && a.tipo_articulo.Equals("principal")
													orderby a.descripcion
													select new OfertaArticuloExtended
													{
														cod_barras = a.cod_barras,
														descripcion_larga = a.descripcion,
														unidad = a.unidad_medida.descripcion,
														precio_venta = a.precio_venta,
														precio_oferta = 0.0m,
														kit = a.kit
													} into a
													orderby a.descripcion_larga
													select a).ToList();
			return findOtherOffers(results);
		}

		private List<OfertaArticuloExtended> findOtherOffers(List<OfertaArticuloExtended> results)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (OfertaArticuloExtended offer in results)
			{
				oferta_articulo oferta_articulo = (from oa in dcContextoSuPlazaDataContext.oferta_articulo
												   orderby oa.fecha_registro descending
												   where oa.cod_barras.Equals(offer.cod_barras) && oa.oferta.fecha_ini <= DateTime.Now && oa.oferta.fecha_fin >= DateTime.Now && oa.status_oferta.Equals("disponible")
												   select oa).FirstOrDefault();
				if (oferta_articulo != null)
				{
					offer.descripcion_larga += $" *** Ofertado en #{oferta_articulo.oferta.num_oferta} ***";
				}
			}
			return results;
		}

		public List<OfertaArticuloExtended> recoveryOfferDetail(Guid id)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from a in dcContextoSuPlazaDataContext.oferta_articulo
					where a.id_oferta.Equals(id)
					orderby a.articulo.descripcion
					select new OfertaArticuloExtended
					{
						cod_barras = a.cod_barras,
						descripcion_larga = a.articulo.descripcion,
						unidad = a.articulo.unidad_medida.descripcion,
						precio_venta = a.articulo.precio_venta,
						precio_oferta = a.precio_oferta,
						kit = a.articulo.kit
					}).ToList();
		}

		public List<VisorOfertasExtended> listaVisorOfertaById(Guid id)
		{
			new List<VisorOfertasExtended>();
			return ((IOfertas)new RepositorioOfertas()).listaArticulosVisorOfertaById(id);
		}

		public Meses[] getListaMeses()
		{
			return new Meses[12]
			{
			new Meses
			{
				idMes = 1,
				Mes = "Enero"
			},
			new Meses
			{
				idMes = 2,
				Mes = "Febrero"
			},
			new Meses
			{
				idMes = 3,
				Mes = "Marzo"
			},
			new Meses
			{
				idMes = 4,
				Mes = "Abril"
			},
			new Meses
			{
				idMes = 5,
				Mes = "Mayo"
			},
			new Meses
			{
				idMes = 6,
				Mes = "Junio"
			},
			new Meses
			{
				idMes = 7,
				Mes = "Julio"
			},
			new Meses
			{
				idMes = 8,
				Mes = "Agosto"
			},
			new Meses
			{
				idMes = 9,
				Mes = "Septiembre"
			},
			new Meses
			{
				idMes = 10,
				Mes = "Octubre"
			},
			new Meses
			{
				idMes = 11,
				Mes = "Noviembre"
			},
			new Meses
			{
				idMes = 12,
				Mes = "Diciembre"
			}
			};
		}

		public DiasPedir[] getDiasPedir()
		{
			return new DiasPedir[24]
			{
			new DiasPedir
			{
				idPedir = 1,
				pedir = "5"
			},
			new DiasPedir
			{
				idPedir = 2,
				pedir = "10"
			},
			new DiasPedir
			{
				idPedir = 3,
				pedir = "15"
			},
			new DiasPedir
			{
				idPedir = 4,
				pedir = "20"
			},
			new DiasPedir
			{
				idPedir = 5,
				pedir = "25"
			},
			new DiasPedir
			{
				idPedir = 6,
				pedir = "30"
			},
			new DiasPedir
			{
				idPedir = 7,
				pedir = "35"
			},
			new DiasPedir
			{
				idPedir = 8,
				pedir = "40"
			},
			new DiasPedir
			{
				idPedir = 9,
				pedir = "45"
			},
			new DiasPedir
			{
				idPedir = 10,
				pedir = "50"
			},
			new DiasPedir
			{
				idPedir = 11,
				pedir = "55"
			},
			new DiasPedir
			{
				idPedir = 12,
				pedir = "60"
			},
			new DiasPedir
			{
				idPedir = 13,
				pedir = "65"
			},
			new DiasPedir
			{
				idPedir = 14,
				pedir = "70"
			},
			new DiasPedir
			{
				idPedir = 15,
				pedir = "75"
			},
			new DiasPedir
			{
				idPedir = 16,
				pedir = "80"
			},
			new DiasPedir
			{
				idPedir = 17,
				pedir = "85"
			},
			new DiasPedir
			{
				idPedir = 18,
				pedir = "90"
			},
			new DiasPedir
			{
				idPedir = 19,
				pedir = "95"
			},
			new DiasPedir
			{
				idPedir = 20,
				pedir = "100"
			},
			new DiasPedir
			{
				idPedir = 21,
				pedir = "105"
			},
			new DiasPedir
			{
				idPedir = 22,
				pedir = "110"
			},
			new DiasPedir
			{
				idPedir = 23,
				pedir = "115"
			},
			new DiasPedir
			{
				idPedir = 24,
				pedir = "120"
			}
			};
		}

		public TipoPlan[] getTipoPlan()
		{
			return new TipoPlan[4]
			{
			new TipoPlan
			{
				idPlan = 1,
				tipoPlan = "Año Actual"
			},
			new TipoPlan
			{
				idPlan = 2,
				tipoPlan = "Año Pasado"
			},
			new TipoPlan
			{
				idPlan = 3,
				tipoPlan = "Promedio"
			},
			new TipoPlan
			{
				idPlan = 4,
				tipoPlan = "Historial"
			}
			};
		}

		public List<PedidoExtended> listaPedidosArticuloByIdPedido(Guid id_pedido)
		{
			new List<PedidoExtended>();
			return ((IPedidos)new RepositorioPedidos()).getListaArticulosByIdPedido(id_pedido);
		}
	}
}