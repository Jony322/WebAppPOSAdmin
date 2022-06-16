using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Scripts;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioArticulos : IArticulos
	{
		public void saveFirstItem(articulo p)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			p.fecha_registro = DateTime.Now;
			articulo articulo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo i) => i.cod_barras.Equals(p.cod_barras));
			if (articulo == null)
			{
				p.last_update_inventory = p.fecha_registro;
				new Procedures().DeleteArticulos(p.cod_barras);
				if (p.cod_asociado != null && dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo i) => i.cod_barras.Equals(p.cod_asociado)) == null)
				{
					throw new Exception("El articulo a asociar o anexar no encuentra al principal.");
				}
				p.stock = 0m;
				dcContextoSuPlazaDataContext.articulo.InsertOnSubmit(p);
				dcContextoSuPlazaDataContext.SubmitChanges();
				if (p.tipo_articulo.Equals("principal") && dcContextoSuPlazaDataContext.estadistica.FirstOrDefault((estadistica est) => est.cod_barras.Equals(p.cod_barras) && est.anio.Equals((short)DateTime.Now.Year)) == null)
				{
					estadistica entity = new estadistica
					{
						cod_barras = p.cod_barras,
						anio = (short)DateTime.Now.Year
					};
					dcContextoSuPlazaDataContext.estadistica.InsertOnSubmit(entity);
					dcContextoSuPlazaDataContext.SubmitChanges();
				}
			}
			else
			{
				articulo.cod_asociado = p.cod_asociado;
				articulo.id_clasificacion = p.id_clasificacion;
				articulo.cod_interno = p.cod_interno;
				articulo.descripcion = p.descripcion;
				articulo.descripcion_corta = p.descripcion_corta;
				articulo.cantidad_um = p.cantidad_um;
				articulo.id_unidad = p.id_unidad;
				articulo.id_proveedor = p.id_proveedor;
				articulo.precio_compra = p.precio_compra;
				articulo.utilidad = p.utilidad;
				articulo.precio_venta = p.precio_venta;
				articulo.tipo_articulo = p.tipo_articulo;
				articulo.stock = p.stock;
				articulo.stock_min = p.stock_min;
				articulo.stock_max = p.stock_max;
				articulo.iva = p.iva;
				articulo.kit_fecha_ini = p.kit_fecha_ini;
				articulo.kit_fecha_fin = p.kit_fecha_fin;
				articulo.articulo_disponible = p.articulo_disponible;
				articulo.kit = p.kit;
				articulo.fecha_registro = p.fecha_registro;
				articulo.cve_producto = p.cve_producto;
				dcContextoSuPlazaDataContext.SubmitChanges();
				affectedAsociados(articulo);
				affectedAnexos(articulo);
			}
		}

		private void affectedAsociados(articulo au)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (articulo item in dcContextoSuPlazaDataContext.articulo.Where((articulo a) => a.cod_asociado.Equals(au.cod_barras) && a.tipo_articulo.Equals("asociado")).ToList())
			{
				item.id_clasificacion = au.id_clasificacion;
				item.cod_interno = null;
				item.descripcion = au.descripcion;
				item.descripcion_corta = au.descripcion_corta;
				item.cantidad_um = au.cantidad_um;
				item.id_unidad = au.id_unidad;
				item.id_proveedor = au.id_proveedor;
				item.precio_compra = au.precio_compra;
				item.utilidad = au.utilidad;
				item.precio_venta = au.precio_venta;
				item.tipo_articulo = "asociado";
				item.stock = au.stock;
				item.stock_min = au.stock_min;
				item.stock_max = au.stock_max;
				item.iva = au.iva;
				item.kit_fecha_ini = null;
				item.kit_fecha_fin = null;
				item.articulo_disponible = au.articulo_disponible;
				item.kit = false;
				item.fecha_registro = DateTime.Now;
				item.cve_producto = au.cve_producto;
				dcContextoSuPlazaDataContext.SubmitChanges();
				Thread.Sleep(10);
			}
		}

		private void affectedAnexos(articulo au)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (articulo item in dcContextoSuPlazaDataContext.articulo.Where((articulo a) => a.cod_asociado.Equals(au.cod_barras) && a.tipo_articulo.Equals("anexo")).ToList())
			{
				item.id_clasificacion = au.id_clasificacion;
				item.id_proveedor = au.id_proveedor;
				item.tipo_articulo = "anexo";
				item.fecha_registro = DateTime.Now;
				item.iva = au.iva;
				item.cve_producto = au.cve_producto;
				item.precio_compra = item.cantidad_um * au.precio_compra;
				item.precio_venta = Math.Floor(Math.Round(item.precio_compra * (1.0m + item.utilidad / 100.0m) * (1.0m + item.iva) * 10.0m)) / 10.0m;
				dcContextoSuPlazaDataContext.SubmitChanges();
				Thread.Sleep(10);
			}
		}

		public void insertarArticulo(articulo a)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			if (dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo i) => i.cod_barras.Equals(a.cod_barras)) != null)
			{
				throw new Exception("El código de barras ya existe");
			}
			a.fecha_registro = DateTime.Now;
			a.last_update_inventory = a.fecha_registro;
			if (a.kit)
			{
				a.id_proveedor = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor p) => p.razon_social.Equals("Proveedor Alterno")).id_proveedor;
				a.id_clasificacion = dcContextoSuPlazaDataContext.vw_clasificacion.FirstOrDefault((vw_clasificacion c) => c.linea.Equals("OBSOLETOS")).id_linea;
			}
			dcContextoSuPlazaDataContext.articulo.InsertOnSubmit(a);
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void save(articulo a)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			articulo articulo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo i) => i.cod_barras.Equals(a.cod_barras));
			articulo.cod_asociado = a.cod_asociado;
			articulo.id_clasificacion = a.id_clasificacion;
			articulo.cod_interno = a.cod_interno;
			articulo.descripcion = a.descripcion;
			articulo.descripcion_corta = a.descripcion_corta;
			articulo.cantidad_um = a.cantidad_um;
			articulo.id_unidad = a.id_unidad;
			articulo.id_proveedor = a.id_proveedor;
			articulo.precio_compra = a.precio_compra;
			articulo.utilidad = a.utilidad;
			articulo.precio_venta = a.precio_venta;
			articulo.tipo_articulo = a.tipo_articulo;
			articulo.stock = a.stock;
			articulo.stock_min = a.stock_min;
			articulo.stock_max = a.stock_max;
			articulo.iva = a.iva;
			articulo.kit_fecha_ini = a.kit_fecha_ini;
			articulo.kit_fecha_fin = a.kit_fecha_fin;
			articulo.articulo_disponible = a.articulo_disponible;
			articulo.kit = a.kit;
			articulo.fecha_registro = DateTime.Now;
			articulo.cve_producto = a.cve_producto;
			if (a.kit)
			{
				articulo.id_proveedor = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor p) => p.razon_social.Equals("Proveedor Alterno")).id_proveedor;
				articulo.id_clasificacion = dcContextoSuPlazaDataContext.vw_clasificacion.FirstOrDefault((vw_clasificacion c) => c.linea.Equals("OBSOLETOS")).id_linea;
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void CreateAsociado(articulo a)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			a.fecha_registro = DateTime.Now;
			a.last_update_inventory = a.fecha_registro;
			dcContextoSuPlazaDataContext.articulo.InsertOnSubmit(a);
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public bool eliminarArticulo(articulo artTemp)
		{
			throw new NotImplementedException();
		}

		public void actualizarArticulo(articulo artTemp)
		{
			new articulo();
			Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_barras == artTemp.cod_barras;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			articulo articulo = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
			articulo.cod_interno = artTemp.cod_interno;
			articulo.descripcion = artTemp.descripcion;
			articulo.descripcion_corta = artTemp.descripcion_corta;
			articulo.id_clasificacion = artTemp.id_clasificacion;
			articulo.id_proveedor = artTemp.id_proveedor;
			articulo.id_unidad = artTemp.id_unidad;
			articulo.cantidad_um = artTemp.cantidad_um;
			articulo.precio_compra = artTemp.precio_compra;
			articulo.precio_venta = artTemp.precio_venta;
			articulo.tipo_articulo = artTemp.tipo_articulo;
			articulo.stock = artTemp.stock;
			articulo.iva = artTemp.iva;
			articulo.stock_max = artTemp.stock_max;
			articulo.stock_min = artTemp.stock_min;
			articulo.utilidad = artTemp.utilidad;
			articulo.fecha_registro = DateTime.Now;
			articulo.cve_producto = artTemp.cve_producto;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public articulo getArticuloById(object idTemp)
		{
			articulo artTemp = new articulo();
			clasificacion clasificacion = new clasificacion();
			proveedor proveedor = new proveedor();
			unidad_medida unidad_medida = new unidad_medida();
			Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_barras.Equals(idTemp);
			using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
			{
				artTemp = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
				if (artTemp != null)
				{
					clasificacion = dcContextoSuPlazaDataContext.clasificacion.Where((clasificacion p) => p.id_clasificacion == artTemp.id_clasificacion).FirstOrDefault();
					proveedor = dcContextoSuPlazaDataContext.proveedor.Where((proveedor p) => p.id_proveedor == artTemp.id_proveedor).FirstOrDefault();
					unidad_medida = dcContextoSuPlazaDataContext.unidad_medida.Where((unidad_medida p) => p.id_unidad == artTemp.id_unidad).FirstOrDefault();
					artTemp.clasificacion = clasificacion;
					artTemp.proveedor = proveedor;
					artTemp.unidad_medida = unidad_medida;
				}
			}
			return artTemp;
		}

		public List<VisorArticuloExtended> getArticuloVisor(int id)
		{
			List<VisorArticuloExtended> list = new List<VisorArticuloExtended>();
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			try
			{
				foreach (var item in (from a in dcContextoSuPlazaDataContext.articulo
									  where a.id_clasificacion == (long)id && a.tipo_articulo.Equals("principal")
									  join p in dcContextoSuPlazaDataContext.proveedor on a.id_proveedor equals p.id_proveedor
									  join u in dcContextoSuPlazaDataContext.unidad_medida on a.id_unidad equals u.id_unidad
									  orderby a.tipo_articulo.Equals("principal")
									  select new
									  {
										  cod_barras = a.cod_barras,
										  cod_interno = a.cod_interno,
										  descripcion_larga = a.descripcion,
										  tipo_articulo = a.tipo_articulo,
										  unidad = u.descripcion,
										  proveedorC = p.razon_social
									  }).ToList())
				{
					VisorArticuloExtended visorArticuloExtended = new VisorArticuloExtended();
					visorArticuloExtended.cod_barras = item.cod_barras;
					visorArticuloExtended.cod_interno = item.cod_interno;
					visorArticuloExtended.descripcion_larga = item.descripcion_larga;
					visorArticuloExtended.proveedor = item.proveedorC;
					visorArticuloExtended.tipo_articulo = item.tipo_articulo;
					visorArticuloExtended.unidad = item.unidad;
					list.Add(visorArticuloExtended);
				}
				return list;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public List<articulo> getArticulosPrincipales()
		{
			return new dcContextoSuPlazaDataContext().articulo.Where((articulo a) => a.tipo_articulo.Equals("principal") || a.tipo_articulo.Equals("anexo")).ToList();
		}

		public articulo findItem(string find)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(find) || a.cod_interno.Equals(find));
		}

		public List<articulo> findArticulosByBarCode(string barCode, bool kit)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.articulo.Where((articulo a) => (a.tipo_articulo.Equals("principal") || a.tipo_articulo.Equals("anexo")) && a.cod_barras.Equals(barCode) && a.kit == kit).ToList();
		}

		public List<articulo> findArticulosByInternalCode(string internalCode, bool kit)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.articulo.Where((articulo a) => (a.tipo_articulo.Equals("principal") || a.tipo_articulo.Equals("anexo")) && a.cod_interno.Equals(internalCode) && a.kit == kit).ToList();
		}

		public List<articulo> findArticulosByDescription(string desc, bool kit)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.articulo.Where((articulo a) => (a.tipo_articulo.Equals("principal") || a.tipo_articulo.Equals("anexo")) && SqlMethods.Like(a.descripcion, $"%{desc}%") && a.kit == kit).ToList();
		}

		public bool actualizarfrmActualizarPrecio(List<ArticuloExtended> lista)
		{
			bool result = false;
			try
			{
				new articulo();
				List<articulo> list = new List<articulo>();
				dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				foreach (ArticuloExtended item in lista)
				{
					Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_barras.Equals(item.cod_barras);
					articulo articulo = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
					articulo.precio_compra = item.precio_compra;
					articulo.precio_venta = item.precio_venta;
					articulo.utilidad = item.utilidad;
					articulo.iva = item.iva;
					string cod = item.cod_barras;
					list = dcContextoSuPlazaDataContext.articulo.Where((articulo c) => c.tipo_articulo == "asociado" && c.cod_asociado == cod).ToList();
					if (list == null)
					{
						continue;
					}
					foreach (articulo item2 in list)
					{
						item2.precio_compra = item.precio_compra;
						item2.precio_venta = item.precio_venta;
						item2.utilidad = item.utilidad;
						item2.iva = item.iva;
					}
				}
				dcContextoSuPlazaDataContext.SubmitChanges();
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public void suspenderfrmActualizarPrecio(List<ArticuloExtended> lista, bool addItems)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			if (addItems)
			{
				foreach (ArticuloExtended e in lista)
				{
					precio_temporal precio_temporal = dcContextoSuPlazaDataContext.precio_temporal.FirstOrDefault((precio_temporal a) => a.cod_barras.Equals(e.cod_barras));
					if (precio_temporal != null)
					{
						precio_temporal.precio_compra = e.precio_compra;
						precio_temporal.utilidad = e.utilidad;
						precio_temporal.precio_venta = e.precio_venta;
						dcContextoSuPlazaDataContext.SubmitChanges();
					}
					else if (dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(e.cod_barras) && (!a.precio_compra.Equals(e.precio_compra) || !a.utilidad.Equals(e.utilidad) || !a.precio_venta.Equals(e.precio_venta))) != null)
					{
						dcContextoSuPlazaDataContext.precio_temporal.InsertOnSubmit(new precio_temporal
						{
							cod_barras = e.cod_barras,
							precio_compra = e.precio_compra,
							utilidad = e.utilidad,
							precio_venta = e.precio_venta,
							iva = e.iva
						});
						dcContextoSuPlazaDataContext.SubmitChanges();
					}
				}
				return;
			}
			dcContextoSuPlazaDataContext.precio_temporal.DeleteAllOnSubmit(dcContextoSuPlazaDataContext.precio_temporal);
			dcContextoSuPlazaDataContext.SubmitChanges();
			suspenderfrmActualizarPrecio(lista, addItems: true);
		}

		public List<ArticuloExtended> getArticulosSuspendidos()
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from at in dcContextoSuPlazaDataContext.precio_temporal
					orderby at.articulo.descripcion
					select new ArticuloExtended
					{
						cod_barras = at.cod_barras,
						descripcion = at.articulo.descripcion,
						descripcion_larga = at.articulo.descripcion,
						precio_compra = at.precio_compra,
						precio_venta = at.precio_venta,
						utilidad = at.utilidad,
						iva = at.iva
					}).ToList();
		}

		public bool existsItemsSuspended()
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.precio_temporal.FirstOrDefault() != null;
		}

		public bool eliminarSuspencionesfrmActualizarPrecio(List<ArticuloExtended> lista)
		{
			bool result = false;
			try
			{
				dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				foreach (ArticuloExtended item in lista)
				{
					precio_temporal precio_temporal = new precio_temporal();
					Expression<Func<precio_temporal, bool>> expression = (precio_temporal p) => p.cod_barras == item.cod_barras;
					precio_temporal = dcContextoSuPlazaDataContext.precio_temporal.Where(expression.Compile()).FirstOrDefault();
					dcContextoSuPlazaDataContext.precio_temporal.DeleteOnSubmit(precio_temporal);
				}
				dcContextoSuPlazaDataContext.SubmitChanges();
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public List<ArticuloAnexoExtended> listaArticulosAnexos(string cod_barras)
		{
			List<ArticuloAnexoExtended> list = new List<ArticuloAnexoExtended>();
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					foreach (var item in (from a in dcContextoSuPlazaDataContext.articulo
										  join u in dcContextoSuPlazaDataContext.unidad_medida on a.id_unidad equals u.id_unidad
										  where (a.cod_asociado == cod_barras) & (a.tipo_articulo == "anexo")
										  select new
										  {
											  cod = a.cod_barras,
											  cod_interno = a.cod_interno,
											  descripcion_larga = a.descripcion,
											  descripcion = u.descripcion,
											  piezas = a.cantidad_um,
											  precio_compra = a.precio_compra,
											  utilidad = a.utilidad,
											  precio_venta = a.precio_venta
										  }).ToList())
					{
						ArticuloAnexoExtended articuloAnexoExtended = new ArticuloAnexoExtended();
						articuloAnexoExtended.cod_barras = item.cod;
						articuloAnexoExtended.cod_interno = item.cod_interno;
						articuloAnexoExtended.descripcion_larga = item.descripcion_larga;
						articuloAnexoExtended.descripcion = item.descripcion;
						articuloAnexoExtended.cantidad_um = item.piezas;
						articuloAnexoExtended.precio_compra = item.precio_compra;
						articuloAnexoExtended.precio_venta = item.precio_venta;
						articuloAnexoExtended.utilidad = item.utilidad;
						list.Add(articuloAnexoExtended);
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

		public List<articulo> listaArticulosAsociados(string cod_barras)
		{
			new List<articulo>();
			try
			{
				return new dcContextoSuPlazaDataContext().articulo.Where((articulo a) => (a.cod_asociado == cod_barras) & (a.tipo_articulo == "asociado")).ToList();
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public bool eliminarArticuloAsociado(string cod_barras)
		{
			bool result = false;
			try
			{
				Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_barras == cod_barras;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					articulo entity = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
					dcContextoSuPlazaDataContext.articulo.DeleteOnSubmit(entity);
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

		public void convertirAsociadoAPrincipal(string cod_barras)
		{
			new List<articulo>();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			Expression<Func<articulo, bool>> expression = (articulo p) => (p.cod_barras == cod_barras) & (p.tipo_articulo == "asociado");
			articulo newAsociado = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
			Expression<Func<articulo, bool>> expression2 = (articulo p1) => (p1.cod_barras == newAsociado.cod_asociado) & (p1.tipo_articulo == "principal");
			articulo oldAsociado = dcContextoSuPlazaDataContext.articulo.Where(expression2.Compile()).FirstOrDefault();
			Expression<Func<articulo, bool>> expression3 = (articulo p2) => (p2.cod_asociado == oldAsociado.cod_barras) & (p2.tipo_articulo == "anexo");
			List<articulo> list = dcContextoSuPlazaDataContext.articulo.Where(expression3.Compile()).ToList();
			oldAsociado.tipo_articulo = "asociado";
			string cod_barras2 = newAsociado.cod_barras;
			oldAsociado.cod_asociado = cod_barras2;
			newAsociado.tipo_articulo = "principal";
			newAsociado.cod_asociado = null;
			string cod_barras3 = newAsociado.cod_barras;
			foreach (articulo item in list)
			{
				item.cod_asociado = cod_barras3;
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void fromAsociadoToPrincipal(string barCode)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			dcContextoSuPlazaDataContext.sp_fromAsociadoToPrincipal(barCode);
		}

		public bool insertarKitArticulo(kit_articulos kit, DateTime fecha_ini, DateTime fecha_fin)
		{
			bool result = false;
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_barras == kit.cod_barras_pro;
					articulo articulo = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
					articulo.kit_fecha_ini = fecha_ini;
					articulo.kit_fecha_fin = fecha_fin;
					dcContextoSuPlazaDataContext.kit_articulos.InsertOnSubmit(kit);
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

		public List<kitArticuloExtended> listaArticulosKit(string cod_barras)
		{
			try
			{
				List<kitArticuloExtended> list = new List<kitArticuloExtended>();
				dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				var list2 = (from k in dcContextoSuPlazaDataContext.kit_articulos
							 join a in dcContextoSuPlazaDataContext.articulo on k.cod_barras_pro equals a.cod_barras
							 where k.cod_barras_kit == cod_barras
							 select new
							 {
								 cod_barras = k.cod_barras_pro,
								 descripcion = a.descripcion,
								 fecha_ini = a.kit_fecha_ini,
								 fecha_fin = a.kit_fecha_fin
							 }).ToList();
				if (list2.Count > 0)
				{
					foreach (var item in list2)
					{
						kitArticuloExtended kitArticuloExtended = new kitArticuloExtended();
						kitArticuloExtended.cod_barras = item.cod_barras;
						kitArticuloExtended.descripcion = item.descripcion;
						kitArticuloExtended.fecha_ini = item.fecha_ini.ToString().Substring(0, 10);
						kitArticuloExtended.fecha_fin = item.fecha_fin.ToString().Substring(0, 10);
						list.Add(kitArticuloExtended);
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

		public bool eliminarArticuloDeKit(string cod_barras)
		{
			bool result = false;
			try
			{
				dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				Expression<Func<kit_articulos, bool>> expression = (kit_articulos p) => p.cod_barras_pro == cod_barras;
				kit_articulos entity = dcContextoSuPlazaDataContext.kit_articulos.Where(expression.Compile()).FirstOrDefault();
				dcContextoSuPlazaDataContext.kit_articulos.DeleteOnSubmit(entity);
				dcContextoSuPlazaDataContext.SubmitChanges();
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public List<articulo> getArticuloByCodBarrasPrincipal(string cod_barras, bool ofertado)
		{
			try
			{
				_ = (Expression<Func<articulo, bool>>)((articulo p) => p.cod_barras.StartsWith(cod_barras));
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				List<articulo> list = new List<articulo>();
				switch (ofertado)
				{
					case true:
						foreach (var item in (from a in dcContextoSuPlazaDataContext.articulo
											  where a.descripcion.ToLower().StartsWith(cod_barras.ToLower())
											  select new
											  {
												  cod_barras = a.cod_barras,
												  decripcion_larga = a.descripcion,
												  precio_venta = a.precio_venta
											  }).ToList())
						{
							articulo articulo2 = new articulo();
							articulo2.cod_barras = item.cod_barras;
							articulo2.descripcion = item.decripcion_larga;
							articulo2.precio_venta = item.precio_venta;
							list.Add(articulo2);
						}
						break;
					case false:
						foreach (var item2 in (from a in dcContextoSuPlazaDataContext.articulo
											   join oa in dcContextoSuPlazaDataContext.oferta_articulo on a.cod_barras equals oa.cod_barras into art
											   from of in art.DefaultIfEmpty()
											   where a.descripcion.ToLower().StartsWith(cod_barras.ToLower())
											   orderby a.descripcion descending
											   select new
											   {
												   cod_barras = a.cod_barras,
												   decripcion_larga = a.descripcion,
												   precio_venta = (((decimal?)of.precio_oferta == null) ? a.precio_venta : of.precio_oferta)
											   }).ToList())
						{
							articulo articulo = new articulo();
							articulo.cod_barras = item2.cod_barras;
							articulo.descripcion = item2.decripcion_larga;
							articulo.precio_venta = item2.precio_venta;
							list.Add(articulo);
						}
						break;
				}
				return list;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public List<ArticuloExtended> changePriceItem(List<ArticuloExtended> lista)
		{
			List<ArticuloExtended> list = new List<ArticuloExtended>();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (ArticuloExtended e in lista)
			{
				articulo articulo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(e.cod_barras) && (!a.precio_compra.Equals(e.precio_compra) || !a.utilidad.Equals(e.utilidad) || !a.precio_venta.Equals(e.precio_venta)));
				if (articulo != null)
				{
					articulo.precio_compra = e.precio_compra;
					articulo.utilidad = e.utilidad;
					articulo.precio_venta = e.precio_venta;
					articulo.fecha_registro = DateTime.Now;
					list.Add(new ArticuloExtended
					{
						cod_barras = articulo.cod_barras,
						descripcion = articulo.descripcion,
						unidad = articulo.unidad_medida.descripcion,
						precio_compra = articulo.precio_compra,
						utilidad = articulo.utilidad,
						precio_venta = articulo.precio_venta,
						iva = articulo.iva
					});
					Thread.Sleep(100);
					dcContextoSuPlazaDataContext.SubmitChanges();
					affectedAnexos(articulo);
					affectedAsociados(articulo);
				}
			}
			return list;
		}

		public bool IsChangedPrice(string barCode, decimal price)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(barCode)).precio_venta != price;
		}

		public articulo getArticuloByCodInterno(string cod_interno)
		{
			articulo articulo = new articulo();
			unidad_medida unidad_medida = new unidad_medida();
			try
			{
				Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_interno == cod_interno;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					articulo = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
					if (articulo != null)
					{
						Guid value = articulo.id_unidad;
						Expression<Func<unidad_medida, bool>> expression2 = (unidad_medida p) => p.id_unidad == value;
						unidad_medida = (articulo.unidad_medida = dcContextoSuPlazaDataContext.unidad_medida.Where(expression2.Compile()).FirstOrDefault());
					}
				}
				return articulo;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public articulo getArticuloByCodAsociado(string cod_asociado)
		{
			articulo articulo = new articulo();
			unidad_medida unidad_medida = new unidad_medida();
			try
			{
				Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_asociado.ToLower() == cod_asociado;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					articulo = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
					if (articulo != null)
					{
						Guid value = articulo.id_unidad;
						Expression<Func<unidad_medida, bool>> expression2 = (unidad_medida p) => p.id_unidad == value;
						unidad_medida = (articulo.unidad_medida = dcContextoSuPlazaDataContext.unidad_medida.Where(expression2.Compile()).FirstOrDefault());
					}
				}
				return articulo;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public articulo getArticuloByCodBarras(string cod_barras)
		{
			articulo articulo = new articulo();
			unidad_medida unidad_medida = new unidad_medida();
			try
			{
				Expression<Func<articulo, bool>> expression = (articulo p) => p.cod_barras == cod_barras;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					articulo = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).FirstOrDefault();
					if (articulo != null)
					{
						Guid value = articulo.id_unidad;
						Expression<Func<unidad_medida, bool>> expression2 = (unidad_medida p) => p.id_unidad == value;
						unidad_medida = (articulo.unidad_medida = dcContextoSuPlazaDataContext.unidad_medida.Where(expression2.Compile()).FirstOrDefault());
					}
				}
				return articulo;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public List<articulo> getArticuloByDescripcion(string descripcion_larga)
		{
			try
			{
				Expression<Func<articulo, bool>> expression = (articulo p) => p.descripcion.ToLower().StartsWith(descripcion_larga.ToLower());
				List<articulo> result = new List<articulo>();
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					result = dcContextoSuPlazaDataContext.articulo.Where(expression.Compile()).ToList();
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public void setObsoleteItem(string barCode)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			articulo articulo = dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(barCode));
			articulo.id_proveedor = dcContextoSuPlazaDataContext.proveedor.FirstOrDefault((proveedor p) => p.razon_social.Equals("Proveedor Alterno")).id_proveedor;
			articulo.id_clasificacion = dcContextoSuPlazaDataContext.vw_clasificacion.FirstOrDefault((vw_clasificacion c) => c.linea.Equals("OBSOLETOS")).id_linea;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public List<ArticuloExtended> mergeList(List<ArticuloExtended> list)
		{
			if (list != null)
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					foreach (ArticuloExtended a in list)
					{
						precio_temporal precio_temporal = dcContextoSuPlazaDataContext.precio_temporal.FirstOrDefault((precio_temporal p) => p.cod_barras.Equals(a.cod_barras));
						if (precio_temporal != null)
						{
							a.precio_compra = precio_temporal.precio_compra;
							a.utilidad = precio_temporal.utilidad;
							a.precio_venta = precio_temporal.precio_venta;
							a.iva = precio_temporal.iva;
						}
					}
					return list;
				}
			}
			return list;
		}

		public long getCodigoNormal()
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			pos_admin_settings setting = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
			while (true)
			{
				if (dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(setting.cod_normal)) == null)
				{
					break;
				}
				setting.cod_normal += 10L;
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
			return setting.cod_normal;
		}

		public long getCodigoPesable()
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			pos_admin_settings setting = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
			while (true)
			{
				if (dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(setting.cod_pesable)) == null)
				{
					break;
				}
				setting.cod_pesable += 1000000L;
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
			return setting.cod_pesable;
		}

		public long getCodigoNoPesable()
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			pos_admin_settings setting = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault();
			while (true)
			{
				if (dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(setting.cod_nopesable)) == null)
				{
					break;
				}
				setting.cod_nopesable += 1000000L;
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
			return setting.cod_nopesable;
		}
	}
}
