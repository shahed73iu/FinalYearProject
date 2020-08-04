using Autofac;
using DevSkill.DocumentPro.Documentship.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class DocumentUpdateModel : BaseModel
    {
        public int TenantId { get; set; }
        public IList<DocumentFile> Files { get; set; }
        public IList<Document> Documents { get; set; }

        private IDocumentService _documentService;
        private ITenantService _tenantService;
        private ILogger<DocumentUpdateModel> _logger;
        public DocumentUpdateModel()
        {
            _documentService = Startup.AutofacContainer.Resolve<IDocumentService>();
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
            _logger = Startup.AutofacContainer.Resolve<ILogger<DocumentUpdateModel>>();
        }
        public DocumentUpdateModel(IDocumentService documentService,ITenantService tenantService,ILogger<DocumentUpdateModel> logger)
        {
            _documentService = documentService;
            _tenantService = tenantService;
            _logger = logger;
        }
        public void AddDocuments()
        {
            try
            {
                int i = 0;
                var filePaths = GetUploadFilePath();
                foreach (var item in filePaths)
                {
                    var file = new FileInfo(item);

                    var document = this.Documents.ElementAt(i);
                    document.FilePath = item;
                    document.Name = file.Name;
                    document.TenantId = this.TenantId;
                    i++;
                }
                _documentService.AddNewDocument(this.Documents);
                Notification = new NotificationModel("Success!! ","Document Uploaded Successfully",NotificationModel.NotificationType.Success);
            }
            catch (Exception e)
            {
                Notification = new NotificationModel("Failed!!", "Failed to Upload Document", NotificationModel.NotificationType.Fail);
                _logger.LogError(e.Message);
            }
        }

        public List<string> GetUploadFilePath()
        {
            try
            {
                var filePath = new List<string>();
                foreach (var item in this.Files)
                {
                    var newFileName = GetRandomizedNewFileName(item);
                    var path = $"wwwroot/upload/{newFileName}";
                    WriteFileInSystemDriveAsync(path, item.FilesInfo);

                    filePath.Add(path);
                }
                return filePath;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
        }

        public void WriteFileInSystemDriveAsync(string path, IFormFile file)
        {
            if (!System.IO.File.Exists(path))
            {

                using (var fileOpenWrite = File.OpenWrite(path))
                {
                    using (var uploadedfile = file.OpenReadStream())
                    {
                        uploadedfile.CopyTo(fileOpenWrite);
                    }
                }
            }
        }

        private object GetRandomizedNewFileName(DocumentFile file)
        {
            var randomName = Path.GetRandomFileName().Replace(".", "");
            var fileName = Path.GetFileName(file.FilesInfo.FileName);
            var newFileName = $"{ randomName }{ Path.GetExtension(file.FilesInfo.FileName).ToLower()}";
            return newFileName;
        }
    }

    public class DocumentFile
    {
        public IFormFile FilesInfo { get; set; }
    }
}
