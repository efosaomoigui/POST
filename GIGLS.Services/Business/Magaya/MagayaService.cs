using GIGLS.Services.Magaya.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Magaya
{
    class MagayaService
    {

        int key = -1;
        int myAccessKey = -1;
        public bool OpenConnection(string user, string password)
        {
            //var magayausername = ConfigurationManager.AppSettings["MagayaUsername"];
            //var magayapassword = ConfigurationManager.AppSettings["MagayaPassword"]; 

            CSSoapServiceSoapClient cs = new CSSoapServiceSoapClient();
            api_session_error result = api_session_error.no_error;
            try
            {
                result = cs.StartSession(user, password, out key);
                //this = cs;
                this.myAccessKey = key;
                return result == api_session_error.no_error;
            }
            catch
            {
                return false;
            }

        }
    }
}
