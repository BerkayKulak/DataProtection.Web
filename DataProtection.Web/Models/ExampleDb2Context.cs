using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataProtection.Web.Models
{
    public partial class ExampleDb2Context : DbContext
    {
        public ExampleDb2Context()
        {
        }

        public ExampleDb2Context(DbContextOptions<ExampleDb2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasIndex(e => e.HouseId, "IX_Addresses_HouseId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
