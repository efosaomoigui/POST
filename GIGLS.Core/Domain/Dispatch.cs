using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class Dispatch : BaseDomain, IAuditable
    {
        public int DispatchId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ManifestNumber { get; set; }
        public decimal Amount { get; set; }
        public int RescuedDispatchId { get; set; }
        public string DriverDetail { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceivedBy { get; set; }
        public DispatchCategory DispatchCategory { get; set; }
        
        public int? DepartureId { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Station Departure { get; set; }
        
        public int? DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public virtual Station Destination { get; set; }

    }
}
