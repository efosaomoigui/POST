using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class Region : BaseDomain, IAuditable
    {
        public Region()
        {
        }

        public int RegionId { get; set; }

        [MaxLength(100), MinLength(2)]
        [Index(IsUnique = true)]
        public string RegionName { get; set; }

    }
}
