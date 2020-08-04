using DevSkill.TenantPro.Billing.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Services
{
    public interface IReadingService
    {
        IEnumerable<Reading> GetReadings(
        int tenantid,
        int pageIndex,
        int pageSize,
        string searchText,
        string sortingCols,
        out int total,
        out int totalFiltered);
        IEnumerable<Reading> Get();
        void AddNewReading(List<Reading> readingList);
        Reading GetReading(int id);
        void EditReading(Reading reading);
        List<Reading> GetReadingList(DateTime time);
        decimal GetPreviousMonthReading(int tenantId, DateTime time);
        void DeleteReading(int id);
        Reading GetReadingOfTenant(int tenantId,DateTime monthYear);
        decimal GetTotalUnitLocal(DateTime month);
        List<Reading> GetPreviousReadingList(DateTime time);
    }
}
