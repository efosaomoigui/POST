using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Magaya.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Magaya
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
        public async Task<string> SetTransactions(int access_key, string type, int flags)
        {
            var magayausername = ConfigurationManager.AppSettings["MagayaUsername"];
            var magayapassword = ConfigurationManager.AppSettings["MagayaPassword"];

            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient();
            Serializer sr = new Serializer();
            string path = string.Empty;
            string xmlInputData = string.Empty;
            string xmlOutputData = string.Empty;
            string trans_xml = string.Empty;

            path = Directory.GetCurrentDirectory() + @"\Business\Magaya\Customer.xml";
            xmlInputData = File.ReadAllText(path);

            ShipmentData shipmentdata = sr.Deserialize<ShipmentData>(xmlInputData);
            
            //create a file that has properties from field that customers filled in
            //shipmentdata
            trans_xml = sr.Serialize<ShipmentData>(shipmentdata);

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
    }

}
