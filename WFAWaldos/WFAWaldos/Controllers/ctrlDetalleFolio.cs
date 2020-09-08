using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;
using WFAWaldos.ViewModels;

namespace WFAWaldos.Controllers
{
    public class ctrlDetalleFolio
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public List<DetalleCaja> DetallesFolio(int _idfolio) 
        {
            var lista = from det in db.wl_detcajassku
                        where det.wl_cajas_Id == _idfolio
                        select new { sku = det.skus.codigobarras, cantidad = det.Cantidad };

            List<DetalleCaja> listaDetalle = new List<DetalleCaja>();

            foreach (var item in lista.GroupBy(x => x.sku))
            {
                DetalleCaja detalleTemp = new DetalleCaja();
                detalleTemp.sku = item.Key;
                detalleTemp.cantidad = (int)lista.Where(x => x.sku.Equals(item.Key)).Sum(x => x.cantidad);
                detalleTemp.cantidadescaneada = 0;
                listaDetalle.Add(detalleTemp);
            }

            return listaDetalle;
        }
    }
}
