using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WebAppPOSAdmin.Util.Impresora
{
    public class RawPrinterHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFOA pDocInfo);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern bool WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern long EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern long EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern long ClosePrinter(IntPtr hPrinter);

        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
        {
            IntPtr phPrinter = default(IntPtr);
            DOCINFOA pDocInfo = new DOCINFOA();
            pDocInfo.pDocName = "My C#.NET RAW Document";
            pDocInfo.pDataType = "RAW";
            if (OpenPrinter(szPrinterName, ref phPrinter, 0))
            {
                if (StartDocPrinter(phPrinter, 1, ref pDocInfo))
                {
                    if (StartPagePrinter(phPrinter))
                    {
                        EndPagePrinter(phPrinter);
                    }
                    EndDocPrinter(phPrinter);
                }
                ClosePrinter(phPrinter);
            }
            if (0 == 0)
            {
                Marshal.GetLastWin32Error();
            }
            return false;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            FileStream fileStream = new FileStream(szFileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            _ = new byte[fileStream.Length];
            IntPtr intPtr = new IntPtr(0);
            int num = Convert.ToInt32(fileStream.Length);
            byte[] source = binaryReader.ReadBytes(num);
            intPtr = Marshal.AllocCoTaskMem(num);
            Marshal.Copy(source, 0, intPtr, num);
            bool result = SendBytesToPrinter(szPrinterName, intPtr, num);
            Marshal.FreeCoTaskMem(intPtr);
            return result;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            int length = szString.Length;
            IntPtr intPtr = Marshal.StringToCoTaskMemAnsi(szString);
            SendBytesToPrinter(szPrinterName, intPtr, length);
            Marshal.FreeCoTaskMem(intPtr);
            return true;
        }
    }
}
