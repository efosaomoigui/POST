using POST.Core.Domain.Partnership;
using POST.Core.DTO.Captains;
using POST.Core.DTO.Fleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core.DTO.Pagination;
using POST.CORE.DTO.Report;

namespace POST.Core.IServices
{
    public interface ICaptainService : IServiceDependencyMarker
    {
        Task DeleteCaptainByIdAsync(int captainId);
        Task<IReadOnlyList<ViewCaptainsDTO>> GetCaptainsByDateAsync(DateTime? date);
        Task<object> RegisterCaptainAsync(RegCaptainDTO captainDTO);
        Task<object> GetCaptainByIdAsync(int partnerId);
        Task EditCaptainAsync(UpdateCaptainDTO partner);
        Task<bool> RegisterVehicleAsync(RegisterVehicleDTO vehicleDTO);
        Task<IReadOnlyList<CaptainDetailsDTO>> GetAllCaptainsAsync();
        Task<IReadOnlyList<VehicleDTO>> GetVehiclesByDateAsync(DateTime? date);
        Task<object> GetVehicleByIdAsync(int fleetId);
        Task<bool> DeleteVehicleByIdAsync(int fleetId);
        Task<bool> EditVehicleAsync(VehicleDetailsDTO vehicle);
        Task<IReadOnlyList<VehicleDetailsDTO>> GetAllVehiclesAsync();
        Task<VehicleDetailsDTO> GetVehicleByRegistrationNumberAsync(string regNum);
        Task<VehicleAnalyticsDto> GetVehicleAnalyticsAsync(string vehicleNumber);
        Task<List<VehicleDTO>> GetVehiclesByDateRangeAsync(DateFilterCriteria filter);
        Task<bool> RegisterVehicleInRangeAsync(List<RegisterVehicleDTO> vehicleDtos);
        Task<object> RegisterCaptainsInRangeAsync(List<RegCaptainDTO> captainDto);
        Task<IReadOnlyList<ViewCaptainsDTO>> GetCaptainsByDateRangeAsync(DateFilterCriteria filter);
        Task<ViewCaptainPagingDto> GetAllCaptainsPaginatedAsync(int currentPage, int pageSize);
        Task<ViewVehiclePagingDto> GetAllVehiclesPaginatedAsync(int currentPage, int pageSize);
    }
}
