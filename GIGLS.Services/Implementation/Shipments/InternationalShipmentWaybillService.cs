using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.DTO.DHL;
using GIGLS.Core.IServices.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
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
    }
}
