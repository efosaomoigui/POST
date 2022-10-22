using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Fleets
{
    public class AssetDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public int NumberOfTrips { get; set; }
        public CaptainDTO Captain { get; set; }
        public decimal TotalRevenue { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string FleetManager { get; set; }
    }

    public class CaptainDTO
    {
        
        public string Name { get { return $"{FirstName} {LastName}"; } } 
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
