﻿using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class PreShipmentItemMobileDTO : BaseDomainDTO
    {
        public int PreShipmentItemMobileId { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ItemType { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }
        public decimal EstimatedPrice { get; set; }

        public string Value { get; set; }

        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; }

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }

        //Foreign key information
        public int PreShipmentMobileId { get; set; }
        public PreShipmentMobileDTO PreShipmentMobile { get; set; }

        //Agility Calculations
        public decimal? CalculatedPrice { get; set; }
    }
}