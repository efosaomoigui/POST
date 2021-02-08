using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;

namespace GIGLS.CORE.DTO.Report
{
    public class AccountFilterCriteria : BaseFilterCriteria
    {
        public CreditDebitType? creditDebitType { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public PaymentType? PaymentType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? IsDeferred { get; set; }
        public string CompanyType { get; set; }
        public PaymentServiceType? PaymentServiceType { get; set; }
        public bool? IsCashOnDelivery { get; set; } = false;
        public bool? IsCancelled { get; set; } = false;
        public int CountryId { get; set; }
        public int ServiceCenterId { get; set; }
        public DateTime dateFrom { get; set; } = DateTime.Now.AddDays(-7);
        //public int RegionId { get; set; } 

    }

    public class DateFilterForDropOff
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class AccountFilterCriteria2
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ServiceCentreId { get; set; }
        public int StationId { get; set; }
        public int StateId { get; set; }

        //public CreditDebitType? creditDebitType { get; set; } = CreditDebitType.Credit;
        public PaymentStatus? Status { get; set; } = PaymentStatus.Paid;
        //public PaymentType? Type { get; set; } = PaymentType.Cash;
        //public DateTime? PaymentDate { get; set; } = DateTime.Now;
        //public bool? IsDeferred { get; set; } = false;
        //public string CompanyType { get; set; } = "Individual Customers";
        public PaymentServiceType? PaymentServiceT { get; set; } = PaymentServiceType.Shipment;
        //public bool? IsCashOnDelivery { get; set; } = false;
        public bool? IsCancelled { get; set; } = false;
        public int CountryId { get; set; } = 1;
        public int limitTime { get; set; } = 1;

    }

    public class dashboard_color_code 
    {
        public List<InvoiceMonitorDTO> one_day_created { get; set; }
        public List<InvoiceMonitorDTO> two_days_created { get; set; }
        public List<InvoiceMonitorDTO> above_one_day_created { get; set; }

        public List<InvoiceMonitorDTO> one_day_expected { get; set; }
        public List<InvoiceMonitorDTO> two_days_expected { get; set; }
        public List<InvoiceMonitorDTO> above_one_day_expected { get; set; }

        public List<InvoiceMonitorDTO_total> total_created { get; set; }
        public List<InvoiceMonitorDTO_total> total_expected { get; set; } 

    }

}
