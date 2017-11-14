using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;

namespace GIGLS.Services.Implementation.Shipments
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
