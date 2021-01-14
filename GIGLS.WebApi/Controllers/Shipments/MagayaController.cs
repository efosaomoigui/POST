using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.User;
using ThirdParty.WebServices.Magaya.DTO;
using ThirdParty.WebServices.Magaya.Business.New;
using ThirdParty.WebServices.Magaya.Services;
using System.ServiceModel;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Customers;
using GIGLS.CORE.DTO.Shipments;
using System;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.Enums;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    /// <summary>
    /// 
    /// </summary>
    //[AllowAnonymous]
    [RoutePrefix("api/shipment/magaya")]
    public class MagayaController : BaseWebApiController
    {
        private readonly IMagayaService _service;
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userService"></param>
        public MagayaController(IMagayaService service, IUserService userService) : base(nameof(MagayaController))
        {
            _service = service;
            _userService = userService;

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
        }


        //[GIGLSActivityAuthorize(Activity = "Create")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MagayaShipmentDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddShipment")]
        public async Task<IServiceResponse<Tuple<api_session_error, string, string>>> AddShipment(TheWarehouseReceiptCombo MagayaShipmentDTO)   
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.SetTransactions(access_key, MagayaShipmentDTO); 

                //4. Pass the return to the view or caller
                return new ServiceResponse<Tuple<api_session_error, string, string>>()
                {
                    Object = await result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitydto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEntity")]
        public async Task<IServiceResponse<string>> AddEntity(EntityDto entitydto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.SetEntity(access_key, entitydto);

                //3. Pass the return to the view or caller
                return new ServiceResponse<string>()
                {
                    Object = result
                };
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitydto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEntityInternational")]
        public async Task<IServiceResponse<string>> AddEntityInternational(CustomerDTO custDTo)  
        {
            return await HandleApiOperationAsync(async () =>
            {

                //3. Call the Magaya SetTransaction Method from MagayaService
                var SetCustomerResult = _service.SetEntityIntl(custDTo);

                //3. Pass the return to the view or caller
                return new ServiceResponse<string>()
                {
                    Object = SetCustomerResult.Result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("internationalShipmentRequest/{requestNumber}")]
        public async Task<IServiceResponse<IntlShipmentRequestDTO>> GetShipment(string requestNumber) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.GetShipmentRequest(requestNumber);
                return new ServiceResponse<IntlShipmentRequestDTO>
                {
                    Object = shipment
                };
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startwithstring"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntity")]
        public async Task<IServiceResponse<EntityList>> GetEntities(string startwithstring)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.GetEntities(access_key, startwithstring);

                //3. Pass the return to the view or caller
                return new ServiceResponse<EntityList>()
                {
                    Object = result
                };
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startwithstring"></param>
        /// <returns></returns>
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetIntltransactionRequest")]
        public async Task<IServiceResponse<Tuple<List<IntlShipmentDTO>, int>>> GetIntltransactionRequest([FromUri]FilterOptionsDto filterOptionsDto) 
        {
            //filter by User Active Country
            var userActiveCountry = await _userService.GetUserActiveCountry();
            filterOptionsDto.CountryId = userActiveCountry?.CountryId;

            return await HandleApiOperationAsync(async () =>
            {
                var result = _service.getIntlShipmentRequests(filterOptionsDto);

                //3. Pass the return to the view or caller
                return new ServiceResponse<Tuple<List<IntlShipmentDTO>, int>>()
                {
                    Object = result.Result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("GetIntltransactionRequest")]
        public async Task<IServiceResponse<Tuple<List<IntlShipmentDTO>, int>>> GetIntltransactionRequest(DateFilterCriteria filterOptionsDto)
        {
            //filter by User Active Country
            var userActiveCountry = await _userService.GetUserActiveCountry();
            filterOptionsDto.CountryId = (int)userActiveCountry?.CountryId;

            return await HandleApiOperationAsync(async () =>
            {
                var result = _service.GetIntlShipmentRequests(filterOptionsDto);

                //3. Pass the return to the view or caller
                return new ServiceResponse<Tuple<List<IntlShipmentDTO>, int>>()
                {
                    Object = result.Result
                };
            });
        }

        [HttpGet]
        [Route("GetEntitiesObject")]
        public async Task<IServiceResponse<EntityList>> GetEntitiesObject()
        {
            return await HandleApiOperationAsync(async () =>
            {
                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.GetEntityObect();

                //3. Pass the return to the view or caller
                return new ServiceResponse<EntityList>()
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("QueryLog")]
        public async Task<IServiceResponse<GUIDItemList>> QueryLog(QuerylogDt0 querydto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.QueryLog(access_key, querydto);

                //3. Pass the return to the view or caller
                return new ServiceResponse<GUIDItemList>()
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("LargeQueryLog")]
        public async Task<IServiceResponse<TransactionResults>> LargeQueryLog(QuerylogDt0 querydto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.LargeQueryLog(access_key, querydto); 

                //3. Pass the return to the view or caller
                return new ServiceResponse<TransactionResults>()
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetModeOfTransportation")]
        public async Task<IServiceResponse<List<ModeOfTransportation>>> GetModeOfTransportation()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.GetModesOfTransportation();

                return new ServiceResponse<List<ModeOfTransportation>>()
                {
                    Object = _result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMagayaWaybillNo")]
        public async Task<IServiceResponse<string>> GetMagayaWaybillNo([FromUri] NumberGeneratorType numbertype = NumberGeneratorType.MagayaWb) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.GetMagayaWayBillNumber(numbertype);

                return new ServiceResponse<string>()
                {
                    Object = await _result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPorts")]
        public async Task<IServiceResponse<PortList>> GetPorts()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.GetPorts();

                return new ServiceResponse<PortList>()
                {
                    Object = _result
                };
            });
        }

        [HttpGet]
        [Route("GetDestinationServiceCenters")]
        public async Task<IServiceResponse<List<ServiceCentreDTO>>> GetDestinationServiceCenters()  
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = await _service.GetDestinationServiceCenters();

                return new ServiceResponse<List<ServiceCentreDTO>>()
                {
                    Object = _result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPackageLists")]
        public async Task<IServiceResponse<PackageList>> GetPackageLists()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var _result = _service.GetPackageList();

                //1. Pass the return to the view or caller
                return new ServiceResponse<PackageList>()
                {
                    Object = _result
                };
            });
        }

        [HttpGet]
        [Route("GetLocations")]
        public async Task<IServiceResponse<LocationList>> GetLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.GetLocations();

                return new ServiceResponse<LocationList>()
                {
                    Object = _result
                };
            });
        }

        [HttpGet]
        [Route("GetListofItemStatus")]
        public async Task<IServiceResponse<List<string>>> GetListofItemStatus()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.GetItemStatus();

                return new ServiceResponse<List<string>>()
                {
                    Object = _result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDescriptions")]
        public async Task<IServiceResponse<Description>> GetDescriptions(string description = " ")
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.CommodityDescription(description);

                return new ServiceResponse<Description>()
                {
                    Object = _result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransactionTypes")]
        public async Task<IServiceResponse<TransactionTypes>> GetTransactionTypes()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var _result = _service.TransactionTypes();

                return new ServiceResponse<TransactionTypes>()
                {
                    Object = _result
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWarehouseReceiptRangeByDate")]
        public async Task<IServiceResponse<WarehouseReceiptList>> GetWarehouseReceiptRangeByDate(QuerylogDt0 querydto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                var _result = _service.GetWarehouseReceiptRangeByDate(access_key, querydto);

                return new ServiceResponse<WarehouseReceiptList>()
                {
                    Object = _result
                };
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetChargeDefinitions")]
        public async Task<IServiceResponse<ChargeDefinitionList>> GetChargeDefinitions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);

                //3. Call the Magaya SetTransaction Method from MagayaService
                var result = _service.GetChargeDefinitionList(access_key);

                //4. Pass the return to the view or caller
                return new ServiceResponse<ChargeDefinitionList>()
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("GetFirstTransbyDate")]
        public async Task<IServiceResponse<TransactionResults>> GetFirstTransbyDate(QuerylogDt0 querydto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);
                string cookies = "";
                int more_results = 0;
                var _result = _service.GetFirstTransbyDate(access_key, querydto, out cookies, out more_results);
                var transactions = new TransactionResults()
                {
                    warehousereceipt = _result.Item1,
                    shipmentlist = _result.Item2,
                    invoicelist = _result.Item3,
                    paymentlist = _result.Item4
                };

                return new ServiceResponse<TransactionResults>()
                {
                    Object = transactions,
                    Cookies = cookies,
                    more_reults = more_results
                };
            });
        }


        [HttpGet]
        [Route("GetNextTransbyDate")]
        public async Task<IServiceResponse<TransactionResults>> GetNextTransbyDate(string cookies, string type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. initialize the access key variable
                int access_key = 0;

                //2. Call the open connection to get the session key
                var openconn = _service.OpenConnection(out access_key);
                string xmlTransList;
                int more_results;
                var _result = _service.GetNextTransByDate2(access_key, out  more_results, ref cookies, type);

                var transactions = new TransactionResults()
                {
                    warehousereceipt = _result.Item1,
                    shipmentlist = _result.Item2,
                    invoicelist = _result.Item3,
                    paymentlist = _result.Item4
                };

                return new ServiceResponse<TransactionResults>()
                {
                    Object = transactions,
                    Cookies = cookies,
                    more_reults= more_results
                };
            });

        }


        [HttpGet]
        [Route("confirmreceipt/{itemID}")]
        public async Task<IServiceResponse<bool>> GetShipmentActivities(int ItemID)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var res = await _service.UpdateReceived(ItemID);

                return new ServiceResponse<bool>
                {
                    Object = res
                };
            });
        }
    }
}
