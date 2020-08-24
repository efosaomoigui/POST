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
        public DateTime DateCreated { get; set; }
    }
}
