using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.WebApi.Filters;
using GIGLS.Core.DTO.Report;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipmentcollection")]
    public class ShipmentCollectionController : BaseWebApiController
    {
        private readonly IShipmentCollectionService _service;

        public ShipmentCollectionController(IShipmentCollectionService service) : base(nameof(ShipmentCollectionController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetAllShipmentCollections()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollections = await _service.GetShipmentCollections();

                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollections
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("search")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetAllShipmentCollections([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetShipmentCollections(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object =  shipmentCollectionTuple.Item1,
                    Total = shipmentCollectionTuple.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("bydate")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetAllShipmentCollectionsByDate(ShipmentCollectionFilterCriteria collectionFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollections = await _service.GetShipmentCollections(collectionFilterCriteria);

                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollections
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<ShipmentCollectionDTO>> GetShipmentCollectionByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollection = await _service.GetShipmentCollectionById(waybill);

                return new ServiceResponse<ShipmentCollectionDTO>
                {
                    Object = shipmentCollection
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeleteShipmentCollectionByCode(string waybill)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.RemoveShipmentCollection(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> AddShipmentCollection(ShipmentCollectionDTO shipmentCollection)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.AddShipmentCollection(shipmentCollection);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("")]
        public async Task<IServiceResponse<bool>> UpdateShipmentCollectionBywaybill(ShipmentCollectionDTO shipmentCollection)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateShipmentCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waitingforcollection")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetShipmentWaitingForCollection()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _service.GetShipmentWaitingForCollection();

                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waitingforcollectionforhub")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetShipmentWaitingForCollectionForHub([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _service.GetShipmentWaitingForCollectionForHub(filterOptionsDto);

                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = result.Item1,
                    Total = result.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waitingforcollection_search")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetShipmentWaitingForCollection([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetShipmentWaitingForCollection(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple.Item1,
                    Total = shipmentCollectionTuple.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("overdueshipment")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipments([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetOverDueShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple.Item1,
                    Total = shipmentCollectionTuple.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("overdueshipmentecommerce")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipmentEcommerce([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetEcommerceOverDueShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple.Item1,
                    Total = shipmentCollectionTuple.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("overdueshipmentecommerce")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipmentEcommerce()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetEcommerceOverDueShipments();
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple
                    
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("collected")]
        public async Task<IServiceResponse<bool>> ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKT;
            if(shipmentCollection.IsComingFromDispatch)
            {
                shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKC;
            }

            return await HandleApiOperationAsync(async () => {
                await _service.ReleaseShipmentForCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //---Added for global customer care and ecommerce
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("overdueshipmentglobal")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipmentsGLOBAL([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetOverDueShipmentsGLOBAL(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple.Item1,
                    Total = shipmentCollectionTuple.Item2
                };
            });
        }
        

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("overdueshipmentecommerceglobal")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipmentEcommerceGLOBAL([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetEcommerceOverDueShipmentsGLOBAL(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple.Item1,
                    Total = shipmentCollectionTuple.Item2
                };
            });
        }
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("overdueshipmentecommerceglobal")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipmentEcommerceGLOBAL()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetEcommerceOverDueShipmentsGLOBAL();
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple

                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("overdueshipmentecommerceglobalByDate")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetOverDueShipmentEcommerceGLOBALl( ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollectionTuple = await _service.GetEcommerceOverDueShipmentsGLOBAL(filterCriteria);
                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollectionTuple
                    
                };
            });
        }


        [HttpPost]
        [Route("arrivedshipment")]
        public async Task<IServiceResponse<List<ShipmentCollectionDTOForArrived>>> GetShipmentContacts(ShipmentContactFilterCriteria baseFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentcontacts = await _service.GetArrivedShipmentCollection(baseFilterCriteria);
                return new ServiceResponse<List<ShipmentCollectionDTOForArrived>>
                {
                    Object = shipmentcontacts
                };
            });
        }


    }
}
