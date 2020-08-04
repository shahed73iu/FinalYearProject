using Autofac;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class TenantViewModel : BaseModel
    {
        public int Id { get; set;}
        public int TotalBill { get; set; }
        public bool ElectricityBill { get; set; }
        public bool Service { get; set; }
        public bool Rent { get; set; }
        public DateTime MonthPicker { get; set; }
        public DateTime FinalBillDate { get; set; }
        public DateTime ReadingTakenDate { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public bool AllCombinations { get; set; }
        public Reading Reading { get; set; }
        public int Count { get; set; }
        public Tenant Tenant { get; set; }
        public Payment Payment { get; set; }
        public List<Printing> Printings { get; set; }
        [Required]
        public List<Payment> Payments { get; set; }

        private ITenantService _tenantService;

        private IReadingService _readingService;
        public TenantViewModel()
        {
            _tenantService = Startup.AutofacContainer.Resolve<ITenantService>();
            _readingService = Startup.AutofacContainer.Resolve<IReadingService>();
        }
        public TenantViewModel(ITenantService tenantService,IReadingService readingService)
        {
            _tenantService = tenantService;
            _readingService = readingService;
        }

        public object GetTenantAndContactPerson(DataTablesAjaxRequestModel tabelModel)
        {
            int total = 0;
            int totalFiltered = 0;
            var records = _tenantService.GetTenants(
                tabelModel.PageIndex,
                tabelModel.PageSize,
                tabelModel.SearchText,
                tabelModel.GetSortText(new string[] { "Name", "Holding", "AdvanceArrear", "Rent", "GasBill", "WaterBill", "ServiceCharge", "ContactPersons" }),
                out total,
                out totalFiltered);
            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                                record.Name,
                                record.PaymentType.ToString(),
                                //record.Holding.ToString(),
                                record.AdvanceArrear.ToString(),
                                record.Rent.ToString(),
                                record.GasBill.ToString(),
                                record.WaterBill.ToString(),
                               //S record.PaymentType.ToString(),
                                record.ServiceCharge.ToString(),
                                record.ContactPersons.Select(x => x.Name +" - "+ x.ContractNumber).FirstOrDefault(),
                                record.Id.ToString(),
                        }
                    ).ToArray()
            };
        }
        public void Delete(int id)
        {
           var tenant = _tenantService.GetTenantOnly(id);
           _tenantService.EditTenantStatus(tenant);
        }
        public void InitiateTenant(int tenantId)
        {
            Tenant = _tenantService.GetTenant(tenantId);
        }
        public void InitiateReading(int tenantId, DateTime startDate)
        {
            Reading = _readingService.GetReadingOfTenant(tenantId, startDate);
        }
        public void TotalCalculate()
        {
            if (Payment.Format.Contains("ElectricityBill") && _readingService.GetReadingOfTenant(Payment.TenantId,FinalBillDate) != null)
            {
                Total += _readingService.GetReadingOfTenant(Payment.TenantId, FinalBillDate).TotalBillofThisMonth;
            }
            var tenant = _tenantService.GetTenant(Payment.TenantId);
            if (Payment.Format.Contains("GasBill")) Total += tenant.GasBill;
            if (Payment.Format.Contains("WaterBill")) Total += tenant.WaterBill;
            if (Payment.Format.Contains("Rent")) Total += tenant.Rent;
            if (Payment.Format.Contains("ServiceCharge")) Total += tenant.ServiceCharge;
        }
        public string ValidateAmountWithTotalCalCulation(int tenantId,DateTime time,string format,decimal amount)
        {
            Reading = _readingService.GetReadingOfTenant(tenantId, time.AddMonths(-1));
            Tenant = _tenantService.GetTenant(tenantId);
            var payment = _tenantService.GetPayment(tenantId, format,time);
            decimal total = 0;

            if (format.Contains("ServiceCharge")) total += Tenant.ServiceCharge;
            if (format.Contains("GasBill")) total += Tenant.GasBill;
            if (format.Contains("WaterBill")) total += Tenant.WaterBill;
            if (format.Contains("Rent")) total += Tenant.Rent;
            if (format.Contains("ElectricityBill")) total += Reading.TotalBillofThisMonth;


            Total = total;

            if(payment==null)
            {
                if (amount > total && (amount-total>=1)) return "InValid";
                else return "Valid";
            }
            else
            {
                if (payment.DueAmount == 0) return "AlreadyExist";
                else if (payment.DueAmount != 0 && amount > payment.DueAmount && (amount-payment.DueAmount>=1)) return "InValid";
            }
            return "Valid";
        }
        public void UpdatePayment()
        {
            try
            {
                var tenantId = Payments.ElementAt(0).TenantId;
                Tenant = _tenantService.GetTenant(tenantId);
                foreach (var Payment in Payments)
                {
                    if (Payment.BillFormat != null)
                    {
                        var payment = _tenantService.GetPayment(tenantId, Payment.Format, Payment.FinalBillDate);
                        Payment.TenantId = tenantId;
                        Reading = _readingService.GetReadingOfTenant(tenantId, Payment.FinalBillDate.AddMonths(-1));
                        ValidateAmountWithTotalCalCulation(tenantId, Payment.FinalBillDate, Payment.Format, Payment.PaymentAmount);
                       
                        if (payment == null)
                        {
                            Payment.DueAmount = Total - Payment.PaymentAmount;
                            if (Payment.DueAmount < 1) Payment.DueAmount = 0;
                            _tenantService.AddPayment(Payment);
                        }
                        else
                        {
                            if (payment.DueAmount != 0)
                            {
                                payment.DueAmount = payment.DueAmount - Payment.PaymentAmount;
                                if (payment.DueAmount < 1) payment.DueAmount = 0;
                                Payment.DueAmount = payment.DueAmount;
                            }

                            payment.PaymentAmount = Payment.PaymentAmount;
                            payment.PaymentMethod = Payment.PaymentMethod;
                            payment.DocumentsDetails = Payment.DocumentsDetails;
                            _tenantService.EditPayment(payment);
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                {
                    Notification = new NotificationModel(
                        "Failed!",
                        "Your amount is greater than total amountof bill",
                        NotificationModel.NotificationType.Fail);

                }
            }
        }
        public void LoadPrintings(int id)
        {
            Printings = _tenantService.GetPrintingsByTenantId(id);
            char[] spearator = { ',' };
            foreach (var item in Printings)
            {
                item.BillFormat = item.Format.Split(spearator);
            }
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
        public void ChangePaymentFormat()
        {
            
            foreach (var item in Payments)
            {
                if (item.BillFormat != null)
                {
                    item.Format = string.Join(",", item.BillFormat);
                }
            }
        }
    }
}
