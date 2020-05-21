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

            if (result.Any())
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

                        if (manifestResult.Any())
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

                        if (groupWaybillResult.Any())
                        {
                            string groupWaybill = null;
                            foreach (string s in groupWaybillResult)
                            {
                                groupWaybill = s;
                            }

                            //Get the manifest the group waybill
                            //var manifestGroupwaybillMapping = await _uow.ManifestGroupWaybillNumberMapping.Where(s => s.GroupWaybillNumber == groupWaybill).AsQueryable();
                            var manifestGroupwaybillMapping = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill);
                            if (manifestGroupwaybillMapping != null)
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

                if (logVisits.Any())
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

            //Replace the placeholders in Scan Status
            await ResolveScanStatusPlaceholders(result);

            return result;
        }

        private async Task<int> ResolveScanStatusPlaceholders(IEnumerable<ShipmentTrackingDTO> result)
        {
            //{0} = Service Centre
            //{1} = Receiver Name
            var strArray = new string[]
            {
                "SERVICE CENTRE",
                "CUSTOMER",
                "DEPARTURE SERVICE CENTRE",
                "DESTINATION SERVICE CENTRE",
            };

            foreach (var shipmentTrackingDTO in result)
            {
                //1. {0} - Service Centre
                if (shipmentTrackingDTO.ScanStatus.Incident.Contains("{0}"))
                {
                    //map the array
                    strArray[0] = shipmentTrackingDTO.Location;
                }

                //2. {1} - Receiver Name
                if (shipmentTrackingDTO.ScanStatus.Incident.Contains("{1}"))
                {
                    var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(shipmentTrackingDTO.Waybill));
                    if(shipmentCollection != null && shipmentCollection.Name != null)
                    {
                        //map the array
                        strArray[1] = shipmentCollection.Name;
                    }
                }

                //2. {2} - Departure Service Centre
                if (shipmentTrackingDTO.ScanStatus.Incident.Contains("{2}"))
                {
                    //map the array
                    strArray[2] = shipmentTrackingDTO.DepartureServiceCentre.Name;
                }

                //3. {3} - Destination Service Centre (REROUTE)
                if (shipmentTrackingDTO.ScanStatus.Incident.Contains("{3}"))
                {
                    //map the array
                    strArray[3] = shipmentTrackingDTO.DestinationServiceCentre.Name;

                    //check ShipmentReroute for waybill
                    var shipmentReroute = await _uow.ShipmentReroute.GetAsync(s => s.WaybillOld == shipmentTrackingDTO.Waybill);
                    if(shipmentReroute != null)
                    {
                        //get the Rerouted Shipment information
                        var currentShipmentInfo = await _uow.Shipment.GetAsync(s => s.Waybill == shipmentReroute.WaybillNew);
                        var destinationServiceCentre = await _uow.ServiceCentre.GetAsync(s => s.ServiceCentreId == currentShipmentInfo.DestinationServiceCentreId);

                        //map the array
                        strArray[3] = destinationServiceCentre.Name;
                    }
                }

                //populate the Incident message
                shipmentTrackingDTO.ScanStatus.Incident = string.Format(shipmentTrackingDTO.ScanStatus.Incident, strArray);
            }
            return 0;
        }

        public async Task<List<ShipmentTrackingDTO>> TrackShipmentForInternational(string waybillNumber)
        {
            try
            {
                var shipmentTrackings = new List<ShipmentTrackingDTO>();

                string key = ConfigurationManager.AppSettings["aramex:API_KEY"];
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
                return await Task.FromResult(shipmentTrackings.ToList().OrderByDescending(x => x.DateTime).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipmentForMobile(string waybillNumber)
        {
            var result = await _shipmentTrackingService.GetShipmentTrackingsForMobile(waybillNumber);

            if (result.Any())
            {
                //get shipment Details
                var shipment = await _shipmentService.GetBasicShipmentDetail(waybillNumber);                               

                ////check for international
                if (shipment != null && shipment.IsInternational)
                {
                    var internationResult = await TrackShipmentForInternational(waybillNumber);
                    result.ToList().AddRange(internationResult);
                }

                foreach (var track in result)
                {
                    track.DepartureServiceCentreId = shipment.DepartureServiceCentreId;
                    track.DepartureServiceCentre = shipment.DepartureServiceCentre;
                    track.DestinationServiceCentreId = shipment.DestinationServiceCentreId;
                    track.DestinationServiceCentre = shipment.DestinationServiceCentre;

                    track.Amount = shipment.GrandTotal;
                    track.PickupOptions = shipment.PickupOptions.ToString();
                    track.DeliveryOptions = shipment.DeliveryOption.Description;
                }
            }

            //Replace the placeholders in Scan Status
            await ResolveScanStatusPlaceholders(result);

            return result;
        }

    }
}
