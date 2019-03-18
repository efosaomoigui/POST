using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Account;
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
        private readonly IInvoiceService _invoiceService;

        public BankShipmentSettlementService(IUnitOfWork uow, IWalletService walletService, IUserService userService, INumberGeneratorMonitorService service, IGlobalPropertyService globalPropertyService, IInvoiceService invoiceservice)
        {
            _uow = uow;
            _walletService = walletService;
            _userService = userService;
            _service = service;
            _invoiceService = invoiceservice;
            MapperConfig.Initialize();
            _globalPropertyService = globalPropertyService;
        }

        public async Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement()
        {
            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            //var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments();
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


        //New bank processing order for shipment
        public async Task<Tuple<string, List<InvoiceViewDTO>, decimal>> GetBankProcessingOrderForShipment(DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            var enddate = DateTime.Now;

            //Get Bank Deposit Module StartDate
            var globalpropertiesdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BankDepositModuleStartDate);
            string globalpropertiesdateStr = globalpropertiesdateObj?.Value;

            var globalpropertiesdate = DateTime.MinValue;
            bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            var refcode = await _service.GenerateNextNumber(NumberGeneratorType.BankProcessingOrderForShipment, getServiceCenterCode[0].Code);

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;

            //var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments();

            allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);
            allShipments = allShipments.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.DateCreated >= globalpropertiesdate);

            //A. get partial payment values
            var allShipmentsPartial = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.DateCreated >= globalpropertiesdate && s.PaymentMethod == "Partial");

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

            //B. combine list for partial and cash shipment
            var cashShipments = new List<GIGLS.Core.DTO.Account.InvoiceViewDTO>();
            if (serviceCenters.Length > 0)
            {
                var shipmentResult = allShipments.Where(s => serviceCenters.Contains(s.DepartureServiceCentreId)).ToList();
                var allShipmentsPartialVals = allShipmentsPartial.Where(s => serviceCenters.Contains(s.DepartureServiceCentreId)).ToList();

                shipmentResult.AddRange(allShipmentsPartialVals);
                cashShipments = Mapper.Map<List<GIGLS.Core.DTO.Account.InvoiceViewDTO>>(shipmentResult);
            }

            //var partialPaymentCash = returnPartialPaymentCashByWaybill();
            var cashShipmentsVal = new List<GIGLS.Core.DTO.Account.InvoiceViewDTO>();
            foreach (var item in cashShipments)
            {
                //1. cash first
                if (item.PaymentMethod == "Cash")
                {
                    cashShipmentsVal.Add(item);
                }

                //2. partial
                if (item.PaymentMethod == "Partial")
                {
                    var partialPaymentCash = await returnPartialPaymentCashByWaybill(item.Waybill);

                    if (partialPaymentCash.Item1 != null && partialPaymentCash.Item1.Count > 0)
                    {
                        item.GrandTotal = partialPaymentCash.Item2;
                        cashShipmentsVal.Add(item);
                    }
                }
            }

            //3. sum total
            decimal total = cashShipmentsVal.Sum(s => s.GrandTotal);
            var comboresult = Tuple.Create(refcode, cashShipmentsVal, total);
            return await Task.FromResult(comboresult);

        }

        private async Task<Tuple<List<PaymentPartialTransaction>, decimal>> returnPartialPaymentCashByWaybill(string waybill)
        {
            decimal total = 0;
            var cashPaymentPartial = _uow.PaymentPartialTransaction.GetAll().Where(s => s.Waybill == waybill && s.PaymentType == PaymentType.Cash).ToList();

            foreach (var item in cashPaymentPartial)
            {
                total += item.Amount;
            }

            var vals = Tuple.Create(cashPaymentPartial, total);

            var comboresult = Tuple.Create(cashPaymentPartial, total);
            return await Task.FromResult(comboresult);
        }

        //New bank processing order for Demurrage
        public async Task<Tuple<string, List<DemurrageRegisterAccountDTO>, decimal>> GetBankProcessingOrderForDemurrage(DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            var enddate = DateTime.Now;

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            var refcode = await _service.GenerateNextNumber(NumberGeneratorType.BankProcessingOrderForDemurrage, getServiceCenterCode[0].Code);
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allDemurrages = _uow.DemurrageRegisterAccount.GetDemurrageAsQueryable();
            allDemurrages = allDemurrages.Where(s => s.DEMStatusHistory == CODStatushistory.RecievedAtServiceCenter);
            allDemurrages = allDemurrages.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.PaymentType == PaymentType.Cash);

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

            var cashdemurrage = new List<DemurrageRegisterAccountDTO>();
            if (serviceCenters.Length > 0)
            {
                var demurrageResults = allDemurrages.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();
                cashdemurrage = Mapper.Map<List<DemurrageRegisterAccountDTO>>(demurrageResults);
            }

            foreach (var item in cashdemurrage)
            {
                total += item.Amount;
            }

            var comboresult = Tuple.Create(refcode, cashdemurrage, total);
            return await Task.FromResult(comboresult);
        }

        //New bank processing order for COD
        public async Task<Tuple<string, List<CashOnDeliveryRegisterAccountDTO>, decimal>> GetBankProcessingOrderForCOD(DepositType type)
        {
            //var isSCA =await _userService.CheckSCA();
            //if (!isSCA)
            //{
            //    throw new GenericException("User is not a Service Center Agent!");
            //}

            var enddate = DateTime.Now;

            //Generate the refcode
            var getServiceCenterCode = await _userService.GetCurrentServiceCenter();
            var refcode = await _service.GenerateNextNumber(NumberGeneratorType.BankProcessingOrderForCOD, getServiceCenterCode[0].Code);
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allCODs = _uow.CashOnDeliveryRegisterAccount.GetCODAsQueryable();
            allCODs = allCODs.Where(s => s.CODStatusHistory == CODStatushistory.RecievedAtServiceCenter);
            allCODs = allCODs.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.PaymentType == PaymentType.Cash);

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

        //General Search
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
                var shipmentResult = accompanyWaybillsVals.Where(s => serviceCenters.Contains(s.ServiceCenterId) && (s.Status == DepositStatus.Pending || s.Status == DepositStatus.Deposited || s.Status == DepositStatus.Verified)).ToList();
                bankedShipments = Mapper.Map<List<BankProcessingOrderForShipmentAndCODDTO>>(shipmentResult);
            }

            if (type == DepositType.Shipment)
            {
                foreach (var item in bankedShipments)
                {
                    total += item.GrandTotal;
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

        public async Task<Tuple<string, List<BankProcessingOrderForShipmentAndCODDTO>, decimal, BankProcessingOrderCodesDTO>> SearchBankProcessingOrder3(string _refcode, DepositType type)
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

            var bankedShipments = new List<BankProcessingOrderForShipmentAndCODDTO>();

            //var shipmentResult = accompanyWaybillsVals.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();
            bankedShipments = accompanyWaybillsVals.OrderByDescending(s => s.DateCreated).ToList(); // Mapper.Map<List<BankProcessingOrderForShipmentAndCODDTO>>(shipmentResult);

            if (type == DepositType.Shipment)
            {
                foreach (var item in bankedShipments)
                {
                    total += item.GrandTotal;
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

        public async Task<Tuple<decimal, List<InvoiceViewDTO>>> GetTotalAmountAndShipments(DateTime searchdate, DepositType type)
        {

            var enddate = searchdate;

            //Generate the refcode
            decimal total = 0;

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            //var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments();

            //Get Bank Deposit Module StartDate
            var globalpropertiesdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BankDepositModuleStartDate);
            string globalpropertiesdateStr = globalpropertiesdateObj?.Value;

            var globalpropertiesdate = DateTime.MinValue;
            bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

            //Filter by deposited code should come here
            allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);
            allShipments = allShipments.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.DateCreated >= globalpropertiesdate);

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

            //return total;
            var comboresult = Tuple.Create(total, cashShipments);
            return await Task.FromResult(comboresult);
        }

        //Add Bank ProcessingOrder Code DemurrageOnly
        public async Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCodeDemurrageOnly(BankProcessingOrderCodesDTO bkoc)
        {
            try
            {
                //1. get the current service user
                var user = await _userService.retUser();
                bkoc.UserId = user.Id;
                bkoc.FullName = user.FirstName + " " + user.LastName;

                //2. get the service centers for the current user
                var scs = await _userService.GetCurrentServiceCenter();
                bkoc.ServiceCenter = scs[0].ServiceCentreId;
                bkoc.ScName = scs[0].Name;

                //3. Get Bank Deposit Module StartDate
                var globalpropertiesdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BankDepositModuleStartDate);
                string globalpropertiesdateStr = globalpropertiesdateObj?.Value;

                var globalpropertiesdate = DateTime.MinValue;
                bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

                var bankordercodes = new BankProcessingOrderCodes();

                //4. updating the startdate
                bkoc.StartDateTime = globalpropertiesdate;
                bkoc.DateAndTimeOfDeposit = DateTime.Now;
                bkoc.Status = DepositStatus.Pending;

                //5. commence preparatiion to insert records in the BankProcessingOrderForShipmentAndCOD
                var enddate = bkoc.DateAndTimeOfDeposit;

                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;

                //1. get data from Demurrage register account as queryable from DemurrageRegisterAccount table
                var allDemurrages = _uow.DemurrageRegisterAccount.GetDemurrageAsQueryable();

                allDemurrages = allDemurrages.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.PaymentType == PaymentType.Cash);
                allDemurrages = allDemurrages.Where(s => s.DEMStatusHistory == CODStatushistory.RecievedAtServiceCenter);

                //2. convert demurrage to list
                var demurrageforservicecenter = allDemurrages.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();

                //Based on the value from CashOnDeliveryRegisterAccount table filter out the waybills
                var result1 = demurrageforservicecenter.Select(s => new DemurrageRegisterAccountDTO()
                {
                    Waybill = s.Waybill
                });

                //all shipments from payload JSON
                var allprocessingordeforshipment = bkoc.ShipmentAndCOD;

                var result2 = allprocessingordeforshipment.Select(s => s.Waybill);
                var allprocessingordefordemurrage = _uow.BankProcessingOrderForShipmentAndCOD.GetAll().Where(s => s.DepositType == bkoc.DepositType && result2.Contains(s.Waybill));

                if (allprocessingordefordemurrage.Count() > 0)
                {
                    throw new GenericException("Error validating one or more Demurrages, Please try requesting again for a fresh record.");
                }

                //var bankorderforshipmentandcod = Mapper.Map<List<BankProcessingOrderForShipmentAndCOD>>(allShipments);
                var bankorderforshipmentandcod = demurrageforservicecenter.Select(s => new BankProcessingOrderForShipmentAndCOD()
                {
                    Waybill = s.Waybill,
                    RefCode = bkoc.Code,
                    ServiceCenterId = bkoc.ServiceCenter,
                    CODAmount = s.Amount,
                    DepositType = bkoc.DepositType,
                    ServiceCenter = bkoc.ScName,
                    Status = DepositStatus.Pending
                });

                var arrDemurrages = demurrageforservicecenter.Select(x => x.Waybill).ToArray();
                //var arrWaybills = allShipments.Select(x => x.Waybill).ToArray();

                //select a values from 
                //var nonDepsitedValue = _uow.CashOnDeliveryRegisterAccount.GetAll().Where(x => arrCODs.Contains(x.Waybill));
                var nonDepsitedValueunprocessed = allDemurrages.Where(s => s.DepositStatus == 0).ToList();

                //Collect total shipment unproceessed and its total
                decimal demurrageTotal = 0;
                foreach (var item in nonDepsitedValueunprocessed)
                {
                    demurrageTotal += item.Amount;
                }

                bkoc.TotalAmount = demurrageTotal;
                bankordercodes = Mapper.Map<BankProcessingOrderCodes>(bkoc);
                nonDepsitedValueunprocessed.ForEach(a => a.DepositStatus = DepositStatus.Pending);
                nonDepsitedValueunprocessed.ForEach(a => a.RefCode = bkoc.Code);

                _uow.BankProcessingOrderCodes.Add(bankordercodes);
                _uow.BankProcessingOrderForShipmentAndCOD.AddRange(bankorderforshipmentandcod);

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

        public async Task<BankProcessingOrderCodesDTO> AddBankProcessingOrderCode(BankProcessingOrderCodesDTO bkoc)
        {
            try
            {
                //1. get the current service user
                var user = await _userService.retUser();
                bkoc.UserId = user.Id;
                bkoc.FullName = user.FirstName + " " + user.LastName;

                //2. get the service centers for the current user
                var scs = await _userService.GetCurrentServiceCenter();
                bkoc.ServiceCenter = scs[0].ServiceCentreId;
                bkoc.ScName = scs[0].Name;

                //3. Get Bank Deposit Module StartDate
                var globalpropertiesdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BankDepositModuleStartDate);
                string globalpropertiesdateStr = globalpropertiesdateObj?.Value;

                var globalpropertiesdate = DateTime.MinValue;
                bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

                var bankordercodes = new BankProcessingOrderCodes();

                //4. updating the startdate
                bkoc.StartDateTime = globalpropertiesdate;
                bkoc.DateAndTimeOfDeposit = DateTime.Now;
                bkoc.Status = DepositStatus.Pending;

                //5. commence preparatiion to insert records in the BankProcessingOrderForShipmentAndCOD
                var enddate = bkoc.DateAndTimeOfDeposit;

                if (bkoc.DepositType == DepositType.Shipment)
                {
                    //5.1 Collect total shipment unproceessed and its total (only cash shipments)
                    //var comboShipmentAndTotal = await GetTotalAmountAndShipments(bkoc.DateAndTimeOfDeposit, bkoc.DepositType);
                    //bkoc.TotalAmount = comboShipmentAndTotal.Item1;
                    //var allShipments = comboShipmentAndTotal.Item2;

                    //var allShipmentsVals = allShipments.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.DateCreated >= globalpropertiesdate
                    //&& s.DepartureServiceCentreId == bkoc.ServiceCenter);


                    //all shipments from payload JSON
                    var allShipmentsVals = bkoc.ShipmentAndCOD;
                    decimal totalShipment = 0;

                    var result1 = allShipmentsVals.Select(s => s.Waybill);// new InvoiceViewDTO()
                    foreach (var item in allShipmentsVals)
                    {
                        totalShipment += item.GrandTotal;
                    }
                    //{
                    //    Waybill = s.Waybill
                    //});

                    //5.2 get the shipments from [[BankProcessingOrderForShipmentAndCOD]]
                    //var allprocessingordeforshipment = await _uow.BankProcessingOrderForShipmentAndCOD.GetProcessingOrderForShipmentAndCOD(bkoc.DepositType);

                    var allprocessingordeforshipment = _uow.BankProcessingOrderForShipmentAndCOD.GetAll().Where(s => s.DepositType == bkoc.DepositType && result1.Contains(s.Waybill));

                    //var validateInsertWaybills = false;
                    if (allprocessingordeforshipment.Count() > 0)
                    {
                        throw new GenericException("Error validating one or more waybills, Please try requesting again for a fresh record.");
                    }

                    //var result2 = allprocessingordeforshipment.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    //{
                    //    Waybill = s.Waybill
                    //});

                    //var validateInsertWaybills = false;
                    //foreach (var rs in result1)
                    //{
                    //    validateInsertWaybills = result2.Any(p => p.Waybill == rs.Waybill);
                    //    if (validateInsertWaybills)
                    //    {
                    //        throw new GenericException("Error validating one or more waybills, Please try requesting again for a fresh record.");
                    //    }
                    //}

                    //var bankorderforshipmentandcod = Mapper.Map<List<BankProcessingOrderForShipmentAndCOD>>(allShipments);
                    var bankorderforshipmentandcod = allShipmentsVals.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    {
                        Waybill = s.Waybill,
                        RefCode = bkoc.Code,
                        DepositType = bkoc.DepositType,
                        GrandTotal = s.GrandTotal,
                        CODAmount = s.CODAmount,
                        ServiceCenterId = bkoc.ServiceCenter,
                        ServiceCenter = bkoc.ScName,
                        UserId = bkoc.UserId,
                        Status = DepositStatus.Pending
                    });

                    var arrWaybills = allShipmentsVals.Select(x => x.Waybill).ToArray();

                    bankordercodes = Mapper.Map<BankProcessingOrderCodes>(bkoc);
                    bankordercodes.TotalAmount = totalShipment;
                    _uow.BankProcessingOrderCodes.Add(bankordercodes);
                    _uow.BankProcessingOrderForShipmentAndCOD.AddRange(bankorderforshipmentandcod);

                    //select a list of values that contains the allshipment from the invoice view
                    var nonDepsitedValue = _uow.Shipment.GetAll().Where(x => arrWaybills.Contains(x.Waybill)).ToList();
                    var nonDepsitedValueunprocessed = nonDepsitedValue.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.DateCreated >= globalpropertiesdate).ToList();
                    nonDepsitedValueunprocessed.ForEach(a => a.DepositStatus = DepositStatus.Pending);

                }
                else if (bkoc.DepositType == DepositType.COD)
                {
                    var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;

                    //1. get data from COD register account as queryable from CashOnDeliveryRegisterAccount table
                    var allCODs = _uow.CashOnDeliveryRegisterAccount.GetCODAsQueryable();

                    allCODs = allCODs.Where(s => s.DepositStatus == DepositStatus.Unprocessed && s.PaymentType == PaymentType.Cash);
                    allCODs = allCODs.Where(s => s.CODStatusHistory == CODStatushistory.RecievedAtServiceCenter);

                    //2. convert COD to list
                    var codsforservicecenter = allCODs.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();

                    //Based on the value from CashOnDeliveryRegisterAccount table filter out the waybills
                    var result1 = codsforservicecenter.Select(s => new CashOnDeliveryRegisterAccountDTO()
                    {
                        Waybill = s.Waybill
                    });

                    //3. GEt the parent bank processing order based on COD type on BankProcessingOrderForShipmentAndCOD table
                    //var allprocessingordeforshipment = await _uow.BankProcessingOrderForShipmentAndCOD.GetProcessingOrderForShipmentAndCOD(bkoc.DepositType);

                    //all shipments from payload JSON
                    var allprocessingordeforshipment = bkoc.ShipmentAndCOD;

                    //4. Based on value collected from BankProcessingOrderForShipmentAndCOD table filter out the waybils
                    //var result2 = allprocessingordeforshipment.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    //{
                    //    Waybill = s.Waybill
                    //});


                    var result2 = allprocessingordeforshipment.Select(s => s.Waybill);
                    var allprocessingordeforcods = _uow.BankProcessingOrderForShipmentAndCOD.GetAll().Where(s => s.DepositType == bkoc.DepositType && result2.Contains(s.Waybill));

                    if (allprocessingordeforcods.Count() > 0)
                    {
                        throw new GenericException("Error validating one or more CODs, Please try requesting again for a fresh record.");
                    }


                    //5. validate to make sure the shipment is not already in BankProcessingOrderForShipmentAndCOD table
                    //var validateInsertWaybills = false;
                    //foreach (var rs in result1)
                    //{
                    //    validateInsertWaybills = result2.Any(p => p.Waybill == rs.Waybill);
                    //    if (validateInsertWaybills)
                    //    {
                    //        throw new GenericException("Error validating one or more CODs, Please try requesting again for a fresh record.");
                    //    }
                    //}

                    //var bankorderforshipmentandcod = Mapper.Map<List<BankProcessingOrderForShipmentAndCOD>>(allShipments);
                    var bankorderforshipmentandcod = codsforservicecenter.Select(s => new BankProcessingOrderForShipmentAndCOD()
                    {
                        Waybill = s.Waybill,
                        RefCode = bkoc.Code,
                        ServiceCenterId = bkoc.ServiceCenter,
                        CODAmount = s.Amount,
                        DepositType = bkoc.DepositType,
                        ServiceCenter = bkoc.ScName,
                        Status = DepositStatus.Pending
                    });

                    var arrCODs = codsforservicecenter.Select(x => x.Waybill).ToArray();
                    //var arrWaybills = allShipments.Select(x => x.Waybill).ToArray();

                    //select a values from 
                    //var nonDepsitedValue = _uow.CashOnDeliveryRegisterAccount.GetAll().Where(x => arrCODs.Contains(x.Waybill));
                    var nonDepsitedValueunprocessed = allCODs.Where(s => s.DepositStatus == 0).ToList();

                    //Collect total shipment unproceessed and its total
                    decimal codTotal = 0;
                    foreach (var item in nonDepsitedValueunprocessed)
                    {
                        codTotal += item.Amount;
                    }

                    bkoc.TotalAmount = codTotal;
                    bankordercodes = Mapper.Map<BankProcessingOrderCodes>(bkoc);
                    nonDepsitedValueunprocessed.ForEach(a => a.DepositStatus = DepositStatus.Pending);
                    nonDepsitedValueunprocessed.ForEach(a => a.RefCode = bkoc.Code);

                    _uow.BankProcessingOrderCodes.Add(bankordercodes);
                    _uow.BankProcessingOrderForShipmentAndCOD.AddRange(bankorderforshipmentandcod);

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

        /// <summary>
        /// Mark shipment bank order processing as deposited
        /// </summary>
        /// <param name="bankrefcode"></param>
        /// <returns></returns>
        public async Task UpdateBankOrderProcessingCode(BankProcessingOrderCodesDTO bankrefcode)
        {
            var bankorder = _uow.BankProcessingOrderCodes.Find(s => s.Code == bankrefcode.Code).FirstOrDefault();

            //var bankorder =  _uow.BankProcessingOrderCodes.GetAll();
            //var bankordervalue = bankorder.Where(s => s.Code == bankrefcode.CodeId);
            if (bankorder == null)
            {
                throw new GenericException("Bank Order Request Does not Exist!");
            }

            //update BankProcessingOrderCodes
            bankorder.Status = DepositStatus.Deposited;

            //Get Bank Deposit Module StartDate
            var globalpropertiesdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BankDepositModuleStartDate);
            string globalpropertiesdateStr = globalpropertiesdateObj?.Value;

            var globalpropertiesdate = DateTime.MinValue;
            bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

            //var serviceCenters = _userService.GetPriviledgeServiceCenters();
            var serviceCenters = await _userService.GetCurrentServiceCenter();
            var currentCenter = serviceCenters[0].ServiceCentreId;
            var accompanyWaybills = await _uow.BankProcessingOrderForShipmentAndCOD.GetAllWaybillsForBankProcessingOrdersAsQueryable(bankrefcode.DepositType);

            //update BankProcessingOrderForShipmentAndCOD
            var accompanyWaybillsVals = accompanyWaybills.Where(s => s.RefCode == bankrefcode.Code).ToList();
            accompanyWaybillsVals.ForEach(a => a.Status = DepositStatus.Deposited);

            var arrWaybills = accompanyWaybillsVals.Select(x => x.Waybill).ToArray();

            var nonDepsitedValueQ = _uow.Shipment.GetAll().Where(x => x.DepositStatus == DepositStatus.Pending && x.DepartureServiceCentreId == currentCenter && x.DateCreated >= globalpropertiesdate);
            var nonDepsitedValue = nonDepsitedValueQ.Where(x => arrWaybills.Contains(x.Waybill)).ToList();

            //update Shipment
            nonDepsitedValue.ForEach(a => a.DepositStatus = DepositStatus.Deposited);

            await _uow.CompleteAsync();
        }

        public async Task MarkAsVerified(BankProcessingOrderCodesDTO bankrefcode)
        {
            var bankorder = _uow.BankProcessingOrderCodes.Find(s => s.Code == bankrefcode.Code).FirstOrDefault();

            //var bankorder =  _uow.BankProcessingOrderCodes.GetAll();
            //var bankordervalue = bankorder.Where(s => s.Code == bankrefcode.CodeId);
            if (bankorder == null)
            {
                throw new GenericException("Bank Order Request Does not Exist!");
            }

            //update BankProcessingOrderCodes
            bankorder.Status = DepositStatus.Verified;
            var accompanyWaybills = await _uow.BankProcessingOrderForShipmentAndCOD.GetAllWaybillsForBankProcessingOrdersAsQueryable(bankrefcode.DepositType);

            //update BankProcessingOrderForShipmentAndCOD
            var accompanyWaybillsVals = accompanyWaybills.Where(s => s.RefCode == bankrefcode.Code).ToList();
            accompanyWaybillsVals.ForEach(a => a.Status = DepositStatus.Verified);

            var arrWaybills = accompanyWaybillsVals.Select(x => x.Waybill).ToArray();

            var nonDepsitedValueQ = _uow.Shipment.GetAll().Where(x => x.DepositStatus == DepositStatus.Deposited);
            var nonDepsitedValue = nonDepsitedValueQ.Where(x => arrWaybills.Contains(x.Waybill)).ToList();

            //update Shipment
            nonDepsitedValue.ForEach(a => a.DepositStatus = DepositStatus.Verified);

            await _uow.CompleteAsync();
        }

        /// <summary>
        /// Mark COD bank order processing as deposited
        /// </summary>
        /// <param name="bankrefcode"></param>
        /// <returns></returns>
        public async Task UpdateBankOrderProcessingCode_cod(BankProcessingOrderCodesDTO bankrefcode)
        {
            var bankorder = _uow.BankProcessingOrderCodes.Find(s => s.Code == bankrefcode.Code).FirstOrDefault();

            //var bankorder =  _uow.BankProcessingOrderCodes.GetAll();
            //var bankordervalue = bankorder.Where(s => s.Code == bankrefcode.CodeId);
            if (bankorder == null)
            {
                throw new GenericException("Bank Order Request Does not Exist!");
            }

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allCODs = _uow.CashOnDeliveryRegisterAccount.GetCODAsQueryable();
            allCODs = allCODs.Where(s => s.DepositStatus == DepositStatus.Pending);
            var codsforservicecenter = allCODs.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();

            var accompanyWaybills = await _uow.BankProcessingOrderForShipmentAndCOD.GetAllWaybillsForBankProcessingOrdersAsQueryable(bankrefcode.DepositType);

            //update BankProcessingOrderForShipmentAndCOD
            var accompanyWaybillsVals = accompanyWaybills.Where(s => s.RefCode == bankrefcode.Code).ToList();
            accompanyWaybillsVals.ForEach(a => a.Status = DepositStatus.Deposited);

            codsforservicecenter.ForEach(a => a.DepositStatus = DepositStatus.Deposited);
            bankorder.Status = bankrefcode.Status;

            await _uow.CompleteAsync();
        }

        public async Task MarkAsVerified_cod(BankProcessingOrderCodesDTO bankrefcode)
        {
            var bankorder = _uow.BankProcessingOrderCodes.Find(s => s.Code == bankrefcode.Code).FirstOrDefault();

            //var bankorder =  _uow.BankProcessingOrderCodes.GetAll();
            //var bankordervalue = bankorder.Where(s => s.Code == bankrefcode.CodeId);
            if (bankorder == null)
            {
                throw new GenericException("Bank Order Request Does not Exist!");
            }

            //var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allCODs = _uow.CashOnDeliveryRegisterAccount.GetCODAsQueryable();
            allCODs = allCODs.Where(s => s.DepositStatus == DepositStatus.Deposited && s.RefCode == bankrefcode.Code);

            //allCODs = allCODs.Where(s => s.RefCode == bankrefcode.Code);
            var allCODsResult = allCODs.ToList();
            //var codsforservicecenter = allCODs.Where(s => serviceCenters.Contains(s.ServiceCenterId)).ToList();

            //var serviceCenters2 = await _userService.GetCurrentServiceCenter();
            //var currentCenter = serviceCenters2[0].ServiceCentreId;
            var accompanyWaybills = await _uow.BankProcessingOrderForShipmentAndCOD.GetAllWaybillsForBankProcessingOrdersAsQueryable(bankrefcode.DepositType);

            //update BankProcessingOrderForShipmentAndCOD
            var accompanyWaybillsVals = accompanyWaybills.Where(s => s.RefCode == bankrefcode.Code).ToList();
            accompanyWaybillsVals.ForEach(a => a.Status = DepositStatus.Verified);

            allCODsResult.ForEach(a => a.DepositStatus = DepositStatus.Verified);
            bankorder.Status = bankrefcode.Status;

            await _uow.CompleteAsync();
        }




        public async Task UpdateBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO refcodeobj)
        {
            var bankorder = await _uow.BankProcessingOrderForShipmentAndCOD.GetAsync(refcodeobj.ProcessingOrderId);
            bankorder.Status = DepositStatus.Deposited;

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
