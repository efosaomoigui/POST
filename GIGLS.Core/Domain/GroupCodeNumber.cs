using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class GroupCodeNumber : BaseDomain, IAuditable
    {
        public int GroupCodeNumberId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string GroupCode { get; set; }
        public bool IsActive { get; set; }
    }
}