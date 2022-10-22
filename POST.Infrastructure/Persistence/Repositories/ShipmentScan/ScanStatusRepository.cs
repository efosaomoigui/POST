using POST.Core.Domain.ShipmentScan;
using POST.Core.IRepositories.ShipmentScan;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO.ShipmentScan;
using AutoMapper;

namespace POST.Infrastructure.Persistence.Repositories.ShipmentScan
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
