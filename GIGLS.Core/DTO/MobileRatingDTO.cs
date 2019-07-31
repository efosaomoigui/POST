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
        public int CustomerRating { get; set; }
        public string UserChannelCode { get; set; }

        public string UserId { get; set; }

        public int Rating { get; set; }
        public UserChannelType UserChannelType { get; set; }

        public int PartnerRating { get; set; }

        public string CustomerId { get; set; }

        public string PartnerId { get; set; }
    }
}
