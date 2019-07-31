using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class MobileRatingDTO : BaseDomainDTO
    {
        public int MobileRatingId { get; set; }
        public string Waybill { get; set; }
        public string Comment { get; set; }
        public string PartnerCode { get; set; }

        public string CustomerCode { get; set; }
        public double? CustomerRating { get; set; }
        public string UserChannelCode { get; set; }

        public string UserId { get; set; }

        public double? Rating { get; set; }
        public string UserChannelType { get; set; }

        public double? PartnerRating { get; set; }

        public string CustomerId { get; set; }

        public string PartnerId { get; set; }
        public bool IsRatedByCustomer { get; set; }

        public bool IsRatedByPartner { get; set; }
        public string CommentByCustomer { get; set; }

        public string CommentByPartner { get; set; }
        public DateTime? DateCustomerRated { get; set; }
        public DateTime? DatePartnerRated { get; set; }
    }
}
