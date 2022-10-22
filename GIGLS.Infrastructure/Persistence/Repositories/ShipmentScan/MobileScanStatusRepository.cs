using AutoMapper;
using POST.Core.Domain.ShipmentScan;
using POST.Core.DTO.ShipmentScan;
using POST.Core.IRepositories.ShipmentScan;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.ShipmentScan
{
    public class MobileScanStatusRepository : Repository<MobileScanStatus, GIGLSContext>, IMobileScanStatusRepository
    {
        public MobileScanStatusRepository(GIGLSContext context) : base(context)
        {
        }
        public Task<IEnumerable<MobileScanStatusDTO>> GetMobileScanStatusAsync()
        {
            try
            {
                var scanstatus = Context.MobileScanStatus.ToList();
                var scanstatusDto = Mapper.Map<IEnumerable<MobileScanStatusDTO>>(scanstatus);
                return Task.FromResult(scanstatusDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
