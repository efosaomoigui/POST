using GIGL.GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class MobilePickUpRequests : BaseDomain
    {
        public int MobilePickUpRequestsId { get; set; }
        public string Status { get; set; }
        public string Waybill { get; set; }
        //foreign key information
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
