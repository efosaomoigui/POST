using System;
using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using System.Threading.Tasks;
using POST.Core.DTO;
using System.Collections.Generic;

namespace POST.Core.IRepositories.Shipments
{
    public interface IMobileShipmentTrackingRepository : IRepository<Domain.MobileShipmentTracking>
    {

       
        Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackingsAsync(string waybill);
    }
}
