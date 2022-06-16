using System;
using System.Runtime.InteropServices;

namespace WebAppPOSAdmin.Util.Impresora
{
    public class ZebraPrinter
    {
        public struct DOCINFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string printerDocumentName;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pOutputFile;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string printerDocumentDataType;
        }

        public class RawPrinter
        {
            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
            public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
            public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern long StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, ExactSpelling = true)]
            public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern long EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern long EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern long ClosePrinter(IntPtr hPrinter);

            public static void SendToPrinter(string printerJobName, string rawStringToSendToThePrinter, string printerNameAsDescribedByPrintManager)
            {
                string text = rawStringToSendToThePrinter.Replace("&#241;", "ñ");
                IntPtr phPrinter = default(IntPtr);
                DOCINFO pDocInfo = default(DOCINFO);
                int pcWritten = 0;
                pDocInfo.printerDocumentName = printerJobName;
                pDocInfo.printerDocumentDataType = "RAW";
                OpenPrinter(printerNameAsDescribedByPrintManager, ref phPrinter, 0);
                StartDocPrinter(phPrinter, 1, ref pDocInfo);
                StartPagePrinter(phPrinter);
                WritePrinter(phPrinter, text, text.Length, ref pcWritten);
                EndPagePrinter(phPrinter);
                EndDocPrinter(phPrinter);
                ClosePrinter(phPrinter);
            }
        }
    }
}
