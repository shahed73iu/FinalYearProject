using Autofac;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class BillViewModel
    {
        public Bill Billing { get; set; }
        public Tenant Tenant { get; set; }
        public Reading Reading { get; set; }

        private IBillService _billService;
        private ITenantService _tenantService;
        public BillViewModel()
        {
            _billService = Startup.AutofacContainer.Resolve<IBillService>();
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
        }
        public BillViewModel(IBillService billService, ITenantService tenantService)
        {
            _billService = billService;
            _tenantService = tenantService;
        }
        public object GetBillInfo(DataTablesAjaxRequestModel tableModel)
        {
            int total = 0;
            int totalFiltered = 0;

            var records = _billService.GetBills(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Month", "TotalUnitLocal", "DescoBillOfThisMonth", "UnitPriceForNextMonth" }),
                out total,
                out totalFiltered);

            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                                record.Month.ToString() + " - "+record.Year.ToString(),
                                record.TotalUnitLocal.ToString(),
                                record.DescoBillOfThisMonth.ToString(),
                                record.UnitPriceForNextMonth.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()

            };
        }

        public void Delete(int id)
        {
            _billService.DeleteBill(id);
        }

    }
}
