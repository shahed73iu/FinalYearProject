using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Entities
{
    public class ContactPerson : IEntity<int>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string ContractNumber { get; set; }
        public string Email { get; set; }
        public Tenant Tenant { get; set; }
    }
}
