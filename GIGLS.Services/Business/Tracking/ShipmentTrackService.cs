using AftershipAPI;
using GIGLS.Core;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
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
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IUnitOfWork _uow;

        public ShipmentTrackService(IShipmentTrackingService shipmentTrackingService, IUnitOfWork uow)
        {
            _shipmentTrackingService = shipmentTrackingService;
            _uow = uow;
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            var result = await _shipmentTrackingService.GetShipmentTrackings(waybillNumber);

            //check for international
            var shipment = await _uow.Shipment.GetAsync(s => s.Waybill == waybillNumber);
            if (shipment != null && shipment.IsInternational)
            {
                var internationResult = await TrackShipmentForInternational(waybillNumber);
                result.ToList().AddRange(internationResult);
            }

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
