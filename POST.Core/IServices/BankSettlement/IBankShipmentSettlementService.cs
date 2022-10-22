using GIGL.POST.Core.Domain;
using POST.Core.Domain.BankSettlement;
using POST.Core.DTO.Account;
using POST.Core.DTO.BankSettlement;
using POST.Core.DTO.Report;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using POST.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.BankSettlement
{
    public interface IBankShipmentSettlementService : IServiceDependencyMarker
    {
        Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement();
		Task<Tuple<string, List<POST.Core.DTO.Account.InvoiceViewDTO>, decimal>> GetBankProcessingOrderForShipment(DepositType type);

        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrder(string refcode, DepositType type);
        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, List<BankProcessingOrderCodesDTO>>> SearchBankProcessingOrder2(string refcode, DepositType type);
        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrder3(string refcode, DepositType type);

        Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCode(BankProcessingOrderCodesDTO refcode);
        Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCodeDemurrageOnly(BankProcessingOrderCodesDTO refcode); 
        //Task<BankProcessingOrderForShipmentAndCODDTO> AddBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcode);

        Task UpdateBankOrderProcessingCode(BankProcessingOrderCodesDTO refcode);
        Task UpdateBankOrderProcessingCode_cod(BankProcessingOrderCodesDTO refcode);
        Task UpdateBankOrderProcessingCode_demurrage(BankProcessingOrderCodesDTO bankrefcode);
        Task UpdateBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcodeobj);
                
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetBankProcessingOrderForShipmentAndCOD(DepositType type);

        Task<Tuple<string, List<CashOnDeliveryRegisterAccountDTO>, decimal>> GetBankProcessingOrderForCOD(DepositType type);
        Task<Tuple<string, List<DemurrageRegisterAccountDTO>, decimal>> GetBankProcessingOrderForDemurrage(DepositType type);

        Task MarkAsVerified(BankProcessingOrderCodesDTO refcode);
        Task MarkAsVerified_cod(BankProcessingOrderCodesDTO refcode);
        Task MarkAsVerified_demurrage(BankProcessingOrderCodesDTO bankrefcode);

        Task<List<NewInvoiceViewDTO>> GetCODCustomersWhoNeedPayOut();
        Task UpdateCODCustomersWhoNeedPayOut(NewInvoiceViewDTO invoiceviewinfo);

        Task<List<CodPayOutList>> GetPaidOutCODLists();
        Task<List<CodPayOutList>> GetPaidOutCODListsByCustomer(string customercode);

        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria);
        Task<IEnumerable<BankDTO>> GetBanks();
        Task<List<BankProcessingOrderCodesDTO>> GetRegionalBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria);
        Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCode_ScheduleTask(BankProcessingOrderCodesDTO bkoc);
        Task<Object> GetBankProcessingOrderForShipment_ScheduleTask(int serviceCenterId, DepositType type);
        Task<object> GetBankProcessingOrderForCOD_ScheduledTask(DepositType type, int ServiceCenterId); 
        Task<object> GetBankProcessingOrderForDemurrage_ScheduleTask(DepositType type, int servicecenterId);
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeByServiceCenter(DepositType type, BankDepositFilterCriteria dateFilterCriteria);
        Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrderV2(string refcode, DepositType type);
        Task<List<BankProcessingOrderCodesDTO>> GetRegionalAndECOBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria);
        Task MarkMultipleBankProcessingOrderAsVerified(List<BankProcessingOrderCodesDTO> bankrefcodes, string type);
    }
}
