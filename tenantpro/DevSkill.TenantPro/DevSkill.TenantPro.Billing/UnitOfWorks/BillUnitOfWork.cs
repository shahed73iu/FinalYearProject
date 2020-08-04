using DevSkill.Data;
using DevSkill.TenantPro.Billing.Contexts;
using DevSkill.TenantPro.Billing.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.UnitOfWorks
{
    public class BillUnitOfWork : UnitOfWork, IBillUnitOfWork
    {
        public IBillRepository BillRepository { get; set; }
        public IReadingRepository ReadingRepository { get; set; }

        public BillUnitOfWork(BillingContext billingContext,IBillRepository billRepository,
                            IReadingRepository readingRepository)
            : base(billingContext)
        {
            BillRepository = billRepository;
            ReadingRepository = readingRepository;
        }
    }
}
