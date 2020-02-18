using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class Bank : BaseDomain, IAuditable
    {
        public int BankId { get; set; }

        [MaxLength(100), MinLength(2)]
        [Index(IsUnique = true)]
        public string BankName { get; set; }

    }
}

