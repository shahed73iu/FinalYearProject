using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.UnitOfWorks
{
    public class TenantUnitOfWork : UnitOfWork, ITenantUnitOfWork
    {
        public ITenantRepository TenantRepository { get; set; }
        public IDocumentRepository DocumentRepository { get; set; }
        public IContactPersonRepository ContactPersonRepository { get; set; }
        public IPrintingRepository PrintingRepository { get; set; }
        public IPaymentRepository PaymentRepository { get; set; }
        public TenantUnitOfWork(TenantContext tenantContext, ITenantRepository tenantRepository,
            IDocumentRepository documentRepository,
            IContactPersonRepository contactPersonRepository , 
            IPrintingRepository printingRepository,
            IPaymentRepository paymentRepository)
           : base(tenantContext)
        {
            TenantRepository = tenantRepository;
            DocumentRepository = documentRepository;
            ContactPersonRepository = contactPersonRepository;
            PrintingRepository = printingRepository;
            PaymentRepository = paymentRepository;
        }
    }
}

