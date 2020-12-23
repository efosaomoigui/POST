using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Stores;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.DTO.Report;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentPackagePriceService : IShipmentPackagePriceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ShipmentPackagePriceService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddShipmentPackagePrice(ShipmentPackagePriceDTO shipmentPackagePriceDto)
        {
            try
            {
                var newshipmentPackagePrice = Mapper.Map<ShipmentPackagePrice>(shipmentPackagePriceDto);
                _uow.ShipmentPackagePrice.Add(newshipmentPackagePrice);
                await _uow.CompleteAsync();
                return new { Id = newshipmentPackagePrice.ShipmentPackagePriceId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteShipmentPackagePrice(int shipmentPackagePriceId)
        {
            try
            {
                var shipmentPackagePrice = await _uow.ShipmentPackagePrice.GetAsync(shipmentPackagePriceId);
                if (shipmentPackagePrice == null)
                {
                    throw new GenericException("shipment Package Price does not exist");
                }
                _uow.ShipmentPackagePrice.Remove(shipmentPackagePrice);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShipmentPackagePriceDTO> GetShipmentPackagePriceById(int shipmentPackagePriceId)
        {
            try
            {
                var shipmentPackagePrice = await _uow.ShipmentPackagePrice.GetShipmentPackagePriceById(shipmentPackagePriceId);
                if (shipmentPackagePrice == null)
                {
                    throw new GenericException("shipment Package Price does not exist");
                }

                return shipmentPackagePrice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices()
        {
            var shipmentPackagePrices = await _uow.ShipmentPackagePrice.GetShipmentPackagePrices();
            return shipmentPackagePrices;
        }

        public async Task<List<NewShipmentPackagePriceDTO>> GetPreciseShipmentPackagePrices()
        {
            var shipmentPackagePrices = await _uow.ShipmentPackagePrice.GetShipmentPackagePrices();
            var newShipmentPackagePrices = Mapper.Map<List<NewShipmentPackagePriceDTO>>(shipmentPackagePrices);
            return newShipmentPackagePrices;
        }

        public async Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePriceByCountry()
        {
            var countryIds = await _userService.GetUserActiveCountryId();
            var shipmentPackagePrices = await _uow.ShipmentPackagePrice.GetShipmentPackagePriceByCountry(countryIds);
            return shipmentPackagePrices;
        }

        public async Task UpdateShipmentPackagePrice(int shipmentPackagePriceId, ShipmentPackagePriceDTO shipmentPackagePriceDto)
        {
            try
            {
                var shipmentPackagePrice = await _uow.ShipmentPackagePrice.GetAsync(shipmentPackagePriceId);
                if (shipmentPackagePrice == null || shipmentPackagePriceDto.ShipmentPackagePriceId != shipmentPackagePriceId)
                {
                    throw new GenericException("shipment Package Price does not exist");
                }

                shipmentPackagePrice.Description = shipmentPackagePriceDto.Description;
                shipmentPackagePrice.Price = shipmentPackagePriceDto.Price;
                shipmentPackagePrice.CountryId = shipmentPackagePriceDto.CountryId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateShipmentPackageQuantity(int shipmentPackagePriceId, ShipmentPackagePriceDTO shipmentPackagePriceDto)
        {
            try
            {
                var shipmentPackagePrice = await _uow.ShipmentPackagePrice.GetAsync(shipmentPackagePriceId);
                if (shipmentPackagePrice == null || shipmentPackagePriceDto.ShipmentPackagePriceId != shipmentPackagePriceId)
                {
                    throw new GenericException("Shipment Package does not exist");
                }

                var user = await _userService.GetCurrentUserId();
                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
                var currentServiceCenterId = serviceCenterIds[0];
                
                shipmentPackagePrice.InventoryOnHand += shipmentPackagePriceDto.QuantityToBeAdded;
                shipmentPackagePrice.MinimunRequired = shipmentPackagePriceDto.MinimunRequired;

                var newInflow = new ShipmentPackagingTransactions
                {
                    ShipmentPackageId = shipmentPackagePrice.ShipmentPackagePriceId,
                    Quantity = shipmentPackagePriceDto.QuantityToBeAdded,
                    UserId = user,
                    ServiceCenterId = currentServiceCenterId,
                    PackageTransactionType = Core.Enums.PackageTransactionType.InflowToStore
                };

                _uow.ShipmentPackagingTransactions.Add(newInflow);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Inventory Management
        public async Task<object> AddShipmentPackage(ShipmentPackagePriceDTO shipmentPackagePriceDto)
        {
            try
            {
                var package = await _uow.ShipmentPackagePrice.GetAsync(x => x.Description.ToLower().Trim() == shipmentPackagePriceDto.Description.ToLower().Trim() && x.CountryId == shipmentPackagePriceDto.CountryId);
                if (package != null)
                {
                    throw new GenericException("Package information already exists");
                }

                var user = await _userService.GetCurrentUserId();
                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
                var currentServiceCenterId = serviceCenterIds[0];

                var newshipmentPackagePrice = new ShipmentPackagePrice
                {
                    Description = shipmentPackagePriceDto.Description.ToUpper(),
                    CountryId = shipmentPackagePriceDto.CountryId,
                    InventoryOnHand = shipmentPackagePriceDto.Balance,
                    MinimunRequired = shipmentPackagePriceDto.MinimunRequired,
                };

                _uow.ShipmentPackagePrice.Add(newshipmentPackagePrice);
                
                await _uow.CompleteAsync();

                var newInflow = new ShipmentPackagingTransactions
                {
                    ShipmentPackageId = newshipmentPackagePrice.ShipmentPackagePriceId,
                    Quantity = shipmentPackagePriceDto.Balance,
                    UserId = user,
                    ServiceCenterId = currentServiceCenterId,
                    PackageTransactionType = Core.Enums.PackageTransactionType.InflowToStore
                };

                _uow.ShipmentPackagingTransactions.Add(newInflow);
                await _uow.CompleteAsync();

                return new { Id = newshipmentPackagePrice.ShipmentPackagePriceId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentPackagingTransactionsDTO>> GetShipmentPackageTransactions(BankDepositFilterCriteria filterCriteria)
        {
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var shipmentPackageTransactions = await _uow.ShipmentPackagingTransactions.GetShipmentPackageTransactions(filterCriteria,serviceCenters);
            return shipmentPackageTransactions;
        }

        public async Task<List<ServiceCenterPackageDTO>> GetShipmentPackageForServiceCenter()
        {
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var shipmentPackages = await _uow.ServiceCenterPackage.GetShipmentPackageForServiceCenter(serviceCenters);
            return shipmentPackages;
        }

    }
}
