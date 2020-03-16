using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Partnership
{
    public class FleetPartnerDTO : BaseDomainDTO
    {
        public int FleetPartnerId { get; set; }        
        public string FleetPartnerCode { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Email { get; set; }       
        public string PhoneNumber { get; set; }        
        public string Address { get; set; }
        public string UserId { get; set; }
        public int UserActiveCountryId { get; set; }
        public CountryDTO Country { get; set; }
        public List<string> PartnerCodes { get; set; }
    }
}
