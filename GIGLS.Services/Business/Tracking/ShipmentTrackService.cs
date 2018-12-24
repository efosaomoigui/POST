using AftershipAPI;
using GIGLS.Core;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Tracking
{
    public class ShipmentTrackService : IShipmentTrackService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IShipmentService _shipmentService;
        private readonly IManifestVisitMonitoringService _monitoringService;

        public ShipmentTrackService(IShipmentTrackingService shipmentTrackingService, IShipmentService shipmentService, IUnitOfWork uow, IManifestVisitMonitoringService monitoringService)
        {
            _shipmentTrackingService = shipmentTrackingService;
            _shipmentService = shipmentService;
            _uow = uow;
            _monitoringService = monitoringService;
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            var result = await _shipmentTrackingService.GetShipmentTrackings(waybillNumber);

            if (result.Count() > 0)
            {
                //get shipment Details
                var shipment = await _shipmentService.GetBasicShipmentDetail(waybillNumber);
                string manifest = null;
                string ManifestTypeValue = ManifestType.Delivery.ToString(); //Default

                //Get Manifest Details
                if (shipment != null)
                {
                    if (shipment.PickupOptions == PickupOptions.HOMEDELIVERY)
                    {
                        //get home delivery manifest
                        var manifestQuery = _uow.ManifestWaybillMapping.GetAllAsQueryable();
                        var manifestResult = manifestQuery.Where(x => x.Waybill == waybillNumber).ToList();
                        
                        if(manifestResult.Count() > 0)
                        {
                            foreach (var query in manifestResult)
                            {
                                if (query.IsActive)
                                {
                                    manifest = query.ManifestCode;
                                    break;
                                }
                                else
                                {
                                    manifest = query.ManifestCode;
                                }
                            }
                        }
                    }

                    if (manifest == null)
                    {
                        //1. Get Group Waybill for the waybill
                        var groupWaybillNumberMapping = _uow.GroupWaybillNumberMapping.GetAllAsQueryable();
                        var groupWaybillResult = groupWaybillNumberMapping.Where(w => w.WaybillNumber == waybillNumber).Select(x => x.GroupWaybillNumber).Distinct().ToList();

                        if(groupWaybillResult.Count() > 0)
                        {
                            string groupWaybill = null;
                            foreach(string s in groupWaybillResult)
                            {
                                groupWaybill = s;
                            }

                            //Get the manifest the group waybill
                            //var manifestGroupwaybillMapping = await _uow.ManifestGroupWaybillNumberMapping.Where(s => s.GroupWaybillNumber == groupWaybill).AsQueryable();
                            var manifestGroupwaybillMapping = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill);
                            if(manifestGroupwaybillMapping != null)
                            {
                                manifest = manifestGroupwaybillMapping.ManifestCode;
                                ManifestTypeValue = ManifestType.External.ToString();
                            }
                        }
                    }
                }

                foreach (var track in result)
                {
                    track.Manifest = manifest;
                    track.ManifestType = ManifestTypeValue;

                    track.DepartureServiceCentreId = shipment.DepartureServiceCentreId;
                    track.DepartureServiceCentre = shipment.DepartureServiceCentre;
                    track.DestinationServiceCentreId = shipment.DestinationServiceCentreId;
                    track.DestinationServiceCentre = shipment.DestinationServiceCentre;

                    track.Amount = shipment.GrandTotal;
                    track.PickupOptions = shipment.PickupOptions.ToString();
                    track.DeliveryOptions = shipment.DeliveryOption.Description;
                }

                ////check for international
                if (shipment != null && shipment.IsInternational)
                {
                    var internationResult = await TrackShipmentForInternational(waybillNumber);
                    result.ToList().AddRange(internationResult);
                }

                ///Add Log Visit Reasons for the waybill to the first element
                var logVisits = await _monitoringService.GetManifestVisitMonitoringByWaybill(waybillNumber);

                if(logVisits.Count() > 0)
                {
                    result.ElementAt(0).ManifestVisitMonitorings = logVisits;
                }
            }

            //check for international
            //var shipment = await _uow.Shipment.GetAsync(s => s.Waybill == waybillNumber);
            //if (shipment != null && shipment.IsInternational)
            //{
            //    var internationResult = await TrackShipmentForInternational(waybillNumber);
            //    result.ToList().AddRange(internationResult);
            //}
            return result;
        }

        public async Task<List<ShipmentTrackingDTO>> TrackShipmentForInternational(string waybillNumber)
        {
            var shipmentTrackings = new List<ShipmentTrackingDTO>();

            try
            {
                //Create an instance of ConnectionAPI using the token of the user
                //String key = System.IO.File.ReadAllText(@"\\psf\Home\Documents\aftership-key.txt");
                //ConnectionAPI connection_api_backup = new ConnectionAPI(key, "https://api-backup.aftership.com/");

                String key = ConfigurationManager.AppSettings["aramex:API_KEY"];
                ConnectionAPI connection_api = new ConnectionAPI(key, null);

                //tracking:
                AftershipAPI.Tracking tracking = new AftershipAPI.Tracking(waybillNumber);
                var trackingResult = connection_api.getTrackingByNumber(tracking);

                foreach (var checkpoint in trackingResult.checkpoints)
                {
                    //check for city
                    var location = "";
                    if (checkpoint.city == null)
                    {
                        location = checkpoint.countryName;
                    }
                    else
                    {
                        location = checkpoint.city + ", " + checkpoint.countryName;
                    }

                    //check for  checkpoint.checkpointTime
                    DateTime checkpointTime = checkpoint.createdAt;
                    DateTime.TryParse(checkpoint.checkpointTime, out checkpointTime);


                    var newShipmentTrackingDTO = new ShipmentTrackingDTO()
                    {
                        DateCreated = checkpoint.createdAt,
                        DateTime = checkpointTime,
                        Location = location,
                        ScanStatus = new ScanStatusDTO()
                        {
                            Code = checkpoint.tag,
                            Comment = checkpoint.message,
                            Incident = checkpoint.message,
                            Reason = checkpoint.message
                        },
                        Waybill = waybillNumber,
                        User = trackingResult.signedBy,
                        Status = checkpoint.tag
                    };

                    shipmentTrackings.Add(newShipmentTrackingDTO);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

            return await Task.FromResult(shipmentTrackings.ToList().OrderByDescending(x => x.DateTime).ToList());
        }

    }
}
