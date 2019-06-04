using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IServices.ShipmentScan;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ShipmentScan
{
    public class MobileScanStatusService : IMobileScanStatusService
    {
        private readonly IUnitOfWork _uow;

        public MobileScanStatusService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddScanStatus(MobileScanStatusDTO scanStatus)
        {
            try
            {
                if (await _uow.MobileScanStatus.ExistAsync(c => c.Code.ToLower() == scanStatus.Code.Trim().ToLower()))
                {
                    throw new GenericException("Scan Status Code already exist");
                }
                var newStatus = Mapper.Map<MobileScanStatus>(scanStatus);
                _uow.MobileScanStatus.Add(newStatus);
                await _uow.CompleteAsync();
                return new { Id = newStatus.MobileScanStatusId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MobileScanStatusDTO>> GetScanStatus()
        {
            return await _uow.MobileScanStatus.GetMobileScanStatusAsync();
        }

        public async Task DeleteScanStatus(int scanStatusId)
        {
            try
            {
                var scanStatus = await _uow.MobileScanStatus.GetAsync(scanStatusId);
                if (scanStatus == null)
                {
                    throw new GenericException("Scan Status does not exist");
                }
                _uow.MobileScanStatus.Remove(scanStatus);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<MobileScanStatusDTO> GetScanStatusByCode(string code)
        {
            try
            {
                var scanStatus = await _uow.MobileScanStatus.GetAsync(x => x.Code.Equals(code));
                if (scanStatus == null)
                {
                    throw new GenericException("Scan Status does not exist");
                }

                var scanStatusDTO = Mapper.Map<MobileScanStatusDTO>(scanStatus);
                return scanStatusDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MobileScanStatusDTO> GetScanStatusById(int scanStatusId)
        {
            try
            {
                var scanStatus = await _uow.MobileScanStatus.GetAsync(scanStatusId);
                if (scanStatus == null)
                {
                    throw new GenericException("Scan Status does not exist");
                }

                var scanStatusDTO = Mapper.Map<MobileScanStatusDTO>(scanStatus);
                return scanStatusDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateScanStatus(int scanStatusId, MobileScanStatusDTO scanStatusDto)
        {
            try
            {
                var scanStatus = await _uow.MobileScanStatus.GetAsync(scanStatusId);
                if (scanStatusDto == null || scanStatusDto.MobileScanStatusId != scanStatusId)
                {
                    throw new GenericException("MobileScan Status does not exist");
                }

                scanStatus.Incident = scanStatusDto.Incident;
                scanStatus.Code = scanStatusDto.Code;
                scanStatus.Comment = scanStatusDto.Comment;
                scanStatus.Reason = scanStatusDto.Reason;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
