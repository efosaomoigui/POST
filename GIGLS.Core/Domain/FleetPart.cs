using GIGLS.Core;
using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class FleetPart : BaseDomain, IAuditable
    {
        [Key]
        public int PartId { get; set; }
        public string PartName { get; set; }

        public int ModelId { get; set; }
        public virtual FleetModel Model { get; set; }

    }
}