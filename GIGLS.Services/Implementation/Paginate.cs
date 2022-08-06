using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GIGLS.Core.DTO.Captains;
using GIGLS.Core.DTO.Pagination;
using Newtonsoft.Json;

namespace GIGLS.Services.Implementation
{
    public class Paginate
    {
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPrev { get; private set; }
        public bool HasNext { get; private set; }

        public PagingDto PaginateData(IReadOnlyList<CaptainDetailsDTO> captainDetails, int currentPage, int pageSize = 25)
        {
            TotalCount = captainDetails.Count;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            HasPrev = CurrentPage > 1;
            HasNext = CurrentPage < TotalPages;

        var result = captainDetails.OrderBy(x => x.PartnerId)
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var captainsPaginated = new PagingDto
            {
                Captains = result,
                TotalCount = TotalCount,
                CurrentPage = currentPage,
                TotalPages = TotalPages,
                NextPage = HasNext,
                PrevPage = HasPrev,
                PageSize = TotalPages
            };

            // header settings
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(captainsPaginated));

            return captainsPaginated;
        }
    }
}
