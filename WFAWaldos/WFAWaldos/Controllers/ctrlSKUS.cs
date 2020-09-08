using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlSKUS
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public bool IsSKUExistente(string _sku) 
        {
			try
			{
                var sku = db.skus.Where(x => x.codigobarras.Equals(_sku)).FirstOrDefault();

                return sku != null ? true : false;
			}
			catch (Exception)
			{
                return false;
			}
        }
    }
}
