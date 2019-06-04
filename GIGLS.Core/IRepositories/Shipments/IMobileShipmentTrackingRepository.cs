using System;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IMobileShipmentTrackingRepository : IRepository<Domain.MobileShipmentTracking>
    {

       
        Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackingsAsync(string waybill);
    }
}
