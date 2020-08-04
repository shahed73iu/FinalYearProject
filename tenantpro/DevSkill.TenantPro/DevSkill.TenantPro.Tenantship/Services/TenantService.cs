using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Services
{
    public class TenantService : ITenantService
    {
        private ITenantUnitOfWork _tenantUnitOfWork;
        private readonly ILogger<TenantService> _logger;
        public TenantService(ITenantUnitOfWork tenantUnitOfWork , ILogger<TenantService> logger)
        {
            _tenantUnitOfWork = tenantUnitOfWork;
            _logger = logger;
        }
        public void AddNewTenant(Tenant tenant, ContactPerson contactPerson,List<Printing> printings)
        {
            if ((tenant == null ||
                string.IsNullOrWhiteSpace(tenant.Name) ||
                string.IsNullOrWhiteSpace(tenant.Holding) ||
                string.IsNullOrWhiteSpace(tenant.Email) ||
                string.IsNullOrWhiteSpace(tenant.PhoneNumber) &&
                (contactPerson == null) ||
                string.IsNullOrWhiteSpace(contactPerson.Name) ||
                string.IsNullOrWhiteSpace(contactPerson.Email) ||
                string.IsNullOrWhiteSpace(contactPerson.ContractNumber)))
                throw new InvalidOperationException("Tenant And Contact Details is Missing !");
            try
            {
                _tenantUnitOfWork.TenantRepository.Add(tenant);
                _tenantUnitOfWork.ContactPersonRepository.Add(contactPerson);
                foreach (var item in printings)
                {
                    item.Tenant = tenant;
                    item.Format = String.Join(",", item.BillFormat);
                    _tenantUnitOfWork.PrintingRepository.Add(item);
                }
                //_tenantUnitOfWork.PrintingRepository.Add(printing);
                _tenantUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Failed to add new tenant");
                throw ex;
            }
        }
        public void EditTenant(Tenant tenant , ContactPerson contactPerson, List<Printing> printings)
        {
            try
            {
                var oldTenant = _tenantUnitOfWork.TenantRepository.GetTenantIncludingChild(tenant.Id);
                oldTenant.Name = tenant.Name;
                oldTenant.Holding = tenant.Holding;
                oldTenant.PhoneNumber = tenant.PhoneNumber;
                oldTenant.Email = tenant.Email;
                oldTenant.AdvanceArrear = tenant.AdvanceArrear;
                oldTenant.Rent = tenant.Rent;
                oldTenant.GasBill = tenant.GasBill;
                oldTenant.ServiceCharge = tenant.ServiceCharge;
                oldTenant.WaterBill = tenant.WaterBill;
                oldTenant.ContractExpirationDate = tenant.ContractExpirationDate;
                oldTenant.ContractSigningDate = tenant.ContractSigningDate;
                oldTenant.PaymentType = tenant.PaymentType;
                oldTenant.Status = true;

                foreach (var item in oldTenant.ContactPersons)
                {
                    item.Name = contactPerson.Name;
                    item.Email = contactPerson.Email;
                    item.ContractNumber = contactPerson.ContractNumber;
                }
                foreach(var item in oldTenant.Printings)
                {
                    _tenantUnitOfWork.PrintingRepository.Remove(item);
                }
                foreach (var item in printings)
                {
                    item.Tenant = oldTenant;
                    item.Format = String.Join(",", item.BillFormat);
                    _tenantUnitOfWork.PrintingRepository.Add(item);
                }
                _tenantUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Failed to edit tenant");
                throw ex;
            }
        }
        public void EditTenantStatus(Tenant tenant)
        {
            try
            {
                var oldTenant = _tenantUnitOfWork.TenantRepository.GetById(tenant.Id);
                oldTenant.Status = false;
                _tenantUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Failed to edit tenant status");
                throw ex;
            }
        }
        public void AddPayment(Payment payment)
        {
            //var editedPaymentFinalDate = payment.FinalBillDate.AddMonths(-1);
            //payment.FinalBillDate = editedPaymentFinalDate; 
            _tenantUnitOfWork.PaymentRepository.Add(payment);    
            _tenantUnitOfWork.Save();
        }
        public void EditPayment(Payment payment)
        {
            _tenantUnitOfWork.PaymentRepository.Edit(payment);
            _tenantUnitOfWork.Save();
        }
        public Payment GetPayment(int tenantId, string format,DateTime time)
        {
            return _tenantUnitOfWork.PaymentRepository.GetPayment(tenantId, format,time);
        }
        public IEnumerable<Tenant> Get()
        {
            return _tenantUnitOfWork.TenantRepository.Get();
        }
        public IEnumerable<Tenant> GetExpiratedTenants()
        {
            return _tenantUnitOfWork.TenantRepository.GetExpirationTenantList();
        }
        public Tenant GetTenant(int id)
        {
            return _tenantUnitOfWork.TenantRepository.GetTenantIncludingChild(id);
        }
        public ContactPerson GetConatctPerson(int id)
        {
            return _tenantUnitOfWork.ContactPersonRepository.GetById(id);
        }
        public Tenant GetTenantOnly(int id)
        {
            return _tenantUnitOfWork.TenantRepository.GetById(id);
        }
        public List<Printing> GetPrintingsByTenantId(int id)
        {
            return _tenantUnitOfWork.PrintingRepository.GetPrintingsByTenant(id);
        }
        public List<Printing> GetPrintingofActiveUsers()
        {
            var activeTenantId = _tenantUnitOfWork.TenantRepository.GetActiveTenatId();
            var printingCombo = _tenantUnitOfWork.PrintingRepository.GetPrintingList();
            var activePrintingCombo = printingCombo.Where(x => activeTenantId.Any(y => x.TenantId == y)).ToList();
            return activePrintingCombo;
        }
        public IEnumerable<Tenant> GetTenants(
            int pageIndex, 
            int pageSize, 
            string searchText,
            string sortText,
            out int total, 
            out int totalFiltered)
        {
            var result = _tenantUnitOfWork.TenantRepository.GetDynamic(
                x => x.Name.Contains(searchText) && x.Status == true,

                sortText,
                y => y.Include(z => z.ContactPersons),
                pageIndex,
                pageSize,
                true);
            
            total = result.total;
            totalFiltered = result.totalDisplay;
            return result.data;
        }
        
    }
}
