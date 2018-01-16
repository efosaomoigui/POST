using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.IRepositories.ShipmentScan;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ShipmentScan;
using AutoMapper;

namespace GIGLS.Infrastructure.Persistence.Repositories.ShipmentScan
{
    public class ScanStatusRepository : Repository<ScanStatus, GIGLSContext>, IScanStatusRepository
    {
        public ScanStatusRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<ScanStatusDTO>> GetScanStatusAsync()
        {
            try
            {
                var scanstatus = Context.ScanStatus.ToList();
                var scanstatusDto = Mapper.Map<IEnumerable<ScanStatusDTO>>(scanstatus);
                return Task.FromResult(scanstatusDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
