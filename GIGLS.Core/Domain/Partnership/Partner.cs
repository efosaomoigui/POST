using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Partnership
{
    public class Partner : BaseDomain, IAuditable
    {
        public int PartnerId { get; set; }

        public string PartnerName { get; set; }

        [MaxLength(100)]
        public string PartnerCode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string OptionalPhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual PartnerType PartnerType { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public bool IsActivated { get; set; }
        [MaxLength(100)]
        public string VehicleType { get; set; }

        public string PictureUrl { get; set; }

        [MaxLength(100)]
        public string BankName { get; set; }
        [MaxLength(100)]
        public string AccountNumber { get; set; }

        [MaxLength(100)]
        public string AccountName { get; set; }

        // Foreign Key
       // public int PartnerApplicationId { get; set; }
        //public virtual PartnerApplication PartnerApplication { get; set; }

       // public int WalletId { get; set; }
      //  public virtual Wallet.Wallet Wallet { get; set; }
    }
}
