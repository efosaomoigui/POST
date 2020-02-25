using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Business.Magaya.Shipment;
using GIGLS.Services.Magaya.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GIGLS.Services.Business.Magaya.Serializer;

namespace GIGLS.Services.Business.Magaya.Shipment
{
    public class MagayaService : IMagayaService 
    {

        int key = -1;
        int myAccessKey = -1;

        public MagayaService()
        {

        }

        //Open Connections
        public bool OpenConnection(out int access_key)
        {
            var magayausername = ConfigurationManager.AppSettings["MagayaUsername"];
            var magayapassword = ConfigurationManager.AppSettings["MagayaPassword"];

            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);

            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress); 
            api_session_error result = api_session_error.no_error;
            try
            {
                result = cs.StartSession(magayausername, magayapassword, out key);
                access_key = key;
                return result == api_session_error.no_error;
            }
            catch
            {
                access_key = 0;
                return false;
            }
        }

        //Close Connections
        public string CloseConnection(int access_key)
        {
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);

            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
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
        public string SetTransactions(int access_key)
        {

            //1. initialize the magaya web service object
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);

            //2. initialize type of shipment and flag
            string type = "SH";
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string trans_xml = string.Empty;
            var errval = string.Empty;

            //initialize the serializer object
            Serializer sr = new Serializer();

            //get the xml for shipment from file directory
            //path = Directory.GetCurrentDirectory() + @"\GIGLS.Services\Business\Magaya\xml\AirBooking.xml";
            //xmlInputData = File.ReadAllText(path);

            //deserialize from file to object
            //ShipmentData shipmentdata = sr.Deserialize<ShipmentData>(xmlInputData);

            var mode = new ModeOfTransportation()
            {
                Code = "AirBB",
                Description = "Descript"
            };

            //serialize object to xml from class warehousereceipt
            WarehouseReceipt shipmentdata = new WarehouseReceipt();

            shipmentdata.CreatedOn = "2018-05-17T17:40:55-04:00";
            shipmentdata.Number = "WEARR";
            shipmentdata.CreatedByName = "Administrator";
            shipmentdata.Version = "Version1";
            shipmentdata.ModeOfTransportation = mode;
            shipmentdata.ModeOfTransportCode = mode.Code;
            shipmentdata.IssuedBy = null;

            //    IssuedBy = null,
            //    IssuedByAddress = null,
            //    IssuedByName = "",
            //    ShipperName = "Efe",
            //    ShipperAddress = null,
            //    Shipper = null,
            //    ConsigneeName = "",
            //    ConsigneeAddress = null,
            //    Consignee = null,
            //    DestinationAgentName = "",
            //    DestinationAgent = null,
            //    Carrier = null,
            //    CarrierName = "",
            //    CarrierTrackingNumber = "",
            //    CarrierPRONumber = "",
            //    DriverName = "",
            //    DriverLicenseNumber = "",
            //    Notes = "",
            //    Items = null,
            //    MeasurementUnits = null,
            //    CreatorNetworkID = "",
            //    Charges = null,
            //    Events = null,
            //    Division = null,
            //    TotalPieces = "",
            //    TotalWeight = null,
            //    TotalVolume = null,
            //    TotalValue = null,
            //    TotalVolumeWeight = null,
            //    ChargeableWeight = null,
            //    OriginPort = null,
            //    DestinationPort = null,
            //    SupplierName = "",
            //    SupplierAddress = null,
            //    Supplier = null,
            //    SupplierInvoiceNumber = "",
            //    SupplierPONumber = "",
            //    FromQuoteNumber = "",
            //    HasAttachments = "",
            //    Attachments = "",
            //    BondedEntry = "",
            //    BondedEntryNumber = "",
            //    BondedEntryDate = "",
            //    CarrierBookingNumber = "",
            //    FromBookingNumber = "",
            //    MainCarrier = null,
            //    BillingClient = null,
            //    LastItemID = "",
            //    URL = "",
            //    CustomFields = null,
            //    IsOnline = "",
            //    HoldStatus = new HoldStatus() { IsOnHold = "true" },
            //    IsLiquidated = "",
            //    Xmlns = "http://www.magaya.com/XMLSchema/V1",
            //    SchemaLocation = "",
            //    GUID = "",
            //    Type = "",
            //    Xsi = ""

            //};

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                trans_xml = sr.Serialize<WarehouseReceipt>(shipmentdata);
                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.SetTransaction(access_key, type, flags, trans_xml, out error_code);
                errval =  error_code;
            }
            catch (Exception ex)
            {
                errval = ex.Message;
            }

            return errval;

        }
    }

}
