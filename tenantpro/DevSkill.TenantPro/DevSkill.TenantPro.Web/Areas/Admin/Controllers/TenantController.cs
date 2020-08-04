using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSkill.DocumentPro.Documentship.Services;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Web.Areas.Admin.Models;
using DevSkill.TenantPro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevSkill.TenantPro.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TenantController : Controller
    {
        private ITenantService _tenantService;
        private IReadingService _readingService;
        private IDocumentService _documentService;
        public TenantController(ITenantService tenantService,IDocumentService documentService,IReadingService readingService)
        {
            _tenantService = tenantService;
            _documentService = documentService;
            _readingService = readingService;
        }
        public IActionResult Index()
        {
            var model = new TenantViewModel();
            ViewBag.BillType = Enum.GetNames(typeof(PrintingFormat));
            ViewBag.PaymentMethod = Enum.GetNames(typeof(PaymentMethod));
            return View(model);
        }
        public IActionResult ValidateAmount(string format,int tenantId,DateTime time,decimal amount)
        {
            var model = new TenantViewModel();
            var result = model.ValidateAmountWithTotalCalCulation(tenantId, time, format, amount);
            if (result == "Valid") return Json("1");
            else if (result == "AlreadyExist") return Json("2");
            else return Json("0");
        }
        public IActionResult GetTenants()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new TenantViewModel();
            var data = model.GetTenantAndContactPerson(tableModel);
            return Json(data);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var tenant = _tenantService.GetTenant(id);
            return View(tenant);
        }
        public IActionResult GetDocuments(int id)
        {
            var records = _documentService.GetDocumentsByTenantId(id);
            var data = (from record in records
                        select new string[]
                        {
                            record.DocumentType.ToString(),
                            record.Name,
                            record.FilePath,
                            record.Id.ToString()
                        }
                    ).ToArray();
            return Json(data);
        }
        public IActionResult Add()
        {
            var model = new TenantUpdateModel();
            ViewBag.PaymentType = Enum.GetNames(typeof(PaymentType));
            ViewBag.BillType = Enum.GetNames(typeof(PrintingFormat));
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TenantUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                model.AddnewTenant();  
            }
            ViewBag.PaymentType = Enum.GetNames(typeof(PaymentType));
            ViewBag.BillType = Enum.GetNames(typeof(PrintingFormat));
            return View(model);
        }
        public IActionResult GetReadingofTenant(int tenantId,DateTime startDate)
        {
            var model = new TenantViewModel();
            model.InitiateReading(tenantId, startDate.AddMonths(-1));
            model.LoadPrintings(tenantId);
            if (model.Reading == null)
            {
                string[] arr = new string[] { "0", model.Printings.Count.ToString() };
                return Json(arr);
            }
            else
            {
                string[] arr = new string[] { "1", "1" };
                return Json(arr);
            }
        }

        public IActionResult GetReading(int tenantId, DateTime time)
        {
            var reading = _readingService.GetReadingOfTenant(tenantId, time.AddMonths(-1));
            if (reading == null) return Json("0");
            else return Json("1");
        }

        [HttpGet]
        public IActionResult GetPrintingsByTenant(int tenantId)
        {
            var model = new TenantViewModel();
            model.LoadPrintings(tenantId);
            string[,] arr = new string[model.Printings.Count,5];
           
            for (int i=0;i<model.Printings.Count;i++)
            {
                for(int j=0;j < model.Printings.ElementAt(i).BillFormat.Length; j++)
                {
                    arr[i, j] = model.Printings.ElementAt(i).BillFormat[j];
                }
              
            }
           
            string output = JsonConvert.SerializeObject(arr);
            return Json(output);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateBill(TenantViewModel model)
        {
            
            model.InitiateTenant(model.Id);
            model.InitiateReading(model.Id, model.MonthPicker.AddMonths(-1));
          //  model.TotalCalculate();
            return View(model);
        }

        
        public JsonResult GetPrintings(int tenantId)
        {
            var model = new TenantViewModel();
            model.LoadPrintings(tenantId);
            string[,] arr = new string[model.Printings.Count, 2];

            for (int i = 0; i < model.Printings.Count; i++)
            {
                arr[i, 0] = model.Printings.ElementAt(i).Id.ToString();
                arr[i, 1] = model.Printings.ElementAt(i).Format;
            }
            
            string output = JsonConvert.SerializeObject(arr);
            return Json(output);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateFinalBill(TenantViewModel model)
        {          
           model.ChangePaymentFormat();
           model.UpdatePayment();        
           return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkPrinting(TenantViewModel model)
        {
            model.LoadPrintings();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var model = new TenantUpdateModel();
            ViewBag.PaymentType = Enum.GetNames(typeof(PaymentType));
            ViewBag.BillType = Enum.GetNames(typeof(PrintingFormat));
            model.Load(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TenantUpdateModel model)
        {
            if(ModelState.IsValid)
            {
                model.EditTenant();
            }
            ViewBag.PaymentType = Enum.GetNames(typeof(PaymentType));
            ViewBag.BillType = Enum.GetNames(typeof(PrintingFormat));
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/tenant/delete")]
        public IActionResult Delete(int id)
        {
            var model = new TenantViewModel();
            model.Delete(id);
           return LocalRedirect("/Admin/Tenant/Index");
        }
    }
}