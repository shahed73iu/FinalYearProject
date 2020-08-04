using DevSkill.DocumentPro.Documentship.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Services
{
    public class DocumentService : IDocumentService
    {
        private ITenantUnitOfWork _tenantUnitOfWork;
        public DocumentService(ITenantUnitOfWork tenantUnitOfWork)
        {
            _tenantUnitOfWork = tenantUnitOfWork;
        }
        public void AddNewDocument(IList<Document> documents)
        {
            foreach (var document in documents)
            {
                _tenantUnitOfWork.DocumentRepository.Add(document);
            }
            _tenantUnitOfWork.Save();
        }

        public void DeleteDocument(int id)
        {
            _tenantUnitOfWork.DocumentRepository.Remove(id);
            _tenantUnitOfWork.Save();
        }

        public IEnumerable<Document> Get()
        {
            throw new NotImplementedException();
        }

        public int GetTotalDocument()
        {
            return _tenantUnitOfWork.DocumentRepository.GetCount();
        }

        public Document GetDocument(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Document> GetDocuments(int pageIndex,
            int pageSize, 
            string searchText, 
            out int total,
            out int totalFiltered)
        {
            var result = _tenantUnitOfWork.DocumentRepository.GetDynamic(
                x => x.Tenant.Name.Contains(searchText),
                null,
                null,//"Tenant",
                pageIndex,
                pageSize,
                true);
            total = result.total;
            totalFiltered = result.totalDisplay;
            return result.data;
        }

        public IEnumerable<Document> GetDocumentsByTenantId(int id)
        {
            return _tenantUnitOfWork.DocumentRepository.GetDocumentsByTenantId(id);
        }
    }
}
