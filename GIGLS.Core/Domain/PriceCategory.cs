﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class PriceCategory : BaseDomain, IAuditable
    {
        public int PriceCategoryId { get; set; }
        public int CountryId { get; set; }
        public string PriceCategoryName { get; set; }
        public decimal CategoryMinimumWeight { get; set; }
        public decimal PricePerWeight { get; set; }
        public decimal CategoryMinimumPrice { get; set; }
    }
}
