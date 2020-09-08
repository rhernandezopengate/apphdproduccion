using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    
    public class ctrlTienda
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public string TiendaByFolio(int _idfolio) 
        {
            string tienda = db.wl_detenvios.Where(x => x.wl_cajas_Id == _idfolio).FirstOrDefault().wl_tiendas.codigo;
            return tienda;
        }
    }
}
