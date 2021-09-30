using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class PaymentMethod : BaseDomain, IAuditable
    {
        public int PaymentMethodId { get; set; }

        [MaxLength(100)]
        public string PaymentMethodName { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
    }
}
