
namespace WebAppPOSAdmin.Funcionalidades
{
    public class Utilerias
    {
		public decimal convertiACajas(decimal stock, decimal cantidad_um)
		{
			return decimal.Floor(stock / cantidad_um);
		}

		public decimal convertirAPiezas(decimal stock, decimal resto, decimal cantidad_um)
		{
			decimal num = resto * cantidad_um;
			return stock - num;
		}
	}
}