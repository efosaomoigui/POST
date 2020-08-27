using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Stores;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentPackagePriceService : IServiceDependencyMarker
    {
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices();
        Task<ShipmentPackagePriceDTO> GetShipmentPackagePriceById(int shipmentPackagePriceId);
        Task<object> AddShipmentPackagePrice(ShipmentPackagePriceDTO shipmentPackagePriceDto);
        Task UpdateShipmentPackagePrice(int shipmentPackagePriceId, ShipmentPackagePriceDTO shipmentPackagePriceDto);
        Task DeleteShipmentPackagePrice(int shipmentPackagePriceId);
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePriceByCountry();
        Task UpdateShipmentPackageQuantity(int shipmentPackagePriceId, ShipmentPackagePriceDTO shipmentPackagePriceDto);
        Task<object> AddShipmentPackage(ShipmentPackagePriceDTO shipmentPackagePriceDto);
        Task<List<ShipmentPackagingTransactionsDTO>> GetShipmentPackageTransactions(BaseFilterCriteria filterCriteria);
        Task<List<ServiceCenterPackageDTO>> GetShipmentPackageForServiceCenter();
    }
}