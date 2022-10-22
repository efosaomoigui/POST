using POST.Core;
using POST.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Services.Implementation
{
    public class ReferrerCodeService : IReferrerCodeService
    {
        private readonly IUnitOfWork _uow;

        public ReferrerCodeService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
    }
}
