using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Entities
{
    public enum PaymentType
    {
        Prepaid = 1,
        Postpaid
    }
    public enum PrintingFormat
    {   
        Rent = 1,
        WaterBill,
        GasBill,
        ElectricityBill,
        ServiceCharge
    }
    public enum PaymentMethod
    {
        Cash=1,
        Check,
        BankTransfer
    }
}
