using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.UnitOfWorks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevSkill.TenantPro.Billing.Services
{
    public class BillService : IBillService
    {
        private IBillUnitOfWork _billUnitOfWork;
        private readonly ILogger<BillService> _logger;

        public BillService(IBillUnitOfWork billUnitOfWork,  ILogger<BillService> logger)
        {
            _billUnitOfWork = billUnitOfWork;
            _logger = logger;
        }
        public void AddNewBill(Bill bill)
        {
            if (bill == null)
                throw new NullReferenceException("Billing is missing");
            try
            {
                _billUnitOfWork.BillRepository.Add(bill);
                _billUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Bill add failed");
                throw ex;
            }
        }

        public void DeleteBill(int id)
        {
            try
            {
                _billUnitOfWork.BillRepository.Remove(id);
                _billUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Delete bill failed");
                throw ex;
            }
        }

        public void EditBill(Bill bill)
        {
            var oldBill = _billUnitOfWork.BillRepository.GetById(bill.Id);
            if (oldBill == null)
                throw new NullReferenceException("Bill not found");
            try
            {
                oldBill.Year = bill.Year;
                oldBill.DescoBillOfThisMonth = bill.DescoBillOfThisMonth;
                oldBill.UnitPriceForNextMonth = bill.UnitPriceForNextMonth;
                oldBill.TotalUnitLocal = bill.TotalUnitLocal;
                _billUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Edit bill failed");
                throw ex;
            }
        }

        public IEnumerable<Bill> Get()
        {
            return _billUnitOfWork.BillRepository.GetAllBills();
        }

        public Bill GetBill(int id)
        {
            return _billUnitOfWork.BillRepository.GetById(id);
        }

        public IEnumerable<Bill> GetBills(
            int pageIndex,
            int pageSize,
            string searchText,
            string sortText,
            out int total,
            out int totalFiltered
            )
        {
            var result = _billUnitOfWork.BillRepository.GetDynamic(
                null,
                sortText,
                //"x => x.OrderByDescending(b => b.Id)",
                null,
                1,
                10,
                true
                );
            total = result.total;
            totalFiltered = result.totalDisplay;
            return result.data;

            //return _billUnitOfWork.BillRepository.Get(
            //  null,
            //  x => x.OrderByDescending(b => b.Id),
            //  "",
            //  pageIndex,
            //  pageSize,
            //  true);
        }

        public Bill GetBillOfThisMonth(Month month, int year)
        {
            return _billUnitOfWork.BillRepository.GetBillByMonthYear(month,year);
        }
    }
}
