using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.User
{
    public class RankHistoryDTO
    {
        public int RankHistoryId { get; set; }
        public RankType RankType { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }

}
