using Autofac;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class TenantUpdateModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Holding { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Rent { get; set; }
        public int ServiceCharge { get; set; }
        public int WaterBill { get; set; }
        public int GasBill { get; set; }
        public int AdvanceArrear { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime ContractSigningDate { get; set; } = DateTime.Today;
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractExpirationDate { get; set; }
        public bool Status { get; set; } = true;

        public int TenantId { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonContractNumber { get; set; }
        public string ContactPersonEmail { get; set; }
        public ContactPerson ContactPerson { get; set; }
        public List<Printing> Printings { get; set; }

        private ITenantService _tenantService;

        private ILogger<TenantUpdateModel> _logger;
        public TenantUpdateModel(ITenantService tenantService, ILogger<TenantUpdateModel> logger)
        {
            _tenantService = tenantService;
            _logger = logger;
        }

        public TenantUpdateModel()
        {
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
            _logger = Startup.AutofacContainer.Resolve<ILogger<TenantUpdateModel>>();
        }
        public void Load(int id)
        {
            var tenant = _tenantService.GetTenant(id);
            LoadPrintings(id);
            if (tenant != null)
            {
                Id = tenant.Id;
                Name = tenant.Name;
                Holding = tenant.Holding;
                PhoneNumber = tenant.PhoneNumber;
                Email = tenant.Email;
                Rent = tenant.Rent;
                GasBill = tenant.GasBill;
                ServiceCharge = tenant.ServiceCharge;
                WaterBill = tenant.WaterBill;
                AdvanceArrear = tenant.AdvanceArrear;
                ContractStartDate = tenant.ContractStartDate;
                ContractExpirationDate = tenant.ContractExpirationDate.Date;
                PaymentType = tenant.PaymentType;

                TenantId = tenant.ContactPersons[0].TenantId;
                ContactPersonName = tenant.ContactPersons[0].Name;
                ContactPersonContractNumber = tenant.ContactPersons[0].ContractNumber;
                ContactPersonEmail = tenant.ContactPersons[0].Email;
            }
        }

        public void EditTenant()
        {
            try
            {
                var tenant = new Tenant
                {
                    Id = this.Id,
                    Name = this.Name,
                    Holding = this.Holding,
                    PhoneNumber = this.PhoneNumber,
                    Email = this.Email,
                    AdvanceArrear = this.AdvanceArrear,
                    Rent = this.Rent,
                    GasBill=this.GasBill,
                    ServiceCharge = this.ServiceCharge,
                    WaterBill = this.WaterBill,
                    ContractStartDate = this.ContractStartDate,
                    ContractExpirationDate = this.ContractExpirationDate,
                    PaymentType=this.PaymentType
                };
                var contactPerson = new ContactPerson
                {
                    Id = this.TenantId,
                    Name = this.ContactPersonName,
                    Email = this.ContactPersonEmail,
                    ContractNumber = this.ContactPersonContractNumber
                };
                _tenantService.EditTenant(tenant, contactPerson,Printings);
                Notification = new NotificationModel("Success!", "Tenant Successfully Updated", NotificationModel.NotificationType.Success);
            }
            catch (InvalidOperationException iex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Failed to update Tenant, please provide valide name",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(iex.Message);
            }
            catch (Exception ex)
            {
                Notification = new NotificationModel(
                    "Failed!!",
                    "Failed to update Tenant , please try again with valid details",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);
            }
        }

        public void AddnewTenant()
        {
            try
            {
                var tenant = new Tenant
                {
                    Name = this.Name,
                    Holding = this.Holding,
                    PhoneNumber = this.PhoneNumber,
                    Email = this.Email,
                    Rent = this.Rent,
                    ServiceCharge = this.ServiceCharge,
                    WaterBill = this.WaterBill,
                    GasBill = this.GasBill,
                    AdvanceArrear = this.AdvanceArrear,
                    ContractStartDate = this.ContractStartDate,
                    ContractExpirationDate = this.ContractExpirationDate,
                    ContractSigningDate = this.ContractSigningDate,
                    Status = this.Status,
                    PaymentType= this.PaymentType
                };
                var contactPerson = new ContactPerson
                {
                    Name = this.ContactPersonName,
                    Email = this.ContactPersonEmail,
                    ContractNumber = this.ContactPersonContractNumber,
                    Tenant = tenant
                };
                
                _tenantService.AddNewTenant(tenant, contactPerson,Printings);
                Notification = new NotificationModel("Success!", "Tenant information successfully inserted", NotificationModel.NotificationType.Success);
            }
            catch (InvalidOperationException iex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Failed To insert Tenant information ! Provide Valid Information.",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(iex.Message);

            }
            catch (Exception ex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Failed To insert Tenant & Contact person information ! Please try again.",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);
            }
        }
       
        public void LoadPrintings(int id)
        {
            Printings = _tenantService.GetPrintingsByTenantId(id);
            char[] spearator = { ',' };
            foreach (var item in Printings)
            {
                item.BillFormat = item.Format.Split(spearator);
            }
        }
    }
}
