using POST.Core;
using POST.Core.Domain;
using POST.CORE.Enums;

namespace GIGL.POST.Core.Domain
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