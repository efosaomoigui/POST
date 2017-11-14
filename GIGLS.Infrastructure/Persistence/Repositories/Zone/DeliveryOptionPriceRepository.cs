using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class DeliveryOptionPriceRepository : Repository<DeliveryOptionPrice, GIGLSContext>, IDeliveryOptionPriceRepository
    {
        public DeliveryOptionPriceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices()
        {
            try
            {
                var options = Context.DeliveryOptionPrice.Include("Zone").Include("DeliveryOption");
                var optionDto = from r in options
                                select new DeliveryOptionPriceDTO
                                {
                                    DeliveryOptionPriceId = r.DeliveryOptionPriceId,
                                    ZoneId = r.ZoneId,
                                    DeliveryOptionId = r.DeliveryOptionId,
                                    Price = r.Price,
                                    ZoneName = r.Zone.ZoneName,
                                    DeliveryOption = r.DeliveryOption.Description
                                };
                return Task.FromResult(optionDto.ToList());
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
