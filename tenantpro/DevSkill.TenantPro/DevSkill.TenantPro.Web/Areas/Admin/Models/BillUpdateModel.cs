using Autofac;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class BillUpdateModel : BaseModel
    { 
        public int Id { get; set; }
        public int TenantId { get; set; }
        public List<SelectListItem> Tenants { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; } 
        [Required]
        public decimal DescoBillOfThisMonth { get; set; }

        [Required]
        public decimal TotalUnitLocal { get; set; }
        public decimal UnitPriceForNextMonth { get; set; }

        private IBillService _billService;

        private ITenantService _tenantService;

        private IReadingService _readingService;

        private ILogger<BillUpdateModel> _logger;
        public BillUpdateModel()
        {
            _billService = Startup.AutofacContainer.Resolve<IBillService>();
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
            _readingService = Startup.AutofacContainer.Resolve<IReadingService>();
            _logger = Startup.AutofacContainer.Resolve<ILogger<BillUpdateModel>>();
        }
        public BillUpdateModel(IBillService billService,ITenantService tenantService,IReadingService readingService,ILogger<BillUpdateModel> logger)
        {
            _billService = billService;
            _tenantService = tenantService;
            _readingService = readingService;
            _logger = logger;
        }

        public void AddBills()
        {
            try
            {
                if (this.TotalUnitLocal == 0)
                        this.TotalUnitLocal = _readingService.GetTotalUnitLocal(new DateTime(this.Year,(int)Month,1));
                        

                var bill = _billService.GetBillOfThisMonth(this.Month, this.Year);
                if (bill != null)
                    throw new NullReferenceException("Bill Already Exists");

                _billService.AddNewBill(new Bill
                {
                    DescoBillOfThisMonth = this.DescoBillOfThisMonth,
                    Month = this.Month,
                    Year = this.Year,
                    TotalUnitLocal = this.TotalUnitLocal,
                    UnitPriceForNextMonth = this.DescoBillOfThisMonth / this.TotalUnitLocal
                });
                Notification = new NotificationModel("Success !", "Bill Added Successfully", NotificationModel.NotificationType.Success);

            }
            catch(NullReferenceException ex)
            {
                Notification = new NotificationModel("Failed !!", "Already Exists Bill of this Month. ", NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                Notification = new NotificationModel("Failed !!", "Failed to Insert Bill", NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);
            }
            
        }

        public void Load(int id)
        {
            var bill = _billService.GetBill(id);
            Id = bill.Id;
            Month = bill.Month;
            TotalUnitLocal = bill.TotalUnitLocal;
            DescoBillOfThisMonth = bill.DescoBillOfThisMonth;
            Year = bill.Year;
        }

        public void LoadTenant()
        {
            var tenant = _tenantService.Get();
            Tenants = (from t in tenant
                       select new SelectListItem
                       {
                           Value = t.Id.ToString(),
                           Text = t.Name
                       }).ToList();
        }

        public void EditBill()
        {
            try
            {
                // var x = this.Month;
                //  var y = this.Year;
                var bill = _billService.GetBill(Id);
               // this.TotalUnitLocal = _billService.GetBillOfThisMonth(this.Month, bill.Year).TotalUnitLocal;

                _billService.EditBill(new Bill
                {
                    Id = this.Id,
                    DescoBillOfThisMonth = this.DescoBillOfThisMonth,
                    TotalUnitLocal = this.TotalUnitLocal,
                    Year=bill.Year,
                    UnitPriceForNextMonth = this.DescoBillOfThisMonth / this.TotalUnitLocal
                });
                Notification = new NotificationModel("Success !", "Bill Edited Sucessfully", NotificationModel.NotificationType.Success);
            }
            catch (Exception e)
            {
                Notification = new NotificationModel("Failed !", "OOPS !! Something went wrong , Failed to Edit Bill", NotificationModel.NotificationType.Fail);
                _logger.LogError(e.Message);
            }
        }
    }
}
