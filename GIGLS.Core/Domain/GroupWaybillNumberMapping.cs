using GIGLS.Core.Domain;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class GroupWaybillNumberMapping : BaseDomain
    {
        public int GroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string GroupWaybillNumber { get; set; }
        public string WaybillNumber { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public virtual ServiceCentre DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentre DestinationServiceCentre { get; set; }

        //Original Information - used for Transit Manifest Tracking
        public int OriginalDepartureServiceCentreId { get; set; }
        public virtual ServiceCentre OriginalDepartureServiceCentre { get; set; }
        
        public string UserId { get; set; }
    }
}