using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Website
{
    public interface IWebsiteService : IServiceDependencyMarker
    {
        Task<bool> SendSchedulePickupMail(WebsiteMessageDTO obj);
        Task<bool> SendQuoteMail(WebsiteMessageDTO obj);
        Task<bool> SendGIGGoIssuesMail(AppMessageDTO obj);
        Task<object> AddEcommerceAgreement(EcommerceAgreementDTO ecommerceAgreementDTO);
        Task<object> AddIntlCustomer(CustomerDTO customerDTO);
    }
    
}
