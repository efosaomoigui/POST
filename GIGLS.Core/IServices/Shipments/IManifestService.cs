using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestService : IServiceDependencyMarker
    {
        Task<List<ManifestDTO>> GetManifests();
        Task<ManifestDTO> GetManifestById(int manifestId);
        Task<ManifestDTO> GetManifestByCode(string manifest);
        Task<object> AddManifest(ManifestDTO manifest);
        Task UpdateManifest(int manifestId, ManifestDTO manifest);
        Task DeleteManifest(int manifestId);
        Task<string> GenerateManifestCode(ManifestDTO manifestDTO);
        Task<Manifest> GetManifestCodeForScan(string manifestCode);
        Task<string> GenerateMovementManifestCode(MovementManifestNumberDTO manifestDTO); 
        Task<PickupManifestDTO> GetPickupManifestByCode(string manifest);
        Task ChangeManifestType(string manifestCode);
        Task<bool> SignOffManifest(string manifestNumber);
    }
}
