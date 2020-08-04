using DevSkill.TenantPro.Billing.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Contexts
{
    public interface IBillingContext
    {
        DbSet<Bill> Bills { get; set; }
        DbSet<Reading> Readings { get; set; }
    }
}
