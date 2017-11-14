using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerApplicationService : IPartnerApplicationService
    {
        private readonly IUnitOfWork _uow;

        public PartnerApplicationService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();            
        }

        public async Task<IEnumerable<PartnerApplicationDTO>> GetPartnerApplications()
        {
            var partnerApplications = await _uow.PartnerApplication.GetPartnerApplicationsAsync();
            return partnerApplications;
        }

        public async Task<object> AddPartnerApplication(PartnerApplicationDTO partnerApplicationDto)
        {
            partnerApplicationDto.FirstName = partnerApplicationDto.FirstName.Trim();
            var partnerApplication = Mapper.Map<PartnerApplication>(partnerApplicationDto);
            _uow.PartnerApplication.Add(partnerApplication);
            await _uow.CompleteAsync();

            return new { id = partnerApplication.PartnerApplicationId };

            //_uow.PartnerApplication.Add(
            //    new PartnerApplication
            //    {
            //        PartnerApplicationId = partnerApplicationDto.PartnerApplicationId,
            //        FirstName = partnerApplicationDto.FirstName,
            //        LastName = partnerApplicationDto.LastName,
            //        Email = partnerApplicationDto.Email,
            //        PhoneNumber = partnerApplicationDto.PhoneNumber,
            //        Address = partnerApplicationDto.Address,
            //        CompanyRcNumber = partnerApplicationDto.CompanyRcNumber,
            //        IdentificationNumber = partnerApplicationDto.IdentificationNumber,
            //        TellAboutYou = partnerApplicationDto.TellAboutYou,
            //        PartnerType = partnerApplicationDto.PartnerType
            //    });

        }

        private async Task<bool> IsValidApplicant(int partnerApplicationId)
        {
            return partnerApplicationId > 0 &&
                 await _uow.PartnerApplication.ExistAsync(m => m.PartnerApplicationId == partnerApplicationId);
        }

        public async Task ApprovePartnerApplication(int partnerApplicationId, PartnerApplicationDTO partnerApplication)
        {
            var applicationresult = await _uow.PartnerApplication.GetAsync(partnerApplicationId);

            if (applicationresult == null)
            {
                throw new GenericException("PARTNER_APPLICATION_NOT_EXIST");
            }

            applicationresult.PartnerApplicationStatus = PartnerApplicationStatus.Approved;
            await _uow.CompleteAsync();
        }

        public async Task UpdatePartnerApplication(int partnerApplicationId, PartnerApplicationDTO partnerApplication)
        {
            var applicationresult = await _uow.PartnerApplication.GetAsync(partnerApplicationId);

            if (applicationresult == null)
            {
                throw new GenericException("PARTNER_APPLICATION_NOT_EXIST");
            }

            applicationresult.FirstName = partnerApplication.FirstName;
            applicationresult.LastName = partnerApplication.LastName;
            applicationresult.Address = partnerApplication.Address;
            applicationresult.Email = partnerApplication.Email;
            applicationresult.PhoneNumber = partnerApplication.PhoneNumber;
            applicationresult.CompanyRcNumber = partnerApplication.CompanyRcNumber;
            applicationresult.Email = partnerApplication.Email;
            applicationresult.IdentificationNumber = partnerApplication.IdentificationNumber;
            applicationresult.TellAboutYou = partnerApplication.TellAboutYou;
            applicationresult.IsRegistered = partnerApplication.IsRegistered;

            await _uow.CompleteAsync();
        }

        public async Task RemovePartnerApplication(int partnerApplicationId)
        {
            var application = await _uow.PartnerApplication.GetAsync(partnerApplicationId);

            if (application == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }

            _uow.PartnerApplication.Remove(application);
            await _uow.CompleteAsync();
        }

        public async Task<PartnerApplicationDTO> GetPartnerApplicationById(int partnerApplicationId)
        {
            var partnerappl = await _uow.PartnerApplication.GetAsync(partnerApplicationId);

            if (partnerappl == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }

            return Mapper.Map<PartnerApplicationDTO>(partnerappl);
        }
    }
}
