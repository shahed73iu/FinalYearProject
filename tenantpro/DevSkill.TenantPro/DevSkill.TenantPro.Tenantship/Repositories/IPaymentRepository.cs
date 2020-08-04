using System;
using System.Collections.Generic;
using System.Text;
using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public interface IPaymentRepository : IRepository<Payment,int,TenantContext>
    {
        Payment GetPayment(int tenantId, string format,DateTime time);
    }
}
