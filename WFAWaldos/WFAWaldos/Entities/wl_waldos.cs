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
    
    public partial class wl_waldos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public wl_waldos()
        {
            this.wl_detenvios = new HashSet<wl_detenvios>();
        }
    
        public int id { get; set; }
        public string folio { get; set; }
        public Nullable<System.DateTime> fechaalta { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wl_detenvios> wl_detenvios { get; set; }
    }
}
