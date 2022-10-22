using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core.DTO.Captains;

namespace POST.Core.DTO.Pagination
{
    public class ViewCaptainPagingDto
    {
        public List<ViewCaptainsDTO> Captains { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 25;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool PrevPage { get; set; }
        public bool NextPage { get; set; }
    }

    public class ViewVehiclePagingDto
    {
        public List<VehicleDTO> Vehicles { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 25;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool PrevPage { get; set; }
        public bool NextPage { get; set; }
    }
}
