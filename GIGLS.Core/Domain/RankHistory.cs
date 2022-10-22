using POST.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POST.Core.Domain
{
    public class RankHistory : BaseDomain, IAuditable
    {
        public int RankHistoryId { get; set; }
        [MaxLength(100), MinLength(2)]
        public string CustomerName { get; set; }
        [MaxLength(100), MinLength(2)]
        public string CustomerCode { get; set; }
        public RankType RankType { get; set; }

    }
}

