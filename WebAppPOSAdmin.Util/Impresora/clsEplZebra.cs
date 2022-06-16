using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace WebAppPOSAdmin.Util.Impresora
{
    public class clsEplZebra
    {
        public struct SECURITY_ATTRIBUTES
        {
            private int nLength;

            private int lpSecurityDescriptor;

            private int bInheritHandle;
        }

        private const int GENERIC_WRITE = 1073741824;

        private const int OPEN_EXISTING = 3;

        private const int FILE_SHARE_WRITE = 2;

        private StreamWriter _fileWriter;

        private FileStream _outFile;

        private SafeFileHandle handle;

        private int _hPort;

        [DllImport("Kernel32")]
        public static extern int CloseHandle(int hObject);

        [DllImport("Kernel32", EntryPoint = "CreateFileA")]
        public static extern int CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, ref SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

        public void StartWrite(string printerPath)
        {
            try
            {
                SECURITY_ATTRIBUTES lpSecurityAttributes = default(SECURITY_ATTRIBUTES);
                IntPtr intPtr = default(IntPtr);
                _hPort = CreateFile(printerPath, 1073741824, 2, ref lpSecurityAttributes, 3, 0, 0);
                intPtr = new IntPtr(_hPort);
                _outFile = new FileStream(intPtr, FileAccess.Write);
                _fileWriter = new StreamWriter(_outFile);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void Write(string rawLine)
        {
            try
            {
                _fileWriter.WriteLine(rawLine);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public void EndWrite()
        {
            try
            {
                _fileWriter.Flush();
                _fileWriter.Close();
                _outFile.Close();
                CloseHandle(_hPort);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
    }
}
