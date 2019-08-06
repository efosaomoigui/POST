using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class MobilePickUpRequests : BaseDomain
    {
        public int MobilePickUpRequestsId { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        //foreign key information
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string Reason { get; set; }
        [MaxLength(100)]
        public string PartnerCode { get; set; }
    }
}
