using System;
using System.Drawing.Printing;
using System.Linq;

namespace WebAppPOSAdmin.Util.Impresora
{
    public class ServerPrinters
    {
        public static string[] getAvaiblePrinters()
        {
            return PrinterSettings.InstalledPrinters.Cast<string>().ToArray();
        }
    }
}
