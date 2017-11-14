using GIGLS.Core;
using GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class FleetMake : BaseDomain, IAuditable
    {
        public FleetMake()
        {
            FleetModels = new HashSet<FleetModel>();
            Fleets = new HashSet<Fleet>();
        }

        [Key]
        public int MakeId { get; set; }
        public string MakeName { get; set; }

        public virtual ICollection<FleetModel> FleetModels { get; set; }
        public virtual ICollection<Fleet> Fleets { get; set; }
    }
}