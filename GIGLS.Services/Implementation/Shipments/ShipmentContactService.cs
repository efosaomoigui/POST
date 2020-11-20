using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using GIGLS.Core.Enums;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.DTO.Report;
using System.Net;
using Newtonsoft.Json.Linq;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentContactService : IShipmentContactService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private readonly IShipmentCollectionService _shipmentCollectionService;

        public ShipmentContactService(IUnitOfWork uow, IUserService userService,
            IShipmentCollectionService shipmentCollectionService)
        {
            _uow = uow;
            _userService = userService;
            _shipmentCollectionService = shipmentCollectionService;          
            MapperConfig.Initialize();
        }

        public async Task<List<ShipmentContactDTO>> GetShipmentContact(ShipmentContactFilterCriteria baseFilterCriteria)
        {
            var today = DateTime.Now;
            var shipmentContacts = new List<ShipmentContactDTO>();
            var shipmentDto = await _shipmentCollectionService.GetShipmentsCollectionForContact(baseFilterCriteria);


            if (shipmentDto.Any())
            {
                var waybills = shipmentDto.Select(x => x.Waybill).ToList();
                var contacts = _uow.ShipmentContact.Query(x => waybills.Contains(x.Waybill)).Select().ToList();

                foreach (var item in shipmentDto)
                {
                    var shc = contacts.Find(x => x.Waybill == item.Waybill);
                    if (shc != null)
                    {
                        var shcDto = JObject.FromObject(shc).ToObject<ShipmentContactDTO>();
                        shcDto.DestinationServiceCentre = item.OriginalDestinationServiceCentre.Name;
                        shcDto.DepartureServiceCentre = item.OriginalDepartureServiceCentre.Name;
                        shcDto.Age = (today - item.DateCreated).Days;
                        shcDto.ContactedBy = shc.ContactedBy;
                        shcDto.ReceiverName = item.Name;
                        shcDto.ReceiverPhoneNumber = item.PhoneNumber;
                        shipmentContacts.Add(shcDto);
                    }
                    else
                    {
                        var shcDto = new ShipmentContactDTO();
                        shcDto.DestinationServiceCentre = item.OriginalDestinationServiceCentre.Name;
                        shcDto.DepartureServiceCentre = item.OriginalDepartureServiceCentre.Name;
                        shcDto.Age = (int)(today - item.DateCreated).TotalDays;
                        shcDto.Waybill = item.Waybill;
                        shcDto.ContactedBy = "";
                        shcDto.Status = ShipmentContactStatus.NotContacted;
                        shcDto.NoOfContact = 0;
                        shcDto.ShipmentStatus = item.ShipmentScanStatus.ToString();
                        shcDto.ReceiverName = item.Name;
                        shcDto.ReceiverPhoneNumber = item.PhoneNumber;
                        shcDto.ShipmentCreatedDate = item.DateCreated;
                        shipmentContacts.Add(shcDto);
                    }
                }

            }

            return shipmentContacts.OrderByDescending(x => x.Age).ToList();
        }


        public async Task<bool> AddOrUpdateShipmentContactAndHistory(ShipmentContactDTO shipmentContactDTO)
        {
            try
            {
                if (shipmentContactDTO == null)
                {
                    throw new GenericException("Invalid payload", $"{(int)HttpStatusCode.BadRequest}");
                }
                var userId = await _userService.GetCurrentUserId();
                var userInfo = await _userService.GetUserById(userId);
                shipmentContactDTO.UserId = userId;
                var contact = await _uow.ShipmentContact.GetAsync(x => x.Waybill == shipmentContactDTO.Waybill);
                if (contact != null)
                {
                    contact.NoOfContact = contact.NoOfContact + 1;
                    contact.ContactedBy = $"{userInfo.FirstName} {userInfo.LastName}";
                    contact.DateModified = DateTime.Now;
                    contact.UserId = userId;

                    //also update history
                    var contactHistory = await _uow.ShipmentContactHistory.GetAsync(x => x.Waybill == shipmentContactDTO.Waybill && x.UserId == userId);
                    if (contactHistory != null)
                    {
                        contactHistory.NoOfContact = contactHistory.NoOfContact + 1;
                        contactHistory.DateModified = DateTime.Now;
                    }
                    else
                    {
                        var newHistory = new ShipmentContactHistory();
                        newHistory.DateCreated = DateTime.Now;
                        newHistory.ContactedBy = $"{userInfo.FirstName} {userInfo.LastName}";
                        newHistory.Waybill = shipmentContactDTO.Waybill;
                        newHistory.UserId = userId;
                        newHistory.NoOfContact = newHistory.NoOfContact + 1;
                        _uow.ShipmentContactHistory.Add(newHistory);
                    }


                }
                else
                {
                    var newContact = JObject.FromObject(shipmentContactDTO).ToObject<ShipmentContact>(); JObject.FromObject(shipmentContactDTO).ToObject<ShipmentContact>();
                    newContact.Status = ShipmentContactStatus.Contacted;
                    newContact.NoOfContact = newContact.NoOfContact + 1;
                    newContact.ContactedBy = $"{userInfo.FirstName} {userInfo.LastName}";
                    _uow.ShipmentContact.Add(newContact);

                    //also insert history
                    var newHistory = new ShipmentContactHistory();
                    newHistory.DateCreated = DateTime.Now;
                    newHistory.ContactedBy = $"{userInfo.FirstName} {userInfo.LastName}";
                    newHistory.Waybill = shipmentContactDTO.Waybill;
                    newHistory.UserId = userId;
                    newHistory.NoOfContact = newHistory.NoOfContact + 1;
                    _uow.ShipmentContactHistory.Add(newHistory);
                }
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ShipmentContactHistoryDTO>> GetShipmentContactHistoryByWaybill(string waybill)
        {
            if (String.IsNullOrEmpty(waybill))
            {
                throw new GenericException("Invalid param", $"{(int)HttpStatusCode.BadRequest}");
            }
            var history = _uow.ShipmentContactHistory.GetAll().Where(x => x.Waybill == waybill).ToList();
            var historyDto = JArray.FromObject(history).ToObject<List<ShipmentContactHistoryDTO>>();

            return historyDto;
        }
    }
}
