using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Partnership
{
    public class PartnerTransactionsDTO : BaseDomainDTO
    {
        public int PartnerTransactionsID { get; set; }
        public string UserId { get; set; }
        public string Destination { get; set; }
        public string Departure { get; set; }
        public decimal AmountReceived { get; set; }
        public string Waybill { get; set; }
        //public PartnerDTO Partner { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrencySymbol { get; set; }

        public bool IsFromServiceCentre { get; set; }
    }
}
