using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlKits
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public bool IsKitValido(string _kit) 
        {
            var kit = db.kits.Where(x => x.codigobarras.Equals(_kit)).FirstOrDefault();
            return kit != null ? true : false;
        }

        public List<kitskus> DetalleKit(string kit)
        {
            return db.kitskus.Where(x => x.kits.codigobarras.Equals(kit)).ToList();
        }
    }
}
