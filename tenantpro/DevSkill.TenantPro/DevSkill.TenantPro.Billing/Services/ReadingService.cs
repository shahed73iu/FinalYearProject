using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.UnitOfWorks;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DevSkill.TenantPro.Billing.Services
{
    public class ReadingService : IReadingService
    {
        private IBillUnitOfWork _billUnitOfWork;
        private ITenantUnitOfWork _tenantUnitOfWork;
        public ReadingService(IBillUnitOfWork billUnitOfWork, ITenantUnitOfWork tenantUnitOfWork)
        {
            _billUnitOfWork = billUnitOfWork;
            _tenantUnitOfWork = tenantUnitOfWork;

        }
        public void AddNewReading(List<Reading> readingList)
        {
            if(readingList.All(x=>x.PresentReading==0 && x.PreviousReading==0))
                  throw new InvalidOperationException("Tenant Already Exist");

            var filteredList = readingList.Where(x => x.PresentReading != 0 && x.PreviousReading != 0).ToList();

            foreach (var reading in filteredList)
            {
              
                    if (reading == null)
                        throw new ArgumentNullException("Reading is missing");

                   
                    reading.NetUnit = reading.PresentReading - reading.PreviousReading;
                    int readingTakenYear = reading.ReadingTakenDate.Year;
                     
                    

                    var thisMonth = reading.ReadingTakenDate.Month;
                    var previousYearMonthTotalDays = 0;
                    DateTime previousMonth=new DateTime(1920,1,1);
                    var daysDiffrence = 0;
                    var monthPrevious = 0;
                    var readingObjectforPreviousMonthReadingTakenDate = _billUnitOfWork.ReadingRepository.GetPreviousMonthReadingTakenDate(reading.TenantId);
                    if (readingObjectforPreviousMonthReadingTakenDate != null)
                    {
                      previousYearMonthTotalDays = DateTime.DaysInMonth
                              (readingObjectforPreviousMonthReadingTakenDate.ReadingTakenDate.Year,
                               readingObjectforPreviousMonthReadingTakenDate.ReadingTakenDate.Month);

                      daysDiffrence = (reading.ReadingTakenDate - readingObjectforPreviousMonthReadingTakenDate.ReadingTakenDate).Days;
                      if (daysDiffrence >= previousYearMonthTotalDays) 
                      {
                            previousMonth = reading.ReadingTakenDate.AddMonths(-2);
                            monthPrevious = thisMonth == 1 ? 12 : thisMonth - 1;
                            reading.DayOffset = reading.ReadingTakenDate.Day - 1;
                            reading.MonthYear = reading.ReadingTakenDate.AddMonths(-1);
                            reading.ReadingTakenDate = new DateTime(reading.ReadingTakenDate.Year, reading.ReadingTakenDate.Month, 1);
                      }
                      else 
                      { 
                            previousMonth = reading.ReadingTakenDate.AddMonths(-1);
                            monthPrevious = thisMonth == 1 ? thisMonth : thisMonth ;
                            var year = thisMonth == 12 ? readingTakenYear + 1 : readingTakenYear;
                            var date = reading.ReadingTakenDate.AddMonths(1);
                            var dateTime = new DateTime(date.Year, date.Month, 1);//for calculating negative day offset;
                            reading.DayOffset = ( reading.ReadingTakenDate- dateTime).Days;
                            reading.MonthYear = reading.ReadingTakenDate;
                            reading.ReadingTakenDate = new DateTime(year, reading.ReadingTakenDate.AddMonths(1).Month, 1);
                      }

                    }
                    else
                    {
                        if (reading.ReadingTakenDate.Day < 20)
                        {
                            previousMonth = reading.ReadingTakenDate.AddMonths(-2);
                            monthPrevious = thisMonth == 1 ? 12 : thisMonth - 1;
                            reading.DayOffset = 0;
                            reading.MonthYear = reading.ReadingTakenDate.AddMonths(-1);
                            reading.ReadingTakenDate = new DateTime(reading.ReadingTakenDate.Year, reading.ReadingTakenDate.Month, 1);
                        }
                        else
                        {
                            previousMonth = reading.ReadingTakenDate.AddMonths(-1);
                            monthPrevious = thisMonth == 1 ? thisMonth : thisMonth;
                            var year = thisMonth == 12 ? readingTakenYear + 1 : readingTakenYear;
                            reading.DayOffset = 0;
                            reading.MonthYear = reading.ReadingTakenDate;
                            reading.ReadingTakenDate = new DateTime(year, reading.ReadingTakenDate.AddMonths(1).Month, 1);
                        }

                    }


                    int daysInPreviousMonth = DateTime.DaysInMonth(readingTakenYear, monthPrevious);
                    int daysLeft = daysInPreviousMonth + reading.DayOffset;
                    decimal avgNetUnit = reading.NetUnit / daysLeft;
                    decimal totalNetUnit = avgNetUnit * daysInPreviousMonth;
                    var formatedNetUnit= String.Format("{0:0.00}", totalNetUnit);
                    reading.NetUnit = Convert.ToDecimal(formatedNetUnit);
                    if (reading.DayOffset != 0)
                        reading.PresentReading = reading.PreviousReading + reading.NetUnit;

                    var previousUnitPrice = _billUnitOfWork.BillRepository.GetUnitPrice((Month)(previousMonth.Month), previousMonth.Year);
                    var formattedpreviousUnitPrice= String.Format("{0:0.00}", previousUnitPrice);
                    reading.PerUnitPrice = Convert.ToDecimal(formattedpreviousUnitPrice);
                    reading.TotalBillofThisMonth = reading.PerUnitPrice * reading.NetUnit;
                    reading.DayOffset = 0;
                    if ((reading.PresentReading != 0 && reading.PreviousReading != 0))
                    {
                         if(_billUnitOfWork.ReadingRepository.GetReadingByTenantId(reading.TenantId, reading.MonthYear) != null)
                         {
                            throw new DuplicateReadingException();  
                         }
                        _billUnitOfWork.ReadingRepository.Add(reading);
                    }
                    
            }
           
            _billUnitOfWork.Save();
        }

        public void DeleteReading(int id)
        {
            _billUnitOfWork.ReadingRepository.Remove(id);
            _billUnitOfWork.Save();
        }
        public decimal GetPreviousMonthReading(int tenantId, DateTime time)
        {
           
            return _billUnitOfWork.ReadingRepository.GetPreviousMonthReading(tenantId, time.AddMonths(-2));
        }

        public List<Reading> GetPreviousReadingList(DateTime time)
        {
            if (time.Day < 20)
            {
                var tenantList = _tenantUnitOfWork.TenantRepository.Get().ToList();
                var readinglist=_billUnitOfWork.ReadingRepository.GetPreviousMonthReadingList(time.AddMonths(-2));
                var filteredreadingList = readinglist.Where(x => tenantList.Any(y => y.Id == x.TenantId && y.Status == true)).OrderBy(z => z.TenantId).ToList();
                return filteredreadingList;
            }
            else
            {
                var tenantList = _tenantUnitOfWork.TenantRepository.Get().ToList();
                var readinglist = _billUnitOfWork.ReadingRepository.GetPreviousMonthReadingList(time.AddMonths(-1));
                var filteredreadingList = readinglist.Where(x => tenantList.Any(y => y.Id == x.TenantId && y.Status == true)).OrderBy(z => z.TenantId).ToList();
                return filteredreadingList;
            }
        }
        public List<Reading> GetReadingList(DateTime time)
        {
            if (time.Day < 20)
            {
                var tenantList = _tenantUnitOfWork.TenantRepository.Get().ToList();
                var readinglist = _billUnitOfWork.ReadingRepository.GetPreviousMonthReadingList(time.AddMonths(-1));
                var filteredreadingList = readinglist.Where(x => tenantList.Any(y => y.Id == x.TenantId && y.Status == true)).OrderBy(z => z.TenantId).ToList();
                return filteredreadingList;
              //  return _billUnitOfWork.ReadingRepository.GetPreviousMonthReadingList(time.AddMonths(-1));
            }
            else
            {
                var tenantList = _tenantUnitOfWork.TenantRepository.Get().ToList();
                var readinglist = _billUnitOfWork.ReadingRepository.GetPreviousMonthReadingList(time);
                var filteredreadingList = readinglist.Where(x => tenantList.Any(y => y.Id == x.TenantId && y.Status == true)).OrderBy(z => z.TenantId).ToList();
                return filteredreadingList;
            }
        }
        public void EditReading(Reading reading)
        {
            var oldreading = _billUnitOfWork.ReadingRepository.GetById(reading.Id);

            reading.NetUnit = reading.PresentReading - reading.PreviousReading;
            int readingTakenYear = reading.ReadingTakenDate.Year;

            var thisMonth = reading.ReadingTakenDate.Month;
            var previousMonth = reading.ReadingTakenDate.AddMonths(-2);
            

            var previousMonthReadingTakenDate = 1;
            reading.DayOffset = reading.ReadingTakenDate.Day - previousMonthReadingTakenDate;


            var monthPrevious = thisMonth == 1 ? 12 : thisMonth - 1;
            int daysInPreviousMonth = DateTime.DaysInMonth(readingTakenYear, monthPrevious);
            int daysLeft = daysInPreviousMonth + reading.DayOffset;
            decimal avgNetUnit = reading.NetUnit / daysLeft;
            decimal totalNetUnit = avgNetUnit * daysInPreviousMonth;
            var formatedNetUnit = String.Format("{0:0.00}", totalNetUnit);
            reading.NetUnit = Convert.ToDecimal(formatedNetUnit);
            if (reading.DayOffset != 0)
                reading.PresentReading = reading.PreviousReading + reading.NetUnit;

            var previousUnitPrice = _billUnitOfWork.BillRepository.GetUnitPrice((Month)previousMonth.Month,previousMonth.Year);
            var formattedpreviousUnitPrice = String.Format("{0:0.00}", previousUnitPrice);
            reading.PerUnitPrice = Convert.ToDecimal(formattedpreviousUnitPrice);
            reading.TotalBillofThisMonth = reading.PerUnitPrice * reading.NetUnit;

            oldreading.PresentReading = reading.PresentReading;
            oldreading.DayOffset = reading.DayOffset;
            oldreading.NetUnit = reading.NetUnit;
            oldreading.PerUnitPrice = reading.PerUnitPrice;
            oldreading.TotalBillofThisMonth = reading.TotalBillofThisMonth;

            _billUnitOfWork.Save();
        }

        public IEnumerable<Reading> Get()
        {
            throw new NotImplementedException();
        }

        public Reading GetReading(int id)
        {
            return _billUnitOfWork.ReadingRepository.GetById(id);
        }

        public Reading GetReadingOfTenant(int tenantId,DateTime monthYear)
        {
            return _billUnitOfWork.ReadingRepository.GetReadingByTenantId(tenantId,monthYear);
        }

        public IEnumerable<Reading> GetReadings(
            int tenantid,
            int pageIndex,
            int pageSize,
            string searchText,
            string sortingCols,
            out int total,
            out int totalFiltered)
        {
            Expression<Func<Reading, bool>> filter = null;
            if (searchText != "")
            {
                filter = x => x.TenantId == Convert.ToInt32(searchText);
            }

            var result = _billUnitOfWork.ReadingRepository.GetDynamic(
              filter,
              sortingCols,
              null,
              pageIndex,
              pageSize,
              true);
            total = result.total;
            totalFiltered = result.totalDisplay;
            return result.data;
        }

        public decimal GetTotalUnitLocal(DateTime month)
        {
            var totalUnit = _billUnitOfWork.ReadingRepository.GetTotalUnit(month);
            if (totalUnit != 0)
            {
                return totalUnit;
            }
            else
            {
                return 0;
            }
        }
    }
}
