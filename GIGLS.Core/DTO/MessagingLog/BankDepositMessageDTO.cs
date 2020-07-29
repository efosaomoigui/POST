using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.MessagingLog
{
    public class BankDepositMessageDTO
    {
        public string DepositorName { get; set; }
        public string ServiceCenter { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountInputted { get; set; }
        public string Email { get; set; }
    }
}
