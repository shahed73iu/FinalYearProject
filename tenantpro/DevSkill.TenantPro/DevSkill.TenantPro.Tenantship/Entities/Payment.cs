using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Entities
{
    public class Payment : IEntity<int>
    {
        public int Id { get; set; }
        [Required]
       // [Required("")]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public decimal PaymentAmount { get; set; }
        [Required]
        public string DocumentsDetails { get; set; }
        public decimal DueAmount { get; set; }
        [Required]
        public string Format { get; set; }
        [Required]
        public DateTime FinalBillDate { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        [NotMapped]
        public string[] BillFormat { get; set; }
    }
}
