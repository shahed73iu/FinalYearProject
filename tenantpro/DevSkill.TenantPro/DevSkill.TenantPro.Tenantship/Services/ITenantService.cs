using DevSkill.TenantPro.Tenantship.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Services
{
    public interface ITenantService
    {
        IEnumerable<Tenant> GetTenants(
            int pageIndex,
            int pageSize,
            string searchText,
            string sortText,
            out int total,
            out int totalFiltered);
        IEnumerable<Tenant> Get();
        void AddNewTenant(Tenant tenant , ContactPerson contactPerson, List<Printing> printings);
        void AddPayment(Payment payment);
        void EditPayment(Payment payment);
        Payment GetPayment(int tenantId, string format,DateTime time);
        Tenant GetTenant(int id);
        void EditTenantStatus(Tenant tenant);
        Tenant GetTenantOnly(int id);
        ContactPerson GetConatctPerson(int id);
        List<Printing> GetPrintingsByTenantId(int id);
        void EditTenant(Tenant tenant, ContactPerson contactPerson, List<Printing> printings);
        IEnumerable<Tenant> GetExpiratedTenants();
        List<Printing> GetPrintingofActiveUsers();
    }
}
