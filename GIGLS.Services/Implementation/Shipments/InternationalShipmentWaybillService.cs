using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.DTO.DHL;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class InternationalShipmentWaybillService : IInternationalShipmentWaybillService
    {
        private readonly IUnitOfWork _uow;
        public InternationalShipmentWaybillService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<InternationalShipmentWaybillDTO> GetInternationalWaybill(string waybill)
        {
            try
            {
                var log = await _uow.InternationalShipmentWaybill.GetAsync(x => x.Waybill == waybill);
                if (log == null)
                {
                    throw new GenericException("Information does not exist");
                }

                var logDto = Mapper.Map<InternationalShipmentWaybillDTO>(log);
                return logDto;
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
            await _uow.CompleteAsync();
            return true;
        }
    }
}
