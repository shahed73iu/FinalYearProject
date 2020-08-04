using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSkill.DocumentPro.Documentship.Services;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.TenantPro.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        private ITenantService _tenantService;
        private IDocumentService _documentService;
        private IBillService _billService;
        public DashboardController(IBillService billService, ITenantService tenantService, IDocumentService documentService)
        {
            _billService = billService;
            _tenantService = tenantService;
            _documentService = documentService;
        }
        public IActionResult Index()
        {
            ViewBag.TenantCount = _tenantService.Get().Where(x => x.Status == true).Count();
            ViewBag.DocumentCount = _documentService.GetTotalDocument();
            ViewBag.LastDescoBill = _billService.Get().OrderByDescending(x => x.Id).FirstOrDefault();

            return View();
        }
    }
}