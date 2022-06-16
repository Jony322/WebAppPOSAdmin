using System;

namespace WebAppPOSAdmin.Util.Validadores
{
    public class Validaciones
    {
        public bool validarFormatoImagen(string extension)
        {
            extension = extension.Substring(extension.LastIndexOf(".") + 1).ToLower();
            if (Array.IndexOf(new string[4] { "jpg", "png", "jpeg", "bmp" }, extension) < 0)
            {
                return false;
            }
            return true;
        }
    }
}
