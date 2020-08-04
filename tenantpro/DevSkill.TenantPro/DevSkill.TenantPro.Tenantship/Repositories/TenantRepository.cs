using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public class TenantRepository:Repository<Tenant,int,TenantContext>, ITenantRepository
    {
        private TenantContext _tenantContext;
        public TenantRepository(TenantContext tenantContext) : base(tenantContext)
        {
            _tenantContext = tenantContext;
        }
        public IEnumerable<Tenant> Get()
        {
            return _tenantContext.Tenants.Where(x=>x.Status==true).ToList();
        }

        public Tenant GetTenantIncludingChild(int id)
        {
            return _tenantContext.Tenants.Where(x => x.Id == id)
                .Include(nameof(Tenant.ContactPersons))
                .Include(nameof(Tenant.Documents))
                .Include(nameof(Tenant.Printings))
                .FirstOrDefault();
        }
        public Tenant GetNameBySearchText(string searchText)
        {
            return _tenantContext.Tenants.Where(x => x.Name.Contains(searchText) && x.Status==true).FirstOrDefault();
        }
        public IEnumerable<Tenant> GetExpirationTenantList()
        {
            DateTime thirtydaysafter = DateTime.Today.AddDays(30);
            var tenantlist = _tenantContext.Tenants.Where(x => x.ContractExpirationDate == thirtydaysafter && x.Status==true)
                .Include(nameof(Tenant.ContactPersons))
                .ToList();
            return tenantlist;
        }
        public List<int> GetActiveTenatId()
        {
           return  _tenantContext.Tenants.Where(x => x.Status == true).Select(y => y.Id).ToList();
        }
    }
}
