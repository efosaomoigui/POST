namespace GIGLS.Core.Domain
{
    public class InternationalRequestReceiverItem : BaseDomain, IAuditable
    {
        public int InternationalRequestReceiverItemId { get; set; }

        public int InternationalRequestReceiverId { get; set; }
        public virtual InternationalRequestReceiver InternationalRequestReceiver { get; set; }

        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Value { get; set; }        
    }
}
