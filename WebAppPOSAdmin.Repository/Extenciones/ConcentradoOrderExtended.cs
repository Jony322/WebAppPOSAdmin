using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppPOSAdmin.Repository.Extenciones
{
	public class ConcentradoOrderExtended
	{
		public decimal[] entradas = new decimal[10];

		public string cod_barras { get; set; }

		public string descripcion { get; set; }

		public string unidad { get; set; }

		public decimal umc { get; set; }

		public decimal costo { get; set; }

		public decimal cant_pedido { get; set; }

		public decimal total
		{
			get
			{
				decimal result = default(decimal);
				decimal[] array = entradas;
				foreach (decimal num in array)
				{
					result += num;
				}
				return result;
			}
		}
	}
}
