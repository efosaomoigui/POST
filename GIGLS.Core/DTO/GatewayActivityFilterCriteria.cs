﻿using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class GatewayActivityFilterCriteria : BaseFilterCriteria
    {
        public string UserId { get; set; }
        public int[] ServiceId { get; set; }
    }
}
