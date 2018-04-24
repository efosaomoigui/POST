using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Account
{
    public class DailySalesDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<InvoiceViewDTO> Invoices { get; set; }
        public List<DailySalesByServiceCentreDTO> DailySalesByServiceCentres { get; set; }
        public decimal TotalSales { get; set; }
        public int SalesCount { get; set; }
        public string Filename { get; set; }
    }
}
