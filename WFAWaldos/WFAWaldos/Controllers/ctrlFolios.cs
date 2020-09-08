using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlFolios
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();
        //Validar que folio existe
        public bool IsFolioExistente(string _folio) 
        {
            try
            {
                var folio = db.wl_cajas.Where(x => x.Codigo.Equals(_folio)).FirstOrDefault();
                return folio != null ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Validar si folio esta cerrada
        public bool IsFolioCerrado(string _folio) 
        {
            try
            {
                var status = db.wl_cajas.Where(x => x.Codigo.Equals(_folio)).FirstOrDefault().wl_statuscajas_Id;
                return status == 2 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int IdByFolio(string _folio)
        {
            try
            {
                var id = db.wl_cajas.Where(x => x.Codigo.Equals(_folio)).FirstOrDefault().id;
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void CerrarFolio(string caja, string auditor, string picker)
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
                Console.WriteLine("Error");
            }
        }
    }
}
