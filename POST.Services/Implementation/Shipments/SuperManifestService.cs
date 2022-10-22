using POST.Core;
using POST.Core.Enums;
using POST.Core.IServices.Shipments;
using POST.Core.IServices.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Shipments
{
    public class SuperManifestService : ISuperManifestService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _service;

        public SuperManifestService(IUnitOfWork uow, INumberGeneratorMonitorService service)
        {
            _uow = uow;
            _service = service;
            MapperConfig.Initialize();
        }


        public async Task<string> GenerateSuperManifestCode()
        {
            try
            {
                var superManifestCode = await _service.GenerateNextNumber(NumberGeneratorType.SuperManifest);
                return superManifestCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
