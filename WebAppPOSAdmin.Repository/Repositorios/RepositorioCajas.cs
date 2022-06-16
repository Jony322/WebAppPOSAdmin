 using System;
using System.Collections.Generic;
using System.Linq;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{
	public class RepositorioCajas : ICajas
	{
		public List<VentaArticuloExtended> listaArticulosByid(Guid id)
		{
			List<VentaArticuloExtended> list = new List<VentaArticuloExtended>();
			dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			foreach (var item in (from v in dcContextoSuPlazaDataContext.venta
								  join va in dcContextoSuPlazaDataContext.venta_articulo on v.id_venta equals va.id_venta
								  join a in dcContextoSuPlazaDataContext.articulo on va.cod_barras equals a.cod_barras
								  where va.id_venta == id
								  select new
								  {
									  cod_barras = va.cod_barras,
									  descripcion = a.descripcion,
									  cantidad = va.cantidad,
									  total = va.cantidad * va.precio_vta - va.precio_vta * va.porcent_desc
								  }).ToList())
			{
				VentaArticuloExtended ventaArticuloExtended = new VentaArticuloExtended();
				ventaArticuloExtended.cod_barras = item.cod_barras;
				ventaArticuloExtended.descripcion = item.descripcion;
				ventaArticuloExtended.cantidad = item.cantidad;
				ventaArticuloExtended.total = decimal.Round(item.total, 3);
				list.Add(ventaArticuloExtended);
			}
			return list;
		}

		public List<VentaSuspendidaExtended> getVentasSuspendidas(DateTime fecha_ini, DateTime fecha_fin, int caja, string cajero)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.id_pos.Equals(caja) && vc.vendedor.Equals(cajero) && !vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaSuspendidaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_suspencion = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaSuspendidaExtended> getVentasSuspendidas(DateTime fecha_ini, DateTime fecha_fin, int caja)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.id_pos.Equals(caja) && !vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaSuspendidaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_suspencion = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaSuspendidaExtended> getVentasSuspendidas(DateTime fecha_ini, DateTime fecha_fin, string cajero)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.vendedor.Equals(cajero) && !vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaSuspendidaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_suspencion = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaSuspendidaExtended> getVentasSuspendidas(DateTime fecha_ini, DateTime fecha_fin)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && !vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaSuspendidaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_suspencion = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(DateTime fecha_ini, DateTime fecha_fin)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(DateTime fecha_ini, DateTime fecha_fin, int id_pos)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.id_pos.Equals(id_pos) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(DateTime fecha_ini, DateTime fecha_fin, int id_pos, string vendedor)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.id_pos.Equals(id_pos) && vc.vendedor.Equals(vendedor) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(string supervisor, DateTime fecha_ini, DateTime fecha_fin, int id_pos)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.id_pos.Equals(id_pos) && vc.supervisor.Equals(supervisor) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(DateTime fecha_ini, DateTime fecha_fin, string vendedor, string supervisor)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.vendedor.Equals(vendedor) && vc.supervisor.Equals(supervisor) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(DateTime fecha_ini, DateTime fecha_fin, int id_pos, string vendedor, string supervisor)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.id_pos.Equals(id_pos) && vc.vendedor.Equals(vendedor) && vc.supervisor.Equals(supervisor) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(DateTime fecha_ini, DateTime fecha_fin, string vendedor)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.vendedor.Equals(vendedor) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<VentaCanceladaExtended> getVentasCanceladas(string supervisor, DateTime fecha_ini, DateTime fecha_fin)
		{
			dcContextoSuPlazaDataContext dc = new dcContextoSuPlazaDataContext();
			try
			{
				return (from vc in dc.venta_cancelada
						where vc.fecha >= fecha_ini && vc.fecha <= fecha_fin && vc.supervisor.Equals(supervisor) && vc.status.Equals("cancelada")
						orderby vc.fecha
						select new VentaCanceladaExtended
						{
							id_venta = vc.id_venta_cancel,
							id_pos = vc.id_pos,
							fecha_cancel = vc.fecha,
							cajero = dc.empleado.First((empleado e) => e.user_name.Equals(vc.vendedor)).fullName(),
							supervisor = ((vc.supervisor != null) ? dc.empleado.First((empleado e) => e.user_name.Equals(vc.supervisor)).fullName() : null),
							status = vc.status
						}).ToList();
			}
			finally
			{
				if (dc != null)
				{
					((IDisposable)dc).Dispose();
				}
			}
		}

		public List<string> getVentasDetail(Guid SaleID)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_cancelada_articulo
					where v.id_venta_cancel.Equals(SaleID)
					select v.cod_barras).ToList();
		}

		public List<VentaMovimientoExtended> getTransactionsPos(DateTime fecha_ini, DateTime fecha_fin)
		{
			return null;
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin, long id_pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.id_pos.Equals(id_pos) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin, long id_pos, string vendedor)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.id_pos.Equals(id_pos) && v.venta.vendedor.Equals(vendedor) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin, string supervisor, long id_pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.id_pos.Equals(id_pos) && v.user_name.Equals(supervisor) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin, string vendedor)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.venta.vendedor.Equals(vendedor) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(string supervisor, DateTime fecha_ini, DateTime fecha_fin)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.user_name.Equals(supervisor) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin, string cajero, string supervisor)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.venta.vendedor.Equals(cajero) && v.venta.supervisor.Equals(supervisor) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<VentaMovimientoExtended> getMovimientos(DateTime fecha_ini, DateTime fecha_fin, string cajero, string supervisor, long id_pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta_articulo
					where v.venta.fecha_venta >= fecha_ini && v.venta.fecha_venta <= fecha_fin && v.venta.vendedor.Equals(cajero) && v.venta.supervisor.Equals(supervisor) && v.id_pos.Equals(id_pos) && (v.porcent_desc > 0.0m || v.cambio_precio == true)
					select v into i
					select new VentaMovimientoExtended
					{
						id_pos = i.id_pos,
						cajero = i.venta.vendedor,
						fecha = i.venta.fecha_venta,
						supervisor = (i.user_name ?? ""),
						ticket = i.venta.folio,
						cod_barras = i.cod_barras,
						descripcion = i.articulo.descripcion,
						precio_regular = i.precio_regular,
						precio_venta = i.precio_vta,
						descuento = i.porcent_desc,
						transaccion = string.Format("{0}{1}{2}", i.cambio_precio ? "Cambio Precio" : "", i.cambio_precio ? "/" : "", (i.porcent_desc > 0.0m) ? "Descuento" : "")
					}).ToList();
		}

		public List<venta> getRelacionVentas(DateTime dateIni, DateTime dateEnd)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.venta.Where((venta v) => v.fecha_venta >= dateIni && v.fecha_venta <= dateEnd).ToList();
		}

		public List<venta> getRelacionVentas(DateTime dateIni, DateTime dateEnd, string barCode)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return dcContextoSuPlazaDataContext.venta.Where((venta v) => v.fecha_venta >= dateIni && v.fecha_venta <= dateEnd).ToList();
		}

		public List<VentaRelacionExtended> getVentasBy(DateTime iniDate, DateTime endDate)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta
					join e in dcContextoSuPlazaDataContext.empleado on v.vendedor equals e.user_name
					where v.fecha_venta >= iniDate && v.fecha_venta <= endDate
					orderby v.fecha_venta, v.folio
					select new VentaRelacionExtended
					{
						id_venta = v.id_venta,
						id_pos = v.id_pos,
						folio = v.folio,
						fecha_venta = v.fecha_venta,
						cajero = e.nombre + " " + e.a_paterno + " " + e.a_materno,
						total_venta = v.total_vendido
					}).ToList();
		}

		public List<VentaRelacionExtended> getVentasBy(DateTime iniDate, DateTime endDate, int pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta
					join e in dcContextoSuPlazaDataContext.empleado on v.vendedor equals e.user_name
					where v.fecha_venta >= iniDate && v.fecha_venta <= endDate && v.id_pos.Equals(pos)
					orderby v.fecha_venta, v.folio
					select new VentaRelacionExtended
					{
						id_venta = v.id_venta,
						id_pos = v.id_pos,
						folio = v.folio,
						fecha_venta = v.fecha_venta,
						cajero = e.nombre + " " + e.a_paterno + " " + e.a_materno,
						total_venta = v.total_vendido
					}).ToList();
		}

		public List<VentaRelacionExtended> getVentasBy(DateTime iniDate, DateTime endDate, string barCode)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta
					join va in dcContextoSuPlazaDataContext.venta_articulo on v.id_venta equals va.id_venta
					join e in dcContextoSuPlazaDataContext.empleado on v.vendedor equals e.user_name
					where v.fecha_venta >= iniDate && v.fecha_venta <= endDate && va.cod_barras.Equals(barCode)
					orderby v.fecha_venta, v.folio
					select new VentaRelacionExtended
					{
						id_venta = v.id_venta,
						id_pos = v.id_pos,
						folio = v.folio,
						fecha_venta = v.fecha_venta,
						cajero = e.nombre + " " + e.a_paterno + " " + e.a_materno,
						total_venta = v.total_vendido
					}).ToList();
		}

		public List<VentaRelacionExtended> getVentasBy(DateTime iniDate, DateTime endDate, string barCode, int pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from v in dcContextoSuPlazaDataContext.venta
					join va in dcContextoSuPlazaDataContext.venta_articulo on v.id_venta equals va.id_venta
					join e in dcContextoSuPlazaDataContext.empleado on v.vendedor equals e.user_name
					where v.fecha_venta >= iniDate && v.fecha_venta <= endDate && va.cod_barras.Equals(barCode) && v.id_pos.Equals(pos)
					orderby v.fecha_venta, v.folio
					select new VentaRelacionExtended
					{
						id_venta = v.id_venta,
						id_pos = v.id_pos,
						folio = v.folio,
						fecha_venta = v.fecha_venta,
						cajero = e.nombre + " " + e.a_paterno + " " + e.a_materno,
						total_venta = v.total_vendido
					}).ToList();
		}

		public List<VentaDevolucionExtended> getDevolucionesBy(DateTime iniDate, DateTime endDate)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from d in dcContextoSuPlazaDataContext.venta_devolucion
					where d.fecha_dev >= iniDate && d.fecha_dev <= endDate
					select new VentaDevolucionExtended
					{
						id_pos = d.id_pos,
						id_devolucion = d.id_devolucion,
						folio = d.folio,
						fecha = d.fecha_dev,
						cajero = d.usuario1.empleado.FirstOrDefault().shortName(),
						supervisor = d.usuario.empleado.FirstOrDefault().shortName(),
						total_devuelto = d.cant_dev
					}).ToList();
		}

		public List<VentaDevolucionExtended> getDevolucionesBy(DateTime iniDate, DateTime endDate, int pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from d in dcContextoSuPlazaDataContext.venta_devolucion
					where d.fecha_dev >= iniDate && d.fecha_dev <= endDate && d.id_pos.Equals(pos)
					select new VentaDevolucionExtended
					{
						id_pos = d.id_pos,
						id_devolucion = d.id_devolucion,
						folio = d.folio,
						fecha = d.fecha_dev,
						cajero = d.usuario1.empleado.FirstOrDefault().shortName(),
						supervisor = d.usuario.empleado.FirstOrDefault().shortName(),
						total_devuelto = d.cant_dev
					}).ToList();
		}

		public List<VentaDevolucionExtended> getDevolucionesBy(DateTime iniDate, DateTime endDate, string barCode)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from d in dcContextoSuPlazaDataContext.venta_devolucion
					where d.fecha_dev >= iniDate && d.fecha_dev <= endDate && d.venta_devolucion_articulo.FirstOrDefault((venta_devolucion_articulo vda) => vda.cod_barras.Equals(barCode)) != null
					select new VentaDevolucionExtended
					{
						id_pos = d.id_pos,
						id_devolucion = d.id_devolucion,
						folio = d.folio,
						fecha = d.fecha_dev,
						cajero = d.usuario1.empleado.FirstOrDefault().shortName(),
						supervisor = d.usuario.empleado.FirstOrDefault().shortName(),
						total_devuelto = d.cant_dev
					}).ToList();
		}

		public List<VentaDevolucionExtended> getDevolucionesBy(DateTime iniDate, DateTime endDate, string barCode, int pos)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			return (from d in dcContextoSuPlazaDataContext.venta_devolucion
					where d.fecha_dev >= iniDate && d.fecha_dev <= endDate && d.id_pos.Equals(pos) && d.venta_devolucion_articulo.FirstOrDefault((venta_devolucion_articulo vda) => vda.cod_barras.Equals(barCode)) != null
					select new VentaDevolucionExtended
					{
						id_pos = d.id_pos,
						id_devolucion = d.id_devolucion,
						folio = d.folio,
						fecha = d.fecha_dev,
						cajero = d.usuario1.empleado.FirstOrDefault().shortName(),
						supervisor = d.usuario.empleado.FirstOrDefault().shortName(),
						total_devuelto = d.cant_dev
					}).ToList();
		}
	}
}
