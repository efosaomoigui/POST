using POST.Core.DTO;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Partnership
{
    public interface IPartnerTransactionsService: IServiceDependencyMarker
    {
        Task<RootObject> GetGeoDetails(LocationDTO location);
        Task<decimal> GetPriceForPartner(PartnerPayDTO partnerpay);
        Task<object> AddPartnerPaymentLog(PartnerTransactionsDTO walletPaymentLogDto);
        Task<string> Decrypt(string cipherText);
        Task ProcessPartnerTransactions(List<ExternalPartnerTransactionsPaymentDTO> paymentLogDto);
        Task<List<PartnerPayoutDTO>> GetPartnersPayout(ShipmentCollectionFilterCriteria filterCriteria);
        Task CreditPartnerTransactionByAdmin(CreditPartnerTransactionsDTO transactionsDTO);
        Task<string> Encrypt(string clearText);
        Task CreditCaptainForMovementManifestTransaction(CreditPartnerTransactionsDTO transactionsDTO);
        Task<GoogleAddressDTO> GetGoogleAddressDetails(GoogleAddressDTO location);
        Task ProcessCaptainTransactions(List<ExternalPartnerTransactionsPaymentDTO> paymentLogDto);
    }
}
