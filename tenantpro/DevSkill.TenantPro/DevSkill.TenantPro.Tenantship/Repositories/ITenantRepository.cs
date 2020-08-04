using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public interface ITenantRepository : IRepository<Tenant,int,TenantContext>
    {
        IEnumerable<Tenant> Get();
        Tenant GetNameBySearchText(string searchText);
        Tenant GetTenantIncludingChild(int id);
        IEnumerable<Tenant> GetExpirationTenantList();
        List<int> GetActiveTenatId();
    }
}
