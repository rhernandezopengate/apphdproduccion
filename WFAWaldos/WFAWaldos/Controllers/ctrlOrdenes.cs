using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlOrdenes
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public bool IsOrdenExistente(string _orden)
        {
            try
            {
                var sku = db.skus.Where(x => x.codigobarras.Equals(_orden)).FirstOrDefault();

                return sku != null ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
