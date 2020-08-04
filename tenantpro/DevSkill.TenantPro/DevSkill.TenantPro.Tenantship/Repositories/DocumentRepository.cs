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
    public class DocumentRepository:Repository<Document,int,TenantContext>,IDocumentRepository
    {
        private TenantContext _tenantContext;
        public DocumentRepository(TenantContext tenantContext) : base(tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public IEnumerable<Document> GetDocumentsByTenantId(int id)
        {
            return _tenantContext.Documents.Where(x => x.TenantId == id).ToList();
        }
    }
}
