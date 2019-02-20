using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class BankShipmentSettlementService : IBankShipmentSettlementService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly IUserService _userService;
        private readonly INumberGeneratorMonitorService _service;
        private readonly IGlobalPropertyService _globalPropertyService;

        public BankShipmentSettlementService(IUnitOfWork uow, IWalletService walletService, IUserService userService, INumberGeneratorMonitorService service, IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _walletService = walletService;
            _userService = userService;
            _service = service;
            MapperConfig.Initialize();
            _globalPropertyService = globalPropertyService;
        }

        public async Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement()
        {
            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);

            //added for GWA and GWARIMPA service centres
            {
                if (serviceCenters.Length == 1)
                {
                    if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                    {
                        serviceCenters = new int[] { 4, 294 };
                    }
                }
            }

            var cashShipments = new List<GIGLS.Core.DTO.Account.InvoiceViewDTO>();
            if (serviceCenters.Length > 0)
            {
                var shipmentResult = allShipments.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId)).ToList();
                cashShipments = Mapper.Map<List<GIGLS.Core.DTO.Account.InvoiceViewDTO>>(shipmentResult);
            }

            return await Task.FromResult(cashShipments);
        }

        public DateTime ReturnBankProcessDate(DepositType type)
        {

            //get order date time from the already available order codes created to determin the datetime
            DateTime today2 = new DateTime(); //.FirstOrDefault()
            var pastorderdatetime = _uow.BankProcessingOrderCodes.GetBankOrderProcessingCodeAsQueryable();

            var pastorderdatetimeresult = pastorderdatetime.Where(s => s.DepositType == type).FirstOrDefault();

            if (pastorderdatetimeresult == null)
            {
                var today = DateTime.Now;
                var Year = today.Year;
                var Month = today.Month;
                var Day = today.Day;

                today2 = new DateTime(Year, Month, Day, 0, 0, 0);
            }
            else
            {
                today2 = pastorderdatetimeresult.DateAndTimeOfDeposit;
            }

            return today2;
        }

        //New bank processing order for shipment
        public async Task<Tuple<string, List<InvoiceViewDTO>, decimal>> GetBankProcessingOrderForShipment(DateTime searchdate, DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            //get the start and end date for retrieving of waybills for the62 bank
            //var startdate = ReturnBankProcessDate(type);
            var enddate = searchdate;

            //Get Bank Deposit Module StartDate
            var globalpropertiesdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BankDepositModuleStartDate);
            string globalpropertiesdateStr = globalpropertiesdateObj?.Value;
            
            var globalpropertiesdate = DateTime.MinValue;
            bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            var refcode = await _service.GenerateNextNumber(NumberGeneratorType.BankProcessingOrderForShipment, getServiceCenterCode[0].Code);
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);
            //allShipments = allShipments.Where(s => s.DateCreated >= startdate && s.DateCreated <= enddate);
            allShipments = allShipments.Where(s => s.DepositStatus ==0 && s.DateCreated >= globalpropertiesdate);

            //added for GWA and GWARIMPA service centres
            {
                if (serviceCenters.Length == 1)
                {
                    if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                    {
                        serviceCenters = new int[] { 4, 294 };
                    }
                }
            }

            var cashShipments = new List<GIGLS.Core.DTO.Account.InvoiceViewDTO>();
            if (serviceCenters.Length > 0)
            {
                var shipmentResult = allShipments.Where(s => serviceCenters.Contains(s.DepartureServiceCentreId)).ToList();
                cashShipments = Mapper.Map<List<GIGLS.Core.DTO.Account.InvoiceViewDTO>>(shipmentResult);
            }

            foreach (var item in cashShipments)
            {
                total += item.GrandTotal;
            }

            var comboresult = Tuple.Create(refcode, cashShipments, total);
            return await Task.FromResult(comboresult);
        }

        //New bank processing order for COD
        public async Task<Tuple<string, List<CashOnDeliveryRegisterAccountDTO>, decimal>> GetBankProcessingOrderForCOD(DateTime searchdate, DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            //get the start and end date for retrieving of waybills for the bank
            //var startdate = ReturnBankProcessDate(type);
            var enddate = searchdate;

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            var refcode = await _service.GenerateNextNumber(NumberGeneratorType.BankProcessingOrderForCOD, getServiceCenterCode[0].Code);
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allCODs = _uow.CashOnDeliveryRegisterAccount.GetCODAsQueryable();
            allCODs = allCODs.Where(s => s.CODStatusHistory == CODStatushistory.RecievedAtServiceCenter);
            //allCODs = allCODs.Where(s => s.DateCreated >= startdate && s.DateCreated <= enddate);
            allCODs = allCODs.Where(s => s.DepositStatus == 0);

            //added for GWA and GWARIMPA service centres
            {
                if (serviceCenters.Length == 1)
                {
                    if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                    {
                        serviceCenters = new int[] { 4, 294 };
                    }
                }
            }

            var cashcods = new List<CashOnDeliveryRegisterAccountDTO>();
            if (serviceCenters.Length > 0)
            {
                var codResults = allCODs.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();
                cashcods = Mapper.Map<List<CashOnDeliveryRegisterAccountDTO>>(codResults);
            }

            foreach (var item in cashcods)
            {
                total += item.Amount;
            }

            var comboresult = Tuple.Create(refcode, cashcods, total);
            return await Task.FromResult(comboresult);
        }


        //search from the accountants end of Agility
        public async Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, List<BankProcessingOrderCodesDTO>>> SearchBankProcessingOrder2(string _refcode, DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            var bankprcessingresult = await _uow.BankProcessingOrderCodes.GetBankOrderProcessingCode(type);
            var bankprcessingresultValue = bankprcessingresult.Where(s => s.Code == _refcode).ToList();

            //get the start and end date for retrieving of waybills for the bank
            //var startdate = ReturnBankProcessDate(type);
            var refcode = _refcode;

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            decimal total = 0;

            var bankedShipments = new List<BankProcessingOrderForShipmentAndCODDTO>();

            var comboresult = Tuple.Create(refcode, bankedShipments, total, bankprcessingresultValue);
            return await Task.FromResult(comboresult);
        }


        public async Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrder(string _refcode, DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            var bankprcessingresult = await _uow.BankProcessingOrderCodes.GetBankOrderProcessingCode(type);
            var bankprcessingresultValue = bankprcessingresult.Where(s => s.Code == _refcode).FirstOrDefault();

            //get the start and end date for retrieving of waybills for the bank
            //var startdate = ReturnBankProcessDate(type);
            var refcode = _refcode;

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var accompanyWaybills = await _uow.BankProcessingOrderForShipmentAndCOD.GetAllWaybillsForBankProcessingOrders(type);

            var accompanyWaybillsVals = accompanyWaybills.Where(s => s.RefCode == refcode);


            //added for GWA and GWARIMPA service centres
            {
                if (serviceCenters.Length == 1)
                {
                    if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                    {
                        serviceCenters = new int[] { 4, 294 };
                    }
                }
            }

            var bankedShipments = new List<BankProcessingOrderForShipmentAndCODDTO>();
            if (serviceCenters.Length > 0)
            {
                var shipmentResult = accompanyWaybillsVals.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();
                bankedShipments = Mapper.Map<List<BankProcessingOrderForShipmentAndCODDTO>>(shipmentResult);
            }

            if (type == DepositType.Shipment)
            {
                foreach (var item in bankedShipments)
                {
                    total += item.Amount;
                }
            }
            else if (type == DepositType.COD)
            {
                foreach (var item in bankedShipments)
                {
                    total += item.CODAmount;
                }
            }

            var comboresult = Tuple.Create(refcode, bankedShipments, total, bankprcessingresultValue);
            return await Task.FromResult(comboresult);
        }

        public async Task<decimal> GetTotalAmount(DateTime searchdate, DepositType type)
        {

            //get the start and end date for retrieving of waybills for the bank
            //var startdate = ReturnBankProcessDate(type);
            var enddate = searchdate;

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allShipments = _uow.Invoice.GetAllFromInvoiceView();

            //Filter by deposited code should come here

            allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);
            allShipments = allShipments.Where(s => s.DepositStatus ==0);

            //added for GWA and GWARIMPA service centres
            {
                if (serviceCenters.Length == 1)
                {
                    if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                    {
                        serviceCenters = new int[] { 4, 294 };
                    }
                }
            }

            var cashShipments = new List<GIGLS.Core.DTO.Account.InvoiceViewDTO>();
            if (serviceCenters.Length > 0)
            {
                var shipmentResult = allShipments.Where(s => serviceCenters.Contains(s.DepartureServiceCentreId)).ToList();
                cashShipments = Mapper.Map<List<GIGLS.Core.DTO.Account.InvoiceViewDTO>>(shipmentResult);
            }

            foreach (var item in cashShipments)
            {
                total += item.GrandTotal;
            }

            return total;
        }

        public async Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCode(BankProcessingOrderCodesDTO bkoc)
        {
            try
            {
                bkoc.TotalAmount = await GetTotalAmount(bkoc.DateAndTimeOfDeposit, bkoc.DepositType);
                var user = await _userService.retUser();
                bkoc.UserId = user.Id;
                bkoc.FullName = user.FirstName + " " + user.LastName;
                var scs = await _userService.GetCurrentServiceCenter();
                bkoc.ServiceCenter = scs[0].ServiceCentreId;
                bkoc.ScName = scs[0].Name;
                //bkoc.StartDateTime = ReturnBankProcessDate(bkoc.DepositType);

                var bankordercodes = Mapper.Map<BankProcessingOrderCodes>(bkoc);

                //commence preparatiion to insert records in the BankProcessingOrderForShipmentAndCOD
                //var startdate = bkoc.StartDateTime;
                var enddate = bkoc.DateAndTimeOfDeposit;

                if (bkoc.DepositType == DepositType.Shipment)
                {
                    //Validate the user search result set for newly generated code to ensuree a unique group always
                    var allShipments = _uow.Invoice.GetAllFromInvoiceView();
                    allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);
                    allShipments = allShipments.Where(s => s.DepositStatus == 0);

                    var result1 = allShipments.Select(s => new InvoiceViewDTO()
                    {
                        Waybill = s.Waybill
                    });

                    var allprocessingordeforshipment = await _uow.BankProcessingOrderForShipmentAndCOD.GetProcessingOrderForShipmentAndCOD(bkoc.DepositType);
                    var result2 = allprocessingordeforshipment.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    {
                        Waybill = s.Waybill
                    });

                    var validateInsertWaybills = false;
                    foreach (var rs in result1)
                    {
                        validateInsertWaybills = result2.Any(p => p.Waybill == rs.Waybill);
                        if (validateInsertWaybills)
                        {
                            throw new GenericException("Error validating one or more waybills, Please try requesting again for a fresh record.");
                        }
                    }

                    //var bankorderforshipmentandcod = Mapper.Map<List<BankProcessingOrderForShipmentAndCOD>>(allShipments);
                    var bankorderforshipmentandcod = allShipments.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    {
                        Waybill = s.Waybill,
                        RefCode = bkoc.Code,
                        DepositType = bkoc.DepositType,
                        Amount = s.Amount,
                        CODAmount = s.CashOnDeliveryAmount,
                        ServiceCenterId = bkoc.ServiceCenter,
                        ServiceCenter = bkoc.ScName,
                        UserId = bkoc.UserId
                    });


                    _uow.BankProcessingOrderCodes.Add(bankordercodes);
                    _uow.BankProcessingOrderForShipmentAndCOD.AddRange(bankorderforshipmentandcod);

                    //select a list of values that contains the allshipment from the invoice view
                    var arrWaybills = allShipments.Select(x => x.Waybill).ToArray();
                    var nonDepsitedValue = _uow.Shipment.GetAll().Where(x => arrWaybills.Contains(x.Waybill));
                    var nonDepsitedValueunprocessed = nonDepsitedValue.Where(s => s.DepositStatus == 0).ToList();
                    nonDepsitedValueunprocessed.ForEach(a => a.DepositStatus = 1);
                    

                }
                else if (bkoc.DepositType == DepositType.COD)
                {
                    var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                    var allCODs = _uow.CashOnDeliveryRegisterAccount.GetCODAsQueryable();
                    //allCODs = allCODs.Where(s => s.DateCreated >= startdate && s.DateCreated <= enddate);
                    allCODs = allCODs.Where(s => s.DepositStatus ==0);
                    var codsforservicecenter = allCODs.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();

                    var result1 = codsforservicecenter.Select(s => new CashOnDeliveryRegisterAccountDTO()
                    {
                        Waybill = s.Waybill
                    });

                    var allprocessingordeforshipment = await _uow.BankProcessingOrderForShipmentAndCOD.GetProcessingOrderForShipmentAndCOD(bkoc.DepositType);
                    var result2 = allprocessingordeforshipment.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    {
                        Waybill = s.Waybill
                    });

                    var validateInsertWaybills = false;
                    foreach (var rs in result1)
                    {
                        validateInsertWaybills = result2.Any(p => p.Waybill == rs.Waybill);
                        if (validateInsertWaybills)
                        {
                            throw new GenericException("Error validating one or more CODs, Please try requesting again for a fresh record.");
                        }
                    }

                    //var bankorderforshipmentandcod = Mapper.Map<List<BankProcessingOrderForShipmentAndCOD>>(allShipments);
                    var bankorderforshipmentandcod = codsforservicecenter.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    {
                        Waybill = s.Waybill,
                        RefCode = bkoc.Code,
                        ServiceCenterId = bkoc.ServiceCenter,
                        CODAmount = s.Amount,
                        DepositType = bkoc.DepositType,
                        ServiceCenter = bkoc.ScName
                    });

                    _uow.BankProcessingOrderCodes.Add(bankordercodes);
                    _uow.BankProcessingOrderForShipmentAndCOD.AddRange(bankorderforshipmentandcod);

                    //select a list of values that contains the allshipment from the invoice view
                    var arrCODs = codsforservicecenter.Select(x => x.Waybill).ToArray();
                    var nonDepsitedValue = _uow.CashOnDeliveryRegisterAccount.GetAll().Where(x => arrCODs.Contains(x.Waybill)); 
                    var nonDepsitedValueunprocessed = nonDepsitedValue.Where(s => s.DepositStatus == 0).ToList();
                    nonDepsitedValueunprocessed.ForEach(a => a.DepositStatus = 1);
                }

                await _uow.CompleteAsync();

                return new BankProcessingOrderCodesDTO()
                {
                    CodeId = bankordercodes.CodeId,
                    Code = bankordercodes.Code,
                    DateAndTimeOfDeposit = bankordercodes.DateAndTimeOfDeposit
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateBankOrderProcessingCode(BankProcessingOrderCodesDTO bankrefcode)
        {
            //var bankorder = _uow.BankProcessingOrderCodes.Find(s=>s.Code == bankrefcode.Code).FirstOrDefault();

            var bankorder = await _uow.BankProcessingOrderCodes.GetAsync(bankrefcode.CodeId);
            if (bankorder == null)
            {
                throw new GenericException("Bank Order Request Does not Exist!");
            }

            bankorder.Status = bankrefcode.Status;

            var nonDepsitedValue = _uow.Shipment.GetAll().Where(x => x.DepositStatus ==0 && x.DepartureServiceCentreId == bankrefcode.ServiceCenter);
            var nonDepsitedValueunprocessed = nonDepsitedValue.Where(s => s.DepositStatus == 0).ToList();
            nonDepsitedValueunprocessed.ForEach(a => a.DepositStatus = 1);

            await _uow.CompleteAsync();
        }

        public async Task UpdateBankOrderProcessingCode_cod(BankProcessingOrderCodesDTO bankrefcode) 
        {
            //var bankorder = _uow.BankProcessingOrderCodes.Find(s=>s.Code == bankrefcode.Code).FirstOrDefault();

            var bankorder = await _uow.BankProcessingOrderCodes.GetAsync(bankrefcode.CodeId);
            if (bankorder == null)
            {
                throw new GenericException("Bank Order Request Does not Exist!");
            }

            bankorder.Status = bankrefcode.Status;
            await _uow.CompleteAsync();
        }



        public async Task UpdateBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcodeobj)
        {
            var bankorder = await _uow.BankProcessingOrderForShipmentAndCOD.GetAsync(refcodeobj.ProcessingOrderId);
            bankorder.Status = true;

            //var bankorderforshipmentandcod = Mapper.Map<BankProcessingOrderForShipmentAndCOD>(bankorder);
            //_uow.BankProcessingOrderForShipmentAndCOD.Add(bankorderforshipmentandcod);

            await _uow.CompleteAsync();
        }

        //Helps to get bankprocessingcode properties from its table
        public async Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode(DepositType type)
        {
            var result = await _uow.BankProcessingOrderCodes.GetBankOrderProcessingCode(type);
            return await Task.FromResult(result);
        }

        //Helps to get bank processing order from bankprocessingorder table
        public async Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetBankProcessingOrderForShipmentAndCOD(DepositType type)
        {
            var result = await _uow.BankProcessingOrderForShipmentAndCOD.GetProcessingOrderForShipmentAndCOD(type);
            return await Task.FromResult(result);
        }

    }
}
