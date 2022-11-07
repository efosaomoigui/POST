using System;
using System.Threading.Tasks;
using POST.Core.DTO.Shipments;
using POST.Core.IServices.Shipments;
using POST.Core;

namespace POST.Services.Implementation.Shipments
{
    public class ShipmentPackageService : IShipmentPackageService
    {
        private readonly IUnitOfWork _uow;

        public ShipmentPackageService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<object> AddShipmentPackage(ShipmentItemDTO package)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task DeleteShipmentPackage(int packageId)
        {
            throw new NotImplementedException();
        }

        public Task<ShipmentItemDTO> GetShipmentPackageById(int packageId)
        {
            throw new NotImplementedException();
        }

        public Task<ShipmentItemDTO> GetShipmentPackages()
        {
            throw new NotImplementedException();
        }

        public Task UpdateShipmentPackage(int packageId, ShipmentItemDTO package)
        {
            throw new NotImplementedException();
        }
    }
}
