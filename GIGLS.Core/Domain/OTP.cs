namespace GIGLS.Core.Domain
{
    public class OTP : BaseDomain, IAuditable
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int Otp { get; set; }
        public bool IsValid { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
