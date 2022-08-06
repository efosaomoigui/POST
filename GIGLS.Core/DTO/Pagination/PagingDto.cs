using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Captains;

namespace GIGLS.Core.DTO.Pagination
{
    public class PagingDto
    {
        public List<CaptainDetailsDTO> Captains { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 25;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool PrevPage { get; set; }
        public bool NextPage { get; set; }
    }
}
