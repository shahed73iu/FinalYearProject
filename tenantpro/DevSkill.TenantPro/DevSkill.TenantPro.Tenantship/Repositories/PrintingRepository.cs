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
    class PrintingRepository : Repository<Printing,int,TenantContext>, IPrintingRepository
    {
        private TenantContext _tenantContext;
        public PrintingRepository(TenantContext tenantContext) : base(tenantContext)
        {
            _tenantContext = tenantContext;
        }
        public List<Printing> GetPrintingsByTenant(int id)
        {
            return _tenantContext.Printing.Where(x => x.TenantId == id).ToList();
        }
        public void Delete(Printing printing)
        {
            _tenantContext.Printing.Remove(printing);
        }

        public List<Printing> GetPrintingList()
        {
           return _tenantContext.Printing
                .Include(x=>x.Tenant)
                .ThenInclude(y=>y.ContactPersons)
                .ToList();
        }
    }
}
