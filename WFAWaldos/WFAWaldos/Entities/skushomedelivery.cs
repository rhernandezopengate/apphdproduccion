//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WFAWaldos.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class skushomedelivery
    {
        public int id { get; set; }
        public Nullable<bool> codigoqr { get; set; }
        public Nullable<bool> qtymanual { get; set; }
        public int skus_Id { get; set; }
    
        public virtual skus skus { get; set; }
    }
}
