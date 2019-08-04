using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class OTP : BaseDomain, IAuditable
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string CustomerId { get; set; }
        public int Otp { get; set; }
        public bool IsValid { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string EmailAddress { get; set; }
    }
}
