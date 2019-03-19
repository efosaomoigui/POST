using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Utility;

namespace GIGLS.Services.Implementation.Shipments
{
    public class PreShipmentMobileService : IPreShipmentMobileService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
        private readonly IDeliveryOptionService _deliveryService;
        private readonly IServiceCentreService _centreService;
        private readonly IUserServiceCentreMappingService _userServiceCentre;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        public PreShipmentMobileService(IUnitOfWork uow, 
            IShipmentService shipmentService,
            IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, 
            IUserServiceCentreMappingService userServiceCentre,
            INumberGeneratorMonitorService numberGeneratorMonitorService
           
            )
        {
            _uow = uow;
            _shipmentService = shipmentService;
            _deliveryService = deliveryService;
            _centreService = centreService;
            _userServiceCentre = userServiceCentre;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            
            MapperConfig.Initialize();
        }
        public Task<PreShipmentMobileDTO> AddPreShipmentMobile(PreShipmentMobileDTO preShipment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelPreShipmentMobile(string waybill)
        {
            throw new NotImplementedException();
        }

        public Task DeletePreShipmentMobile(string waybill)
        {
            throw new NotImplementedException();
        }

        public Task DeletePreShipmentMobile(int shipmentId)
        {
            throw new NotImplementedException();
        }

        public Task<PreShipmentMobileDTO> GetPreShipmentMobile(string waybill)
        {
            throw new NotImplementedException();
        }

        public Task<PreShipmentMobileDTO> GetPreShipmentMobile(int preShipmentMobileId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePreShipmentMobile(string waybill, PreShipmentMobileDTO preShipment)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePreShipmentMobile(int preShipmentMobileId, PreShipmentMobileDTO preShipment)
        {
            throw new NotImplementedException();
        }
    }
}
