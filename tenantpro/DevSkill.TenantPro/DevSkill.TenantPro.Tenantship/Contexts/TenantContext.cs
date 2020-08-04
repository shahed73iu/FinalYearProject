
using DevSkill.TenantPro.Tenantship.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Contexts
{
    public class TenantContext : DbContext
    {
        private string _connectionString;
        private string _migrationAssemblyName;

        public TenantContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Document>()
                .HasOne(d => d.Tenant)
                .WithMany(d => d.Documents)
                .HasForeignKey(d => d.TenantId);

            builder.Entity<ContactPerson>()
                .HasOne(cp => cp.Tenant)
                .WithMany(cp => cp.ContactPersons)
                .HasForeignKey(cp => cp.TenantId);

            builder.Entity<Printing>()
                .HasOne(cp => cp.Tenant)
                .WithMany(cp => cp.Printings)
                .HasForeignKey(cp => cp.TenantId);

            builder.Entity<Payment>()
                .HasOne(p => p.Tenant)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.TenantId);

            base.OnModelCreating(builder);
        }


        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<Printing> Printing { get; set; }
        public DbSet<Payment> Payment { get; set; }
    }
}
