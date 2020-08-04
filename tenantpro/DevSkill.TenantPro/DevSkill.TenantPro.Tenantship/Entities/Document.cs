using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Entities
{
    public class Document : IEntity<int>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DocumentType DocumentType { get; set; }
        public Tenant Tenant { get; set; }
    }
    public enum DocumentType
    {
        AggrementPaper = 1,
        BankStatement,
        TradeLicense,
        NIDCopy,
        Others
    }
}
