using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.BankSettlement
{
    public interface IBankShipmentSettlementService : IServiceDependencyMarker
    {
        Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement();
		Task<Tuple<string, List<GIGLS.Core.DTO.Account.InvoiceViewDTO>, decimal>> GetBankProcessingOrderForShipment(DepositType type);

        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrder(string refcode, DepositType type);
        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, List<BankProcessingOrderCodesDTO>>> SearchBankProcessingOrder2(string refcode, DepositType type);
        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrder3(string refcode, DepositType type);

        Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCode(BankProcessingOrderCodesDTO refcode);
        Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCodeDemurrageOnly(BankProcessingOrderCodesDTO refcode); 
        //Task<BankProcessingOrderForShipmentAndCODDTO> AddBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcode);

        Task UpdateBankOrderProcessingCode(BankProcessingOrderCodesDTO refcode);
        Task UpdateBankOrderProcessingCode_cod(BankProcessingOrderCodesDTO refcode);
        Task UpdateBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcodeobj);

        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode(DepositType type);
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetBankProcessingOrderForShipmentAndCOD(DepositType type);

        Task<Tuple<string, List<CashOnDeliveryRegisterAccountDTO>, decimal>> GetBankProcessingOrderForCOD(DepositType type);
        Task<Tuple<string, List<DemurrageRegisterAccountDTO>, decimal>> GetBankProcessingOrderForDemurrage(DepositType type);

        Task MarkAsVerified(BankProcessingOrderCodesDTO refcode);
        Task MarkAsVerified_cod(BankProcessingOrderCodesDTO refcode);
    }
}
