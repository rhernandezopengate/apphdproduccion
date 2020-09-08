using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlPickers
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public bool IsPickerExistente(string _nombre)
        {
            try
            {
                var picker = db.pickers.Where(x => x.nombres.Equals(_nombre)).FirstOrDefault();

                return picker != null ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
