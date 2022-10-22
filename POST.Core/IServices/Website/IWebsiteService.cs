using POST.Core.DTO;
using POST.Core.DTO.Customers;
using POST.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace POST.Core.IServices.Website
{
    public interface IWebsiteService : IServiceDependencyMarker
    {
        Task<bool> SendSchedulePickupMail(WebsiteMessageDTO obj);
        Task<bool> SendQuoteMail(WebsiteMessageDTO obj);
        Task<bool> SendGIGGoIssuesMail(AppMessageDTO obj);
        Task<object> AddEcommerceAgreement(EcommerceAgreementDTO ecommerceAgreementDTO);
        Task<object> AddIntlCustomer(CustomerDTO customerDTO);
        Task<IntlShipmentRequestDTO> AddIntlShipmentRequest(IntlShipmentRequestDTO shipmentDTO);
    }
    
}
