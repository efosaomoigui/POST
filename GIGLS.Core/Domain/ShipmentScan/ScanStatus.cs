﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.ShipmentScan
{
    public class ScanStatus : BaseDomain, IAuditable
    {
        public int ScanStatusId { get; set; }

        [Index(IsUnique = true)]
        public string Code { get; set; }

        public string Incident { get; set; }
        public string ScanReason { get; set; }
        public string Comment { get; set; }
    }
}
