using DevSkill.Data;
using DevSkill.TenantPro.Billing.Contexts;
using DevSkill.TenantPro.Billing.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Repositories
{
    public interface IReadingRepository : IRepository<Reading,int,BillingContext>
    {
        //   int GetPreviousMonthReading(int tenantId,int month);
        //    Reading GetReadingByTenantId(int tenantId, Month month, int year);
        //   decimal GetTotalUnit(Month month);
        //   int GetPreviousMonthReadingTakenDate(int tenantId, int month);
         decimal GetPreviousMonthReading(int tenantId, DateTime monthYear);
        Reading GetPreviousMonthReadingTakenDate(int tenantId);
        Reading GetReadingByTenantId(int tenantId, DateTime monthYear);
        decimal GetTotalUnit(DateTime monthYear);
        List<Reading> GetPreviousMonthReadingList(DateTime monthYear);
    }
}
