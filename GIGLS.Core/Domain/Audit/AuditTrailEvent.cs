using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Audit
{
    public class AuditTrailEvent
    {
        [Key]
        public int EventId { get; set; }
        public Nullable<DateTime> InsertedDate { get; set; }
        public Nullable<DateTime> LastUpdatedDate { get; set; }
        public string Data { get; set; }
    }
}
