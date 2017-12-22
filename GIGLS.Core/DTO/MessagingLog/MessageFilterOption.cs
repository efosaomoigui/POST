using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.DTO.MessagingLog
{
    public class MessageFilterOption
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public MessagingLogStatus? Status { get; set; }
    }
}
