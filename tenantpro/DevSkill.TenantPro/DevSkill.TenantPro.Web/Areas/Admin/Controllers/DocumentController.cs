using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Web.Areas.Admin.Models;
using DevSkill.TenantPro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.TenantPro.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DocumentController : Controller
    {
        private ITenantService _tenantService;
        public DocumentController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        public IActionResult Index()
        {
            var model = new DocumentViewModel();
            return View(model);
        }

        public IActionResult GetDocuments()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new DocumentViewModel();
            var data = model.GetDocumentInfo(tableModel);
            return Json(data);
        }

        public IActionResult Add()
        {
            var model = new DocumentUpdateModel();
            ViewBag.Tenants = _tenantService.Get();
            ViewBag.DocumentType = Enum.GetNames(typeof(DocumentType));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(DocumentUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                model.AddDocuments();
            }
            ViewBag.Tenants = _tenantService.Get();
            ViewBag.DocumentType = Enum.GetNames(typeof(DocumentType));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var model = new DocumentViewModel();
            model.Delete(id);
            return LocalRedirect("/Admin/Tenant/Index");
        }
    }
}