﻿using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class PaymentMethodDTO 
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsActive { get; set; }
    }
}