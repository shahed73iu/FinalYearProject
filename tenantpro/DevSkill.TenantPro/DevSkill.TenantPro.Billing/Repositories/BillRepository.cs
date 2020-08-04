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
    public class BillRepository:Repository<Bill, int, BillingContext>, IBillRepository
    {
        private BillingContext _billingContext;
        public BillRepository(BillingContext billingContext) : base(billingContext)
        {
            _billingContext = billingContext;
        }

        public IEnumerable<Bill> GetAllBills()
        {
            return _billingContext.Bills.ToList();
        }

        public Bill GetBillByMonthYear(Month month, int year)
        {
            return _billingContext.Bills.Where(x => x.Month == month && x.Year == year).FirstOrDefault();
        }

        public decimal GetUnitPrice(Month month,int year)
        {
          var  bill= _billingContext.Bills.Where(x =>  x.Month==month && x.Year==year).FirstOrDefault();
          return bill.UnitPriceForNextMonth;
        }
    }
}
