﻿namespace GIGLS.Core.Enums
{
    public enum MessageType
    {
        ShipmentCreation,
        UserAccountCreation,
        CRT, //1ST SCAN FOR EVERY SHIPMENT
        TRO, //WHEN YOU TRANSFER SHIPMENT FROM WHERE IT IS CREATED TO OPS CENTER WITHIN OR TO THE HUB
        ARO, //WHEN SHIPMENT ARRIVED THE OPS CENTER OR HUB FOR FURTHER PROCESSING
        DSC, //WHEN SHIPMENT DEPARTS SERVICE CENTER DIRECTLY TO DESTINATION
        DTR, //WHEN SHIPMENT DEPARTS SERVICE CENTER TO THE HUB OR ANOTHER CENTER THAT IS NOT THE FINAL DESTINATION
        AST, //WHEN SHIPMENT ARRIVES THE HUB OR ANOTHER CENTER THAT IS NOT THE FINAL DESTINATION
        DST, //WHEN SHIPMENT IN TRANSIT DEPARTS THE HUB OR ANOTHER CENTER THAT IS NOT THE FINAL DESTINATION
        ARP, //WHEN SHIPMENT ARRIVED THE HUB FOR FURTHER PROCESSING TO ANOTHER TRANSIT CENTER OR FINAL DESTINATION
        APT, // WHEN SHIPMENT IS ALREADY PROCESSED BUT IN TRANSIT THROUGH THE HUB
        DPC, //WHEN SHIPMENT DEPARTS PROCESSING CENTER
        ARF, //WHEN SHIPMENT ARRIVED FINAL DESTINATION
        AD, //DAILY SCAN UNTIL SHIPMENT IS DELIVERED
        OKT, //WHEN SHIPMENT IS DELIVERED AT THE TERMINAL TO THE RECEIVER
        GOP, //SCAN AT SERVICE CENTER WHEN SHIPMENT IS TRANSFERRED TO OPS FOR HOME DELIVERY
        WC, //SCAN BEFORE SHIPMENT IS TAKEN OUT FOR DELIVERY TO RECEIVER
        OKC, //SCAN TO SHOW SHIPMENT HAS BEEN DELIVERED
        SSR,  //SCAN SHIPMENT FOR RETURNS
        SSC,  //SCAN SHIPMENT FOR CANCELLED
        SRR,  //SCAN SHIPMENT FOR REROUTE
        SRC, //SCAN FOR SHIPMENT RECEIVED FROM COURIER
        AHD, //WHEN SHIPMENT ARRIVED FINAL DESTINATION FOR HOME DELIVERY
        PreShipmentCreation,
        CRH, //SHIPMENT CREATION SCAN FOR HOME DELIVERY - EMAIL
        ISE, //SHIPMENT CREATION SCAN FOR INTERNAIONAL SHIPMENT - EMAIL
        USER_LOGIN, //USER LOGIN
        IEMAIL, //FOR DUE INVOICES FOR CORPORATE CLIENTS
        WEMAIL, //WALLET BALANCES AT 10K AND 5K FOR CORPORATE CLIENTS
        SSC_Email,  //Message Type for use in sending email
        MATD  //Message for Attempted Delivery
    } 
}
