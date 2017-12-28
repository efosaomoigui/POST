using System.ComponentModel;

namespace GIGLS.Core.Enums
{
    public enum ShipmentScanStatus
    {
        Recieved,
        Transit,
        Processing,
        Delivered,
        Collected,
        Returns,

        [Description("ARRIVAL SCAN AT SERVICE CENTER(TERMINAL PICK UP)")]
        ASP,

        [Description("DESTINATION DAILY SCAN AWAITING DELIVERY(TERMINAL PICK UP SHIPMENTS)")]
        ASPD,

        [Description("DESTINATION ARRIVAL SCAN (HOME DELIVERY SHIPMENTS)")]
        DASA,

        [Description("DESTINATION ARRIVAL SCAN (HOME DELIVERY SHIPMENTS)")]
        DASD,
        
        [Description("DESTINATION ARRIVAL SCAN (TERMINAL PICK UP SHIPMENTS)")]  
        DASP,

        [Description("DESTINATION DELIVERY SCAN (ALL SHIPMENTS)")]
        DDSA,

        [Description("DEPART SERVICE CENTER TO DESTINATION (HOME DELIVERY SHIPMENTS)")]
        DSCD,

        [Description("DEPART SERVICE CENTER TO GATEWAY FOR DELIVERY WITHIN ABUJA")]
        DSCDA,

        [Description("DEPART SERVICE CENTER TO GATEWAY FOR DELIVERY WITHIN LAGOS")]
        DSCDL,

        [Description("DEPART SERVICE CENTER TO GATEWAY LAGOS")]
        DSCG,

        [Description("DEPART SERVICE CENTER TO DESTINATION(TERMINAL PICK UP SHIPMENTS)")]
        DSCP,

        [Description("GATEWAY SHIPMENT ARRIVAL FROM ALL SERVICE CENTERS (HOME DELIVERY SHIPMENTS)")]
        GAH,

        [Description("GATEWAY SHIPMENT ARRIVAL FROM ALL SERVICE CENTERS (TERMINAL PICK UP SHIPMENTS)")]
        GAP,

        [Description("GATEWAY SHIPMENT DEPARTURE FOR DELIVERY")]
        GDD,

        [Description("GATEWAY SHIPMENT DEPARTURE TO ALL DESTINATIONS (HOME DELIVERY SHIPMENTS)")]
        GDH,

        [Description("GATEWAY SHIPMENT DEPARTURE TO ALL DESTINATIONS (TERMINAL PICK UP SHIPMENTS)")]
        GDP
    }
}
