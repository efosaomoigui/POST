using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentPackagePriceService : IShipmentPackagePriceService
    {
        private readonly IUnitOfWork _uow;

        public ShipmentPackagePriceService(IUnitOfWork uow)
        {
            _uow = uow;
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
                var shipmentPackagePrice = await _uow.ShipmentPackagePrice.GetAsync(shipmentPackagePriceId);
                if (shipmentPackagePrice == null)
                {
                    throw new GenericException("shipment Package Price does not exist");
                }

                var shipmentPackagePriceDto = Mapper.Map<ShipmentPackagePriceDTO>(shipmentPackagePrice);
                return shipmentPackagePriceDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<ShipmentPackagePriceDTO>> GetShipmentPackagePrices()
        {
            var shipmentPackagePrices = _uow.ShipmentPackagePrice.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<ShipmentPackagePriceDTO>>(shipmentPackagePrices));
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
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
