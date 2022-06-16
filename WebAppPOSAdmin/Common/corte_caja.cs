using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPOSAdmin.Common
{
	public class corte_caja
	{
		public DateTime fecha_inicial { get; set; }

		public DateTime fecha_final { get; set; }

		public string cajero { get; set; }

		public int caja { get; set; }

		public int transacciones { get; set; }

		public long folio_inicial { get; set; }

		public long folio_final { get; set; }

		public decimal total_vendido { get; set; }

		public decimal pago_tc { get; set; }

		public decimal pago_cheque { get; set; }

		public decimal pago_vales { get; set; }

		public decimal pago_efectivo { get; set; }

		public decimal total_devoluciones { get; set; }

		public decimal iva { get; set; }
	}
}