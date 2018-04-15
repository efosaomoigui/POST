using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.Services.Implementation.Shipments
{
    public class TransitWaybillNumberService : ITransitWaybillNumberService
    {
        private readonly IUnitOfWork _uow;

        public TransitWaybillNumberService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddTransitWaybillNumber(TransitWaybillNumberDTO transitWaybillNumberDto)
        {
            try
            {
                if (await _uow.TransitWaybillNumber.ExistAsync(c => c.WaybillNumber.ToLower() == transitWaybillNumberDto.WaybillNumber.Trim().ToLower()))
                {
                    throw new GenericException("TransitWaybillNumber already Exist");
                }
                var newTransitWaybillNumber = Mapper.Map<TransitWaybillNumber>(transitWaybillNumberDto);
                _uow.TransitWaybillNumber.Add(newTransitWaybillNumber);
                await _uow.CompleteAsync();
                return new { Id = newTransitWaybillNumber.TransitWaybillNumberId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteTransitWaybillNumber(int transitWaybillNumberId)
        {
            try
            {
                var transitWaybillNumber = await _uow.TransitWaybillNumber.GetAsync(transitWaybillNumberId);
                if (transitWaybillNumber == null)
                {
                    throw new GenericException("TransitWaybillNumber does not exist");
                }
                _uow.TransitWaybillNumber.Remove(transitWaybillNumber);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<TransitWaybillNumberDTO>> GetTransitWaybillNumbers()
        {
            var transitWaybillNumbers = _uow.TransitWaybillNumber.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<TransitWaybillNumberDTO>>(transitWaybillNumbers));

        }

        public async Task<TransitWaybillNumberDTO> GetTransitWaybillNumberById(int transitWaybillNumberId)
        {
            try
            {
                var transitWaybillNumber = await _uow.TransitWaybillNumber.GetAsync(transitWaybillNumberId);
                if (transitWaybillNumber == null)
                {
                    throw new GenericException("TransitWaybillNumber Not Exist");
                }

                var transitWaybillNumberDto = Mapper.Map<TransitWaybillNumberDTO>(transitWaybillNumber);
                return transitWaybillNumberDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TransitWaybillNumberDTO> GetTransitWaybillNumberByCode(string waybillNumber)
        {
            try
            {
                var transitWaybillNumber = await _uow.TransitWaybillNumber.GetAsync(s => s.WaybillNumber == waybillNumber);
                if (transitWaybillNumber == null)
                {
                    throw new GenericException("TransitWaybillNumber Not Exist");
                }

                var transitWaybillNumberDto = Mapper.Map<TransitWaybillNumberDTO>(transitWaybillNumber);
                return transitWaybillNumberDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task UpdateTransitWaybillNumber(int transitWaybillNumberId, TransitWaybillNumberDTO transitWaybillNumberDto)
        {
            try
            {
                var transitWaybillNumber = await _uow.TransitWaybillNumber.GetAsync(transitWaybillNumberId);
                if (transitWaybillNumber == null || transitWaybillNumberDto.TransitWaybillNumberId != transitWaybillNumberId)
                {
                    throw new GenericException("TransitWaybillNumber information does not exist");
                }
                transitWaybillNumber.ServiceCentreId = transitWaybillNumberDto.ServiceCentreId;
                transitWaybillNumber.IsGrouped = transitWaybillNumberDto.IsGrouped;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
