using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using POST.Core.IRepositories.Zone;
using POST.Core.DTO.Zone;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
{
    public class DeliveryOptionRepository : Repository<DeliveryOption, GIGLSContext>, IDeliveryOptionRepository
    {
        public DeliveryOptionRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<DeliveryOptionDTO>> GetActiveDeliveryOptions()
        {
            var options = Context.DeliveryOption.Where(x => x.IsActive == true);

            var optionDto = from r in options
                            select new DeliveryOptionDTO
                            {
                                DeliveryOptionId = r.DeliveryOptionId,
                                Code = r.Code,
                                Description = r.Description,
                                IsActive = r.IsActive,
                                DateCreated = r.DateCreated,
                                DateModified = r.DateModified,
                                CustomerType = r.CustomerType
                                //UserName = r.User.FirstName + " " + r.User.LastName,
                            };
            return Task.FromResult(optionDto.ToList());
        }

        public Task<List<DeliveryOptionDTO>> GetDeliveryOptions()
        {
            var options = Context.DeliveryOption;

            var optionDto = from r in options
                             select new DeliveryOptionDTO
                             {
                                 DeliveryOptionId = r.DeliveryOptionId,
                                 Code = r.Code,
                                 Description = r.Description,
                                 IsActive = r.IsActive,                               
                                 DateCreated = r.DateCreated,
                                 DateModified = r.DateModified,
                                 CustomerType = r.CustomerType
                                 //UserName = r.User.FirstName + " " + r.User.LastName,
                             };
            return Task.FromResult(optionDto.ToList());
        }
    }
}
