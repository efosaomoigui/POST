using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IServices.Partnership;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerPayoutService : IPartnerPayoutService
    {
        private readonly IUnitOfWork _uow;

        public PartnerPayoutService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddPartnerPayout(PartnerPayoutDTO partnerPayoutDTO)
        {
            var newPartnerPayout = Mapper.Map<PartnerPayout>(partnerPayoutDTO);
            newPartnerPayout.DateProcessed = DateTime.Now;

            _uow.PartnerPayout.Add(newPartnerPayout);
            await _uow.CompleteAsync();
            return new { id = newPartnerPayout.PartnerPayoutId };
        }
    }
}
