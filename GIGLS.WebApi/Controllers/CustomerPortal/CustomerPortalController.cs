using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [Authorize]
    [RoutePrefix("api/portal")]
    public class CustomerPortalController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;

        public CustomerPortalController(ICustomerPortalService portalService) : base(nameof(CustomerPortalController))
        {
            _portalService = portalService;
        }

        [HttpPost]
        [Route("transaction")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _portalService.GetShipmentTransactions(f_Criteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices
                };
            });
        }

        [HttpGet]
        [Route("wallet")]
        public async Task<IServiceResponse<WalletTransactionSummaryDTO>> GetWalletTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactionSummary = await _portalService.GetWalletTransactions();

                return new ServiceResponse<WalletTransactionSummaryDTO>
                {
                    Object = walletTransactionSummary
                };
            });
        }

        [HttpGet]
        [Route("invoice")]
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetInvoices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetInvoices();

                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = invoice
                };
            });
        }

        [HttpGet]
        [Route("bywaybill/{waybill}")]
        public async Task<IServiceResponse<InvoiceDTO>> GetInvoiceByWaybill([FromUri]  string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetInvoiceByWaybill(waybill);

                return new ServiceResponse<InvoiceDTO>
                {
                    Object = invoice
                };
            });
        }

        [HttpGet]
        [Route("{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> TrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.TrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("public/{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> PublicTrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.PublicTrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("cod")]
        public async Task<IServiceResponse<CashOnDeliveryAccountSummaryDTO>> GetCashOnDeliveryAccount()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.GetCashOnDeliveryAccount();

                return new ServiceResponse<CashOnDeliveryAccountSummaryDTO>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("partialPaymentTransaction/{waybill}")]
        public async Task<IServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>> GetPartialPaymentTransaction(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _portalService.GetPartialPaymentTransaction(waybill);

                return new ServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>
                {
                    Object = payment
                };
            });
        }

        [HttpGet]
        [Route("dashboard")]
        public async Task<IServiceResponse<DashboardDTO>> GetDashboard()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dashboard = await _portalService.GetDashboard();

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }

        [HttpGet]
        [Route("state")]
        public async Task<IServiceResponse<IEnumerable<StateDTO>>> GetStates(int pageSize = 10, int page = 1)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _portalService.GetStates(pageSize, page);
                var total = _portalService.GetStatesTotal();

                return new ServiceResponse<IEnumerable<StateDTO>>
                {
                    Total = total,
                    Object = state
                };
            });
        }

        [HttpGet]
        [Route("localservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetLocalServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _portalService.GetLocalServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [HttpGet]
        [Route("deliveryoption")]
        public async Task<IServiceResponse<IEnumerable<DeliveryOptionDTO>>> GetDeliveryOptions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _portalService.GetDeliveryOptions();

                return new ServiceResponse<IEnumerable<DeliveryOptionDTO>>
                {
                    Object = delivery
                };
            });
        }

        [HttpGet]
        [Route("specialdomesticpackage")]
        public async Task<IServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>> GetSpecialDomesticPackages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packages = await _portalService.GetSpecialDomesticPackages();

                return new ServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>
                {
                    Object = packages
                };
            });
        }

        [HttpGet]
        [Route("haulage")]
        public async Task<IServiceResponse<IEnumerable<HaulageDTO>>> GetHaulages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulage = await _portalService.GetHaulages();

                return new ServiceResponse<IEnumerable<HaulageDTO>>
                {
                    Object = haulage
                };
            });
        }

        [HttpGet]
        [Route("vat")]
        public async Task<IServiceResponse<VATDTO>> GetVATs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vat = await _portalService.GetVATs();
                return new ServiceResponse<VATDTO>
                {
                    Object = vat.FirstOrDefault()
                };
            });
        }

        [HttpGet]
        [Route("insurance")]
        public async Task<IServiceResponse<InsuranceDTO>> GetInsurances()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var insurance = await _portalService.GetInsurances();
                return new ServiceResponse<InsuranceDTO>
                {
                    Object = insurance.FirstOrDefault()
                };
            });
        }

        [HttpGet]
        [Route("{departure:int}/{destination:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int departure, int destination)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _portalService.GetZone(departure, destination);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }
        
        [HttpPost]
        [Route("price")]
        public async Task<IServiceResponse<decimal>> GetPrice(PricingDTO pricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _portalService.GetPrice(pricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }
        
        [HttpPost]
        [Route("haulageprice")]
        public async Task<IServiceResponse<decimal>> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _portalService.GetHaulagePrice(haulagePricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }
        
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IServiceResponse<CustomerDTO>> GetUser(string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _portalService.GetCustomer(userId);
                return new ServiceResponse<CustomerDTO>
                {
                    Object = user
                };
            });
        }

        [HttpPut]
        [Route("changepassword/{userid}/{currentPassword}/{newPassword}")]
        public async Task<IServiceResponse<bool>> ChangePassword(string userid, string currentPassword, string newPassword)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.ChangePassword(userid, currentPassword, newPassword);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete successfully");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
