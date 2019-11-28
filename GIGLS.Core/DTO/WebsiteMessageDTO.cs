using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class WebsiteMessageDTO 
    {
        public string senderFullName { get; set; }
        public string senderMail { get; set; }
        public string senderPhone { get; set; }
        public string receiverFullName { get; set; }
        public string receiverMail { get; set; }
        public string receiverPhone { get; set; }
        public string pickupAddress { get; set; }
        public string destAddress { get; set; }
        public string pickupCity { get; set; }
        public string pickupState { get; set; }
        public string pickupZip { get; set; }
        public string DestZip { get; set; }
        public string DestState { get; set; }
        public string DestCity { get; set; }
        public string numberofPieces { get; set; }
        public string weight { get; set; }
        public string dimension { get; set; }
        public string speciaInstruct { get; set; }
        public string packageInfo { get; set; }
        public string gigMail { get; set; }
        public string contactFullName { get; set; }
        public string contactMail { get; set; }
        public string contactPhone { get; set; }



    }
}
