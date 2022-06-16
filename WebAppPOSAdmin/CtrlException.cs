using System;
using System.IO;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin
{
    public class CtrlException
    {
        private static string fileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\error.log";

        public static void SetError(string msg)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }
            StreamWriter streamWriter = new StreamWriter(fileName, append: true);
            streamWriter.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), msg));
            streamWriter.Close();
        }

        public static void SetErrorDB(string Message)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            dcContextoSuPlazaDataContext.ctrl_errores.InsertOnSubmit(new ctrl_errores
            {
                id_error = Guid.NewGuid(),
                fecha_log = DateTime.Now,
                descripcion = Message
            });
            dcContextoSuPlazaDataContext.SubmitChanges();
        }
    }
}