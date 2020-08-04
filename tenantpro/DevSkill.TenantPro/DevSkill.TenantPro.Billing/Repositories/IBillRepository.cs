using DevSkill.Data;
using DevSkill.TenantPro.Billing.Contexts;
using DevSkill.TenantPro.Billing.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.Repositories
{
    public interface IBillRepository:IRepository<Bill,int,BillingContext>
    {
        IEnumerable<Bill> GetAllBills();
        decimal GetUnitPrice(Month month,int year);
        Bill GetBillByMonthYear(Month month, int year);
    }
}
