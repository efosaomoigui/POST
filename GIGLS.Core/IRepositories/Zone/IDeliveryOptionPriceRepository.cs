using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface IDeliveryOptionPriceRepository : IRepository<DeliveryOptionPrice>
    {
        Task<List<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices();
    }
}
