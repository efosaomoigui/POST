using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Captains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Core.IRepositories
{
    public interface ICaptainRepository : IRepository<Partner>
    {
        Task<IReadOnlyList<Partner>> GetAllCaptainsAsync();
        Task<IReadOnlyList<ViewCaptainsDTO>> GetAllCaptainsByDateAsync(DateTime? date);
        Task<IReadOnlyList<VehicleDTO>> GetAllVehiclesByDateAsync(DateTime? date);
        Task<Partner> GetCaptainByIdAsync(int partnerId);
        Task<VehicleDetailsDTO> GetVehicleByRegistrationNumberAsync(string regNum);
        Task<List<VehicleDTO>> GetAllVehiclesByDateRangeAsync(DateFilterCriteria filter);
        Task<List<ViewCaptainsDTO>> GetAllCaptainsByDateRangeAsync(DateFilterCriteria filter);
        Task<IList<VehicleDTO>> GetAllVehiclesAsync();
    }
}
