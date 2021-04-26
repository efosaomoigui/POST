using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface ICaptainBonusByZoneMapingService : IServiceDependencyMarker
    {
        Task<IEnumerable<CaptainBonusByZoneMapingDTO>> GetCaptainBonusByZoneMapping();
        Task<CaptainBonusByZoneMapingDTO> GetCaptainBonusByZoneMappingById(int zoneMappingId);
        Task<decimal> GetCaptainBonusByZoneMappingByZone(int departureId, int destinationId);
        Task<object> AddCaptainBonusByZoneMapping(CaptainBonusByZoneMapingDTO mappingDto);
        Task UpdateCaptainBonusByZoneMapping(int zoneMappingId, CaptainBonusByZoneMapingDTO mappingDto);
        Task DeleteCaptainBonusByZoneMapping(int zoneMappingId);
    }
}
