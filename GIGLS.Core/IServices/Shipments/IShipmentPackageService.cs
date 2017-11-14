using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentPackageService : IServiceDependencyMarker
    {
        Task<ShipmentItemDTO> GetShipmentPackages();
        Task<ShipmentItemDTO> GetShipmentPackageById(int packageId);
        Task<object> AddShipmentPackage(ShipmentItemDTO package);
        Task UpdateShipmentPackage(int packageId, ShipmentItemDTO package);
        Task DeleteShipmentPackage(int packageId);
    }
}
