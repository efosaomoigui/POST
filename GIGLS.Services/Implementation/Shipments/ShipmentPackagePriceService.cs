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

                shipmentPackagePrice.Balance += shipmentPackagePriceDto.QuantityToBeAdded;
               
                _uow.Complete();
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
                var package = await _uow.ShipmentPackagePrice.GetAsync(x => x.Description.ToLower().Trim() == shipmentPackagePriceDto.Description.ToLower().Trim());
                if (package != null)
                {
                    throw new GenericException("Package information already exists");
                }
               
                var newshipmentPackagePrice = new ShipmentPackagePrice
                {
                    Description = shipmentPackagePriceDto.Description.ToUpper(),
                    Balance = shipmentPackagePriceDto.Balance,
                    CountryId = shipmentPackagePriceDto.CountryId,
                };

                _uow.ShipmentPackagePrice.Add(newshipmentPackagePrice);
                await _uow.CompleteAsync();
                return new { Id = newshipmentPackagePrice.ShipmentPackagePriceId };
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
