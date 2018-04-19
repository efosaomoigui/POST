using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Account
{
    public class DailySalesByServiceCentreDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public string DepartureServiceCentreName { get; set; }
        public List<InvoiceViewDTO> Invoices { get; set; }
        public decimal TotalSales { get; set; }
        public int SalesCount { get; set; }
    }

}
