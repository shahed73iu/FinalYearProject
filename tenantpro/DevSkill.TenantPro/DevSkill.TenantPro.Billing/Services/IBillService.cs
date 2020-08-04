using DevSkill.TenantPro.Billing.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Services
{
    public interface IBillService
    {
        IEnumerable<Bill> GetBills(
        int pageIndex,
        int pageSize,
        string searchText,
        string sortText,
        out int total,
        out int totalFiltered);
        IEnumerable<Bill> Get();
        void AddNewBill(Bill bill);
        Bill GetBill(int id);
        void EditBill(Bill bill);
        void DeleteBill(int id);
        Bill GetBillOfThisMonth(Month month, int year);
    }
}
