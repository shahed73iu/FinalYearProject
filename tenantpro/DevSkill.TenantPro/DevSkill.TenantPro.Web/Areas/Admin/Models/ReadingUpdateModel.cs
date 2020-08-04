using Autofac;
using DevSkill.TenantPro.Billing;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class ReadingUpdateModel : BaseModel
    {
        public int Id { get; set; }
        public int TenantId { get; set; } 
        public Month Month { get; set; }
        public DateTime MonthYear { get; set; } 
        public int Year { get; set; }
        public decimal PreviousReading { get; set; }
        public decimal PresentReading { get; set; }
        public DateTime ReadingTakenDate { get; set; } = DateTime.Today;
        public int DayOffset { get; set; }
        public List<SelectListItem> Tenants { get; set; }
        public List<Printing> Printings { get; set; }
        public IList<Tenant> TenantList { get; set; }
        public List<Reading> ReadingList { get; set; }
        public string[] TenantIdArray { get; set; }

        private IReadingService _readingService;

        private ITenantService _tenantService;

        private ILogger<ReadingUpdateModel> _logger;
        public ReadingUpdateModel()
        {
            _readingService = Startup.AutofacContainer.Resolve<IReadingService>();
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
            _logger = Startup.AutofacContainer.Resolve<ILogger<ReadingUpdateModel>>();
        }
        public ReadingUpdateModel(IReadingService readingService,ITenantService tenantService,ILogger<ReadingUpdateModel> logger)
        {
            _readingService = readingService;
            _tenantService = tenantService;
            _logger = logger;
        }

        public void AddReading()
        {
            try
            {
              //  MonthYear = ReadingTakenDate.AddMonths(-1);
              //  var reading = _readingService.GetReadingOfTenant(this.TenantId,MonthYear);
              //  if (reading != null)
               //     throw new InvalidOperationException("Tenant Already Exist");

                //_readingService.AddNewReading(new Reading
                //{
                //    TenantId=this.TenantId,
                //    MonthYear=this.ReadingTakenDate.AddMonths(-1),
                //    PreviousReading = this.PreviousReading,
                //    PresentReading = this.PresentReading,
                //    ReadingTakenDate = this.ReadingTakenDate,
                //    DayOffset = this.DayOffset

                //});
                _readingService.AddNewReading(ReadingList);
                Notification = new NotificationModel("Success!", "Reading successfuly created", NotificationModel.NotificationType.Success);
            }
            catch(InvalidOperationException ex)
            {
                Notification = new NotificationModel(
                    "Failed !! ",
                    "Please insert atleast one tenant",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);
            }
            catch (DuplicateReadingException iex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Duplicate reading found in your insertion list",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(iex.Message);

            }

            catch (Exception ex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Failed to create Reading, please insert previous month bill unit price",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);

            }
        }
        public void LoadTenant()
        {
            var tenant = _tenantService.Get();
            Tenants = (from t in tenant
                          select new SelectListItem
                          {
                              Value = t.Id.ToString(),
                              Text = t.Name
                          }).ToList();
        }
        public void GetDropDownList()
        {
            this.TenantList = _tenantService.Get()
                .GroupBy(x => new { x.Name })
                .Select(x => x.First())
                .ToList();
            this.TenantList = this.TenantList.Select(x => new Tenant { Name = x.Name, Id = x.Id }).ToList();

        }
        public void PopulateTenant()
        {
            TenantList = _tenantService.Get().ToList();
        }
        public void EditReading()
        {
            try
            {
                _readingService.EditReading(new Reading
                {
                    Id = this.Id,
                    TenantId = this.TenantId,
                  //  Month=this.Month,
                    DayOffset=this.DayOffset,
                    PreviousReading=this.PreviousReading,
                    PresentReading=this.PresentReading,
                    ReadingTakenDate=this.ReadingTakenDate
                });

                Notification = new NotificationModel("Success!", "Reading successfuly updated", NotificationModel.NotificationType.Success);
            }
            catch (InvalidOperationException iex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Failed to update Reading",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(iex.Message);

            }
            catch (Exception ex)
            {
                Notification = new NotificationModel(
                    "Failed!",
                    "Failed to update reading, please try again",
                    NotificationModel.NotificationType.Fail);
                _logger.LogError(ex.Message);

            }
        }

        public void Load(int id)
        {
            var reading = _readingService.GetReading(id);
            if (reading != null)
            {
                Id = reading.Id;
               // Month = reading.Month;
                DayOffset = reading.DayOffset;
                PreviousReading = reading.PreviousReading;
                PresentReading = reading.PresentReading;
                ReadingTakenDate = reading.ReadingTakenDate;
                TenantId = reading.TenantId;
            }
        }
        public List<Reading> LoadPreviousReading(DateTime time)
        {
            return _readingService.GetPreviousReadingList(time);
        }
        public void LoadPrintings()
        {
            Printings = _tenantService.GetPrintingofActiveUsers();
            char[] spearator = { ',' };
            foreach (var item in Printings)
            {
                item.BillFormat = item.Format.Split(spearator);
            }
        }

    }
}
