using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class MobileRating : BaseDomain, IAuditable
    {
        public int MobileRatingId { get; set; }
        public string Waybill { get; set; }
        public string Comment { get; set; }
        public string PartnerCode { get; set; }

        public string CustomerCode { get; set; }
        public int CustomerRating { get; set; }

        public int PartnerRating { get; set; }

        public string CustomerId { get; set; }

        public string PartnerId { get; set; }



    }
}
