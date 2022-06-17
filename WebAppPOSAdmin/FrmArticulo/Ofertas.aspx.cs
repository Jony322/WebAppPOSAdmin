using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Clases;
using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Scripts;

using WebAppPOSAdmin.Common;
using NLog;

namespace WebAppPOSAdmin.FrmArticulo
{
    public partial class Ofertas : System.Web.UI.Page
    {
        #region  logger
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Logger loggerdb = LogManager.GetLogger("databaseLogger");
        #endregion

        private enum FilterBy
        {
            All,
            Available,
            Suspended,
            Canceled
        }

        private enum Label
        {
            Adhesive,
            Anaquel
        }

        private FilterBy filter;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadOffers();
                Session.Remove("oferta");
                Session.Remove("OfferItems");
            }
        }

        protected void rbtFilter_CheckedChanged(object sender, EventArgs e)
        {
            switch (((CheckBox)sender).ID)
            {
                case "rbtAll":
                    filter = FilterBy.All;
                    break;
                case "rbtAvailable":
                    filter = FilterBy.Available;
                    break;
                case "rbtSuspended":
                    filter = FilterBy.Suspended;
                    break;
                case "rbtCanceled":
                    filter = FilterBy.Canceled;
                    break;
            }
            loadOffers();
        }

        private void loadOffers()
        {
            try
            {
                using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                switch (filter)
                {
                    case FilterBy.All:
                        gvOferta.DataSource = from o in dcContextoSuPlazaDataContext.oferta
                                              orderby o.num_oferta descending
                                              select new
                                              {
                                                  id_oferta = o.id_oferta,
                                                  num_oferta = o.num_oferta,
                                                  descripcion = o.descripcion,
                                                  fecha_ini = o.fecha_ini,
                                                  fecha_fin = o.fecha_fin,
                                                  days_expires = o.daysToExpires(),
                                                  status_oferta = o.status_oferta
                                              };
                        break;
                    case FilterBy.Available:
                        gvOferta.DataSource = from o in dcContextoSuPlazaDataContext.oferta
                                              orderby o.num_oferta descending
                                              where o.status_oferta.Equals("disponible")
                                              select new
                                              {
                                                  id_oferta = o.id_oferta,
                                                  num_oferta = o.num_oferta,
                                                  descripcion = o.descripcion,
                                                  fecha_ini = o.fecha_ini,
                                                  fecha_fin = o.fecha_fin,
                                                  days_expires = o.daysToExpires(),
                                                  status_oferta = o.status_oferta
                                              };
                        break;
                    case FilterBy.Suspended:
                        gvOferta.DataSource = from o in dcContextoSuPlazaDataContext.oferta
                                              orderby o.num_oferta descending
                                              where o.status_oferta.Equals("suspendida")
                                              select new
                                              {
                                                  id_oferta = o.id_oferta,
                                                  num_oferta = o.num_oferta,
                                                  descripcion = o.descripcion,
                                                  fecha_ini = o.fecha_ini,
                                                  fecha_fin = o.fecha_fin,
                                                  days_expires = o.daysToExpires(),
                                                  status_oferta = o.status_oferta
                                              };
                        break;
                    case FilterBy.Canceled:
                        gvOferta.DataSource = from o in dcContextoSuPlazaDataContext.oferta
                                              orderby o.num_oferta descending
                                              where o.status_oferta.Equals("cancelada")
                                              select new
                                              {
                                                  id_oferta = o.id_oferta,
                                                  num_oferta = o.num_oferta,
                                                  descripcion = o.descripcion,
                                                  fecha_ini = o.fecha_ini,
                                                  fecha_fin = o.fecha_fin,
                                                  days_expires = o.daysToExpires(),
                                                  status_oferta = o.status_oferta
                                              };
                        break;
                }
                gvOferta.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Ofertas " + "Acción: loadOffers " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"MyMessageBox('VISOR DE OFERTAS','ERROR: {ex.Message}','error')", addScriptTags: true);
            }
        }

        private void loadDetailOffer(Guid id_oferta)
        {
            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
            gvDetailOferta.DataSource = from d in dcContextoSuPlazaDataContext.oferta_articulo
                                        orderby d.articulo.descripcion
                                        where d.id_oferta.Equals(id_oferta)
                                        select new
                                        {
                                            d.id_oferta,
                                            d.cod_barras,
                                            d.articulo.descripcion,
                                            d.precio_oferta,
                                            d.status_oferta
                                        };
            gvDetailOferta.DataBind();
        }

        protected void gvOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "view":
                        gvDetailOferta.DataSource = null;
                        gvDetailOferta.DataBind();
                        loadDetailOffer(Guid.Parse(e.CommandArgument.ToString()));
                        break;
                    case "recover":
                        {
                            Guid id = Guid.Parse(e.CommandArgument.ToString());
                            if (new RepositorioOfertas().isSuspendedOffer(id))
                            {
                                base.Response.Redirect($"~/FrmArticulo/OfertaArticulos.aspx?id={e.CommandArgument.ToString()}", endResponse: false);
                                break;
                            }
                            throw new Exception("Sólo ofertas suspendidas se pueden recuperar");
                        }
                    case "cancel":
                        new RepositorioOfertas().cancelarOferta(Guid.Parse(e.CommandArgument.ToString()));
                        loadOffers();
                        break;
                    case "adherible":
                        {
                            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext3 = new dcContextoSuPlazaDataContext();
                            foreach (oferta_articulo item in (from a in dcContextoSuPlazaDataContext3.oferta_articulo
                                                              where a.id_oferta.Equals(Guid.Parse(e.CommandArgument.ToString()))
                                                              orderby a.articulo.descripcion
                                                              select a).ToList())
                            {
                                new ZebraPrinterController().printLblAdherible(item.cod_barras, 1, normalPrice: false);
                            }
                            break;
                        }
                    case "anaquel":
                        {
                            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext2 = new dcContextoSuPlazaDataContext();
                            foreach (oferta_articulo item2 in (from a in dcContextoSuPlazaDataContext2.oferta_articulo
                                                               where a.id_oferta.Equals(Guid.Parse(e.CommandArgument.ToString()))
                                                               orderby a.articulo.descripcion
                                                               select a).ToList())
                            {
                                new ZebraPrinterController().printLblAnaquel(item2.cod_barras, 1, normalPrice: false);
                            }
                            break;
                        }
                    case "estadistica":
                        {
                            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                            oferta value = dcContextoSuPlazaDataContext.oferta.FirstOrDefault((oferta o) => o.id_oferta.Equals(Guid.Parse(e.CommandArgument.ToString())));
                            List<OfferItems> list = new Procedures().OfferStatistics(Guid.Parse(e.CommandArgument.ToString()));
                            if (list == null)
                            {
                                throw new Exception("No hay estadística por mostrar de la oferta.");
                            }
                            Session["oferta"] = value;
                            Session["OfferItems"] = list;
                            string arg = "http://" + base.Request["HTTP_HOST"] + "/PdfReports/EstadisticaOferta.aspx";
                            string script = $"window.open('{arg}', '_blank', 'toolbar=no,scrollbars=no,resizable=yes,top=0,left=0,width=800,height=600');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "modal", script, addScriptTags: true);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Ofertas " + "Acción: gvOferta_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }

        protected void gvDetailOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] array = e.CommandArgument.ToString().Split(',');
            Guid offerID = Guid.Parse(array[0]);
            string barCode = array[1];
            try
            {
                switch (e.CommandName)
                {
                    case "adherible":
                        {
                            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext2 = new dcContextoSuPlazaDataContext();
                            oferta_articulo oferta_articulo2 = dcContextoSuPlazaDataContext2.oferta_articulo.FirstOrDefault((oferta_articulo a) => a.id_oferta.Equals(offerID) && a.cod_barras.Equals(barCode));
                            new ZebraPrinterController().printLblAdherible(oferta_articulo2.cod_barras, 1, normalPrice: false);
                            break;
                        }
                    case "anaquel":
                        {
                            using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
                            oferta_articulo oferta_articulo = dcContextoSuPlazaDataContext.oferta_articulo.FirstOrDefault((oferta_articulo a) => a.id_oferta.Equals(offerID) && a.cod_barras.Equals(barCode));
                            new ZebraPrinterController().printLblAnaquel(oferta_articulo.cod_barras, 1, normalPrice: false);
                            break;
                        }
                    case "delete":
                        new RepositorioOfertas().deleteItemOffer(offerID, barCode);
                        loadDetailOffer(offerID);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excepción Generada en: Ofertas " + "Acción: gvDetailOferta_RowCommand " + ex.Message);
                loggerdb.Error(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", $"alert('{ex.Message}');", addScriptTags: true);
            }
        }
    }
}