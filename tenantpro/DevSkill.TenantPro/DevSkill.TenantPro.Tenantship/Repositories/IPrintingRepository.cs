using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public interface IPrintingRepository : IRepository<Printing,int,TenantContext>
    {
        List<Printing> GetPrintingsByTenant(int id);
        List<Printing> GetPrintingList();
    }
}
