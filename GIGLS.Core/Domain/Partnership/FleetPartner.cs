using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain.Partnership
{
    public class FleetPartner : BaseDomain, IAuditable
    {
        public int FleetPartnerId { get; set; }

        [MaxLength(100)]
        public string FleetPartnerCode { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }       

        [MaxLength(128)]
        public string UserId { get; set; }
                                    
        public int UserActiveCountryId { get; set; }

    }
}
