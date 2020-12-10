using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class PreshipmentManifestDTO
    {
        public PreshipmentManifestDTO()
        {
            Waybills = new List<PreShipmentMobileWaybill>();
        }
        public int ManifestId { get; set; }
        public string ManifestCode { get; set; }
        public DateTime DateTime { get; set; }
        public ManifestType ManifestType { get; set; }
        public bool Picked { get; set; }
        public List<PreShipmentMobileWaybill> Waybills { get; set; }
    }


    public class PreShipmentMobileWaybill
    {
        public PreShipmentMobileWaybill()
        {
            Receiver = new ReceiverDTO();
            Sender = new SenderDTO();
        }
        public string Waybill { get; set; }
        public ReceiverDTO Receiver { get; set; }
        public SenderDTO Sender { get; set; }
    }

    public class ReceiverDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
    }

    public class SenderDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
    }



}
