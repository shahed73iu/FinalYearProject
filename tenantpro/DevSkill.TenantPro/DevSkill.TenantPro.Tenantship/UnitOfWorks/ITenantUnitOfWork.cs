using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.UnitOfWorks
{
    public interface ITenantUnitOfWork:IUnitOfWork
    {
        ITenantRepository TenantRepository { get; set; }
        IDocumentRepository DocumentRepository { get; set; }
        IContactPersonRepository ContactPersonRepository { get; set; }
        IPrintingRepository PrintingRepository { get; set; }
        IPaymentRepository PaymentRepository { get; set; }

    }
}
