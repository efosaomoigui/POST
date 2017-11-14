using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
