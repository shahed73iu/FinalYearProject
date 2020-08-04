using Autofac;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class ReadingViewModel : BaseModel
    {
        public  int TenantId { get; set; }
        public  static int TemporaryTenantid { get; set; }
        public List<SelectListItem> Tenants { get; set; }
        public Tenant Tenant { get; set; }
        public Reading Reading { get; set; }
        public IList<Tenant> TenantList { get; set; }
        public string[] TenantIdArray { get; set; }
        public int Id { get; set; }
        public bool ElectricityBill { get; set; }
        public bool Service { get; set; }
        public bool Rent { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        private IReadingService _readingService;

        private ITenantService _tenantService;
        public ReadingViewModel()
        {
            _readingService = Startup.AutofacContainer.Resolve<IReadingService>();
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
        }
        public ReadingViewModel(IReadingService readingService, ITenantService tenantService)
        {
            _readingService = readingService;
            _tenantService = tenantService;
        }
        public void GetDropDownList()
        {
            this.TenantList = _tenantService.Get()
                .GroupBy(x => new { x.Name })
                .Select(x => x.First())
                .ToList();
            this.TenantList = this.TenantList.Select(x => new Tenant { Name = x.Name, Id = x.Id }).ToList();

        }
        public object GetReadings(DataTablesAjaxRequestModel tableModel)
        {
            int total = 0;
            int totalFiltered = 0;
            
            var records = _readingService.GetReadings(
                TemporaryTenantid,
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "TenantId", "MonthYear", "PreviousReading", "PresentReading", "DayOffset", "NetUnit", "PerUnitPrice", "TotalBillofThisMonth" }),
                out total,
                out totalFiltered);

            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                               
                                _tenantService.GetTenant(record.TenantId).Name,
                                record.ReadingTakenDate.AddMonths(-1).ToString("MMMM,yyyy"),
                                record.PreviousReading.ToString(),
                                record.PresentReading.ToString(),
                               // record.DayOffset.ToString(),
                                record.NetUnit.ToString(),
                                record.PerUnitPrice.ToString(),
                                record.TotalBillofThisMonth.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()

            };
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

        public void Delete(int id)
        {
            _readingService.DeleteReading(id);
        }
        public void TotalCalculate()
        {
            if(ElectricityBill)
            {
                Total += Reading.TotalBillofThisMonth;
            }
            if (Rent)
            {
                Total += Tenant.Rent;
            }
            if (Service)
            {
                Total += Tenant.ServiceCharge+Tenant.WaterBill;
            }
        }

    }
}
