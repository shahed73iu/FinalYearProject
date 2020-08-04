using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Entities
{
    public class Reading : IEntity<int>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public DateTime MonthYear { get; set; }
        public decimal PreviousReading { get; set; }
        public decimal PresentReading { get; set; }
        public DateTime ReadingTakenDate { get; set; }
        public int DayOffset { get; set; }
        public decimal NetUnit { get; set; }
        public decimal PerUnitPrice { get; set; }
        public decimal TotalBillofThisMonth { get; set; }
    }

    public enum Month
    {
        January=1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
