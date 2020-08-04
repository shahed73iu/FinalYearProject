using Autofac;
using DevSkill.DocumentPro.Documentship.Services;
using DevSkill.TenantPro.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class DocumentViewModel
    {
        private IDocumentService _documentService;
        public DocumentViewModel(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        public DocumentViewModel()
        {
            _documentService = Startup.AutofacContainer.Resolve<IDocumentService>();
        }
        public object GetDocumentInfo(DataTablesAjaxRequestModel tableModel)
        {
            int total = 0;
            int totalFiltered = 0;

            var records = _documentService.GetDocuments(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                out total,
                out totalFiltered);

            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                                record.Tenant.Name,
                                record.DocumentType.ToString(),
                                record.Name.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()

            };
        }

        public void Delete(int id)
        {
            _documentService.DeleteDocument(id);
        }
    }
}
