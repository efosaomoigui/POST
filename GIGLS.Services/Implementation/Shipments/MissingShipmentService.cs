using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Shipments
{
    public class MissingShipmentService : IMissingShipmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public MissingShipmentService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddMissingShipment(MissingShipmentDTO missingShipment)
        {
            try
            {
                if (!await _uow.Shipment.ExistAsync(c => c.Waybill == missingShipment.Waybill))
                {
                    throw new GenericException($"Waybill: {missingShipment.Waybill} Information does not exist");
                }

                if (await _uow.MissingShipment.ExistAsync(c => c.Waybill == missingShipment.Waybill))
                {
                    throw new GenericException($"Incident report for the waybill {missingShipment.Waybill}  already exist");
                }

                var user = await _userService.GetCurrentUserId();
                var serviceCentre = await _userService.GetPriviledgeServiceCenters();

                var newMissingShipment = Mapper.Map<MissingShipment>(missingShipment);
                newMissingShipment.Status = "Pending";
                newMissingShipment.CreatedBy = user;
                newMissingShipment.ServiceCentreId = serviceCentre[0];
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
                    throw new GenericException("Incident Report information does not exist");
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
                return await _uow.MissingShipment.GetMissingShipmentById(missingShipmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        public async Task<List<MissingShipmentDTO>> GetMissingShipments()
        {
            try
            {
                return await _uow.MissingShipment.GetMissingShipments();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateMissingShipment(int missingShipmentId, MissingShipmentDTO missingShipmentDto)
        {
            try
            {
                var missingShipment = await _uow.MissingShipment.GetAsync(missingShipmentId);
                if (missingShipment == null || missingShipmentDto.MissingShipmentId != missingShipmentId)
                {
                    throw new GenericException("Incident report information does not exist");
                }
                
                var user = await _userService.GetCurrentUserId();

                //missingShipment.Waybill = missingShipmentDto.Waybill;
                //missingShipment.Comment = missingShipmentDto.Comment;
                //missingShipment.Reason = missingShipmentDto.Reason;
                missingShipment.SettlementAmount = missingShipmentDto.SettlementAmount;
                missingShipment.Status = missingShipmentDto.Status;
                missingShipment.Feedback = missingShipmentDto.Feedback;
                missingShipment.ResolvedBy = user;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
