using POST.Core.Domain.Devices;
using POST.Core.IRepositories.Devices;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Devices
{
    public class DeviceRepository : Repository<Device, GIGLSContext>, IDeviceRepository
    {
        public DeviceRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
