using GIGLS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class ClientNode : BaseDomain, IAuditable
    {
        [Key]
        [MaxLength(32)]
        public string ClientNodeId { get; set; }

        [MaxLength(80)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public bool Status { get; set; }
    }
}
