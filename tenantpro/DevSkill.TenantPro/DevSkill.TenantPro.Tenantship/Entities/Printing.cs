using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Entities
{
    public class Printing : IEntity<int>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Format { get; set; }

        [NotMapped]
        public string[] BillFormat { get; set; }
        public virtual Tenant Tenant { get; set; }
        
    }
}
