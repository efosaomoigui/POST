using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Customers
{
    public class EcommerceWalletDTO : BaseDomainDTO
    {
        public decimal WalletBalance { get; set; }
        public CountryDTO Country { get; set; }
    }
}
