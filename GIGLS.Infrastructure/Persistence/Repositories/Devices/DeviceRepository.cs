using GIGLS.Core.Domain.Devices;
using GIGLS.Core.IRepositories.Devices;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Devices
{
    public class DeviceRepository : Repository<Device, GIGLSContext>, IDeviceRepository
    {
        public DeviceRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
