using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Utility
{
    public class GlobalProperty : BaseDomain, IAuditable
    {
        public int GlobalPropertyId { get; set; }

        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(500)]
        public string Value { get; set; } = "";
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CountryId { get; set; }
    }
}
