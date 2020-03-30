using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Partnership
{
    public interface IPartnerTransactionsService: IServiceDependencyMarker
    {
        Task<RootObject> GetGeoDetails(LocationDTO location);
        Task<decimal> GetPriceForPartner(PartnerPayDTO partnerpay);
        Task<object> AddPartnerPaymentLog(PartnerTransactionsDTO walletPaymentLogDto);
        Task<string> Decrypt(string cipherText);
        Task ProcessPartnerTransactions(List<ExternalPartnerTransactionsPaymentDTO> paymentLogDto);
    }
}
