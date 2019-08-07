using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class MobileRating : BaseDomain, IAuditable
    {
        public int MobileRatingId { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public string CommentByCustomer { get; set; }

        public string CommentByPartner { get; set; }

        [MaxLength(100)]
        public string PartnerCode { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }
        public double? CustomerRating { get; set; }

        public double? PartnerRating { get; set; }

        public bool IsRatedByCustomer { get; set; }

        public bool IsRatedByPartner { get; set; }

        public DateTime? DateCustomerRated { get; set; }
        public DateTime? DatePartnerRated { get; set; }




    }
}
