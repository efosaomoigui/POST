using POST.Core.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IServices.Business
{
    public interface IGIGGoPricingService : IServiceDependencyMarker
    {
        Task<MobilePriceDTO> GetGIGGOPrice(PreShipmentMobileDTO preShipment);
    }
}
