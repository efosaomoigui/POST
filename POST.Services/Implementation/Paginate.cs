using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using POST.Core.DTO.Captains;
using POST.Core.DTO.Pagination;
using POST.Infrastructure;
using Newtonsoft.Json;

namespace POST.Services.Implementation
{
    public class Paginate
    {
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPrev { get; private set; }
        public bool HasNext { get; private set; }

        public ViewCaptainPagingDto PaginateData(IReadOnlyList<ViewCaptainsDTO> captainDetails, int currentPage, int pageSize = 25)
        {
            TotalCount = captainDetails.Count;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            HasPrev = CurrentPage > 1;
            HasNext = CurrentPage < TotalPages;

            if (currentPage > TotalPages)
            {
                throw new GenericException($"Required page {currentPage} is outside the available number of pages. Total pages available: {TotalPages}");
            }

            var result = captainDetails.OrderByDescending(x => x.EmploymentDate)
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var captainsPaginated = new ViewCaptainPagingDto
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

        public ViewVehiclePagingDto PaginateVehicles(IList<VehicleDTO> vehicleDetails, int currentPage, int pageSize = 25)
        {
            TotalCount = vehicleDetails.Count;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            HasPrev = CurrentPage > 1;
            HasNext = CurrentPage < TotalPages;

            if (currentPage > TotalPages)
            {
                throw new GenericException($"Required page {currentPage} is outside the available number of pages. Total pages available: {TotalPages}");
            }

            var result = vehicleDetails.OrderByDescending(x => x.FleetId)
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vehiclesPaginated = new ViewVehiclePagingDto
            {
                Vehicles = result,
                TotalCount = TotalCount,
                CurrentPage = currentPage,
                TotalPages = TotalPages,
                NextPage = HasNext,
                PrevPage = HasPrev,
                PageSize = TotalPages
            };

            // header settings
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(vehiclesPaginated));

            return vehiclesPaginated;
        }
    }
}
