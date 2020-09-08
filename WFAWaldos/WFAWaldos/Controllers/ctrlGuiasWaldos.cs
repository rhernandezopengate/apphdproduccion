using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlGuiasWaldos
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public string GuiaByFolio(int _idfolio)
        {
            string guia = db.wl_detenvios.Where(x => x.wl_cajas_Id == _idfolio).FirstOrDefault().wl_guias.guia;
            return guia;
        }

        public bool IsGuiaExistente(string _guia)
        {
            var guia = db.wl_guias.Where(x => x.guia.Equals(_guia)).FirstOrDefault();
            return guia != null ? true : false;
        }

        public bool IsGuiaPerteneceFolio(string _folio, string _guia) 
        {
            int folio = db.wl_cajas.Where(x => x.Codigo.Equals(_folio)).FirstOrDefault().id;
            int guia = db.wl_guias.Where(x => x.guia.Equals(_guia)).FirstOrDefault().id;
            var detalleenvio = db.wl_detenvios.Where(x => x.wl_cajas_Id == folio && x.wl_guias_Id == guia).FirstOrDefault();

            return detalleenvio != null ? true : false;
        }
    }
}
