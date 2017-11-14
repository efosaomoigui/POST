using GIGLS.Core.Config;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessage.Config
{
    public class EmailConfig : IMessageConfig
    {
        public string SmtpServerHost
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServer"];
            }
        }
        public int SmtpPort
        {
            get
            {
                var port = ConfigurationManager.AppSettings["SmtpPort"];
                return Convert.ToInt32(port);
            }
        }
        public string SenderEmail
        {
            get{ return ConfigurationManager.AppSettings["SenderEmail"]; }
        }
        public string SenderPassword
        {
            get{ return ConfigurationManager.AppSettings["SenderPassword"]; }
        }
    }

}
