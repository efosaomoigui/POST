﻿using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentRepository : IRepository<Shipment>
    {
        Task<Tuple<List<ShipmentDTO>, int>> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<ShipmentDTO>, int>> GetDestinationShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<ShipmentDTO>, int>> GetShipmentDetailByWaybills(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds, List<string> waybills);
        Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria queryDto, int[] serviceCentreIds);
        Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds);
        Task<List<ShipmentDTO>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria);
        IQueryable<Shipment> ShipmentsAsQueryable();
        Task<ShipmentDTO> GetBasicShipmentDetail(string waybill);
        Task<List<InvoiceViewDTO>> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Tuple<Task<List<IntlShipmentRequestDTO>>, int> GetIntlTransactionShipmentRequest(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
    }

    public interface IIntlShipmentRequestRepository : IRepository<IntlShipmentRequest>  
    {
        Task<List<IntlShipmentRequestDTO>> GetShipments(int[] serviceCentreIds);
        Task<Tuple<List<IntlShipmentRequestDTO>, int>> GetIntlTransactionShipmentRequest(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
    }
}
