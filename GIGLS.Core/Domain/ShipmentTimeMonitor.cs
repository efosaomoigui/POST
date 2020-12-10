using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class ShipmentTimeMonitor : BaseDomain, IAuditable
    {
        [Key]
        public int ShipmentTimeMonitorId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        [MaxLength(128)]
        public string UserName { get; set; }

        public int TimeInSeconds { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public int UserServiceCentreId { get; set; }
        [MaxLength(128)]
        public string UserServiceCentreName { get; set; }

    }
}
