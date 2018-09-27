using GIGLS.Core.Enums;
using GIGLS.Core.DTO.Customers;
using System;
using System.Collections.Generic;
using GIGLS.CORE.DTO;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Zone;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.DTO.Account;

namespace GIGLS.Core.DTO.Shipments
{
    public class ThirdPartyPreShipmentDTO
    {
        //PickUp' Information
        public string PickUpPhoneNumber { get; set; }
        public string PickUpAddress { get; set; }
        public string PickUpCity { get; set; }
        public string PickUpState { get; set; }
        public string PickUpCountry { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }

        //PreShipment Items
        public List<ThirdPartyPreShipmentItemDTO> PreShipmentItems { get; set; }


        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; } = 0;

        //
        public bool IsdeclaredVal { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; } = 0;

        public decimal? Total { get; set; } = 0;

        public string Description { get; set; }

        public PreShipmentRequestStatus RequestStatus { get; set; }
        public PreShipmentProcessingStatus ProcessingStatus { get; set; }

        //Receivers Information
        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }

    }
}
