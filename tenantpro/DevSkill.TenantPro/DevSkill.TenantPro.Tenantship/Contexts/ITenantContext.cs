using DevSkill.TenantPro.Tenantship.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Contexts
{
    public interface ITenantContext
    {
        DbSet<Tenant> Tenants { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<ContactPerson> ContactPersons { get; set; }
        DbSet<Printing> Printing { get; set; }
        DbSet<Payment> Payment { get; set; }
    }
}
