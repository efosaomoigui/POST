using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class GroupWaybillNumberMonitorService : IGroupWaybillNumberMonitorService
    {
        private readonly IUnitOfWork _uow;
        public GroupWaybillNumberMonitorService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task AddGroupWaybillNumberMonitor(string centre, string code)
        {
            try
            {
                _uow.GroupWaybillNumberMonitor.Add(new GroupWaybillNumberMonitor
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

        public async Task<GroupWaybillNumberMonitor> GetLastGroupWaybillNumberMonitor(string serviceCode)
        {
            try
            {
                var monitor = _uow.GroupWaybillNumberMonitor.SingleOrDefault(x => x.ServiceCentreCode == serviceCode);
                return await Task.FromResult(monitor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGroupWaybillNumberMonitor(string centre, string code)
        {
            try
            {
                var monitor = _uow.GroupWaybillNumberMonitor.SingleOrDefault(x => x.ServiceCentreCode == centre);
                if (monitor == null)
                {
                    throw new GenericException("GroupWayBill Number Can not be Genereted");
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
