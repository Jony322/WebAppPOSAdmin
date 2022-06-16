
namespace WebAppPOSAdmin.Common
{
	public class almacen_entrada_salida
	{
		public string cod_barras { get; set; }

		public string descripcion_larga { get; set; }

		public string unidad_medida { get; set; }

		public string cod_interno { get; set; }

		public string usuario { get; set; }

		public string fecha { get; set; }

		public string observacion { get; set; }

		public long id { get; set; }

		public decimal total_entrada { get; set; }

		public decimal cantidad_anexo { get; set; }

		public decimal cantidad_um { get; set; }

		public decimal total_piezas { get; set; }

		public decimal precio_compra { get; set; }
	}
}