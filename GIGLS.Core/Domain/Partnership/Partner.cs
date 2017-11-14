using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain.Partnership
{
    public class Partner : BaseDomain, IAuditable
    {
        public int PartnerId { get; set; }

        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OptionalPhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual PartnerType PartnerType { get; set; }

        // Foreign Key
       // public int PartnerApplicationId { get; set; }
        //public virtual PartnerApplication PartnerApplication { get; set; }

       // public int WalletId { get; set; }
      //  public virtual Wallet.Wallet Wallet { get; set; }
    }
}
