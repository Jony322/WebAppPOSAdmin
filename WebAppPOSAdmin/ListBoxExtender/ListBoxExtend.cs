using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using WebAppPOSAdmin.Repository.Entidad;
using WebAppPOSAdmin.Repository.Repositorios;
using WebAppPOSAdmin.Repository.Infraestructura;

namespace WebAppPOSAdmin.ListBoxExtender
{
    public static class ListBoxExtend
    {
        public static void getListBox(this ListBox _list, ref RadioButtonList _chk)
        {
            IPermisos permisos = new RepositorioPermisos();
            string tipo_sistema = null;
            for (int i = 0; i < _chk.Items.Count; i++)
            {
                if (_chk.Items[i].Selected)
                {
                    tipo_sistema = _chk.Items[i].Value.ToString();
                    break;
                }
            }
            List<permiso> allPermisos = permisos.getAllPermisos(tipo_sistema);
            _list.DataTextField = "descripcion";
            _list.DataValueField = "id_permiso";
            _list.DataSource = allPermisos;
            _list.DataBind();
        }

        public static void GetListBoxFiltroPermiso(this ListBox _list, Guid id_empleado, string filtro)
        {
            List<permiso> filtroListBoxPermiso = ((IPermisos)new RepositorioPermisos()).getFiltroListBoxPermiso(id_empleado, filtro);
            _list.DataTextField = "descripcion";
            _list.DataValueField = "id_permiso";
            _list.DataSource = filtroListBoxPermiso;
            _list.DataBind();
        }

        public static void GetListBoxFiltro(this ListBox _list, usuario userTemp)
        {
            List<permiso> permisoFiltro = ((IPermisos)new RepositorioPermisos()).getPermisoFiltro(userTemp);
            _list.DataTextField = "descripcion";
            _list.DataValueField = "id_permiso";
            _list.DataSource = permisoFiltro;
            _list.DataBind();
        }
    }
}