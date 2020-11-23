﻿using System;

namespace GIGLS.Core.DTO
{
    public class ShipmentAndPreShipmentParamDTO 
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
