using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using System;
using System.Collections.Generic;

namespace GIGLS.CORE.DTO.Shipments
{
    public class GroupWaybillNumberMappingDTO : BaseDomain
    {
        public int GroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string GroupWaybillNumber { get; set; }
        public string WaybillNumber { get; set; }

        public List<string> WaybillNumbers { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public virtual ServiceCentre DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentre DestinationServiceCentre { get; set; }
    }
}