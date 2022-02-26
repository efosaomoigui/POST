using GIGLS.Core.IServices.PaymentTransactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IMessageService;
using System.Security.Cryptography;
using System.Text;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGL.GIGLS.Core.Domain;
using System.Linq;
using GIGLS.Core.IServices.Utility;

namespace GIGLS.Services.Implementation.PaymentTransactions
{
    public class PaymentPartialTransactionService : IPaymentPartialTransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IFinancialReportService _financialReportService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        public PaymentPartialTransactionService(IUnitOfWork uow, IUserService userService, IWalletService walletService, 
            IMessageSenderService messageSenderService, IFinancialReportService financialReportService, INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _uow = uow;
            _userService = userService;
            _walletService = walletService;
            _messageSenderService = messageSenderService;
            _financialReportService = financialReportService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;

            MapperConfig.Initialize();
        }

        //used for transaction, hence private
        private async Task<object> AddPaymentPartialTransaction(PaymentPartialTransactionDTO paymentPartialTransaction)
        {
            if (paymentPartialTransaction == null)
                throw new GenericException("Null Input");

            var payment = Mapper.Map<PaymentPartialTransaction>(paymentPartialTransaction);
            _uow.PaymentPartialTransaction.Add(payment);
            return await Task.FromResult(new { Id = payment.PaymentPartialTransactionId });
        }

        public async Task<IEnumerable<PaymentPartialTransactionDTO>> GetPaymentPartialTransactionById(string waybill)
        {
            var transaction = await _uow.PaymentPartialTransaction.FindAsync(x => x.Waybill.Equals(waybill));
            return Mapper.Map<IEnumerable<PaymentPartialTransactionDTO>>(transaction);
        }

        public Task<IEnumerable<PaymentPartialTransactionDTO>> GetPaymentPartialTransactions()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<PaymentPartialTransactionDTO>>(_uow.PaymentPartialTransaction.GetAll()));
        }

        public async Task RemovePaymentPartialTransaction(string waybill)
        {
            var transaction = await _uow.PaymentPartialTransaction.GetAsync(x => x.Waybill.Equals(waybill));

            if (transaction == null)
            {
                throw new GenericException("Payment Partial Transaction does not exist");
            }
            _uow.PaymentPartialTransaction.Remove(transaction);
            await _uow.CompleteAsync();
        }

        public async Task UpdatePaymentPartialTransaction(string waybill, PaymentPartialTransactionDTO paymentPartialTransaction)
        {
            if (paymentPartialTransaction == null)
                throw new GenericException("Null Input");

            var payment = await _uow.PaymentPartialTransaction.GetAsync(x => x.Waybill.Equals(waybill));
            if (payment == null)
                throw new GenericException($"No Payment Partial Transaction exist for {waybill} waybill");

            payment.TransactionCode = paymentPartialTransaction.TransactionCode;
            payment.PaymentStatus = paymentPartialTransaction.PaymentStatus;
            payment.PaymentType = paymentPartialTransaction.PaymentType;
            await _uow.CompleteAsync();
        }

        public async Task<bool> ProcessPaymentPartialTransaction(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            var result = false;

            if (paymentPartialTransactionProcessDTO == null)
                throw new GenericException("Null Input");

            decimal cash = 0;
            decimal pos = 0;
            decimal transfer = 0;
            string cashType = null;
            string posType = null;
            string transferType = null;

            foreach (var item in paymentPartialTransactionProcessDTO.PaymentPartialTransactions)
            {
                if (item.PaymentType == PaymentType.Cash)
                {
                    cash += item.Amount;
                    cashType = item.PaymentType.ToString();
                }
                else if (item.PaymentType == PaymentType.Pos)
                {
                    pos += item.Amount;
                    posType = item.PaymentType.ToString();
                }
                else if (item.PaymentType == PaymentType.Transfer)
                {
                    transfer += item.Amount;
                    transferType = item.PaymentType.ToString();
                }
            }

            // get the current user info
            var currentUserId = await _userService.GetCurrentUserId();

            //get Ledger and Invoice
            var waybill = paymentPartialTransactionProcessDTO.Waybill;
            var generalLedgerEntity = await _uow.GeneralLedger.GetAsync(s => s.Waybill == waybill);
            var invoiceEntity = await _uow.Invoice.GetAsync(s => s.Waybill == waybill);
            var shipment = await _uow.Shipment.GetAsync(s => s.Waybill == waybill);

            //get the GrandTotal Amount to be paid
            var grandTotal = invoiceEntity.Amount;

            //get total amount already paid
            decimal totalAmountAlreadyPaid = 0;
            var partialTransactionsForWaybill = await _uow.PaymentPartialTransaction.FindAsync(x => x.Waybill.Equals(waybill));
            foreach (var item in partialTransactionsForWaybill)
            {
                totalAmountAlreadyPaid += item.Amount;
            }

            //get total amount customer is paying
            decimal totalAmountPaid = 0;
            foreach (var paymentPartialTransaction in paymentPartialTransactionProcessDTO.PaymentPartialTransactions)
            {
                //settlement by wallet
                if (paymentPartialTransaction.PaymentType == PaymentType.Wallet)
                {
                    //I used transaction code to represent wallet number when process for wallet
                    var wallet = await _walletService.GetWalletById(paymentPartialTransaction.TransactionCode);

                    //deduct the price for the wallet and update wallet transaction table
                    if (wallet.Balance < paymentPartialTransaction.Amount)
                    {
                        throw new GenericException("Insufficient Balance in the Wallet");
                    }

                    wallet.Balance = wallet.Balance - paymentPartialTransaction.Amount;

                    var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

                    var newWalletTransaction = new WalletTransaction
                    {
                        WalletId = wallet.WalletId,
                        Amount = paymentPartialTransaction.Amount,
                        DateOfEntry = DateTime.Now,
                        ServiceCentreId = serviceCenterIds[0],
                        UserId = currentUserId,
                        CreditDebitType = CreditDebitType.Debit,
                        PaymentType = PaymentType.Wallet,
                        Waybill = waybill,
                        Description = generalLedgerEntity.Description
                    };

                    _uow.WalletTransaction.Add(newWalletTransaction);
                }

                // create paymentPartial
                paymentPartialTransaction.Waybill = waybill;
                paymentPartialTransaction.UserId = currentUserId;
                paymentPartialTransaction.PaymentStatus = PaymentStatus.Paid;
                var paymentTransactionId = await AddPaymentPartialTransaction(paymentPartialTransaction);


                totalAmountPaid += paymentPartialTransaction.Amount;
            }
            // get the balance
            decimal balanceAmount = (grandTotal - totalAmountAlreadyPaid) - totalAmountPaid;

            //if customer over pays, throw an exception
            if (balanceAmount < 0)
            {
                throw new GenericException($"The amount of {totalAmountPaid} you are trying to pay is greater than {grandTotal - totalAmountAlreadyPaid} balance you are to pay.");
            }

            /////2.  When payment is complete and balance is 0
            if (balanceAmount == 0)
            {
                foreach (var item in partialTransactionsForWaybill)
                {
                    if (item.PaymentType == PaymentType.Cash)
                    {
                        cash += item.Amount;
                        cashType = item.PaymentType.ToString();
                    }
                    else if (item.PaymentType == PaymentType.Pos)
                    {
                        pos += item.Amount;
                        posType = item.PaymentType.ToString();
                    }
                    else if (item.PaymentType == PaymentType.Transfer)
                    {
                        transfer += item.Amount;
                        transferType = item.PaymentType.ToString();
                    }
                }

                // update GeneralLedger
                generalLedgerEntity.IsDeferred = false;
                generalLedgerEntity.PaymentType = PaymentType.Partial;
                //generalLedgerEntity.PaymentTypeReference = paymentPartialTransaction.TransactionCode;

                //update invoice
                invoiceEntity.PaymentDate = DateTime.Now;
                //invoiceEntity.PaymentMethod = PaymentType.Partial.ToString();
                invoiceEntity.PaymentMethod = PaymentType.Partial.ToString() + " - " + (cashType != null ? cashType + "(" + cash + ") " : "") +
                                                                      (posType != null ? posType + "(" + pos + ") " : "") +
                                                                      (transferType != null ? transferType + "(" + transfer + ") " : "");
                invoiceEntity.Cash = cash;
                invoiceEntity.Transfer = transfer;
                invoiceEntity.Pos = pos;
                invoiceEntity.PaymentStatus = PaymentStatus.Paid;
            }

            await _uow.CompleteAsync();
            result = true;

            /////2.  When payment is complete and balance is 0, send sms to customer
            if (balanceAmount == 0)
            {
                if (shipment != null)
                {
                    //Add to Financial Reports
                    var financialReport = new FinancialReportDTO
                    {
                        Source = ReportSource.Agility,
                        Waybill = shipment.Waybill,
                        PartnerEarnings = 0.0M,
                        GrandTotal = invoiceEntity.Amount,
                        Earnings = invoiceEntity.Amount,
                        Demurrage = 0.00M,
                        CountryId = invoiceEntity.CountryId
                    };

                    await _financialReportService.AddReport(financialReport);

                    //QR Code
                    var deliveryNumber = await _uow.DeliveryNumber.GetAsync(s => s.Waybill == shipment.Waybill);

                    //send sms to the customer
                    var smsData = new Core.DTO.Shipments.ShipmentTrackingDTO
                    {
                        Waybill = invoiceEntity.Waybill,
                        QRCode = deliveryNumber.SenderCode
                    };

                    if (shipment.DepartureServiceCentreId == 309)
                    {
                        await _messageSenderService.SendMessage(MessageType.HOUSTON, EmailSmsType.SMS, smsData);
                        await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.Email, smsData);
                    }
                    else
                    {
                        await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.All, smsData);
                    }
                }
            }
            if (result)
            {
                //grouping and manifesting shipment
                if (shipment.IsBulky)
                {
                    await MappingWaybillNumberToGroupForBulk(shipment.Waybill);
                }
                else
                {
                    await MappingWaybillNumberToGroup(shipment.Waybill);
                }
            }

            return result;
        }

        private async Task<DeliveryNumberDTO> GenerateDeliveryNumber(int value, string waybill)
        {
            int maxSize = 6;
            char[] chars = new char[54];
            string a;
            a = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            var strippedText = result.ToString();
            var number = new DeliveryNumber
            {
                SenderCode = "DN" + strippedText.ToUpper(),
                IsUsed = false,
                Waybill = waybill
            };
            var deliverynumberDTO = Mapper.Map<DeliveryNumberDTO>(number);
            _uow.DeliveryNumber.Add(number);
            await _uow.CompleteAsync();
            return await Task.FromResult(deliverynumberDTO);
        }


        #region IMPLEMENTING GROUPING AND MANIFESTING

        //map waybillNumber to groupWaybillNumber
        private async Task MappingWaybillNumberToGroup(string waybill)
        {
            try
            {
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                //validate waybill 
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                if (shipment is null)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();
                var destServiceCentre = await _uow.ServiceCentre.GetAsync(shipment.DestinationServiceCentreId);
                var deptServiceCentre = await _uow.ServiceCentre.GetAsync(departureServiceCenterId);
                var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));



                //check if the group already exist for centre
                var groupwaybillExist = _uow.GroupWaybillNumber.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId && x.ExpressDelivery == shipment.ExpressDelivery && x.IsBulky == false).FirstOrDefault();
                if (groupwaybillExist == null)
                {
                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                }
                else
                {

                    //check if it has a manifest mapping
                    var isManifestGroupWaybillMapped = _uow.ManifestGroupWaybillNumberMapping.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.GroupWaybillNumber == groupwaybillExist.GroupWaybillCode).FirstOrDefault();
                    if (isManifestGroupWaybillMapped is null)
                    {
                        //map new waybill to existing groupwaybill 
                        await CreateNewManifestGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupwaybillExist);
                    }

                    else
                    {
                        //confirm if the manifest has been dispatched
                        var manifestDispatched = await _uow.Manifest.ExistAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode && x.IsDispatched);
                        if (manifestDispatched)
                        {
                            await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                        }
                        else
                        {
                            var manifest = await _uow.Manifest.GetAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode);
                            if (manifest is null)
                            {
                                await CreateNewManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupwaybillExist);
                            }
                            else if (manifest != null)
                            {
                                //get date for the manifest
                                var today = DateTime.Now;
                                int hours = Convert.ToInt32((today - manifest.DateCreated).TotalHours);
                                if (hours >= 24)
                                {
                                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                                }
                                else
                                {
                                    //map new waybill to existing groupwaybill 
                                    await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, manifest, groupwaybillExist);
                                }
                            }
                        }
                    }
                }
                shipment.IsGrouped = true;
                var updateTransitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipment.Waybill);
                if (updateTransitWaybill != null)
                {
                    updateTransitWaybill.IsGrouped = true;
                }
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private async Task MappingWaybillNumberToGroupForBulk(string waybill)
        {
            try
            {
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                //validate waybill 
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                if (shipment is null)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();
                var destServiceCentre = await _uow.ServiceCentre.GetAsync(shipment.DestinationServiceCentreId);
                var deptServiceCentre = await _uow.ServiceCentre.GetAsync(departureServiceCenterId);
                var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));



                //check if the bulk manifest exist
                var bulkManifest = _uow.Manifest.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.IsBulky && !x.IsDispatched && x.ExpressDelivery == shipment.ExpressDelivery).FirstOrDefault();
                if (bulkManifest is null)
                {
                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                }
                else
                {
                    var today = DateTime.Now;
                    int hours = Convert.ToInt32((today - bulkManifest.DateCreated).TotalHours);
                    if (hours >= 24)
                    {
                        await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                    }
                    else
                    {
                        var groupwaybillExist = _uow.GroupWaybillNumber.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId && x.ExpressDelivery == shipment.ExpressDelivery && x.IsBulky == shipment.IsBulky).FirstOrDefault();
                        if (groupwaybillExist == null)
                        {
                            bulkManifest = _uow.Manifest.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.IsBulky && !x.IsDispatched && x.ExpressDelivery == shipment.ExpressDelivery).FirstOrDefault();
                            await MapNewGroupWaybillToExistingManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest);
                        }
                        else
                        {
                            //map new waybill to existing groupwaybill
                            var isManifestGroupWaybillMapped = _uow.ManifestGroupWaybillNumberMapping.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.GroupWaybillNumber == groupwaybillExist.GroupWaybillCode).FirstOrDefault();
                            if (isManifestGroupWaybillMapped is null)
                            {
                                //map new waybill to existing groupwaybill 
                                await CreateNewManifestGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupwaybillExist);
                            }
                            var manifestDispatched = await _uow.Manifest.ExistAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode && x.IsDispatched);
                            if (manifestDispatched)
                            {
                                await MapNewGroupWaybillToExistingManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest);
                            }
                            else
                            {
                                await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest, groupwaybillExist);
                            }
                        }
                    }
                }
                shipment.IsGrouped = true;
                var updateTransitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipment.Waybill);
                if (updateTransitWaybill != null)
                {
                    updateTransitWaybill.IsGrouped = true;
                }
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private async Task NewGroupWaybillProcess(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId)
        {
            // generate new manifest code
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, destServiceCentre.Code);

            // create a group waybillnumbermapping
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybillNumber,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);

            //also create a minifest group manifest and add group waybill to it
            //Add new Mapping
            var newMapping = new ManifestGroupWaybillNumberMapping
            {
                ManifestCode = manifestCode,
                GroupWaybillNumber = newGroupWaybill.GroupWaybillCode,
                IsActive = true,
                DateMapped = DateTime.Now
            };
            _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);

            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky
            };
            _uow.Manifest.Add(newManifest);
        }

        private async Task CreateNewManifestGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, GroupWaybillNumber groupWaybill)
        {
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky
            };
            _uow.Manifest.Add(newManifest);

            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                //also  map group waybill to existing manifest
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

        }

        private async Task MapExistingGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, Manifest manifest, GroupWaybillNumber groupWaybill)
        {
            //also  map group waybill to existing manifest
            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifest.ManifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

        }

        private async Task CreateNewManifest(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, GroupWaybillNumber groupWaybill)
        {
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky
            };
            _uow.Manifest.Add(newManifest);

            //also  map group waybill to existing manifest
            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);
        }

        private async Task MapNewGroupWaybillToExistingManifest(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, Manifest manifest)
        {
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, destServiceCentre.Code);

            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);

            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
            if (!exist)
            {
                //also  map group waybill to existing manifest
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifest.ManifestCode,
                    GroupWaybillNumber = groupWaybillNumber,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybillNumber,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);
        }

        #endregion

    }
}