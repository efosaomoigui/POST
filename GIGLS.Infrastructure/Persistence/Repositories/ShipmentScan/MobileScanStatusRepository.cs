using AutoMapper;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IRepositories.ShipmentScan;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.ShipmentScan
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
