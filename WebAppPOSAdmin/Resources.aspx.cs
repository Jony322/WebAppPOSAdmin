using System;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;

using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Security.SeguridadSession;

namespace WebAppPOSAdmin
{
    public partial class Resources : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new CtrlSession().validaSession(this);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getEstadistica(string barCode)
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                return JsonConvert.SerializeObject((from e in dcContextoSuPlazaDataContext.estadistica
                                                    where e.cod_barras.Equals(barCode)
                                                    orderby e.anio descending
                                                    select new
                                                    {
                                                        e.anio,
                                                        e.cod_barras,
                                                        e.ene,
                                                        e.feb,
                                                        e.mar,
                                                        e.abr,
                                                        e.may,
                                                        e.jun,
                                                        e.jul,
                                                        e.ago,
                                                        e.sep,
                                                        e.oct,
                                                        e.nov,
                                                        e.dic
                                                    }).Take(2), Formatting.None);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getCodigo(string typeCode)
        {
            try
            {
                return typeCode switch
                {
                    "normal" => new RepositorioArticulos().getCodigoNormal().ToString(),
                    "pesable" => new RepositorioArticulos().getCodigoPesable().ToString(),
                    "nopesable" => new RepositorioArticulos().getCodigoNoPesable().ToString(),
                    _ => null,
                };
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string validateBarCode(string barCode)
        {
            try
            {
                return new ArticuloCtrl().getArticulo(barCode)?.cod_barras;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string validateBarCodeKit(string barCode)
        {
            try
            {
                return JsonConvert.SerializeObject(from a in new ArticuloCtrl().findBy(barCode, ArticuloCtrl.FindArticuloBy.BarCode, kit: true)
                                                   select new { a.cod_barras, a.kit }, Formatting.None);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool existInternalCode(string barCode)
        {
            return true;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string findArticulos(string findOption, string findText, bool kit)
        {
            try
            {
                if (findOption.Equals("barCode"))
                {
                    return JsonConvert.SerializeObject(from a in new ArticuloCtrl().findBy(findText, ArticuloCtrl.FindArticuloBy.BarCode, kit)
                                                       select new { a.cod_barras, a.cod_interno, a.descripcion, a.stock }, Formatting.None);
                }
                if (findOption.Equals("internalCode"))
                {
                    return JsonConvert.SerializeObject(from a in new ArticuloCtrl().findBy(findText, ArticuloCtrl.FindArticuloBy.InternalCode, kit)
                                                       select new { a.cod_barras, a.cod_interno, a.descripcion, a.stock }, Formatting.None);
                }
                if (findOption.Equals("description"))
                {
                    return JsonConvert.SerializeObject(from a in new ArticuloCtrl().findBy(findText, ArticuloCtrl.FindArticuloBy.Descripcion, kit)
                                                       select new { a.cod_barras, a.cod_interno, a.descripcion, a.stock }, Formatting.None);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool validateInternalCode(string internalCode)
        {
            try
            {
                return new ArticuloCtrl().existInternalCode(internalCode);
            }
            catch
            {
                return false;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getArticuloAnexo(string barCode)
        {
            try
            {
                return JsonConvert.SerializeObject(new ArticuloCtrl().getArticuloAnexo(barCode), Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool KeepActiveSession()
        {
            if (HttpContext.Current.Session["usuarioSession"] != null)
            {
                return true;
            }
            return false;
        }
    }
}