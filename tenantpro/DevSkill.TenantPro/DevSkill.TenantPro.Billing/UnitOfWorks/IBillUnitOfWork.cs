using DevSkill.Data;
using DevSkill.TenantPro.Billing.Contexts;
using DevSkill.TenantPro.Billing.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing.UnitOfWorks
{
    public interface IBillUnitOfWork:IUnitOfWork
    {
        IBillRepository BillRepository { get; set; }
        IReadingRepository ReadingRepository { get; set; }
    }
}
