﻿using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentPackagePriceRepository : IRepository<ShipmentPackagePrice> 
    {
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices();
        Task<ShipmentPackagePriceDTO> GetShipmentPackagePriceById(int shipmentPackagePriceId);
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePriceByCountry(int countryId);
    }
}