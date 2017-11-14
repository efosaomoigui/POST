using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestMonitorService : IManifestMonitorService
    {
        private readonly IUnitOfWork _uow;
        public ManifestMonitorService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task AddManifestMonitor(string centre, string code)
        {
            try
            {
                _uow.ManifestMonitor.Add(new ManifestMonitor
                {
                    Code = code,
                    ServiceCentreCode = centre
                });
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ManifestMonitor> GetLastManifestMonitor(string serviceCode)
        {
            try
            {
                var monitor = _uow.ManifestMonitor.SingleOrDefault(x => x.ServiceCentreCode == serviceCode);
                return await Task.FromResult(monitor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateManifestMonitor(string centre, string code)
        {
            try
            {
                var monitor = _uow.ManifestMonitor.SingleOrDefault(x => x.ServiceCentreCode == centre);
                if (monitor == null)
                {
                    throw new GenericException("Manifest Number Can not be Genereted");
                }
                monitor.Code = code;
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
