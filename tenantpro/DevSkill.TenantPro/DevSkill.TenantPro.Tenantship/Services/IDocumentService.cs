using DevSkill.TenantPro.Tenantship.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.DocumentPro.Documentship.Services
{
    public interface IDocumentService
    {
        int GetTotalDocument();
        IEnumerable<Document> GetDocuments(
            int pageIndex,
            int pageSize,
            string searchText,
            out int total,
            out int totalFiltered);
        IEnumerable<Document> Get();
        void AddNewDocument(IList<Document> documents);
        Document GetDocument(int id);
        void DeleteDocument(int id);
        IEnumerable<Document> GetDocumentsByTenantId(int id);
    }
}
