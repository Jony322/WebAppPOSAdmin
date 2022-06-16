using System;
using System.Collections;

namespace WebAppPOSAdmin.Security.SeguridadSession
{
    public class SessionManager
    {
        private Hashtable parametros;

        public Guid IdUsuario { get; set; }

        public string IdGenerico { get; set; }

        public string pantalla { get; set; }

        public Hashtable Parametros
        {
            get
            {
                if (parametros == null)
                {
                    parametros = new Hashtable();
                }
                return parametros;
            }
            set
            {
                parametros = value;
            }
        }
    }
}
