using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class CancelShipmentDTO
    {
        public string Waybill { get; set; }
        public string CancelReason { get; set; }
        public string Userchanneltype { get; set; }
    }

    public class CancelledShipmentDTO
    {
        public string Waybill { get; set; }
        public string SenderName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CustomerCancelReason { get; set; }
        public string SenderAddress { get; set; }
    }
}
