using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Account;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _uow;
        private INumberGeneratorMonitorService _service { get; set; }
        private IShipmentService _shipmentService { get; set; }
        private ICustomerService _customerService { get; set; }
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IGlobalPropertyService _globalPropertyService;

        public InvoiceService(IUnitOfWork uow, INumberGeneratorMonitorService service,
            IShipmentService shipmentService, ICustomerService customerService,
            IMessageSenderService messageSenderService,
            IUserService userService,
            IWalletService walletService,
            IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _service = service;
            _shipmentService = shipmentService;
            _customerService = customerService;
            _userService = userService;
            _walletService = walletService;
            _messageSenderService = messageSenderService;
            _globalPropertyService = globalPropertyService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<InvoiceDTO>> GetInvoices()
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var invoices = await _uow.Invoice.GetInvoicesAsync(serviceCenterIds);
            return invoices.ToList().OrderByDescending(x => x.DateCreated);
        }

        public async Task<string> SendEmailForDueInvoices(int daystoduedate)
        {

            //1.Get start date for this feature
            var globalpropertiesreminderdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.globalpropertiesreminderdate);
            string globalpropertiesdateStr = globalpropertiesreminderdateObj?.Value;

            var globalpropertiesdate = DateTime.MinValue;
            bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

            var invoices = _uow.Invoice.GetInvoicesForReminderAsync();
            var invoiceResults = invoices.Where(s => s.DateCreated >= globalpropertiesdate).ToList();

            //2. get today's date
            DateTime today = DateTime.Now;

            var allinvoiceintherange = new List<InvoiceView>();

            //3. filter all due invoices by due date
            foreach (var invoice in invoices)
            {
                var dateofexpiry = today.AddDays(daystoduedate).ToShortDateString();
                var duedateinview = invoice.DueDate.ToShortDateString();

                if (duedateinview.Equals(dateofexpiry))
                {
                    allinvoiceintherange.Add(invoice);
                }
            }

            string message = "";
            int count = 0;

            //4. send Email For all Due Invoices
            if (allinvoiceintherange != null)
            {
                foreach (var invoice in allinvoiceintherange)
                {
                    InvoiceViewDTO invoiceviewDTO = Mapper.Map<InvoiceViewDTO>(invoice);

                    invoiceviewDTO.PhoneNumber = invoice.PhoneNumber;
                    invoiceviewDTO.InvoiceDueDays = daystoduedate.ToString(); //invoice.DueDate.AddDays(daystoduedate);

                    await _messageSenderService.SendGenericEmailMessage(MessageType.IEMAIL, invoiceviewDTO);
                    message = "Operation Completed Successfully";
                    count++;
                }
            }
            else
            {
                message = "No Due Invoices at this time!";
            }

            return message + " Count:" + count;
        }

        public async Task<string> SendEmailForWalletBalanceCheck(decimal amountforreminder) 
        {

            //1.Get start date for this feature
            var globalpropertiesreminderdateObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.globalpropertiesreminderdate);
            string globalpropertiesdateStr = globalpropertiesreminderdateObj?.Value;

            var globalpropertiesdate = DateTime.MinValue;
            bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesdate);

            //get all wallets matching the amountstatedforreminder supplied
            var wallets = _walletService.GetWalletAsQueryableService();
            var allwalletsintherange = wallets.Where(s => s.Balance.Equals(amountforreminder)).ToList();
            var walletthatqualifies_Result = allwalletsintherange.Select(s => s.CustomerCode).ToArray();

            //Get all the customers who are Ecommercce
            var users = _userService.GetCorporateCustomerUsersAsQueryable();
            var usersResults = users.Where(s=>s.UserChannelType == UserChannelType.Ecommerce );
            
            var invResults = usersResults.Where(s => walletthatqualifies_Result.Contains(s.UserChannelCode)).ToList();

            string message = "";
            int count = 0;

            //4. send Email For all Due Invoices
            if (invResults != null)
            {
                foreach (var user in invResults)
                {
                    InvoiceViewDTO invoiceviewDTO = new InvoiceViewDTO();
                    var wallinfo = allwalletsintherange.Find(s=>s.CustomerCode == user.UserChannelCode);
                    invoiceviewDTO.PhoneNumber = user.PhoneNumber;
                    invoiceviewDTO.WalletBalance = wallinfo.Balance.ToString();

                    await _messageSenderService.SendGenericEmailMessage(MessageType.WEMAIL, invoiceviewDTO);
                    count++;
                }

                message = "Operation Completed Successfully";
            }
            else
            {
                message = "No Due Invoices at this time!";
            }

            return message+" Count:"+count;
        }

        public Tuple<Task<List<InvoiceDTO>>, int> GetInvoices(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all invoices by servicecentre
                var serviceCenterIds = _userService.GetPriviledgeServiceCenters().Result;
                var invoicesDto = _uow.Invoice.GetInvoicesAsync(serviceCenterIds).Result;
                invoicesDto = invoicesDto.OrderByDescending(x => x.DateCreated);

                var count = invoicesDto.ToList().Count();

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        invoicesDto = invoicesDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(Invoice).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            invoicesDto = invoicesDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            invoicesDto = invoicesDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    invoicesDto = invoicesDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<Task<List<InvoiceDTO>>, int>(Task.FromResult(invoicesDto.ToList()), count);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<InvoiceDTO> GetInvoiceById(int invoiceId)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            var invoiceDTO = Mapper.Map<InvoiceDTO>(invoice);

            // get Shipment
            var waybill = invoiceDTO.Waybill;
            invoiceDTO.Shipment = await _shipmentService.GetShipment(waybill);

            // get Customer
            invoiceDTO.Customer = invoiceDTO.Shipment.CustomerDetails;

            //To get amount paid for demurrage

            if (invoiceDTO.IsShipmentCollected)
            {
                var demurage = await _uow.Demurrage.GetAsync(s => s.WaybillNumber == invoiceDTO.Waybill);

                if (demurage != null)
                {
                    invoiceDTO.Shipment.Demurrage.AmountPaid = demurage.AmountPaid;
                    invoiceDTO.Shipment.Demurrage.ApprovedBy = demurage.ApprovedBy;
                }
            }         
           
            //get wallet number
            if (invoiceDTO.Customer.CustomerType == CustomerType.Company)
            {
                var wallet = await _uow.Wallet.GetAsync(
                    s => s.CustomerId == invoiceDTO.Customer.CompanyId &&
                    s.CustomerType == CustomerType.Company);
                invoiceDTO.Customer.WalletNumber = wallet?.WalletNumber;
            }
            else
            {
                var wallet = await _uow.Wallet.GetAsync(
                    s => s.CustomerId == invoiceDTO.Customer.IndividualCustomerId &&
                    s.CustomerType == CustomerType.IndividualCustomer);
                invoiceDTO.Customer.WalletNumber = wallet?.WalletNumber;
            }

            ///// Partial Payments, if invoice status is pending
            if (invoiceDTO.PaymentStatus == PaymentStatus.Pending)
            {
                var partialTransactionsForWaybill = await _uow.PaymentPartialTransaction.FindAsync(x => x.Waybill.Equals(waybill));

                if (partialTransactionsForWaybill.Count() > 0)
                {
                    invoiceDTO.PaymentPartialTransaction = new PaymentPartialTransactionProcessDTO()
                    {
                        Waybill = waybill,
                        PaymentPartialTransactions = Mapper.Map<List<PaymentPartialTransactionDTO>>(partialTransactionsForWaybill)
                    };
                }
            }

            return invoiceDTO;
        }

        public async Task<InvoiceDTO> GetInvoiceByWaybill(string waybl)
        {
            //var invoices = await GetInvoices();
            var invoice = await _uow.Invoice.GetAsync(e => e.Waybill == waybl);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            return await GetInvoiceById(invoice.InvoiceId);
        }

        public async Task<object> AddInvoice(InvoiceDTO invoiceDto)
        {
            var newInvoice = Mapper.Map<Invoice>(invoiceDto);
            newInvoice.InvoiceNo = await _service.GenerateNextNumber(NumberGeneratorType.Invoice);

            _uow.Invoice.Add(newInvoice);
            await _uow.CompleteAsync();
            return new { id = newInvoice.InvoiceId };
        }

        public async Task UpdateInvoice(int invoiceId, InvoiceDTO invoiceDto)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            invoice.InvoiceId = invoiceId;
            invoice.InvoiceNo = invoiceDto.InvoiceNo;
            invoice.PaymentDate = invoiceDto.PaymentDate;
            invoice.PaymentMethod = invoiceDto.PaymentMethod;
            invoice.PaymentStatus = invoiceDto.PaymentStatus;
            invoice.Waybill = invoiceDto.Waybill;

            await _uow.CompleteAsync();
        }

        public async Task RemoveInvoice(int invoiceId)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }
            _uow.Invoice.Remove(invoice);
            await _uow.CompleteAsync();
        }
    }
}
