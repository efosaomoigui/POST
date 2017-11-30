using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class WaybillNumberMonitorService : IWaybillNumberMonitorService
    {
        private readonly IUnitOfWork _uow;
        public WaybillNumberMonitorService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task AddWaybillNumberMonitor(string centre, string code)
        {
            try
            {
                _uow.WaybillNumberMonitor.Add(new WaybillNumberMonitor
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

        public async Task<WaybillNumberMonitor> GetLastWaybillNumberMonitor(string serviceCode)
        {
            try
            {
                var monitor = await Task.Run(() => _uow.WaybillNumberMonitor.SingleOrDefault(x => x.ServiceCentreCode == serviceCode));
                return monitor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateWaybillNumberMonitor(string centre, string code)
        {
            try
            {
                var monitor = _uow.WaybillNumberMonitor.SingleOrDefault(x => x.ServiceCentreCode == centre);
                if (monitor == null)
                {
                    throw new GenericException("WayBill Number Can not be Genereted");
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
