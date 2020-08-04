using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public class PaymentRepository : Repository<Payment,int,TenantContext>,IPaymentRepository
    {
        private TenantContext _tenantContext;
        public PaymentRepository(TenantContext tenantContext) : base(tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public Payment GetPayment(int tenantId,string format,DateTime time)
        {
            return _tenantContext.Payment.Where(x => x.TenantId == tenantId
            && x.Format == format && (x.FinalBillDate.Year==time.Year && x.FinalBillDate.Month==time.Month))
                .FirstOrDefault();
        }
    }
}
