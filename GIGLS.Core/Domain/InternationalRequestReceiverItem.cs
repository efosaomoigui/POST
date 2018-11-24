using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class InternationalRequestReceiverItem : BaseDomain, IAuditable
    {
        public string ReceiverId { get; set; }
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
