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
}
