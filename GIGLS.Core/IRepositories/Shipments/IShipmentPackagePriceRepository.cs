using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IShipmentPackagePriceRepository : IRepository<ShipmentPackagePrice> 
    {
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices();
        Task<ShipmentPackagePriceDTO> GetShipmentPackagePriceById(int shipmentPackagePriceId);
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePriceByCountry(int countryId);
    }
}