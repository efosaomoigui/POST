using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class UserLoginEmail : BaseDomain, IAuditable
    {
        [Key]
        public int UserLoginEmailId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public DateTime DateLastSent { get; set; }
        public int NumberOfEmailsSent { get; set; }
    }
}
