using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Repositorios;

using WebAppPOSAdmin.Util.RecursoPedidos;

using WebAppPOSAdmin.Funcionalidades;

namespace WebAppPOSAdmin.DropDownListExtender
{
	public static class DropDownListExtend
	{
		public static void getEntidades(this DropDownList _control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<entidad> list = new List<entidad>();
			entidad entidad = new entidad();
			entidad.nombre = "--SELECCIONAR--";
			entidad.id_entidad = 0;
			list.Insert(0, entidad);
			list.AddRange(dropDownListGeneric.getDropDownList<entidad>());
			_control.DataTextField = "nombre";
			_control.DataValueField = "id_entidad";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getUsoCFDI(this DropDownList control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<uso_cfdi> list = new List<uso_cfdi>();
			uso_cfdi uso_cfdi = new uso_cfdi();
			uso_cfdi.descripcion = "--SELECCIONAR--";
			uso_cfdi.id_uso = "";
			list.Insert(0, uso_cfdi);
			list.AddRange(dropDownListGeneric.getDropDownList<uso_cfdi>());
			control.DataTextField = "descripcion";
			control.DataValueField = "id_uso";
			control.DataSource = list;
			control.DataBind();
		}

		public static void getCondicionPago(this DropDownList control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<condicion_pago> list = new List<condicion_pago>();
			list.AddRange(dropDownListGeneric.getDropDownList<condicion_pago>());
			control.DataTextField = "descripcion";
			control.DataValueField = "id_condicion";
			control.DataSource = list;
			control.DataBind();
		}

		public static void getTipoComprobante(this DropDownList control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<comprobante> list = new List<comprobante>();
			list.AddRange(dropDownListGeneric.getDropDownList<comprobante>());
			control.DataTextField = "documento";
			control.DataValueField = "id_comprobante";
			control.DataSource = list;
			control.DataBind();
		}

		public static void getMunicipios(this DropDownList _control, long id)
		{
			List<municipio> dropDownListMunicipioByID = ((IDropDownListGeneric)new RepositorioDropDownList()).getDropDownListMunicipioByID(id);
			municipio municipio = new municipio();
			municipio.nombre = "--SELECCIONAR--";
			municipio.id_municipio = 0;
			dropDownListMunicipioByID.Insert(0, municipio);
			dropDownListMunicipioByID.OrderBy((municipio m) => m.nombre);
			_control.DataTextField = "nombre";
			_control.DataValueField = "id_municipio";
			_control.DataSource = dropDownListMunicipioByID;
			_control.DataBind();
		}

		public static void getUnidad(this DropDownList _control)
		{
			List<unidad_medida> dropDownList = ((IDropDownListGeneric)new RepositorioDropDownList()).getDropDownList<unidad_medida>();
			new unidad_medida();
			_control.DataTextField = "descripcion";
			_control.DataValueField = "id_unidad";
			_control.DataSource = dropDownList;
			_control.DataBind();
		}

		public static void getProveedores(this DropDownList _control)
		{
			List<proveedor> list = (from p in ((IDropDownListGeneric)new RepositorioDropDownList()).getDropDownList<proveedor>()
									orderby p.razon_social
									select p).ToList();
			proveedor proveedor = new proveedor();
			proveedor.id_proveedor = new Guid("00000000-0000-0000-0000-000000000000");
			proveedor.razon_social = "--SELECCIONAR--";
			list.Insert(0, proveedor);
			_control.DataTextField = "razon_social";
			_control.DataValueField = "id_proveedor";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getProveedores(this DropDownList _control, string firstOption)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<proveedor> list = new List<proveedor>();
			if (firstOption != null)
			{
				list.Add(new proveedor
				{
					id_proveedor = new Guid("00000000-0000-0000-0000-000000000000"),
					razon_social = firstOption
				});
			}
			list.AddRange((from p in dropDownListGeneric.getDropDownList<proveedor>()
						   orderby p.razon_social
						   select p).ToList());
			_control.DataTextField = "razon_social";
			_control.DataValueField = "id_proveedor";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getPedidosPendientes(this DropDownList ddl)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			List<PedidosSuspendidoExtended> list = new List<PedidosSuspendidoExtended>();
			list.Add(new PedidosSuspendidoExtended
			{
				id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
				razon_social = "--SELECCIONE UN PEDIDO--"
			});
			list.AddRange((from p in dcContextoSuPlazaDataContext.pedido
						   orderby p.num_pedido descending
						   where p.status_pedido == "pendiente"
						   select new PedidosSuspendidoExtended
						   {
							   id = p.id_pedido,
							   razon_social = p.num_pedido + " | " + p.proveedor.razon_social
						   }).ToList());
			ddl.DataTextField = "razon_social";
			ddl.DataValueField = "id";
			ddl.DataSource = list;
			ddl.DataBind();
		}

		public static void GetClasificacionDepartamentos(this DropDownList _control)
		{
			List<vw_clasificacion> allDepartamentos = ((IDropDownListGeneric)new RepositorioDropDownList()).GetAllDepartamentos();
			vw_clasificacion vw_clasificacion = new vw_clasificacion();
			vw_clasificacion.departamento = "--SELECCIONAR--";
			vw_clasificacion.id_dpto = 0L;
			allDepartamentos.Insert(0, vw_clasificacion);
			_control.DataTextField = "departamento";
			_control.DataValueField = "id_dpto";
			_control.DataSource = allDepartamentos;
			_control.DataBind();
		}

		public static void GetPrimeraLiniaCategoria(this DropDownList _control, int value)
		{
			List<vw_clasificacion> primeraLiniaCategoria = ((IDropDownListGeneric)new RepositorioDropDownList()).GetPrimeraLiniaCategoria(value);
			vw_clasificacion vw_clasificacion = new vw_clasificacion();
			vw_clasificacion.categoria = "--SELECCIONAR--";
			vw_clasificacion.id_cat = 0L;
			primeraLiniaCategoria.Insert(0, vw_clasificacion);
			_control.DataTextField = "categoria";
			_control.DataValueField = "id_cat";
			_control.DataSource = primeraLiniaCategoria;
			_control.DataBind();
		}

		public static void GetSegundaLiniaCategoria(this DropDownList _control, int value)
		{
			List<vw_clasificacion> segundaLiniaCategoria = ((IDropDownListGeneric)new RepositorioDropDownList()).GetSegundaLiniaCategoria(value);
			vw_clasificacion vw_clasificacion = new vw_clasificacion();
			vw_clasificacion.subcategoria = "--SELECCIONAR--";
			vw_clasificacion.id_subcat = 0L;
			segundaLiniaCategoria.Insert(0, vw_clasificacion);
			_control.DataTextField = "subcategoria";
			_control.DataValueField = "id_subcat";
			_control.DataSource = segundaLiniaCategoria;
			_control.DataBind();
		}

		public static void GetTerceraLiniaCategoria(this DropDownList _control, int value)
		{
			List<vw_clasificacion> terceraLiniaCategoria = ((IDropDownListGeneric)new RepositorioDropDownList()).GetTerceraLiniaCategoria(value);
			vw_clasificacion vw_clasificacion = new vw_clasificacion();
			vw_clasificacion.linea = "--SELECCIONAR--";
			vw_clasificacion.id_linea = 0L;
			terceraLiniaCategoria.Insert(0, vw_clasificacion);
			_control.DataTextField = "linea";
			_control.DataValueField = "id_linea";
			_control.DataSource = terceraLiniaCategoria;
			_control.DataBind();
		}

		public static void GetDepartamentoByNivel(this DropDownList _control, int value)
		{
			List<clasificacion> departamentoByNivel = ((IDepartamentos)new RepositorioDepartamentos()).getDepartamentoByNivel(value);
			clasificacion clasificacion = new clasificacion();
			clasificacion.descripcion = "--SELECCIONAR--";
			clasificacion.id_clasificacion = 0L;
			departamentoByNivel.Insert(0, clasificacion);
			_control.DataTextField = "descripcion";
			_control.DataValueField = "id_clasificacion";
			_control.DataSource = departamentoByNivel;
			_control.DataBind();
		}

		public static void GetClasificacionById(this DropDownList _control, int value)
		{
			List<clasificacion> categoriasById = ((IDepartamentos)new RepositorioDepartamentos()).getCategoriasById(value);
			clasificacion clasificacion = new clasificacion();
			clasificacion.descripcion = "--SELECCIONAR--";
			clasificacion.id_clasificacion = 0L;
			categoriasById.Insert(0, clasificacion);
			_control.DataTextField = "descripcion";
			_control.DataValueField = "id_clasificacion";
			_control.DataSource = categoriasById;
			_control.DataBind();
		}

		public static void getMvtoAlmacen(this DropDownList _control, string tipo_mvo)
		{
			List<movimiento_almacen> mvtoAlmacenByTipo = ((IAlmacen)new RepositorioAlmacen()).getMvtoAlmacenByTipo(tipo_mvo);
			new movimiento_almacen();
			_control.DataTextField = "descripcion";
			_control.DataValueField = "id_movto";
			_control.DataSource = mvtoAlmacenByTipo;
			_control.DataBind();
		}

		public static void getListaCajeros(this DropDownList _control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<empleado> list = new List<empleado>();
			list.Add(new empleado
			{
				user_name = "",
				nombre = "--SELECCIONAR--"
			});
			list.AddRange(from e in dropDownListGeneric.getListaCajeros()
						  select new empleado
						  {
							  id_empleado = e.id_empleado,
							  user_name = e.user_name,
							  nombre = e.nombre + " " + e.a_paterno + " " + e.a_materno
						  });
			_control.DataTextField = "nombre";
			_control.DataValueField = "user_name";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getListPOS(this DropDownList ddl)
		{
			using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
			List<vw_cajas> list = new List<vw_cajas>();
			list.Add(new vw_cajas
			{
				id_pos = 0,
				descripcion = "--SELECCIONAR--"
			});
			list.AddRange(from c in dcContextoSuPlazaDataContext.vw_cajas.ToList()
						  orderby c.id_pos
						  select c);
			ddl.DataTextField = "descripcion";
			ddl.DataValueField = "id_pos";
			ddl.DataSource = list;
			ddl.DataBind();
		}

		public static void getSupervisoresCancelaVentas(this DropDownList _control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<empleado> list = new List<empleado>();
			list.Add(new empleado
			{
				user_name = "",
				nombre = "--SELECCIONAR--"
			});
			list.AddRange(from e in dropDownListGeneric.getSupervisoresCancelaVentas()
						  select new empleado
						  {
							  id_empleado = e.id_empleado,
							  user_name = e.user_name,
							  nombre = e.nombre + " " + e.a_paterno + " " + e.a_materno
						  });
			_control.DataTextField = "nombre";
			_control.DataValueField = "user_name";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getSupervisoresMovimientos(this DropDownList _control)
		{
			IDropDownListGeneric dropDownListGeneric = new RepositorioDropDownList();
			List<empleado> list = new List<empleado>();
			list.Add(new empleado
			{
				user_name = "",
				nombre = "--SELECCIONAR--"
			});
			list.AddRange(from e in dropDownListGeneric.getSupervisoresMovimientos()
						  select new empleado
						  {
							  id_empleado = e.id_empleado,
							  user_name = e.user_name,
							  nombre = e.nombre + " " + e.a_paterno + " " + e.a_materno
						  });
			_control.DataTextField = "nombre";
			_control.DataValueField = "user_name";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getListaIdCompras(this DropDownList _control)
		{
			List<compra> list = ((ICompras)new RepositorioCompras()).listaComprasPorIngresarId();
			compra compra = new compra();
			compra.id_compra = new Guid("00000000-0000-0000-0000-000000000000");
			list.Insert(0, compra);
			_control.DataTextField = "id_compra";
			_control.DataValueField = "id_compra";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getListaMeses(this DropDownList _control)
		{
			Meses[] listaMeses = new Funciones().getListaMeses();
			_control.DataTextField = "Mes";
			_control.DataValueField = "idMes";
			_control.DataSource = listaMeses;
			_control.DataBind();
		}

		public static void getListaDias(this DropDownList _control)
		{
			DiasPedir[] diasPedir = new Funciones().getDiasPedir();
			_control.DataTextField = "pedir";
			_control.DataValueField = "idPedir";
			_control.DataSource = diasPedir;
			_control.DataBind();
		}

		public static void getListaTipoPlan(this DropDownList _control)
		{
			TipoPlan[] tipoPlan = new Funciones().getTipoPlan();
			_control.DataTextField = "tipoPlan";
			_control.DataValueField = "idPlan";
			_control.DataSource = tipoPlan;
			_control.DataBind();
		}

		public static void getListaPedidosSuspendidos(this DropDownList _control)
		{
			RepositorioPedidos repositorioPedidos = new RepositorioPedidos();
			List<PedidosSuspendidoExtended> list = new List<PedidosSuspendidoExtended>();
			list = ((IPedidos)repositorioPedidos).listapedidosSuspendidos();
			_control.DataTextField = "razon_social";
			_control.DataValueField = "id";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getListaInventariosAbierto(this DropDownList _control)
		{
			RepositorioProveedores repositorioProveedores = new RepositorioProveedores();
			List<PedidosSuspendidoExtended> list = new List<PedidosSuspendidoExtended>();
			PedidosSuspendidoExtended pedidosSuspendidoExtended = new PedidosSuspendidoExtended();
			list = ((IProveedores)repositorioProveedores).getProveedorByInventarioAbierto();
			pedidosSuspendidoExtended.razon_social = "--SELECCIONAR--";
			pedidosSuspendidoExtended.id = new Guid("00000000-0000-0000-0000-000000000000");
			list.Insert(0, pedidosSuspendidoExtended);
			_control.DataTextField = "razon_social";
			_control.DataValueField = "id";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getListaPedidosPendientes(this DropDownList _control)
		{
			RepositorioPedidos repositorioPedidos = new RepositorioPedidos();
			List<PedidosSuspendidoExtended> list = new List<PedidosSuspendidoExtended>();
			PedidosSuspendidoExtended pedidosSuspendidoExtended = new PedidosSuspendidoExtended();
			list = ((IPedidos)repositorioPedidos).listapedidosPendientes();
			pedidosSuspendidoExtended.id = new Guid("00000000-0000-0000-0000-000000000000");
			pedidosSuspendidoExtended.razon_social = "--SELECCIONAR--";
			list.Insert(0, pedidosSuspendidoExtended);
			_control.DataTextField = "razon_social";
			_control.DataValueField = "id";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void getOfertasSuspendidas(this DropDownList _control)
		{
			List<oferta> list = ((IOfertas)new RepositorioOfertas()).listaOfertaSuspendida();
			oferta oferta = new oferta();
			oferta.descripcion = "--SELECCIONAR--";
			oferta.id_oferta = new Guid("00000000-0000-0000-0000-000000000000");
			list.Insert(0, oferta);
			_control.DataTextField = "descripcion";
			_control.DataValueField = "id_oferta";
			_control.DataSource = list;
			_control.DataBind();
		}

		public static void setYear(this DropDownList ddl)
		{
			for (int num = DateTime.Now.Year; num >= 2016; num--)
			{
				ddl.Items.Add(new ListItem(num.ToString(), num.ToString()));
			}
		}

		public static void getPurchases(this DropDownList _control, Guid OrderID)
		{
			List<CompraExtended> list = new List<CompraExtended>();
			List<CompraExtended> purchases = new RepositorioCompras().getPurchases(OrderID);
			if (purchases.Count > 1)
			{
				list.Add(new CompraExtended
				{
					id_compra = default(Guid),
					id_pedido = OrderID,
					num_captura = "-- TODAS --"
				});
				int num = 1;
				foreach (CompraExtended item in purchases)
				{
					list.Add(new CompraExtended
					{
						id_compra = item.id_compra,
						id_pedido = item.id_pedido,
						num_captura = string.Format("{0} | {1}", num++, item.fecha_compra.ToString("dd/MM/yyyy HH:mm:ss"))
					});
				}
			}
			else
			{
				int num2 = 1;
				foreach (CompraExtended item2 in purchases)
				{
					list.Add(new CompraExtended
					{
						id_compra = item2.id_compra,
						id_pedido = item2.id_pedido,
						num_captura = string.Format("{0} | {1}", num2++, item2.fecha_compra.ToString("dd/MM/yyyy HH:mm:ss"))
					});
				}
			}
			_control.DataTextField = "num_captura";
			_control.DataValueField = "id_compra";
			_control.DataSource = list;
			_control.DataBind();
		}
	}
}