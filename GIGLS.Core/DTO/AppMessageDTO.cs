using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class AppMessageDTO
    {
        public string AppType { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }
        public UserDetailsDTO UserDetails { get; set; }
        public string ScreenShots1 { get; set; }
        public string ScreenShots2 { get; set; }
        public string ScreenShots3 { get; set; }
    }
    public class UserDetailsDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
