using System;
using System.Text;
using System.Linq;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Util.Impresora;

namespace WebAppPOSAdmin.Common
{

    public class ZebraPrinterController
    {
        public void printLblAnaquel(string barCode, int qty, bool normalPrice)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            string text = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().path_ptr_label ?? null;
            if (text == null)
            {
                throw new Exception("No hay impresora de etiquetas configurada");
            }
            vw_oferta_articulo vw_oferta_articulo = dcContextoSuPlazaDataContext.vw_oferta_articulo.FirstOrDefault((vw_oferta_articulo i) => i.cod_barras.Equals(barCode));
            string text2 = $"{'"'}";
            string text3 = $"{'\n'}";
            StringBuilder stringBuilder = new StringBuilder("");
            for (int j = 1; j <= qty; j++)
            {
                stringBuilder.Append(string.Format("N{1}S4{1}A50,27,1,1,2,1,N,{0}{3}{0}{1}B280,66,1,1,3,9,59,B,{0}{2}{0}{1}A180,110,1,4,4,2,N,{0}${0}{1}A180,145,1,4,4,2,N,{0}{4}{0}{1}P1{1}", text2, text3, barCode, vw_oferta_articulo.descripcion, normalPrice ? vw_oferta_articulo.precio_venta.ToString("F2") : vw_oferta_articulo.precio_oferta.ToString("F2")));
            }
            ZebraPrinter.RawPrinter.SendToPrinter("Etiqueta Anaquel", stringBuilder.ToString(), text);
        }

        public void printLblAdherible(string barCode, int qty, bool normalPrice, decimal offerPrice)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            string text = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().path_ptr_label ?? null;
            if (text == null)
            {
                throw new Exception("No hay impresora de etiquetas configurada");
            }
            vw_oferta_articulo vw_oferta_articulo = dcContextoSuPlazaDataContext.vw_oferta_articulo.FirstOrDefault((vw_oferta_articulo i) => i.cod_barras.Equals(barCode));
            string text2 = $"{'"'}";
            string text3 = $"{'\n'}";
            StringBuilder stringBuilder = new StringBuilder("");
            for (int j = 1; j <= qty; j++)
            {
                stringBuilder.Append(string.Format("N{1}S4{1}A50,27,1,1,2,1,N,{0}{3}{0}{1}B280,66,1,1,3,9,59,B,{0}{2}{0}{1}A120,110,1,1,2,2,N,{0}${0}{1}A120,145,1,1,2,2,N,{0}{4}{0}{1}P1{1}", text2, text3, barCode, vw_oferta_articulo.descripcion, offerPrice.ToString("F2")));
            }
            ZebraPrinter.RawPrinter.SendToPrinter("Etiqueta Adherible", stringBuilder.ToString(), text);
        }

        public void printLblAnaquel(string barCode, int qty, bool normalPrice, decimal offerPrice)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            string text = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().path_ptr_label ?? null;
            if (text == null)
            {
                throw new Exception("No hay impresora de etiquetas configurada");
            }
            vw_oferta_articulo vw_oferta_articulo = dcContextoSuPlazaDataContext.vw_oferta_articulo.FirstOrDefault((vw_oferta_articulo i) => i.cod_barras.Equals(barCode));
            string text2 = $"{'"'}";
            string text3 = $"{'\n'}";
            StringBuilder stringBuilder = new StringBuilder("");
            for (int j = 1; j <= qty; j++)
            {
                stringBuilder.Append(string.Format("N{1}S4{1}A50,27,1,1,2,1,N,{0}{3}{0}{1}B280,66,1,1,3,9,59,B,{0}{2}{0}{1}A180,110,1,4,4,2,N,{0}${0}{1}A180,145,1,4,4,2,N,{0}{4}{0}{1}P1{1}", text2, text3, barCode, vw_oferta_articulo.descripcion, offerPrice.ToString("F2")));
            }
            ZebraPrinter.RawPrinter.SendToPrinter("Etiqueta Anaquel", stringBuilder.ToString(), text);
        }

        public void printLblAdherible(string barCode, int qty, bool normalPrice)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            string text = dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().path_ptr_label ?? null;
            if (text == null)
            {
                throw new Exception("No hay impresora de etiquetas configurada");
            }
            vw_oferta_articulo vw_oferta_articulo = dcContextoSuPlazaDataContext.vw_oferta_articulo.FirstOrDefault((vw_oferta_articulo i) => i.cod_barras.Equals(barCode));
            string text2 = $"{'"'}";
            string text3 = $"{'\n'}";
            StringBuilder stringBuilder = new StringBuilder("");
            for (int j = 1; j <= qty; j++)
            {
                stringBuilder.Append(string.Format("N{1}S4{1}A50,27,1,1,2,1,N,{0}{3}{0}{1}B280,66,1,1,3,9,59,B,{0}{2}{0}{1}A120,110,1,1,2,2,N,{0}${0}{1}A120,145,1,1,2,2,N,{0}{4}{0}{1}P1{1}", text2, text3, barCode, vw_oferta_articulo.descripcion, normalPrice ? vw_oferta_articulo.precio_venta.ToString("F2") : vw_oferta_articulo.precio_oferta.ToString("F2")));
            }
            ZebraPrinter.RawPrinter.SendToPrinter("Etiqueta Adherible", stringBuilder.ToString(), text);
        }
    }

}