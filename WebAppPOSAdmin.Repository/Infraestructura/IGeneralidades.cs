using System;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Infraestructura
{
	public interface IGeneralidades
	{
		bool insertarGenradorCodBarras(generacion_codigos _gc);

		bool actualizarGenradorCodBarras(generacion_codigos _gc, string tipo_codigo);

		generacion_codigos getCodigoByTipoCodigo(int id);

		pos_admin_settings printerLabel(Guid id_setting);

		bool insertarSetting(pos_admin_settings _pos);

		void actualizarSetting(Guid id, pos_admin_settings _pos);

		List<pos_admin_settings> listaSettings();
	}

}
