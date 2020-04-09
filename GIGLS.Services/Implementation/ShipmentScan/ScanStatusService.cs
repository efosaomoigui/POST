using GIGLS.Core.IServices.ShipmentScan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain.ShipmentScan;
using System.Linq;

namespace GIGLS.Services.Implementation.ShipmentScan
{
    public class ScanStatusService : IScanStatusService
    {
        private readonly IUnitOfWork _uow;

        public ScanStatusService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddScanStatus(ScanStatusDTO scanStatus)
        {
            try
            {
                if (await _uow.ScanStatus.ExistAsync(c => c.Code.ToLower() == scanStatus.Code.Trim().ToLower()))
                {
                    throw new GenericException("Scan Status Code already exist");
                }
                var newStatus = Mapper.Map<ScanStatus>(scanStatus);
                _uow.ScanStatus.Add(newStatus);
                await _uow.CompleteAsync();
                return new { Id = newStatus.ScanStatusId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ScanStatusDTO>> GetScanStatus()
        {
            return await _uow.ScanStatus.GetScanStatusAsync();
        }

        public async Task DeleteScanStatus(int scanStatusId)
        {
            try
            {
                var scanStatus = await _uow.ScanStatus.GetAsync(scanStatusId);
                if (scanStatus == null)
                {
                    throw new GenericException("Scan Status does not exist");
                }
                _uow.ScanStatus.Remove(scanStatus);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ScanStatusDTO> GetScanStatusByCode(string code)
        {
            try
            {
                var scanStatus = await _uow.ScanStatus.GetAsync(x => x.Code.Equals(code));
                if (scanStatus == null)
                {
                    throw new GenericException("Scan Status does not exist");
                }

                var scanStatusDTO = Mapper.Map<ScanStatusDTO>(scanStatus);
                return scanStatusDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ScanStatusDTO> GetScanStatusById(int scanStatusId)
        {
            try
            {
                var scanStatus = await _uow.ScanStatus.GetAsync(scanStatusId);
                if (scanStatus == null)
                {
                    throw new GenericException("Scan Status does not exist");
                }

                var scanStatusDTO = Mapper.Map<ScanStatusDTO>(scanStatus);
                return scanStatusDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateScanStatus(int scanStatusId, ScanStatusDTO scanStatusDto)
        {
            try
            {
                var scanStatus = await _uow.ScanStatus.GetAsync(scanStatusId);
                if (scanStatusDto == null || scanStatusDto.ScanStatusId != scanStatusId)
                {
                    throw new GenericException("Scan Status does not exist");
                }

                scanStatus.Incident = scanStatusDto.Incident;
                scanStatus.Code = scanStatusDto.Code;
                scanStatus.Comment = scanStatusDto.Comment;
                scanStatus.Reason = scanStatusDto.Reason;
                scanStatus.HiddenFlag = scanStatusDto.HiddenFlag;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
              

        public async Task<IEnumerable<ScanStatusDTO>> GetNonHiddenScanStatus()
        {
            try
            {
                var scanstatus = _uow.ScanStatus.GetAllAsQueryable().Where(x => x.HiddenFlag == false).ToList();
                var scanstatusDto = Mapper.Map<IEnumerable<ScanStatusDTO>>(scanstatus.OrderBy(x => x.Reason));
                return await Task.FromResult(scanstatusDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
