using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Entities
{
    public class Tenant : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Holding { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Rent { get; set; }
        public int ServiceCharge { get; set; }
        public int WaterBill { get; set; }
        public int GasBill { get; set; }
        public PaymentType PaymentType { get; set; }
        public int AdvanceArrear { get; set; }
        public DateTime ContractSigningDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractExpirationDate { get; set; }
        public bool Status { get; set; }
        public IList<Document> Documents { get; set; }
        public IList<ContactPerson> ContactPersons { get; set; }
        public IList<Printing> Printings { get; set; }
        public IList<Payment> Payments { get; set; }
    }
    
}
