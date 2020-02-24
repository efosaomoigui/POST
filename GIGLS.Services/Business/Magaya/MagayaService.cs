using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Business.Magaya.Shipment;
using GIGLS.Services.Magaya.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Magaya.Shipment
{
    class MagayaService : IMagayaService
    {

        int key = -1;
        int myAccessKey = -1;

        //Open Connections
        public async Task<bool> OpenConnection()
        {
            var magayausername = ConfigurationManager.AppSettings["MagayaUsername"];
            var magayapassword = ConfigurationManager.AppSettings["MagayaPassword"];

            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient();
            api_session_error result = api_session_error.no_error;
            try
            {
                result = cs.StartSession(magayausername, magayapassword, out key);
                //this = cs;
                this.myAccessKey = key;
                var resValue = await Task.FromResult(api_session_error.no_error);
                return result == resValue;
            }
            catch
            {
                return false;
            }

        }

        //Close Connections
        public async Task<string> CloseConnection(int access_key)
        {
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient();
            try
            {
                var result = cs.EndSession(access_key);
                return result.ToString();
            }
            catch
            {
                return null;
            }

        }

        //For creating shipment in Magaya
        public async Task<string> SetTransactions(int access_key)
        {
            //get the magaya user name and password from the config file
            var magayausername = ConfigurationManager.AppSettings["MagayaUsername"];
            var magayapassword = ConfigurationManager.AppSettings["MagayaPassword"];

            //initialize the magaya web service object
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient();

            //initialize type of shipment and flag
            string type = "SH";
            int flags = 0x00000800;

            //initilize the variables to hold some parameters and return values
            string path = string.Empty;
            string xmlInputData = string.Empty;
            string xmlOutputData = string.Empty;
            string trans_xml = string.Empty;

            //initialize the serializer object
            Serializer sr = new Serializer();

            //get the xml for shipment from file directory
            //path = Directory.GetCurrentDirectory() + @"\GIGLS.Services\Business\Magaya\xml\AirBooking.xml";
            //xmlInputData = File.ReadAllText(path);

            //deserialize from file to object
            //ShipmentData shipmentdata = sr.Deserialize<ShipmentData>(xmlInputData);

            //deserialize xml to object from class warehousereceipt
            WarehouseReceipt shipmentdata = new WarehouseReceipt() {
                CreatedOn = "2020-19-02",
                Number = "WEARR",
                CreatedByName = "Administrator",
                Version = "Version1",
                ModeOfTransportation = null,
                ModeOfTransportCode = "Air",
                IssuedBy = null,
                IssuedByAddress = null,
                IssuedByName = "",
                ShipperName = "Efe",
                ShipperAddress = null, 
                Shipper = null,
                ConsigneeName = "",
                ConsigneeAddress = null,
                Consignee = null,
                DestinationAgentName = "",
                DestinationAgent = null,
                Carrier = null,
                CarrierName= "",
                CarrierTrackingNumber = "",
                CarrierPRONumber = "",
                DriverName = "",
                DriverLicenseNumber = "",
                Notes = "",
                Items = null,
                MeasurementUnits = null,
                CreatorNetworkID = "", 
                Charges = null,
                Events = null,
                Division = null,
                TotalPieces = "", 
                TotalWeight = null,
                TotalVolume = null,
                TotalValue = null,
                TotalVolumeWeight = null,
                ChargeableWeight = null,
                OriginPort = null,
                DestinationPort = null,
                SupplierName = "",
                SupplierAddress = null,
                Supplier = null,
                SupplierInvoiceNumber = "",
                SupplierPONumber = "",
                FromQuoteNumber = "",
                HasAttachments = "",
                Attachments = "",
                BondedEntry = "",
                BondedEntryNumber = "",
                BondedEntryDate = "", 
                CarrierBookingNumber = "",
                FromBookingNumber = "",
                MainCarrier = null,
                BillingClient = null,
                LastItemID = "",
                URL = "", 
                CustomFields = null,
                IsOnline = "",
                HoldStatus = new HoldStatus(){ IsOnHold = "true"},
                IsLiquidated = "",
                Xmlns = "", 
                SchemaLocation = "",
                GUID = "", 
                Type = "",
                Xsi = ""

            };

            //serialize to xml for the magaya request
            trans_xml = sr.Serialize<WarehouseReceipt>(shipmentdata);
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";
                result = cs.SetTransaction(access_key, type, flags, trans_xml, out error_code);
                return error_code;
            }
            catch
            {
                return "";
            }

        }

        public Task<string> SetTransactions(int access_key, string type, int flags, string trans_xml)
        {
            throw new NotImplementedException();
        }
    }

}
