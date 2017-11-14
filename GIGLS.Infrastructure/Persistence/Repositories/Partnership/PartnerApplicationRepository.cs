using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Partnership
{
    public class PartnerApplicationRepository : Repository<PartnerApplication, GIGLSContext>, IPartnerApplicationRepository
    {
        public PartnerApplicationRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<PartnerApplicationDTO>> GetPartnerApplicationsAsync()
        {
            
            var partnerapplication = Context.PartnerApplications.Where(x => x.IsRegistered == false);

            var partnerDto = from partner in partnerapplication
                                select new PartnerApplicationDTO
                                {
                                    PartnerApplicationId = partner.PartnerApplicationId,
                                    FirstName = partner.FirstName,
                                    LastName = partner.LastName,
                                    Email = partner.Email,
                                    PhoneNumber = partner.PhoneNumber,
                                    Address = partner.Address,
                                    CompanyRcNumber = partner.CompanyRcNumber,
                                    IdentificationNumber = partner.IdentificationNumber,
                                    TellAboutYou = partner.TellAboutYou,
                                    PartnerApplicationStatus = partner.PartnerApplicationStatus,
                                    PartnerType = partner.PartnerType,
                                    IsRegistered = partner.IsRegistered
                                };
            return Task.FromResult(partnerDto.ToList());
        }
    }
}
