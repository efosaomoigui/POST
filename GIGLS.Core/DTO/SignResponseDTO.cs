using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class SignResponseDTO
    {
        public bool PhoneSent { get; set; }
        public bool EmailSent { get; set; }

    }

    public class ResponseDTO
    {
        public bool Succeeded { get; set; } = false;
        public bool Exist { get; set; } = false;
        public string Message { get; set; }
        public object Entity { get; set; }
    }

    public class BVNVerificationDTO
    {
        public bool Status { get; set; } 
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string First_Nmae { get; set; }
        public string Last_Name { get; set; }
        public string DOB { get; set; }
        public string mobile { get; set; }
        public string BVN { get; set; }
    }
}
