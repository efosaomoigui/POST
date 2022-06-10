using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class InboundCategory : BaseDomain
    {
        [Key]
        public string CategoryId { get; set; } = Guid.NewGuid().ToString();
        public string CategoryName { get; set; }
        public bool IsGoStandard { get; set; }
        public bool IsGoFaster { get; set; }
    }
}
