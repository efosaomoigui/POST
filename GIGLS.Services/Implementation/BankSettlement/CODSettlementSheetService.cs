using GIGLS.Core.IServices.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Devices;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain.Devices;
using System.Linq;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.BankSettlement
{
    public class CODSettlementSheetService : ICODSettlementSheetService
    {
        private IUserService _userService;
        private readonly IUnitOfWork _uow;

        public CODSettlementSheetService(IUserService userService, IUnitOfWork uow)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddCODSettlementSheet(CODSettlementSheetDTO codSettlementSheetDto)
        {
            try
            {
                if (await _uow.CODSettlementSheet.ExistAsync(c => c.Waybill.ToLower() == codSettlementSheetDto.Waybill.Trim().ToLower()))
                {
                    throw new GenericException("CODSettlementSheet already Exist");
                }
                var newCODSettlementSheet = Mapper.Map<CODSettlementSheet>(codSettlementSheetDto);
                _uow.CODSettlementSheet.Add(newCODSettlementSheet);
                await _uow.CompleteAsync();
                return new { Id = newCODSettlementSheet.CODSettlementSheetId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCODSettlementSheet(int codSettlementSheetId)
        {
            try
            {
                var codSettlementSheet = await _uow.CODSettlementSheet.GetAsync(codSettlementSheetId);
                if (codSettlementSheet == null)
                {
                    throw new GenericException("CODSettlementSheet does not exist");
                }
                _uow.CODSettlementSheet.Remove(codSettlementSheet);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CODSettlementSheetDTO>> GetCODSettlementSheets()
        {
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var codSettlementSheets = await _uow.CODSettlementSheet.GetCODSettlementSheetsAsync(serviceCenters);
            return codSettlementSheets;

        }


        public async Task<CODSettlementSheetDTO> GetCODSettlementSheetById(int codSettlementSheetId)
        {
            try
            {
                var codSettlementSheet = await _uow.CODSettlementSheet.GetAsync(codSettlementSheetId);
                if (codSettlementSheet == null)
                {
                    throw new GenericException("CODSettlementSheet Not Exist");
                }

                var codSettlementSheetDto = Mapper.Map<CODSettlementSheetDTO>(codSettlementSheet);
                return codSettlementSheetDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCODSettlementSheet(int codSettlementSheetId, CODSettlementSheetDTO codSettlementSheetDto)
        {
            try
            {
                var codSettlementSheet = await _uow.CODSettlementSheet.GetAsync(codSettlementSheetId);
                if (codSettlementSheet == null || codSettlementSheetDto.CODSettlementSheetId != codSettlementSheetId)
                {
                    throw new GenericException("CODSettlementSheet information does not exist");
                }
                codSettlementSheet.ReceiverAgentId = codSettlementSheetDto.ReceiverAgentId;
                codSettlementSheet.DateSettled = codSettlementSheetDto.DateSettled;
                codSettlementSheet.CollectionAgentId = codSettlementSheetDto.CollectionAgentId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateMultipleStatusCODSettlementSheet(List<string> WaybillNumbers)
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();

                foreach (var waybill in WaybillNumbers)
                {
                    var codSettlementSheet = _uow.CODSettlementSheet.SingleOrDefault(s => s.Waybill == waybill);
                    if (codSettlementSheet == null)
                    {
                        throw new GenericException("Waybill information does not exist");
                    }
                    codSettlementSheet.ReceivedCOD = true;
                    codSettlementSheet.CollectionAgentId = currentUserId;
                }
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CODSettlementSheetDTO>> GetUnbankedCODShipmentSettlement()
        {
            var codSettlementSheets = _uow.CODSettlementSheet.GetAllAsQueryable().Where(s => s.ReceivedCOD == true).ToList();
            var codSettlementSheetsList = Mapper.Map<List<CODSettlementSheetDTO>>(codSettlementSheets);
            return await Task.FromResult(codSettlementSheetsList);
        }
    }
}
