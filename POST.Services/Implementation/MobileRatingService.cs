using POST.Core;
using POST.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Services.Implementation
{
    public class MobileRatingService : IMobileRatingService
    {
        private readonly IUnitOfWork _uow;

        public MobileRatingService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
    }
    
}
