using System.Collections.Generic;

namespace GIGLS.Core.Domain
{
    public class InternationalRequestReceiver : BaseDomain, IAuditable
    {
        public InternationalRequestReceiver()
        {
            InternationalRequestReceiverItems = new HashSet<InternationalRequestReceiverItem>();
        }

        public int InternationalRequestReceiverId { get; set; }
        public string CustomerId { get; set; }
        public string GenerateCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string  City { get; set; }
        public string  Country { get; set; }

        public virtual ICollection<InternationalRequestReceiverItem> InternationalRequestReceiverItems { get; set; }

    }
}
