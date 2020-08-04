using DevSkill.Data;
using DevSkill.TenantPro.Billing.Contexts;
using DevSkill.TenantPro.Billing.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevSkill.TenantPro.Billing.Repositories
{
    public class ReadingRepository : Repository<Reading,int,BillingContext>, IReadingRepository
    {
        private BillingContext _billingContext;
        public ReadingRepository(BillingContext billingContext) : base(billingContext)
        {
            _billingContext = billingContext;
        }
       
        public decimal GetPreviousMonthReading(int tenantId, DateTime monthYear)
        {

            var previousMonthReading = _billingContext.Readings
                .Where(x => x.TenantId == tenantId && x.MonthYear.Month == monthYear.Month && x.MonthYear.Year == monthYear.Year)
                .FirstOrDefault();

            if (previousMonthReading == null) return 0;
            else return previousMonthReading.PresentReading;
        }
        public Reading GetPreviousMonthReadingTakenDate(int tenantId)
        {
           
            var previousMonthReading = _billingContext.Readings
                .Where(x => x.TenantId == tenantId).OrderByDescending(y=>y.ReadingTakenDate)
                .FirstOrDefault();
            return previousMonthReading;
        }
        public Reading GetReadingByTenantId(int tenantId, DateTime monthYear)
        {
            return _billingContext.Readings.Where(x => x.TenantId == tenantId &&
            x.MonthYear.Month == monthYear.Month &&
            x.MonthYear.Year == monthYear.Year).FirstOrDefault();
        }
        public decimal GetTotalUnit(DateTime monthYear)
        {
            return _billingContext.Readings.Where(x => x.MonthYear.Month == monthYear.Month && x.MonthYear.Year == monthYear.Year).Sum(x => x.NetUnit);
        }
        public List<Reading> GetPreviousMonthReadingList(DateTime monthYear)
        {
            return _billingContext.Readings.Where(x => x.MonthYear.Year == monthYear.Year && x.MonthYear.Month == monthYear.Month).ToList();
        }               
    }
}
