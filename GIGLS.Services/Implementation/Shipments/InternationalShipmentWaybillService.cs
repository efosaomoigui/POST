using AutoMapper;
using POST.Core;
using POST.Core.Domain.DHL;
using POST.Core.DTO.DHL;
using POST.Core.Enums;
using POST.Core.IServices.DHL;
using POST.Core.IServices.Shipments;
using POST.CORE.DTO.Report;
using POST.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Shipments
{
    public class InternationalShipmentWaybillService : IInternationalShipmentWaybillService
    {
        private readonly IUnitOfWork _uow;
        private readonly IDHLService _dhlService;
        public InternationalShipmentWaybillService(IUnitOfWork uow, IDHLService dhlService)
        {
            _uow = uow;
            _dhlService = dhlService;
        }

        public async Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybill(string waybill)
        {
            try
            {
                var dataToReturn = new List<InternationalShipmentWaybill>();
                var waybillArr = waybill.Split(',');
                if (waybillArr.Length > 1)
                {
                    foreach (var item in waybillArr)
                    {
                        var result = await _uow.InternationalShipmentWaybill.GetAsync(x => x.Waybill.Trim() == item.Trim() || x.ShipmentIdentificationNumber.Trim() == item.Trim());
                        if (result != null)
                        {
                            dataToReturn.Add(result);
                        }
                    }

                    dataToReturn = dataToReturn.Distinct().ToList();
                }
                else
                {
                    var result = await _uow.InternationalShipmentWaybill.GetAsync(x => x.Waybill == waybill || x.ShipmentIdentificationNumber == waybill);
                    if (result != null)
                    {
                        dataToReturn.Add(result);
                    }
                }
                return Mapper.Map<List<InternationalShipmentWaybillDTO>>(dataToReturn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybills(DateFilterCriteria dateFilterCriteria)
        {
            if (dateFilterCriteria == null)
            {
                dateFilterCriteria = new DateFilterCriteria
                {
                    StartDate = null,
                    EndDate = null
                };
            }
            return await _uow.InternationalShipmentWaybill.GetInternationalWaybills(dateFilterCriteria);
        }

        public async Task<List<InternationalShipmentWaybillDTO>> GetInternationalShipmentOnwardDeliveryWaybills()
        {
            var shipment = await _uow.InternationalShipmentWaybill.FindAsync(x => x.InternationalShipmentStatus == InternationalShipmentStatus.OnwardDelivery);
            return Mapper.Map<List<InternationalShipmentWaybillDTO>>(shipment);
        }

        public async Task<List<InternationalShipmentWaybillDTO>> GetInternationalShipmentArrivedWaybills()
        {
            var shipment = await _uow.InternationalShipmentWaybill.FindAsync(x => x.InternationalShipmentStatus == InternationalShipmentStatus.Arrived);
            return Mapper.Map<List<InternationalShipmentWaybillDTO>>(shipment);
        }

        public async Task<bool> UpdateToEnrouteDelivery(List<string> waybills)
        {
            var isWaybillArrived = _uow.InternationalShipmentWaybill.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();

            if (isWaybillArrived.Count != waybills.Count)
            {
                //check the one that is not available from the list and display it
                var extraWaybillsResult = isWaybillArrived.Select(x => x.Waybill).Distinct().ToList();
                var waybillsNotIncluded = waybills.Where(x => !extraWaybillsResult.Contains(x));
                if (waybillsNotIncluded.Any())
                {
                    throw new GenericException($"Error: The following waybills [{string.Join(", ", waybillsNotIncluded)}] status can not be updated to {InternationalShipmentStatus.OnwardDelivery}");
                }
            }

            //check if those waybill carry Arrived Status
            var isNotArrived = isWaybillArrived.Where(x => x.InternationalShipmentStatus != InternationalShipmentStatus.Arrived);
            if (isNotArrived.Any())
            {
                var isWaybillsMappedActiveResult = isNotArrived.Select(x => x.Waybill).Distinct().ToList();
                throw new GenericException($"Error: The following waybills [{string.Join(", ", isWaybillsMappedActiveResult.ToList())}] status can not be updated to {InternationalShipmentStatus.OnwardDelivery}");
            }

            //update the status
            isWaybillArrived.ForEach(x => x.InternationalShipmentStatus = InternationalShipmentStatus.OnwardDelivery);

            //add tracking history

            await _uow.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateToDelivered(List<string> waybills)
        {
            var isWaybillArrived = _uow.InternationalShipmentWaybill.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();

            if (isWaybillArrived.Count != waybills.Count)
            {
                //check the one that is not available from the list and display it
                var extraWaybillsResult = isWaybillArrived.Select(x => x.Waybill).Distinct().ToList();
                var waybillsNotIncluded = waybills.Where(x => !extraWaybillsResult.Contains(x));
                if (waybillsNotIncluded.Any())
                {
                    throw new GenericException($"Error: The following waybills [{string.Join(", ", waybillsNotIncluded)}] status can not be updated to {InternationalShipmentStatus.Delivered}");
                }
            }

            //check if those waybill carry Arrived or Enroute Delivery Status
            var isNotArrived = isWaybillArrived.Where(x => x.InternationalShipmentStatus != InternationalShipmentStatus.Arrived && x.InternationalShipmentStatus != InternationalShipmentStatus.OnwardDelivery);
            if (isNotArrived.Any())
            {
                var isWaybillsMappedActiveResult = isNotArrived.Select(x => x.Waybill).Distinct().ToList();
                throw new GenericException($"Error: The following waybills [{string.Join(", ", isWaybillsMappedActiveResult.ToList())}] status can not be updated to {InternationalShipmentStatus.Delivered}");
            }

            //update the status
            isWaybillArrived.ForEach(x => x.InternationalShipmentStatus = InternationalShipmentStatus.Delivered);

            //release those shipment  too 
            //add tracking history
            await _uow.CompleteAsync();
            return true;
        }

        public async Task<InternationalShipmentTracking> TrackInternationalShipment(string internationalWaybill)
        {
            var shipment = await _dhlService.TrackInternationalShipment(internationalWaybill);
            return shipment;
        }

    }
}
