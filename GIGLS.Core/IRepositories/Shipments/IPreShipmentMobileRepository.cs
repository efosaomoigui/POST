using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IPreShipmentMobileRepository : IRepository<PreShipmentMobile>
    {
       Task<List<PreShipmentMobileDTO>> GetPreShipmentsForMobile();
       Task<PreShipmentMobileDTO> GetBasicPreShipmentMobileDetail(string waybill);
       IQueryable<PreShipmentMobileDTO> GetPreShipmentForUser(string userChannelCode);
       IQueryable<PreShipmentMobileDTO> GetShipmentForEcommerce(string userChannelCode);
    }
}
