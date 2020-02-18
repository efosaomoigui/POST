using GIGLS.Core;
using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class ActivationCampaignEmailService : IActivationCampaignEmailService
    {
        private readonly IUnitOfWork _uow;

        public ActivationCampaignEmailService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
    }
}