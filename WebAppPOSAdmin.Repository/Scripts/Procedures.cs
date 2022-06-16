using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using WebAppPOSAdmin.Repository.Properties;
using WebAppPOSAdmin.Repository.Extenciones;
using WebAppPOSAdmin.Repository.Clases;

namespace WebAppPOSAdmin.Repository.Scripts
{
    public class Procedures : POSAdmin
    {
        private string connectionString;

        public Procedures()
        {
            connectionString = Settings.Default.pos_adminConnectionString;
        }

        public void DeleteArticulos(string barCode)
        {
            string sql = $"DELETE FROM articulos WHERE cod_barras='{barCode}'";
            ExecuteSQL(sql);
        }

        public List<Statistics> GetStatistics(string dateIni, string dateEnd, string proveedor, int departamento, string barCode, string orderBy, string filter)
        {
            string sql = string.Format("SELECT a.cod_barras, a.descripcion, um.descripcion [medida], SUM(v.cantidad) [cantidad], SUM(v.total) [total] FROM articulo a INNER JOIN unidad_medida um ON a.id_unidad=um.id_unidad\r\nINNER JOIN (SELECT aa.cod_group cod_barras, SUM(va.cantidad * aa.cantidad_um) cantidad, SUM(va.cantidad * va.precio_vta) total\r\nFROM (SELECT cod_barras,descripcion,ISNULL(cod_asociado,cod_barras) cod_group,cantidad_um,id_proveedor,id_clasificacion FROM articulo WHERE kit=0 {6}) aa INNER JOIN venta_articulo va ON aa.cod_barras = va.cod_barras INNER JOIN venta v ON va.id_venta = v.id_venta \r\nWHERE (v.fecha_venta BETWEEN '{0}' AND '{1}') {2} {3} {5} GROUP BY aa.cod_group\r\nUNION SELECT ka.cod_barras_pro cod_barras, SUM(va.cantidad * ka.cantidad) [cantidad], SUM(va.cantidad * ka.cantidad * va.precio_vta) [total] \r\nFROM venta v INNER JOIN venta_articulo va ON v.id_venta = va.id_venta INNER JOIN kit_articulos ka ON ka.cod_barras_kit = va.cod_barras INNER JOIN articulo aa ON aa.cod_barras=ka.cod_barras_pro\r\nWHERE (v.fecha_venta BETWEEN '{0}' AND '{1}') {2} {3} {4} {5} GROUP BY ka.cod_barras_pro) v ON a.cod_barras = v.cod_barras GROUP BY a.cod_barras, a.descripcion, um.descripcion", dateIni, dateEnd, (proveedor != null) ? $"AND aa.id_proveedor='{proveedor}'" : "", (departamento != 0) ? $"AND aa.id_clasificacion={departamento}" : "", (barCode.Length > 0) ? $"AND aa.cod_barras='{barCode}'" : "", (filter.Length > 0) ? $"AND aa.descripcion LIKE '%{filter}%'" : "", (barCode.Length > 0) ? string.Format("AND (cod_barras='{0}' OR cod_asociado='{0}')", barCode) : "");
            DataSet dataSet = GetDataSet(sql);
            List<Statistics> list = new List<Statistics>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new Statistics
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                    medida = row["medida"].ToString(),
                    cantidad = decimal.Parse(row["cantidad"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            if (!orderBy.Equals("cantidad"))
            {
                return list.OrderBy((Statistics s) => s.total).ToList();
            }
            return list.OrderBy((Statistics s) => s.cantidad).ToList();
        }

        public List<Statistics> GetStatistics(int month, int year, string barCode)
        {
            string sql = string.Format("SELECT a.cod_barras, a.descripcion, um.descripcion [medida], SUM(v.cantidad) [cantidad], SUM(v.total) [total] FROM articulo a INNER JOIN unidad_medida um ON a.id_unidad=um.id_unidad\r\nINNER JOIN (SELECT ap.cod_barras, SUM(va.cantidad * aa.cantidad_um) cantidad, SUM(va.cantidad * va.precio_vta) total\r\nFROM articulo ap INNER JOIN vw_articulo aa ON ap.cod_barras = aa.cod_asociado INNER JOIN venta_articulo va ON aa.cod_barras = va.cod_barras INNER JOIN venta v ON va.id_venta = v.id_venta \r\nWHERE (ap.tipo_articulo = 'principal' AND ap.kit = 0) AND MONTH(v.fecha_venta)={0} AND YEAR(v.fecha_venta)={1} AND ap.cod_barras='{2}' GROUP BY ap.cod_barras\r\nUNION SELECT ka.cod_barras_pro, SUM(va.cantidad * ka.cantidad) [cantidad], SUM(va.cantidad * ka.cantidad * va.precio_vta) [total] \r\nFROM venta v INNER JOIN venta_articulo va ON v.id_venta = va.id_venta INNER JOIN kit_articulos ka ON ka.cod_barras_kit = va.cod_barras INNER JOIN articulo ap ON ap.cod_barras=ka.cod_barras_pro\r\nWHERE MONTH(v.fecha_venta)={0} AND YEAR(v.fecha_venta)={1} AND ap.cod_barras='{2}' GROUP BY ka.cod_barras_pro) v ON a.cod_barras = v.cod_barras GROUP BY a.cod_barras, a.descripcion, um.descripcion", month, year, barCode);
            DataSet dataSet = GetDataSet(sql);
            List<Statistics> list = new List<Statistics>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new Statistics
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                    medida = row["medida"].ToString(),
                    cantidad = decimal.Parse(row["cantidad"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            return list;
        }

        public List<Statistics> GetStatistics(string dateIni, string dateEnd, string barCode)
        {
            string sql = string.Format("SELECT a.cod_barras, a.descripcion, um.descripcion [medida], v.fecha, SUM(v.cantidad) [cantidad], SUM(v.total) [total]\r\nFROM articulo a INNER JOIN unidad_medida um ON a.id_unidad=um.id_unidad INNER JOIN (SELECT s.cod_barras, CONVERT(DATE, s.fecha) [fecha], cantidad, total\r\nFROM (SELECT ap.cod_barras, v.fecha_venta [fecha], SUM(va.cantidad * aa.cantidad_um) [cantidad], SUM(va.cantidad * va.precio_vta) [total]\r\nFROM articulo ap INNER JOIN vw_articulo aa ON ap.cod_barras = aa.cod_asociado INNER JOIN venta_articulo va ON aa.cod_barras = va.cod_barras INNER JOIN venta v ON va.id_venta = v.id_venta \r\nWHERE (ap.tipo_articulo = 'principal' AND ap.kit = 0) AND v.fecha_venta BETWEEN '{0}' AND '{1}' AND va.cod_barras='{2}' GROUP BY ap.cod_barras, v.fecha_venta\r\nUNION SELECT ka.cod_barras_pro, v.fecha_venta [fecha], SUM(va.cantidad * ka.cantidad) [cantidad], SUM(va.cantidad * ka.cantidad * va.precio_vta) [total]\r\nFROM venta v INNER JOIN venta_articulo va ON v.id_venta = va.id_venta INNER JOIN kit_articulos ka ON ka.cod_barras_kit = va.cod_barras INNER JOIN articulo ap ON ap.cod_barras=ka.cod_barras_pro \r\nWHERE v.fecha_venta BETWEEN '{0}' AND '{1}' AND va.cod_barras='{2}' GROUP BY ka.cod_barras_pro, v.fecha_venta) s) v ON a.cod_barras = v.cod_barras GROUP BY a.cod_barras, a.descripcion, um.descripcion, v.fecha", dateIni, dateEnd, barCode);
            DataSet dataSet = GetDataSet(sql);
            List<Statistics> list = new List<Statistics>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new Statistics
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                    medida = row["medida"].ToString(),
                    fecha = DateTime.Parse(row["fecha"].ToString()),
                    cantidad = decimal.Parse(row["cantidad"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<OfferItems> OfferStatistics(Guid OfferID)
        {
            string sql = $"EXEC sp_estadistica_oferta '{OfferID}'";
            DataSet dataSet = GetDataSet(sql);
            List<OfferItems> list = new List<OfferItems>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                OfferItems offerItems = new OfferItems();
                offerItems.cod_barras = row["cod_barras"].ToString();
                offerItems.descripcion = row["descripcion"].ToString();
                offerItems.unidad = row["um"].ToString();
                offerItems.cantidad_um = decimal.Parse(row["umc"].ToString());
                offerItems.cantidad_vta = decimal.Parse(row["venta"].ToString());
                offerItems.cantidad_dev = decimal.Parse(row["devolucion"].ToString());
                list.Add(offerItems);
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<Compras> PurchasesByOrder(DateTime dateIni, DateTime dateEnd)
        {
            string sql = string.Format("SELECT pe.fecha_pedido,pe.num_pedido,p.razon_social,c.no_factura,c.fecha_compra FROM compra c INNER JOIN proveedor p ON c.id_proveedor=p.id_proveedor INNER JOIN pedido pe ON c.id_pedido=pe.id_pedido\r\nWHERE c.fecha_compra BETWEEN '{0}' AND '{1}' GROUP BY pe.fecha_pedido,pe.num_pedido,p.razon_social,c.no_factura,c.fecha_compra ORDER BY p.razon_social", dateIni.ToString("dd/MM/yyyy HH:mm:ss"), dateEnd.ToString("dd/MM/yyyy HH:mm:ss"));
            DataSet dataSet = GetDataSet(sql);
            List<Compras> list = new List<Compras>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new Compras
                {
                    fecha_pedido = DateTime.Parse(row["fecha_pedido"].ToString()),
                    num_pedido = long.Parse(row["num_pedido"].ToString()),
                    razon_social = row["razon_social"].ToString(),
                    no_factura = row["no_factura"].ToString(),
                    fecha_compra = DateTime.Parse(row["fecha_compra"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<Compras> PurchasesByOrder(DateTime dateIni, DateTime dateEnd, Guid ProviderID)
        {
            string sql = string.Format("SELECT pe.fecha_pedido,pe.num_pedido,p.razon_social,c.no_factura,c.fecha_compra FROM compra c INNER JOIN proveedor p ON c.id_proveedor=p.id_proveedor INNER JOIN pedido pe ON c.id_pedido=pe.id_pedido\r\nWHERE c.id_proveedor='{0}' AND c.fecha_compra BETWEEN '{1}' AND '{2}' GROUP BY pe.fecha_pedido,pe.num_pedido,p.razon_social,c.no_factura,c.fecha_compra ORDER BY p.razon_social", ProviderID, dateIni.ToString("dd/MM/yyyy HH:mm:ss"), dateEnd.ToString("dd/MM/yyyy HH:mm:ss"));
            DataSet dataSet = GetDataSet(sql);
            List<Compras> list = new List<Compras>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new Compras
                {
                    fecha_pedido = DateTime.Parse(row["fecha_pedido"].ToString()),
                    num_pedido = long.Parse(row["num_pedido"].ToString()),
                    razon_social = row["razon_social"].ToString(),
                    no_factura = row["no_factura"].ToString(),
                    fecha_compra = DateTime.Parse(row["fecha_compra"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<CompraArticuloExtended> getPurchaseByOrderDetail(Guid id_compra)
        {
            string sql = $"SELECT c.cod_barras,c.cod_interno,c.desc_principal,c.unidad_anexo,c.cantidad_uma,c.precio_compra, SUM(c.cant_cja) cant_cja, SUM(c.cant_pza) cant_pza, SUM(c.precio_compra*c.cant_pza) total\r\nFROM (SELECT ca.cod_barras, CASE WHEN va.cod_principal IS NULL THEN a.cod_interno ELSE va.cod_interno END cod_interno, ISNULL(va.desc_principal,a.descripcion) desc_principal,ISNULL(va.unidad_anexo,um.descripcion) unidad_anexo, ISNULL(va.cantidad_uma, a.cantidad_um) cantidad_uma, a.precio_compra, ca.cant_cja, ca.cant_pza\r\nFROM compra c JOIN compra_articulo ca ON c.id_compra=ca.id_compra JOIN articulo a ON ca.cod_barras=a.cod_barras JOIN unidad_medida um ON a.id_unidad=um.id_unidad LEFT JOIN vw_articulos_principales va ON ca.cod_barras=va.cod_principal\r\nWHERE c.id_pedido IS NULL AND c.id_compra='{id_compra}') c GROUP BY  c.cod_barras,c.cod_interno,c.desc_principal,c.unidad_anexo,c.cantidad_uma,c.precio_compra ORDER BY c.desc_principal";
            DataSet dataSet = GetDataSet(sql);
            List<CompraArticuloExtended> list = new List<CompraArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new CompraArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    cod_interno = row["cod_interno"].ToString(),
                    descripcion = row["desc_principal"].ToString(),
                    unidad = row["unidad_anexo"].ToString(),
                    umc = decimal.Parse(row["cantidad_uma"].ToString()),
                    costo = decimal.Parse(row["precio_compra"].ToString()),
                    cant_cja = decimal.Parse(row["cant_cja"].ToString()),
                    cant_pza = decimal.Parse(row["cant_pza"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<CompraArticuloExtended> getPurchaseByOrderDetail(Guid id_pedido, Guid id_compra)
        {
            string empty = string.Empty;
            empty = ((!id_compra.Equals(default(Guid))) ? $"SELECT c.cod_barras,c.cod_interno,c.desc_principal,c.unidad_anexo,c.cantidad_uma,c.precio_compra, SUM(c.cant_cja) cant_cja, SUM(c.cant_pza) cant_pza, SUM(c.precio_compra*c.cant_pza) total\r\nFROM (SELECT ca.cod_barras, CASE WHEN va.cod_principal IS NULL THEN a.cod_interno ELSE va.cod_interno END cod_interno, ISNULL(va.desc_principal,a.descripcion) desc_principal,ISNULL(va.unidad_anexo,um.descripcion) unidad_anexo, ISNULL(va.cantidad_uma, a.cantidad_um) cantidad_uma, a.precio_compra, ca.cant_cja, ca.cant_pza\r\nFROM compra c JOIN compra_articulo ca ON c.id_compra=ca.id_compra JOIN articulo a ON ca.cod_barras=a.cod_barras JOIN unidad_medida um ON a.id_unidad=um.id_unidad LEFT JOIN vw_articulos_principales va ON ca.cod_barras=va.cod_principal\r\nWHERE c.id_pedido='{id_pedido}' AND c.id_compra='{id_compra}') c GROUP BY  c.cod_barras,c.cod_interno,c.desc_principal,c.unidad_anexo,c.cantidad_uma,c.precio_compra ORDER BY c.desc_principal" : $"SELECT c.cod_barras,c.cod_interno,c.desc_principal,c.unidad_anexo,c.cantidad_uma,c.precio_compra, SUM(c.cant_cja) cant_cja, SUM(c.cant_pza) cant_pza, SUM(c.precio_compra*c.cant_pza) total\r\nFROM (SELECT ca.cod_barras, CASE WHEN va.cod_principal IS NULL THEN a.cod_interno ELSE va.cod_interno END cod_interno, ISNULL(va.desc_principal,a.descripcion) desc_principal,ISNULL(va.unidad_anexo,um.descripcion) unidad_anexo, ISNULL(va.cantidad_uma, a.cantidad_um) cantidad_uma, a.precio_compra, ca.cant_cja, ca.cant_pza\r\nFROM compra c JOIN compra_articulo ca ON c.id_compra=ca.id_compra JOIN articulo a ON ca.cod_barras=a.cod_barras JOIN unidad_medida um ON a.id_unidad=um.id_unidad LEFT JOIN vw_articulos_principales va ON ca.cod_barras=va.cod_principal\r\nWHERE c.id_pedido='{id_pedido}') c GROUP BY  c.cod_barras,c.cod_interno,c.desc_principal,c.unidad_anexo,c.cantidad_uma,c.precio_compra ORDER BY c.desc_principal");
            DataSet dataSet = GetDataSet(empty);
            List<CompraArticuloExtended> list = new List<CompraArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new CompraArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    cod_interno = row["cod_interno"].ToString(),
                    descripcion = row["desc_principal"].ToString(),
                    unidad = row["unidad_anexo"].ToString(),
                    umc = decimal.Parse(row["cantidad_uma"].ToString()),
                    costo = decimal.Parse(row["precio_compra"].ToString()),
                    cant_cja = decimal.Parse(row["cant_cja"].ToString()),
                    cant_pza = decimal.Parse(row["cant_pza"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<BackOrderExtended> getBackOrder(Guid id_pedido)
        {
            string sql = string.Format("SELECT pa.cod_barras,a.descripcion,um.descripcion [um],a.cantidad_um [umc],cantidad [cant_pedido],CASE WHEN com.cant_pza IS NULL THEN 0.000000 ELSE com.cant_pza/a.cantidad_um END [cant_compra],CASE WHEN com.cant_pza IS NULL THEN pa.cantidad ELSE pa.cantidad-com.cant_pza/a.cantidad_um END [diferencia]\r\nFROM pedido_articulo pa INNER JOIN articulo a ON pa.cod_anexo=a.cod_barras INNER JOIN unidad_medida um ON a.id_unidad=um.id_unidad LEFT JOIN (SELECT ca.cod_barras,SUM(cant_pza) [cant_pza],SUM(cant_cja) [cant_cja] FROM compra_articulo ca INNER JOIN compra c ON ca.id_compra=c.id_compra WHERE c.id_pedido='{0}' GROUP BY ca.cod_barras) com ON pa.cod_barras=com.cod_barras\r\nWHERE pa.id_pedido='{0}' ORDER BY descripcion", id_pedido);
            DataSet dataSet = GetDataSet(sql);
            List<BackOrderExtended> list = new List<BackOrderExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new BackOrderExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                    unidad = row["um"].ToString(),
                    umc = decimal.Parse(row["umc"].ToString()),
                    cant_pedido = decimal.Parse(row["cant_pedido"].ToString()),
                    cant_compra = decimal.Parse(row["cant_compra"].ToString()),
                    diferencia = decimal.Parse(row["diferencia"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ConcentradoOrderExtended> getConcentradoCompras(Guid id_pedido)
        {
            string sql = string.Format("SELECT sc.cod_barras,sc.descripcion,sc.um,sc.umc,sc.precio_articulo,sc.cant_pedido,ISNULL(sc.[1],0.000) [1],ISNULL(sc.[2],0.000) [2],ISNULL(sc.[3],0.000) [3],ISNULL(sc.[4],0.000) [4],ISNULL(sc.[5],0.000) [5],ISNULL(sc.[6],0.000) [6],ISNULL(sc.[7],0.000) [7],ISNULL(sc.[8],0.000) [8],ISNULL(sc.[9],0.000) [9],ISNULL(sc.[10],0.000) [10] \r\nFROM ( SELECT com.no_captura,pa.cod_barras,a.descripcion,um.descripcion [um],a.cantidad_um [umc],cantidad [cant_pedido],CASE WHEN com.cant_pza IS NULL THEN 0.000000 ELSE com.cant_pza/a.cantidad_um END [cant_compra], pa.precio_articulo\r\nFROM pedido_articulo pa INNER JOIN articulo a ON pa.cod_anexo=a.cod_barras INNER JOIN unidad_medida um ON a.id_unidad=um.id_unidad LEFT JOIN (SELECT sc.no_captura,c.id_compra,ca.cod_barras,SUM(cant_pza) [cant_pza],SUM(cant_cja) [cant_cja] FROM compra_articulo ca INNER JOIN compra c ON ca.id_compra=c.id_compra INNER JOIN (SELECT ROW_NUMBER() OVER (ORDER BY fecha_compra) no_captura, id_compra FROM compra WHERE id_pedido='{0}') sc ON c.id_compra=sc.id_compra\r\nWHERE c.id_pedido='{0}' GROUP BY sc.no_captura,c.id_compra,c.fecha_compra,ca.cod_barras) com ON pa.cod_barras=com.cod_barras WHERE pa.id_pedido='{0}') pc PIVOT ( SUM(cant_compra) FOR no_captura IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10]) ) sc", id_pedido);
            DataSet dataSet = GetDataSet(sql);
            List<ConcentradoOrderExtended> list = new List<ConcentradoOrderExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                ConcentradoOrderExtended concentradoOrderExtended = new ConcentradoOrderExtended();
                concentradoOrderExtended.cod_barras = row["cod_barras"].ToString();
                concentradoOrderExtended.descripcion = row["descripcion"].ToString();
                concentradoOrderExtended.unidad = row["um"].ToString();
                concentradoOrderExtended.umc = decimal.Parse(row["umc"].ToString());
                concentradoOrderExtended.costo = decimal.Parse(row["precio_articulo"].ToString());
                concentradoOrderExtended.cant_pedido = decimal.Parse(row["cant_pedido"].ToString());
                concentradoOrderExtended.entradas[0] = decimal.Parse(row["1"].ToString());
                concentradoOrderExtended.entradas[1] = decimal.Parse(row["2"].ToString());
                concentradoOrderExtended.entradas[2] = decimal.Parse(row["3"].ToString());
                concentradoOrderExtended.entradas[3] = decimal.Parse(row["4"].ToString());
                concentradoOrderExtended.entradas[4] = decimal.Parse(row["5"].ToString());
                concentradoOrderExtended.entradas[5] = decimal.Parse(row["6"].ToString());
                concentradoOrderExtended.entradas[6] = decimal.Parse(row["7"].ToString());
                concentradoOrderExtended.entradas[7] = decimal.Parse(row["8"].ToString());
                concentradoOrderExtended.entradas[8] = decimal.Parse(row["9"].ToString());
                concentradoOrderExtended.entradas[9] = decimal.Parse(row["10"].ToString());
                list.Add(concentradoOrderExtended);
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloAnexoExtended> getPedidoSuspendido(Guid id_pedido)
        {
            string sql = $"EXEC sp_pedido_captura_pendiente '{id_pedido}'";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloAnexoExtended> list = new List<ArticuloAnexoExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                ArticuloAnexoExtended articuloAnexoExtended = new ArticuloAnexoExtended();
                articuloAnexoExtended.cod_barras = row["cod_barras"].ToString();
                articuloAnexoExtended.articulo = row["articulo"].ToString();
                articuloAnexoExtended.unidad = row["unidad"].ToString();
                articuloAnexoExtended.cantidad_um = decimal.Parse(row["cantidad_um"].ToString());
                articuloAnexoExtended.costo = decimal.Parse(row["precio_articulo"].ToString());
                articuloAnexoExtended.stock_cja = decimal.Parse(row["stock_cja"].ToString());
                articuloAnexoExtended.stock_pza = decimal.Parse(row["stock_pza"].ToString());
                articuloAnexoExtended.sugerido = decimal.Parse(row["sugerido"].ToString());
                articuloAnexoExtended.pedir = decimal.Parse(row["cantidad"].ToString());
                list.Add(articuloAnexoExtended);
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloAnexoExtended> listaPedidoArticuloByIdProveedor(Guid id_proveedor, short anio, short mes_val, short dias_pedido)
        {
            string sql = $"EXEC sp_pedido_anio '{id_proveedor}',{anio},{mes_val},{(short)DateTime.DaysInMonth(anio, mes_val)},{dias_pedido}";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloAnexoExtended> list = new List<ArticuloAnexoExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                ArticuloAnexoExtended articuloAnexoExtended = new ArticuloAnexoExtended();
                articuloAnexoExtended.cod_barras = row["cod_barras"].ToString();
                articuloAnexoExtended.cod_anexo = row["cod_anexo"].ToString();
                articuloAnexoExtended.articulo = row["articulo"].ToString();
                articuloAnexoExtended.unidad = row["unidad"].ToString();
                articuloAnexoExtended.cantidad_um = decimal.Parse(row["cantidad_um"].ToString());
                articuloAnexoExtended.costo = decimal.Parse(row["costo"].ToString());
                articuloAnexoExtended.stock_cja = decimal.Parse(row["stock_cja"].ToString());
                articuloAnexoExtended.stock_pza = decimal.Parse(row["stock_pza"].ToString());
                articuloAnexoExtended.sugerido = decimal.Parse(row["sugerido"].ToString());
                articuloAnexoExtended.pedir = 0m;
                list.Add(articuloAnexoExtended);
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public void CerrarInventario(Guid id_inventario)
        {
            string sql = $"EXEC sp_cierre_inventario '{id_inventario}'";
            ExecuteSQL(sql);
        }

        public List<ArticuloAnexoExtended> listaPedidoArticuloByIdProveedorPromedio(Guid id_proveedor, short anio, short mes_val, short dias_pedido)
        {
            string sql = $"EXEC sp_pedido_promedio '{id_proveedor}',{anio},{mes_val},{(short)DateTime.DaysInMonth(anio, mes_val)},{dias_pedido}";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloAnexoExtended> list = new List<ArticuloAnexoExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                ArticuloAnexoExtended articuloAnexoExtended = new ArticuloAnexoExtended();
                articuloAnexoExtended.cod_barras = row["cod_barras"].ToString();
                articuloAnexoExtended.cod_anexo = row["cod_anexo"].ToString();
                articuloAnexoExtended.articulo = row["articulo"].ToString();
                articuloAnexoExtended.unidad = row["unidad"].ToString();
                articuloAnexoExtended.cantidad_um = decimal.Parse(row["cantidad_um"].ToString());
                articuloAnexoExtended.costo = decimal.Parse(row["costo"].ToString());
                articuloAnexoExtended.stock_cja = decimal.Parse(row["stock_cja"].ToString());
                articuloAnexoExtended.stock_pza = decimal.Parse(row["stock_pza"].ToString());
                articuloAnexoExtended.sugerido = decimal.Parse(row["sugerido"].ToString());
                articuloAnexoExtended.pedir = 0m;
                list.Add(articuloAnexoExtended);
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloExtended> getEntradas(Guid id)
        {
            string sql = $"SELECT ent.cod_barras,ent.cod_interno,ent.descripcion,ent.um,ent.cantidad_um,SUM(cant_cja) cant_cja, SUM(cant_pza) cant_pza, ent.precio_compra, SUM(ent.total) total\r\nFROM (SELECT ea.cod_barras, a.cod_interno, CASE WHEN ea.cant_ent > 0 THEN a.desc_anexo ELSE a.desc_principal END descripcion, CASE WHEN ea.cant_ent > 0 THEN a.unidad_anexo ELSE a.unidad_principal END um, CASE WHEN ea.cant_ent > 0 THEN a.cantidad_uma ELSE a.cantidad_ump END cantidad_um, ea.cant_ent cant_cja, ea.cant_pza, a.precio_compra, (a.precio_compra * ea.cant_pza) total\r\nFROM entrada_articulo ea JOIN vw_articulos_principales a ON ea.cod_barras=a.cod_principal WHERE ea.id_entrada='{id}') ent \r\nGROUP BY ent.cod_barras,ent.cod_interno,ent.descripcion,ent.um,ent.cantidad_um, ent.precio_compra ORDER BY ent.descripcion";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloExtended> list = new List<ArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new ArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    cod_interno = row["cod_interno"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                    unidad = row["um"].ToString(),
                    umc = decimal.Parse(row["cantidad_um"].ToString()),
                    cant_cja = decimal.Parse(row["cant_cja"].ToString()),
                    cant_pza = decimal.Parse(row["cant_pza"].ToString()),
                    precio_compra = decimal.Parse(row["precio_compra"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloExtended> getSalidas(Guid id)
        {
            string sql = $"SELECT ent.cod_barras,ent.cod_interno,ent.descripcion,ent.um,ent.cantidad_um,SUM(cant_cja) cant_cja, SUM(cant_pza) cant_pza, ent.precio_compra, SUM(ent.total) total\r\nFROM (SELECT ea.cod_barras, a.cod_interno, CASE WHEN ea.cant_sal > 0 THEN a.desc_anexo ELSE a.desc_principal END descripcion, CASE WHEN ea.cant_sal > 0 THEN a.unidad_anexo ELSE a.unidad_principal END um, CASE WHEN ea.cant_sal > 0 THEN a.cantidad_uma ELSE a.cantidad_ump END cantidad_um, ea.cant_sal cant_cja, ea.cant_pza, a.precio_compra, (a.precio_compra * ea.cant_pza) total\r\nFROM salida_articulo ea JOIN vw_articulos_principales a ON ea.cod_barras=a.cod_principal WHERE ea.id_salida='{id}') ent \r\nGROUP BY ent.cod_barras,ent.cod_interno,ent.descripcion,ent.um,ent.cantidad_um, ent.precio_compra ORDER BY ent.descripcion";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloExtended> list = new List<ArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new ArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    cod_interno = row["cod_interno"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                    unidad = row["um"].ToString(),
                    umc = decimal.Parse(row["cantidad_um"].ToString()),
                    cant_cja = decimal.Parse(row["cant_cja"].ToString()),
                    cant_pza = decimal.Parse(row["cant_pza"].ToString()),
                    precio_compra = decimal.Parse(row["precio_compra"].ToString()),
                    total = decimal.Parse(row["total"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloExtended> findAndGetItems(Guid id_proveedor, string orderBy, string asc_des)
        {
            string sql = $"SELECT a.cod_barras,a.descripcion,um.descripcion unidad,a.precio_compra,a.utilidad,a.precio_venta,a.iva FROM articulo a JOIN unidad_medida um ON a.id_unidad=um.id_unidad\r\nWHERE a.tipo_articulo='principal' AND a.id_proveedor='{id_proveedor}' ORDER BY a.{orderBy}";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloExtended> list = new List<ArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new ArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion_larga = row["descripcion"].ToString(),
                    unidad = row["unidad"].ToString(),
                    precio_compra = decimal.Parse(row["precio_compra"].ToString()),
                    utilidad = decimal.Parse(row["utilidad"].ToString()),
                    precio_venta = decimal.Parse(row["precio_venta"].ToString()),
                    iva = decimal.Parse(row["iva"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloExtended> findAndGetItems(string desc, string orderBy, string asc_des)
        {
            string sql = $"SELECT a.cod_barras,a.descripcion,um.descripcion unidad,a.precio_compra,a.utilidad,a.precio_venta,a.iva FROM articulo a JOIN unidad_medida um ON a.id_unidad=um.id_unidad\r\nWHERE a.tipo_articulo='principal' AND a.descripcion LIKE '%{desc}%' ORDER BY a.{orderBy}";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloExtended> list = new List<ArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new ArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion_larga = row["descripcion"].ToString(),
                    unidad = row["unidad"].ToString(),
                    precio_compra = decimal.Parse(row["precio_compra"].ToString()),
                    utilidad = decimal.Parse(row["utilidad"].ToString()),
                    precio_venta = decimal.Parse(row["precio_venta"].ToString()),
                    iva = decimal.Parse(row["iva"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }

        public List<ArticuloExtended> findAndGetItems(Guid id_proveedor, string desc, string orderBy, string asc_des)
        {
            string sql = $"SELECT a.cod_barras,a.descripcion,um.descripcion unidad,a.precio_compra,a.utilidad,a.precio_venta,a.iva FROM articulo a JOIN unidad_medida um ON a.id_unidad=um.id_unidad\r\nWHERE (a.tipo_articulo='principal' AND a.id_proveedor='{id_proveedor}') AND a.descripcion LIKE '%{desc}%' ORDER BY a.{orderBy}";
            DataSet dataSet = GetDataSet(sql);
            List<ArticuloExtended> list = new List<ArticuloExtended>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(new ArticuloExtended
                {
                    cod_barras = row["cod_barras"].ToString(),
                    descripcion_larga = row["descripcion"].ToString(),
                    unidad = row["unidad"].ToString(),
                    precio_compra = decimal.Parse(row["precio_compra"].ToString()),
                    utilidad = decimal.Parse(row["utilidad"].ToString()),
                    precio_venta = decimal.Parse(row["precio_venta"].ToString()),
                    iva = decimal.Parse(row["iva"].ToString())
                });
            }
            dataSet.Dispose();
            if (list.Count <= 0)
            {
                return null;
            }
            return list;
        }
    }
}
