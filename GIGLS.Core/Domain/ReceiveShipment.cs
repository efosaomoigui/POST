namespace GIGLS.Core.Domain
{
    public class ReceiveShipment : BaseDomain
    {
        //Third Party Receiver (Optional) to Receive shipment
        public string Comments { get; set; }
        public string ActualReceiverName { get; set; }
        public string ActualreceiverPhone { get; set; }
        public string IdentificationType { get; set; }
        public string IndentificationUrl { get; set; }
    }
}
