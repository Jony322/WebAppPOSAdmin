using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Extenciones;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IPedidos
	{
		Guid insertarPedido(pedido _pedido);

		void insertarPedidoArticulo(pedido_articulo _pedido);

		void actualizarPedidoArticulo(List<pedido_articulo> _pedido, Guid id_pedido, string estado);

		void eliminarPedidoById(Guid id_pedido);

		void eliminarListaPedidoArticulosById(Guid id);

		void autorizarPedido(Guid id_pedido);

		pedido pedidoPendienteById(Guid id);

		pedido pedidoSuspendido(Guid id_pedido);

		List<PedidosSuspendidoExtended> listapedidosSuspendidos();

		List<PedidosSuspendidoExtended> listapedidosPendientes();

		List<PedidoExtended> getListaArticulosByIdPedido(Guid id_pedido);

		void listaPedidoArticuloActualizar(List<pedido_articulo> lista, Guid id_pedido);

		List<PedidoCapturaPendiente> listaPedidoArticulosPendiente(Guid id_pedido);
	}

}
