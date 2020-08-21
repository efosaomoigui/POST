namespace GIGLS.Core.DTO
{
    public class ApproveShipmentDTO
    {
        public string WaybillNumber { get; set; }
        public int ReceiverServiceCentreId { get; set; }
        public int SenderServiceCentreId { get; set; }
        public string ReceiverAddress { get; set; }
    }
}
