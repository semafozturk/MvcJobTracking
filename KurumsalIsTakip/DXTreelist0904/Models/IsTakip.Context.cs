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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KurumsalIsTakipEntities : DbContext
    {
        public KurumsalIsTakipEntities()
            : base("name=KurumsalIsTakipEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aciliyetler> Aciliyetler { get; set; }
        public virtual DbSet<Adminler> Adminler { get; set; }
        public virtual DbSet<AdminTalepler> AdminTalepler { get; set; }
        public virtual DbSet<Dosyalar> Dosyalar { get; set; }
        public virtual DbSet<Kategoriler> Kategoriler { get; set; }
        public virtual DbSet<TalepDetaylar> TalepDetaylar { get; set; }
    }
}
