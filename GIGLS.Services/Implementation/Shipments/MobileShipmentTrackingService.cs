using POST.Core;
using POST.Core.IServices.User;
using POST.Core.IMessageService;
using POST.Core.IServices.Shipments;
using POST.Core.DTO;
using POST.Core.Enums;
using System;
using System.Threading.Tasks;
using System.Linq;
using POST.Infrastructure;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.ShipmentScan;
using POST.Core.IServices.Business;
using System.Net;
using System.Collections.Generic;

namespace POST.Services.Implementation.Shipments
{
    public class MobileShipmentTrackingService : IMobileShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IShipmentTrackService _shipmentTrackService;
        private readonly IManifestVisitMonitoringService _monitoringService;


        public MobileShipmentTrackingService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService,
            IShipmentTrackService shipmentTrackService, IManifestVisitMonitoringService monitoringService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _shipmentTrackService = shipmentTrackService;
            _monitoringService = monitoringService;

        }

        public async Task<MobileShipmentTrackingHistoryDTO> GetMobileShipmentTrackings(string waybill)
        {
            try
            {
                //1. call agility core tracking
                var shipmentTracking = await _shipmentTrackService.TrackShipmentForMobile(waybill);

                //remove missing and not found status from customer history
                if (shipmentTracking.Any())
                {
                    string smim = ShipmentScanStatus.SMIM.ToString();
                    string fms = ShipmentScanStatus.FMS.ToString();
                    string thirdparty = ShipmentScanStatus.THIRDPARTY.ToString();

                    shipmentTracking = shipmentTracking.Where(x => !(x.Status == smim || x.Status == fms || x.Status == thirdparty));
                }

                //2. call mobile tracking
                var MobileshipmentTracking = await _uow.MobileShipmentTracking.GetMobileShipmentTrackingsAsync(waybill);

                //3. convert agility tracking object to mobile tracking object
                var shipmentTrackingMobileVersion = MobileTrackIngMapping(shipmentTracking.ToList());

                //4. append the two lists together
                MobileshipmentTracking.AddRange(shipmentTrackingMobileVersion);

                var orderedtrackings = MobileshipmentTracking.OrderByDescending(x => x.DateTime).ToList();

                var addresses = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == waybill);

                var trackings = new MobileShipmentTrackingHistoryDTO();

                if (addresses != null)
                {
                    trackings = new MobileShipmentTrackingHistoryDTO
                    {
                        Origin = addresses.SenderAddress,
                        Destination = addresses.ReceiverAddress,
                        MobileShipmentTrackings = orderedtrackings
                    };
                }
                else
                {
                    var shipmentaddress = await _uow.Shipment.GetAsync(s => s.Waybill == waybill);
                    if (shipmentaddress != null)
                    {
                        var deptCentre = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == shipmentaddress.DepartureServiceCentreId);
                        var destCentre = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == shipmentaddress.DestinationServiceCentreId);
                        trackings = new MobileShipmentTrackingHistoryDTO
                        {
                            Origin = shipmentaddress.SenderAddress == null ? deptCentre.FormattedServiceCentreName : shipmentaddress.SenderAddress,
                            Destination = shipmentaddress.ReceiverAddress == null ? destCentre.FormattedServiceCentreName : shipmentaddress.ReceiverAddress,
                            MobileShipmentTrackings = orderedtrackings
                        };
                    }
                    else
                    {
                        throw new GenericException("Invalid waybill number!!");
                    }
                }

                // also check for  manifest visit monitoring
                ///Add Log Visit Reasons for the waybill to the first element
                var logVisits = await _monitoringService.GetManifestVisitMonitoringByWaybill(waybill);
                if (logVisits.Any())
                {
                    trackings.ManifestVisitMonitorings = logVisits;
                }

                return trackings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MobileShipmentTrackingDTO> GetMobileShipmentTrackingById(int trackingId)
        {
            try
            {
                var shipmentTracking = await _uow.MobileShipmentTracking.GetAsync(trackingId);
                if (shipmentTracking == null)
                {
                    throw new GenericException($"MobileShipmentTrackingId: {trackingId} does Not Exist");
                }
                return new MobileShipmentTrackingDTO
                {
                    Waybill = shipmentTracking.Waybill,
                    DateTime = shipmentTracking.DateTime,
                    Location = shipmentTracking.Location,
                    MobileShipmentTrackingId = shipmentTracking.MobileShipmentTrackingId,
                    TrackingType = shipmentTracking.TrackingType,
                    Status = shipmentTracking.Status,
                    User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddMobileShipmentTracking(MobileShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus)
        {

            try
            {
                if (tracking.User == null)
                {
                    tracking.User = await _userService.GetCurrentUserId();
                }

                if (tracking.ServiceCentreId == 0)
                {
                    var gigGOServiceCenter = await _userService.GetGIGGOServiceCentre();
                    tracking.ServiceCentreId = gigGOServiceCenter.ServiceCentreId;
                }

                //check if the waybill has not been scan for the status before
                bool shipmentTracking = await _uow.MobileShipmentTracking.ExistAsync(x => x.Waybill.Equals(tracking.Waybill) && x.Status.Equals(tracking.Status));

                if (!shipmentTracking || scanStatus.Equals(ShipmentScanStatus.AD))
                {
                    var newShipmentTracking = new MobileShipmentTracking
                    {
                        Waybill = tracking.Waybill,
                        Status = tracking.Status,
                        DateTime = DateTime.Now,
                        UserId = tracking.User,
                        ServiceCentreId = tracking.ServiceCentreId
                    };
                    _uow.MobileShipmentTracking.Add(newShipmentTracking);
                    await _uow.CompleteAsync();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateShipmentTracking(int trackingId, MobileShipmentTrackingDTO tracking)
        {
            try
            {
                var shipmentTracking = await _uow.MobileShipmentTracking.GetAsync(trackingId);
                if (shipmentTracking == null || tracking.MobileShipmentTrackingId != trackingId)
                {
                    throw new GenericException("MobileShipmentTracking Not Exist", $"{(int)HttpStatusCode.BadRequest}");
                }
                shipmentTracking.Location = shipmentTracking.Location;
                shipmentTracking.TrackingType = shipmentTracking.TrackingType;
                shipmentTracking.User = shipmentTracking.User;
                shipmentTracking.Status = tracking.Status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> CheckMobileShipmentTracking(string waybill, string status)
        {
            bool shipmentTracking = await _uow.MobileShipmentTracking.ExistAsync(x => x.Waybill.Equals(waybill) && x.Status.Equals(status));
            return shipmentTracking;
        }

        private List<MobileShipmentTrackingDTO> MobileTrackIngMapping(List<ShipmentTrackingDTO> shipmentTracking)
        {
            var shipmentTrackingMobileVersion = shipmentTracking.Select(s => new MobileShipmentTrackingDTO
            {
                Waybill = s.Waybill,
                Location = s.Location,
                Status = s.Status,
                DateTime = s.DateTime,
                TrackingType = s.TrackingType,
                User = s.User,
                MobileShipmentTrackingId = s.ShipmentTrackingId,
                ScanStatus = new MobileScanStatusDTO
                {
                    Code = s.ScanStatus.Code,
                    Incident = s.ScanStatus.Incident,
                    Reason = s.ScanStatus.Reason,
                    Comment = s.ScanStatus.Comment
                }
            });

            return shipmentTrackingMobileVersion.ToList();
        }

    }
}
