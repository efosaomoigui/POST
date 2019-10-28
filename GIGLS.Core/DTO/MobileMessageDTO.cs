using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class MobileMessageDTO
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string WaybillNumber { get; set; }
        public string SenderPhoneNumber { get; set; }

        public int OTP { get; set; }
        public string ExpectedTimeofDelivery { get; set; }
       
    }
}
