using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Captains;
using GIGLS.Core.DTO.Fleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Core.IServices
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
    }
}
