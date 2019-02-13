using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.BankSettlement
{
    public interface IBankShipmentSettlementService : IServiceDependencyMarker
    {
        Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement();
		Task<Tuple<string, List<GIGLS.Core.DTO.Account.InvoiceViewDTO>, decimal>> GetBankProcessingOrderForShipment(DateTime requestdate);

        Task<Tuple<string, List<GIGLS.Core.DTO.Account.InvoiceViewDTO>, decimal>> SearchBankOrderForShipment(string refcode);

        Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCode(BankProcessingOrderCodesDTO refcode);
        //Task<BankProcessingOrderForShipmentAndCODDTO> AddBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcode);

        Task UpdateBankOrderProcessingCode(BankProcessingOrderCodesDTO refcode);
        Task UpdateBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcodeobj);

        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode();
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetBankProcessingOrderForShipmentAndCOD();

        Task<Tuple<string, List<CashOnDeliveryRegisterAccountDTO>, decimal>> GetBankProcessingOrderForCOD(DateTime searchdate);
    }
}
