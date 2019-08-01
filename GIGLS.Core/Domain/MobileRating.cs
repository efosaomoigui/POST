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
        public string CommentByCustomer { get; set; }

        public string CommentByPartner { get; set; }

        public string PartnerCode { get; set; }

        public string CustomerCode { get; set; }
        public double? CustomerRating { get; set; }

        public double? PartnerRating { get; set; }

        public bool IsRatedByCustomer { get; set; }

        public bool IsRatedByPartner { get; set; }

        public DateTime? DateCustomerRated { get; set; }
        public DateTime? DatePartnerRated { get; set; }




    }
}
