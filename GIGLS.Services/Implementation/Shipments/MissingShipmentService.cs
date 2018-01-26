using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;

namespace GIGLS.Services.Implementation.Shipments
{
    public class MissingShipmentService : IMissingShipmentService
    {
        private readonly IUnitOfWork _uow;
        public MissingShipmentService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddMissingShipment(MissingShipmentDTO missingShipment)
        {
            try
            {
                if (await _uow.MissingShipment.ExistAsync(c => c.Waybill == missingShipment.Waybill))
                {
                    throw new GenericException($"Shipment with waybill: {missingShipment.Waybill} already exist");
                }
                var newMissingShipment = Mapper.Map<MissingShipment>(missingShipment);
                _uow.MissingShipment.Add(newMissingShipment);
                await _uow.CompleteAsync();
                return new { Id = newMissingShipment.MissingShipmentId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteMissingShipment(int missingShipmentId)
        {
            try
            {
                var missingShipment = await _uow.MissingShipment.GetAsync(missingShipmentId);
                if (missingShipment == null)
                {
                    throw new GenericException("Missing Shipment does not exist");
                }
                _uow.MissingShipment.Remove(missingShipment);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MissingShipmentDTO> GetMissingShipmentById(int missingShipmentId)
        {
            try
            {
                var missingShipment = await _uow.MissingShipment.GetAsync(missingShipmentId);
                if (missingShipment == null)
                {
                    throw new GenericException("Missing Shipment does not exist");
                }
                var missingShipmentDTO = Mapper.Map<MissingShipmentDTO>(missingShipment);
                return missingShipmentDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<MissingShipmentDTO>> GetMissingShipments()
        {
            var missingShipment = _uow.MissingShipment.GetAll();
            var missingShipmentDto = Mapper.Map<IEnumerable<MissingShipmentDTO>>(missingShipment);
            return Task.FromResult(missingShipmentDto);
        }

        public async Task UpdateMissingShipment(int missingShipmentId, MissingShipmentDTO missingShipmentDto)
        {
            try
            {
                var missingShipment = await _uow.MissingShipment.GetAsync(missingShipmentId);
                if (missingShipment == null || missingShipmentDto.MissingShipmentId != missingShipmentId)
                {
                    throw new GenericException("Missing Shipment does not exist");
                }
                missingShipment.Waybill = missingShipmentDto.Waybill;
                missingShipment.SettlementAmount = missingShipmentDto.SettlementAmount;
                missingShipment.Comment = missingShipmentDto.Comment;                       
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
