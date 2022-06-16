using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioPedidos : IPedidos
	{
		public enum Plan
		{
			CurrentYear,
			LastYear,
			Average
		}

		public pedido pedidoSuspendido(Guid id_pedido)
		{
			try
			{
				pedido result = new pedido();
				Expression<Func<pedido, bool>> expression = (pedido p) => p.id_pedido == id_pedido;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					result = dcContextoSuPlazaDataContext.pedido.Where(expression.Compile()).FirstOrDefault();
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public pedido pedidoPendienteById(Guid id)
		{
			try
			{
				pedido result = new pedido();
				Expression<Func<pedido, bool>> expression = (pedido p) => p.id_pedido == id;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					result = dcContextoSuPlazaDataContext.pedido.Where(expression.Compile()).FirstOrDefault();
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public Guid insertarPedido(pedido p)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			if (dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido o) => o.id_pedido.Equals(p.id_pedido)) != null)
			{
				throw new Exception("El Pedido ya fue registrado.");
			}
			p.fecha_pedido = DateTime.Now;
			long num = ((dcContextoSuPlazaDataContext.pedido.FirstOrDefault() != null) ? dcContextoSuPlazaDataContext.pedido.Max((pedido e) => e.num_pedido) : 0);
			num = (p.num_pedido = num + 1);
			dcContextoSuPlazaDataContext.pedido.InsertOnSubmit(p);
			dcContextoSuPlazaDataContext.SubmitChanges();
			return p.id_pedido;
		}

		public void insertarPedidoArticulo(pedido_articulo _pedido)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			dcContextoSuPlazaDataContext.pedido_articulo.InsertOnSubmit(_pedido);
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void actualizarPedidoArticulo(List<pedido_articulo> _pedido, Guid id_pedido, string estado)
		{
			Expression<Func<pedido_articulo, bool>> expression = (pedido_articulo p) => p.id_pedido == id_pedido;
			new List<pedido_articulo>();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			if (!string.IsNullOrEmpty(estado))
			{
				Expression<Func<pedido, bool>> expression2 = (pedido p) => p.id_pedido == id_pedido;
				new pedido();
				dcContextoSuPlazaDataContext.pedido.Where(expression2.Compile()).FirstOrDefault().status_pedido = estado;
			}
			foreach (pedido_articulo item in dcContextoSuPlazaDataContext.pedido_articulo.Where(expression.Compile()).ToList())
			{
				foreach (pedido_articulo item2 in _pedido)
				{
					if (item.cod_barras == item2.cod_barras)
					{
						decimal num = (item.cantidad = item2.cantidad);
						break;
					}
				}
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public List<PedidoExtended> getListaArticulosByIdPedido(Guid id_pedido)
		{
			List<PedidoExtended> list = new List<PedidoExtended>();
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					var list2 = (from u in dcContextoSuPlazaDataContext.unidad_medida
								 join a in dcContextoSuPlazaDataContext.articulo on u.id_unidad equals a.id_unidad
								 join pa in dcContextoSuPlazaDataContext.pedido_articulo on a.cod_barras equals pa.cod_barras
								 where pa.id_pedido == id_pedido
								 select new
								 {
									 cod_barras = pa.cod_barras,
									 descripcion_larga = a.descripcion,
									 unidad = u.descripcion,
									 pidio = pa.cantidad
								 }).ToList();
					if (list2 != null)
					{
						foreach (var item in list2)
						{
							PedidoExtended pedidoExtended = new PedidoExtended();
							pedidoExtended.cod_barras = item.cod_barras;
							pedidoExtended.descripcion_larga = item.descripcion_larga;
							pedidoExtended.no_piezas = item.pidio;
							pedidoExtended.unidad = item.unidad;
							list.Add(pedidoExtended);
						}
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

		public void eliminarPedidoById(Guid id_pedido)
		{
			eliminarListaPedidoArticulosById(id_pedido);
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			Expression<Func<pedido, bool>> expression = (pedido p) => p.id_pedido == id_pedido && p.status_pedido.Equals("pendiente");
			pedido entity = dcContextoSuPlazaDataContext.pedido.Where(expression.Compile()).FirstOrDefault();
			dcContextoSuPlazaDataContext.pedido.DeleteOnSubmit(entity);
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void eliminarListaPedidoArticulosById(Guid id_pedido)
		{
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			Expression<Func<pedido_articulo, bool>> expression = (pedido_articulo c) => c.id_pedido == id_pedido;
			List<pedido_articulo> list = dcContextoSuPlazaDataContext.pedido_articulo.Where(expression.Compile()).ToList();
			if (list == null)
			{
				return;
			}
			foreach (pedido_articulo item in list)
			{
				dcContextoSuPlazaDataContext.pedido_articulo.DeleteOnSubmit(item);
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void autorizarPedido(Guid id_pedido)
		{
			Expression<Func<pedido, bool>> expression = (pedido p) => p.id_pedido == id_pedido;
			new pedido();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			pedido pedido = dcContextoSuPlazaDataContext.pedido.Where(expression.Compile()).FirstOrDefault();
			pedido.status_pedido = "autorizado";
			pedido.fecha_autorizado = DateTime.Now;
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public void OrderAuthorized(Guid OrderID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			pedido p = dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido pe) => pe.id_pedido.Equals(OrderID));
			dcContextoSuPlazaDataContext.orden.InsertOnSubmit(new orden
			{
				id_pedido = p.id_pedido,
				num_pedido = p.num_pedido,
				fecha_pedido = p.fecha_pedido,
				id_proveedor = p.id_proveedor,
				status_pedido = p.status_pedido,
				no_dias = p.no_dias,
				fecha_autorizado = p.fecha_autorizado,
				plan = p.plan,
				anio = p.anio,
				mes = p.mes,
				fecha_registro = DateTime.Now
			});
			dcContextoSuPlazaDataContext.SubmitChanges();
			foreach (pedido_articulo pa2 in dcContextoSuPlazaDataContext.pedido_articulo.Where((pedido_articulo pa) => pa.pedido.Equals(p)).ToList())
			{
				dcContextoSuPlazaDataContext.orden_articulo.InsertOnSubmit(new orden_articulo
				{
					id_pedido = pa2.id_pedido,
					no_articulo = pa2.no_articulo,
					cod_barras = pa2.cod_barras,
					cod_anexo = pa2.cod_anexo,
					cantidad = pa2.cantidad,
					precio_articulo = pa2.precio_articulo,
					por_surtir = pa2.cantidad,
					por_surtir_pzas = pa2.cantidad * dcContextoSuPlazaDataContext.articulo.FirstOrDefault((articulo a) => a.cod_barras.Equals(pa2.cod_anexo)).cantidad_um,
					fecha_registro = DateTime.Now
				});
				dcContextoSuPlazaDataContext.SubmitChanges();
			}
		}

		public List<PedidosSuspendidoExtended> listapedidosSuspendidos()
		{
			List<PedidosSuspendidoExtended> list = new List<PedidosSuspendidoExtended>();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (pedido item in (from p in dcContextoSuPlazaDataContext.pedido
									 orderby p.num_pedido
									 where p.status_pedido == "suspendido"
									 select p).ToList())
			{
				PedidosSuspendidoExtended pedidosSuspendidoExtended = new PedidosSuspendidoExtended();
				pedidosSuspendidoExtended.id = item.id_pedido;
				pedidosSuspendidoExtended.razon_social = "Pedido #" + item.num_pedido + " | " + item.proveedor.razon_social;
				list.Add(pedidosSuspendidoExtended);
			}
			return list;
		}

		public List<PedidoCapturaPendiente> listaPedidoArticulosPendiente(Guid id_pedido)
		{
			List<PedidoCapturaPendiente> list = new List<PedidoCapturaPendiente>();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (sp_pedido_captura_pendienteResult item in dcContextoSuPlazaDataContext.sp_pedido_captura_pendiente(id_pedido).ToList())
			{
				PedidoCapturaPendiente pedidoCapturaPendiente = new PedidoCapturaPendiente();
				pedidoCapturaPendiente.cod_barras = item.cod_barras;
				pedidoCapturaPendiente.descripcion = item.articulo;
				pedidoCapturaPendiente.unidad = item.unidad;
				pedidoCapturaPendiente.precio_costo = Convert.ToDecimal(item.precio_articulo);
				pedidoCapturaPendiente.umc = Convert.ToDecimal(item.cantidad_um);
				_ = item.cant_original;
				pedidoCapturaPendiente.cant_original = item.cant_original;
				_ = item.cantidad;
				pedidoCapturaPendiente.cantidad = item.cantidad;
				pedidoCapturaPendiente.total = 0.0m;
				pedidoCapturaPendiente.pedido_real = 0.0m;
				pedidoCapturaPendiente.existencia_cja = item.stock_cja ?? 0.000m;
				pedidoCapturaPendiente.existencia_pza = item.stock_pza ?? 0.000m;
				list.Add(pedidoCapturaPendiente);
			}
			return list;
		}

		public List<sp_pedido_anioResult> listaPedidoArticuloByIdProveedor(Guid id_proveedor, short anio, short mes_val, short dias_pedido)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.sp_pedido_anio(id_proveedor, anio, mes_val, (short)DateTime.DaysInMonth(anio, mes_val), dias_pedido).ToList();
		}

		public List<sp_pedido_promedioResult> listaPedidoArticuloByIdProveedorPromedio(Guid id_proveedor, short anio, short mes_val, short dias_pedido)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.sp_pedido_promedio(id_proveedor, anio, mes_val, (short)DateTime.DaysInMonth(anio, mes_val), dias_pedido).ToList();
		}

		public List<sp_pedido_suspendidoResult> getPedidoSuspendido(Guid id_pedido, int anio, int mes)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.sp_pedido_suspendido(id_pedido, (short)DateTime.DaysInMonth(anio, mes)).ToList();
		}

		public List<sp_pedido_suspendido_promedioResult> getPedidoSuspendidoPromedio(Guid id_pedido, int anio, int mes)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.sp_pedido_suspendido_promedio(id_pedido, (short)DateTime.DaysInMonth(anio, mes)).ToList();
		}

		public List<PedidosSuspendidoExtended> listapedidosPendientes()
		{
			List<PedidosSuspendidoExtended> list = new List<PedidosSuspendidoExtended>();
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (pedido item in dcContextoSuPlazaDataContext.pedido.Where((pedido p) => p.status_pedido == "pendiente").ToList())
			{
				PedidosSuspendidoExtended pedidosSuspendidoExtended = new PedidosSuspendidoExtended();
				pedidosSuspendidoExtended.id = Guid.Parse(item.id_pedido.ToString());
				pedidosSuspendidoExtended.razon_social = item.proveedor.razon_social;
				list.Add(pedidosSuspendidoExtended);
			}
			return list;
		}

		public void listaPedidoArticuloActualizar(List<pedido_articulo> lista, Guid id_pedido)
		{
			new List<pedido_articulo>();
			Expression<Func<pedido_articulo, bool>> expression = (pedido_articulo p) => p.id_pedido == id_pedido;
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (pedido_articulo item in dcContextoSuPlazaDataContext.pedido_articulo.Where(expression.Compile()).ToList())
			{
				foreach (pedido_articulo listum in lista)
				{
					if (item.cod_barras == listum.cod_barras)
					{
						decimal num = (item.cantidad = listum.cantidad);
						break;
					}
				}
			}
			dcContextoSuPlazaDataContext.SubmitChanges();
		}

		public bool existsOrder(Guid id)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido p) => p.id_pedido.Equals(id)) != null;
		}

		public pedido getOrderById(Guid id)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido p) => p.id_pedido.Equals(id));
		}

		public long getOrderNumber(Guid OrderID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido p) => p.id_pedido.Equals(OrderID)).num_pedido;
		}

		public string getOrderProvider(Guid OrderID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.pedido.FirstOrDefault((pedido p) => p.id_pedido.Equals(OrderID)).proveedor.razon_social;
		}
	}
}
