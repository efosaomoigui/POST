using AutoMapper;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Magaya.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.WebServices;
using ThirdParty.WebServices.Magaya.Business;
using ThirdParty.WebServices.Magaya.DTO;

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
        public string SetTransactions(int access_key, MagayaShipmentDto magayaShipmentDTO)
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

            //serialize object to xml from class warehousereceipt
            WarehouseReceipt shipmentdata = new WarehouseReceipt();
            var xmlobject = Mapper.Map<WarehouseReceipt>(magayaShipmentDTO); 

            var obWh = new WarehouseReceipt()
            {
                CreatedOn = "2018-05-17T17:40:55-04:00",
                Number = "WEARR",
                CreatedByName = "Administrator",
                Version = "Version1",
                ModeOfTransportation = new ModeOfTransportation() {
                    Code = "123",
                    Method = "Air", 
                    Description = "Air Method"
                },
                ModeOfTransportCode = null,
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
                CarrierName = "",
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
                Attachments = null,
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
                HoldStatus = new HoldStatus() { IsOnHold = "true" },
                IsLiquidated = "",
                Xmlns = "http://www.magaya.com/XMLSchema/V1",
                SchemaLocation = "http://www.magaya.com/XMLSchema/V1",
                GUID = "",
                Type = ""
            };
            //};

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                trans_xml = sr.Serialize<WarehouseReceipt>(xmlobject);

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

        //For creating shipment in Magaya
        public string SetEntity(int access_key, EntityDto entitydto) 
        {

            //1. initialize the magaya web service object
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);

            //2. initialize type of shipment and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var errval = string.Empty;

            //initialize the serializer object
            Serializer sr = new Serializer();

            //serialize object to xml from class warehousereceipt
            var entitydata = new Entity();
            var xmlobject = Mapper.Map<Entity>(entitydto);

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                entity_xml = sr.Serialize<Entity>(xmlobject);

                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.SetEntity(access_key,flags, entity_xml, out error_code); 
                errval = error_code;
            }
            catch (Exception ex)
            {
                errval = ex.Message;
            }

            return errval;

        }

        public string GetTransactions(int access_key, MagayaShipmentDto magayaShipmentDTO) 
        {
            //1. initialize the magaya web service object
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);

            //2. initialize type of shipment and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var errval = string.Empty;

            //4.initialize the serializer object
            Serializer sr = new Serializer();

            //serialize object to xml from class warehousereceipt
            WarehouseReceipt shipmentdata = new WarehouseReceipt();
            var xmlobject = Mapper.Map<WarehouseReceipt>(magayaShipmentDTO);

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                entity_xml = sr.Serialize<WarehouseReceipt>(xmlobject);

                string error_code = "";
                result = cs.SetEntity(access_key, flags, entity_xml, out error_code);
                errval = error_code;
            }
            catch (Exception ex)
            {
                errval = ex.Message;
            }

            return errval;
        }

        //Get customers, forwarding agents etc
        public Entities GetEntities(int access_key, string startwithstring)  
        {

            //1. initialize the magaya web service object
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);

            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            Entities errval = null;

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.GetEntities(access_key, flags, startwithstring, out error_code);
                var objectOfXml  = sr.Deserialize<Entities>(error_code); 
                errval = objectOfXml;
            }
            catch (Exception ex) 
            {
            }

            return errval;
        }


        //Get Magaya employees
        public Employees GetEmployees(int access_key, string startwithstring)
        {
            //1. initialize the magaya web service object
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);

            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            Employees errval = null;

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.GetEntities(access_key, flags, startwithstring, out error_code);
                var objectOfXml = sr.Deserialize<Employees>(error_code);
                errval = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return errval;
        }

        //Get Magaya Vendors
        public Employees GetVendors(int access_key, string startwithstring) 
        {
            //1. initialize the magaya web service object
            var _webServiceUrl = "http://35652.magayacloud.com:3691/Invoke?Handler=CSSoapService";
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);

            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            Employees errval = null;

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.GetEntities(access_key, flags, startwithstring, out error_code);
                var objectOfXml = sr.Deserialize<Employees>(error_code);
                errval = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return errval;
        }

    }

}
