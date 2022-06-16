using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using WebAppPOSAdmin.Repository.Infraestructura;
using WebAppPOSAdmin.Repository.Entidad;

namespace WebAppPOSAdmin.Repository.Repositorios
{

	public class RepositorioGeneralidades : IGeneralidades
	{
		public bool insertarGenradorCodBarras(generacion_codigos _gc)
		{
			bool result = false;
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					dcContextoSuPlazaDataContext.generacion_codigos.InsertOnSubmit(_gc);
					dcContextoSuPlazaDataContext.SubmitChanges();
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public bool actualizarGenradorCodBarras(generacion_codigos _gc, string tipo_codigo)
		{
			bool result = false;
			try
			{
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					Expression<Func<generacion_codigos, bool>> expression = (generacion_codigos p) => p.id_gencode == _gc.id_gencode;
					generacion_codigos generacion_codigos = dcContextoSuPlazaDataContext.generacion_codigos.Where(expression.Compile()).FirstOrDefault();
					if (generacion_codigos != null)
					{
						switch (tipo_codigo)
						{
							case "Normal":
								generacion_codigos.cod_barras = _gc.cod_barras;
								break;
							case "Pesable":
								generacion_codigos.cod_pesable = _gc.cod_pesable;
								break;
							case "No Pesable":
								generacion_codigos.cod_inpesable = _gc.cod_inpesable;
								break;
						}
						dcContextoSuPlazaDataContext.SubmitChanges();
						result = true;
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return result;
			}
		}

		public generacion_codigos getCodigoByTipoCodigo(int id)
		{
			generacion_codigos result = new generacion_codigos();
			try
			{
				Expression<Func<generacion_codigos, bool>> expression = (generacion_codigos p) => p.id_gencode == (long)id;
				using (dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext())
				{
					result = dcContextoSuPlazaDataContext.generacion_codigos.Where(expression.Compile()).FirstOrDefault();
				}
				return result;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public pos_admin_settings printerLabel(Guid id_setting)
		{
			try
			{
				_ = string.Empty;
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				Expression<Func<pos_admin_settings, bool>> expression = (pos_admin_settings p) => p.id_setting == id_setting;
				new pos_admin_settings();
				return dcContextoSuPlazaDataContext.pos_admin_settings.Where(expression.Compile()).FirstOrDefault();
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public bool insertarSetting(pos_admin_settings _pos)
		{
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				dcContextoSuPlazaDataContext.pos_admin_settings.InsertOnSubmit(_pos);
				dcContextoSuPlazaDataContext.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return false;
			}
		}

		public void actualizarSetting(Guid id, pos_admin_settings _pos)
		{
			try
			{
				Expression<Func<pos_admin_settings, bool>> expression = (pos_admin_settings p) => p.id_setting == id;
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				new pos_admin_settings();
				dcContextoSuPlazaDataContext.pos_admin_settings.Where(expression.Compile()).FirstOrDefault().path_ptr_label = _pos.path_ptr_label;
				dcContextoSuPlazaDataContext.SubmitChanges();
			}
			catch (Exception ex)
			{
				_ = ex.Message;
			}
		}

		public List<pos_admin_settings> listaSettings()
		{
			try
			{
				new List<pos_admin_settings>();
				return new dcContextoSuPlazaDataContext().pos_admin_settings.ToList();
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return null;
			}
		}

		public pos_admin_settings getSettingsPrinter()
		{
			return new dcContextoSuPlazaDataContext().pos_admin_settings.FirstOrDefault() ?? null;
		}

		public void saveSetting(string printName)
		{
			try
			{
				using dcContextoSuPlazaDataContext dcContextoSuPlazaDataContext = new dcContextoSuPlazaDataContext();
				if (dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault() == null)
				{
					pos_admin_settings pos_admin_settings = new pos_admin_settings();
					pos_admin_settings.path_ptr_label = printName;
					dcContextoSuPlazaDataContext.pos_admin_settings.InsertOnSubmit(pos_admin_settings);
					dcContextoSuPlazaDataContext.SubmitChanges();
				}
				else
				{
					dcContextoSuPlazaDataContext.pos_admin_settings.FirstOrDefault().path_ptr_label = printName;
					dcContextoSuPlazaDataContext.SubmitChanges();
				}
			}
			catch (Exception ex)
			{
				_ = ex.Message;
			}
		}
	}
}
