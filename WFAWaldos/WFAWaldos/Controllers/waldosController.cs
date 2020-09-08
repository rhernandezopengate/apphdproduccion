using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;
using WFAWaldos.ViewModels;

namespace WFAWaldos.Controllers
{
    public class waldosController
    {
		DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

		public bool IsKit(string sku) 
		{
			try
			{
				var kit = db.kits.Where(x => x.codigobarras.Equals(sku)).FirstOrDefault();
				
				if (kit != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}			
		}

		public List<kitskus> DetalleKit(string kit) 
		{
			return db.kitskus.Where(x => x.kits.codigobarras.Equals(kit)).ToList();
		}

		public bool IsPicker(string picker) 
		{
			var pickervalido = db.pickers.Where(x => x.nombres.Equals(picker)).FirstOrDefault();

			return pickervalido != null ? true : false;
		}

		public string UsuarioByEmail(string email) 
		{
			try
			{
				string auditor = "";
				string nombre = db.empleados.Where(x => x.Email.Equals(email)).FirstOrDefault().Nombres;
				string ap = db.empleados.Where(x => x.Email.Equals(email)).FirstOrDefault().ApellidoPaterno;
				auditor = nombre + " " + ap;
				return auditor.ToUpper();
			}
			catch (Exception)
			{
				return "";
			}
		}

		public bool IsCajaCerrada(string codigocaja) 
		{
			try
			{
				wl_cajas caja = db.wl_cajas.Where(x => x.Codigo.Equals(codigocaja)).FirstOrDefault();

				return caja.wl_statuscajas_Id == 2 ? false : true;
			}
			catch (Exception)
			{
				return false;
			}
		}

        public bool ValidarCaja(string codigocaja) 
        {
			try
			{
				wl_cajas cajas = db.wl_cajas.Where(x => x.Codigo.Equals(codigocaja)).FirstOrDefault();
				
				return cajas != null ? true : false;
			}
			catch (Exception)
			{
				return false;
			}        
        }

		public int IdByCaja(string codigocaja)
		{
			try
			{
				wl_cajas tienda = db.wl_cajas.Where(x => x.Codigo.Equals(codigocaja)).FirstOrDefault();
				return tienda.id;
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public string Tienda(int IdCaja)
		{
			try
			{

				wl_detenvios tienda = db.wl_detenvios.Where(x => x.wl_cajas_Id == IdCaja).FirstOrDefault();
				return tienda.wl_tiendas.codigo;
			}
			catch (Exception)
			{
				return "";
			}
		}

		public string Guia(int IdCaja)
		{
			try
			{

				wl_detenvios tienda = db.wl_detenvios.Where(x => x.wl_cajas_Id == IdCaja).FirstOrDefault();
				return tienda.wl_guias.guia;
			}
			catch (Exception)
			{
				return "";
			}
		}

		public List<DetalleCaja> listaDetalle(int IdCaja) 
		{
			try
			{
				List<DetalleCaja> listaTemp = new List<DetalleCaja>();

				foreach (var item in db.wl_detcajassku.Where(x => x.wl_cajas_Id == IdCaja).ToList())
				{
					DetalleCaja detalle = new DetalleCaja();
					detalle.sku = db.skus.Where(x => x.id == item.skus_Id).FirstOrDefault().codigobarras;
					detalle.cantidad = (int)item.Cantidad;
					detalle.cantidadescaneada = 0;

					listaTemp.Add(detalle);
				}

				return listaTemp;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public bool BusquedaGuia(string caja, string guia) 
		{
			try
			{
				int cajabusqueda = db.wl_cajas.Where(x => x.Codigo.Equals(caja)).FirstOrDefault().id;
				int guiabusqueda = db.wl_guias.Where(x => x.guia.Equals(guia)).FirstOrDefault().id;
				wl_detenvios detalleenvio = db.wl_detenvios.Where(x => x.wl_cajas_Id == cajabusqueda && x.wl_guias_Id == guiabusqueda).FirstOrDefault();
				
				return detalleenvio != null ? true : false;			
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void CerrarCaja(string caja, string auditor, string picker) 
		{
			try
			{
				wl_cajas cajabusqueda = db.wl_cajas.Where(x => x.Codigo.Equals(caja)).FirstOrDefault();
				cajabusqueda.wl_statuscajas_Id = 2;
				cajabusqueda.Auditores_Id = db.auditores.Where(x => x.nombres.Equals(auditor)).FirstOrDefault().id;
				cajabusqueda.Pickers_Id = db.pickers.Where(x => x.nombres.Equals(picker)).FirstOrDefault().id;
				
				db.SaveChanges();				
			}
			catch (Exception)
			{
				
			}
		}
	}
}
