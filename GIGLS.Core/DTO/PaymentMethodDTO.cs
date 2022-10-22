using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class PaymentMethodDTO 
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentMethodNewDTO : BaseDomainDTO
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsActive { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
