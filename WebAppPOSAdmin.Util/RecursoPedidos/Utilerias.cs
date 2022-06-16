using System;

namespace WebAppPOSAdmin.Util.RecursoPedidos
{
    public class Utilerias
    {
        public decimal convertiACajas(decimal stock, decimal cantidad_um)
        {
            if (cantidad_um > stock)
            {
                return 0.00m;
            }
            return decimal.Floor(stock / cantidad_um);
        }

        public decimal convertirAPiezas(decimal stock, decimal resto, decimal cantidad_um)
        {
            decimal num = resto * cantidad_um;
            return stock - num;
        }

        public decimal calcularSugerido(decimal diasApedir, int mes, decimal valor)
        {
            decimal num = valor;
            switch (mes)
            {
                case 1:
                    num /= 31m;
                    break;
                case 2:
                    num /= 28m;
                    break;
                case 3:
                    num /= 31m;
                    break;
                case 4:
                    num /= 30m;
                    break;
                case 5:
                    num /= 31m;
                    break;
                case 6:
                    num /= 30m;
                    break;
                case 7:
                    num /= 31m;
                    break;
                case 8:
                    num /= 31m;
                    break;
                case 9:
                    num /= 30m;
                    break;
                case 10:
                    num /= 31m;
                    break;
                case 11:
                    num /= 30m;
                    break;
                case 12:
                    num /= 31m;
                    break;
            }
            return num * diasApedir;
        }

        public decimal regresarValor(decimal[] lista, int mes)
        {
            decimal result = 0.0m;
            int num = 1;
            foreach (decimal result2 in lista)
            {
                if (num == mes)
                {
                    return result2;
                }
                num++;
            }
            return result;
        }
    }
}
