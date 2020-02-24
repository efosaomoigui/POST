using System;

namespace GIGLS.Services.Business.Magaya.Shipment 
{
    internal class ShipmentData
    {

        //General details
        public string Number { get; set; }
        public DateTime DateOfShipment { get; set; }
        public TimeSpan TimeOfShipment { get; set; }

        //Employee who create the Shipment
        public string IssuedBy { get; set; }

        //Destination Agent
        //public AgentInfo Agent { get; set; }

        //shipper customer
        public Customer Customer { get; set; }

        //Consignee Customer
        public Customer Consignee { get; set; }

        //supplier
        public Customer Supplier { get; set; }
        


    }

    public class Customer
    {
        public string Name { get; set; }
        //public Address Address { get; set; } 
    }
}