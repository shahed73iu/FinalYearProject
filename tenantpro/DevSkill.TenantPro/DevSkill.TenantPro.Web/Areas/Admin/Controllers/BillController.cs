using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Web.Areas.Admin.Models;
using DevSkill.TenantPro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.TenantPro.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class BillController : Controller
    {
        private IBillService _billService;
        private ITenantService _tenantService;
        private IReadingService _readingService;
        public BillController(IBillService billService, ITenantService tenantService,IReadingService readingService)
        {
            _billService = billService;
            _tenantService = tenantService;
            _readingService = readingService;
        }
        public IActionResult Index()
        {
            var model = new BillViewModel();
            return View(model);
        }

        public IActionResult GetBills()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new BillViewModel();
            var data = model.GetBillInfo(tableModel);
            return Json(data);
        }

        public IActionResult Add()
        {
            var model = new BillUpdateModel();
            model.LoadTenant();
            ViewBag.Months = Enum.GetNames(typeof(Month));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(BillUpdateModel model)
        {
            model.AddBills();
            model.LoadTenant();
            ViewBag.Months = Enum.GetNames(typeof(Month));
            return View(model);
        }

        public IActionResult GetTotalUnit(string month,int year)
        {
            Month enumMonth = (Month)Enum.Parse(typeof(Month), month);
            var data = _readingService.GetTotalUnitLocal(new DateTime(year,(int)enumMonth,1));
            return Json(data);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            var model = new BillUpdateModel();
            model.Load(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BillUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                model.EditBill();
            }
            model.Load(model.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var model = new BillViewModel();
            model.Delete(id);
            return LocalRedirect("/Admin/Bill/Index");
        }
    }
}