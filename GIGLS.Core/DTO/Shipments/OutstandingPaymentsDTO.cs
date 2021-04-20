using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class OutstandingPaymentsDTO
    {
        public string Waybill { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public decimal Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public CountryDTO Country { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
