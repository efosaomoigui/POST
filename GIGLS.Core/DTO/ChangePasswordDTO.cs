using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
