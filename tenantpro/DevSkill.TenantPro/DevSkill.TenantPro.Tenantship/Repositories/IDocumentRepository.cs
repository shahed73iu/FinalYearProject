using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public interface IDocumentRepository : IRepository<Document,int,TenantContext>
    {
        IEnumerable<Document> GetDocumentsByTenantId(int id);
    }
}
