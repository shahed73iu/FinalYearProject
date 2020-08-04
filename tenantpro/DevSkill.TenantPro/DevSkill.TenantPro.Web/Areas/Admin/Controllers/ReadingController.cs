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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DevSkill.TenantPro.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReadingController : Controller
    {
        private IReadingService _readingService;
        private IBillService _billService;
        private ITenantService _tenantService;
        private readonly ILogger<ReadingController> _logger;
        public ReadingController(IReadingService readingService,
            ITenantService tenantService,IBillService billService, ILogger<ReadingController> logger)
        {
            _billService = billService;
            _tenantService = tenantService;
            _readingService = readingService;
            _logger = logger;
        }
       
        public IActionResult Index()
        {
            ReadingViewModel model = new ReadingViewModel();
            model.LoadTenant();
            model.GetDropDownList();
            return View(model);
        }
        //public IActionResult data(int tenantId,DateTime time)
        //{
        //    var model = new ReadingUpdateModel();
        //    var reading = model.LoadPreviousReading(tenantId,time);
        //    return Json(reading.ToString());
        //}

        //public IActionResult Add()
        //{
        //    var model = new ReadingUpdateModel();
        //    model.LoadTenant();
        //    ViewBag.Months = Enum.GetNames(typeof(Month));
        //    return View(model);
        //}

        public IActionResult Add()
        {
           
            var model = new ReadingUpdateModel();
            model.PopulateTenant();
            return View(model);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult GenerateBill(ReadingUpdateModel model)
        {
            model.LoadPrintings();
            return View(model);
        }

        [HttpGet]
        public JsonResult ReturnData(DateTime time)
        {
            var model = new ReadingUpdateModel();
            var readingList = model.LoadPreviousReading(time);
            if (readingList.Count == 0) return Json("0");
            else
            {
                string[,] arr = new string[readingList.Count, 2];
                for (int i = 0; i < readingList.Count; i++)
                { 
                    arr[i, 0] = "#PreviousReading" + readingList.ElementAt(i).TenantId;
                    arr[i, 1] = readingList.ElementAt(i).PresentReading.ToString();
                }
                string output = JsonConvert.SerializeObject(arr);
                return Json(output);
            }
        }

        public JsonResult GetPresentReading(DateTime time)
        {
            var model = new ReadingUpdateModel();
           // if (_readingService.GetPreviousReadingList(time).Count == 0) return Json("0");
            var readingList = _readingService.GetReadingList(time);
            if (readingList.Count == 0) return Json("0");
            else
            {
                string[,] arr = new string[readingList.Count, 4];
                for (int i = 0; i < readingList.Count; i++)
                {
                    arr[i, 0] = "#PresentReading" + readingList.ElementAt(i).TenantId;
                    arr[i, 1] = readingList.ElementAt(i).PresentReading.ToString();
                    arr[i, 2] = "#PreviousReading" + readingList.ElementAt(i).TenantId;
                    arr[i, 3] = readingList.ElementAt(i).PreviousReading.ToString();
                }
                string output = JsonConvert.SerializeObject(arr);
                return Json(output);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ReadingUpdateModel model)
        {
            try
            {               
                model.AddReading();
                model.PopulateTenant();            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "reading data is missing");
            }
            return View(model);
        }
        [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult Delete([FromForm]int id)
        {
            var model = new ReadingViewModel();
            model.Delete(id);
            return LocalRedirect("/Admin/Reading/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReadingUpdateModel model)
        {
            model.EditReading();
            model.LoadTenant();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var model = new ReadingUpdateModel();
            ViewBag.Months = Enum.GetNames(typeof(Month));
            model.Load(id);
            model.LoadTenant();
            return View(model);
        }
        
        
        public IActionResult getreadings(int id)
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new ReadingViewModel();
            var data = model.GetReadings(tableModel);
            return Json(data);
        }

    }
}