using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.CORE.Enums;

namespace GIGL.GIGLS.Core.Domain
{
    public class DeliveryOption : BaseDomain, IAuditable
    {
        public int DeliveryOptionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public FilterCustomerType CustomerType { get; set; }
    }
}