using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class MobilePickUpRequestsService : IMobilePickUpRequestsService
    {

        private readonly IUnitOfWork _uow;
        public MobilePickUpRequestsService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task AddMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest)
        {
            try
            {
                PickUpRequest.Status = MobilePickUpRequestStatus.Created.ToString();
                var newMobilePickUpRequest = Mapper.Map<MobilePickUpRequests>(PickUpRequest);
                _uow.MobilePickUpRequests.Add(newMobilePickUpRequest);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
