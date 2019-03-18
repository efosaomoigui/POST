using System;

namespace GIGLS.Core.DTO
{
    public class UserLoginEmailDTO
    {
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastSent { get; set; }
        public int NumberOfEmailsSent { get; set; }
    }
}
