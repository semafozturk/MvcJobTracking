//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DXTreelist0904.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Adminler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Adminler()
        {
            this.AdminTalepler = new HashSet<AdminTalepler>();
            this.TalepDetaylar = new HashSet<TalepDetaylar>();
        }
    
        public int AdminId { get; set; }
        public string AdminKAdi { get; set; }
        public string AdminSifre { get; set; }
        public string AdminAdSoyad { get; set; }
        public string AdminGorev { get; set; }
        public string AdminBolum { get; set; }
        public Nullable<int> Yetki { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminTalepler> AdminTalepler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TalepDetaylar> TalepDetaylar { get; set; }
    }
}
