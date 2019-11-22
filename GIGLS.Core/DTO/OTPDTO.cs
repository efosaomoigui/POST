namespace GIGLS.Core.DTO
{
    public class OTPDTO
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public int Otp { get; set; }
        public bool IsValid { get; set; }

        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
