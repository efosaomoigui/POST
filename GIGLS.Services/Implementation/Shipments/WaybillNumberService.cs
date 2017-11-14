using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Implementation.Shipments
{
    public class WaybillNumberService : IWaybillNumberService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _service;

        public WaybillNumberService(IUnitOfWork uow, INumberGeneratorMonitorService service)
        {
            _uow = uow;
            _service = service;
            MapperConfig.Initialize();
        }


        //This methosd add Waybill Number to Waybill Number Table
        public async Task<string> AddWaybillNumber(string serviceCentreCode, string user, int servicecentre)
        {
            try
            {
                var waybill = await _service.GenerateNextNumber(NumberGeneratorType.WaybillNumber, serviceCentreCode);

                var newWaybill = new WaybillNumber
                {
                    WaybillCode = waybill,
                    UserId = user,
                    ServiceCentreId = servicecentre,
                    IsActive = true
                };

                _uow.WaybillNumber.Add(newWaybill);
                await _uow.CompleteAsync();
                return waybill;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //This is the method to call to get a waybill Number. It return waybill number to who call it
        public async Task<string> GenerateWaybillNumber(string serviceCentreCode, string userId, int servicecentreId)
        {
            var waybill = await AddWaybillNumber(serviceCentreCode, userId, servicecentreId);
            return waybill;
        }

        public async Task<IEnumerable<WaybillNumberDTO>> GetAllWayBillNumbers()
        {
            try
            {
                return await _uow.WaybillNumber.GetWaybills();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WaybillNumberDTO>> GetActiveWayBillNumbers()
        {
            try
            {
                var waybill = await _uow.WaybillNumber.FindAsync(x => x.IsActive == true, "ServiceCentre");

                return waybill.Select(w => new WaybillNumberDTO
                {
                    IsActive = w.IsActive,
                    WaybillCode = w.WaybillCode,
                    WaybillNumberId = w.WaybillNumberId,
                    ServiceCentreId = w.ServiceCentreId,
                    UserId = w.UserId,
                    ServiceCentre = w.ServiceCentre.Name
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WaybillNumberDTO>> GetDeliverWayBillNumbers()
        {
            try
            {
                var waybill = await _uow.WaybillNumber.FindAsync(x => x.IsActive == false, "ServiceCentre");

                return waybill.Select(w => new WaybillNumberDTO
                {
                    IsActive = w.IsActive,
                    WaybillCode = w.WaybillCode,
                    WaybillNumberId = w.WaybillNumberId,
                    ServiceCentreId = w.ServiceCentreId,
                    UserId = w.UserId,
                    ServiceCentre = w.ServiceCentre.Name
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WaybillNumberDTO> GetWayBillNumberById(string waybillNumber)
        {
            try
            {
                var waybill = await _uow.WaybillNumber.GetAsync(x => x.WaybillCode.Equals(waybillNumber), "ServiceCentre");

                if (waybill == null)
                {
                    throw new GenericException("Waybill information does not exist");
                }

                var waybillDto = Mapper.Map<WaybillNumberDTO>(waybill);
                waybillDto.ServiceCentre = waybill.ServiceCentre.Name;
                return waybillDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WaybillNumberDTO> GetWayBillNumberById(int waybillId)
        {
            try
            {
                var waybill = await _uow.WaybillNumber.GetAsync(x => x.WaybillNumberId == waybillId, "ServiceCentre");

                if (waybill == null)
                {
                    throw new GenericException("Waybill information does not exist");
                }

                var waybillDto = Mapper.Map<WaybillNumberDTO>(waybill);
                waybillDto.ServiceCentre = waybill.ServiceCentre.Name;
                return waybillDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveWaybillNumber(int waybillId)
        {
            try
            {
                var waybill = await _uow.WaybillNumber.GetAsync(waybillId);

                if (waybill == null)
                {
                    throw new GenericException("Waybill Not Exist");
                }
                _uow.WaybillNumber.Remove(waybill);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveWaybillNumber(string waybillId)
        {
            try
            {
                var waybill = _uow.WaybillNumber.SingleOrDefault(x => x.WaybillCode == waybillId);

                if (waybill == null)
                {
                    throw new GenericException("Waybill Not Exist");
                }
                _uow.WaybillNumber.Remove(waybill);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateWaybillNumber(string waybillNumber)
        {
            try
            {
                var waybill = await _uow.WaybillNumber.GetAsync(x => x.WaybillCode == waybillNumber);

                if (waybill == null)
                {
                    throw new GenericException("Waybill Not Exist");
                }
                waybill.IsActive = false;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateWaybillNumber(int waybillId)
        {
            try
            {
                var waybill = await _uow.WaybillNumber.GetAsync(waybillId);

                if (waybill == null)
                {
                    throw new GenericException("Waybill Not Exist");
                }
                waybill.IsActive = false;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
