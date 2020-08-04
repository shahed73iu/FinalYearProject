using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Entities
{
    public class Bill : IEntity<int>
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public decimal DescoBillOfThisMonth { get; set; }
        public decimal TotalUnitLocal { get; set; }
        public decimal UnitPriceForNextMonth { get; set; }
    }
}
