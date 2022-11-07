using System.Collections.Generic;

namespace POST.Core.DTO
{
    public class ApproveShipmentDTO
    {
        public string WaybillNumber { get; set; }
        public int ReceiverServiceCentreId { get; set; }
        public int SenderServiceCentreId { get; set; }
        public string ReceiverAddress { get; set; }
        public bool IsBulky { get; set; }
        public bool ExpressDelivery { get; set; }
        public List<int> PackageOptionIds { get; set; } = new List<int>();
    }
}
