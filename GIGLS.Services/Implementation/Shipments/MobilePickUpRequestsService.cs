using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class MobilePickUpRequestsService : IMobilePickUpRequestsService
    {

        private readonly IUnitOfWork _uow;
        private readonly IUserService _userservice;

        public MobilePickUpRequestsService(IUnitOfWork uow, IUserService userservice)
        {
            _uow = uow;
            _userservice = userservice;
            MapperConfig.Initialize();
        }

        public async Task AddMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest)
        {
            try
            {               
                var newMobilePickUpRequest = Mapper.Map<MobilePickUpRequests>(PickUpRequest);
                _uow.MobilePickUpRequests.Add(newMobilePickUpRequest);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddOrUpdateMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest)
        {
            var request = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == PickUpRequest.Waybill && s.UserId == PickUpRequest.UserId);

            if(request == null)
            {
                await AddMobilePickUpRequests(PickUpRequest);
            }
            else
            {
                request.Status = PickUpRequest.Status;
                await _uow.CompleteAsync();
            }
        }

        public async Task AddOrUpdateMobilePickUpRequestsMultipleShipments(MobilePickUpRequestsDTO pickUpRequest, List<string> waybillList)
        {
            var request =  _uow.MobilePickUpRequests.GetAllAsQueryable().Where(s => waybillList.Contains(s.Waybill) && s.UserId == pickUpRequest.UserId).ToList();

            if (!request.Any())
            {
                List<MobilePickUpRequests> mobilePickUpRequests = new List<MobilePickUpRequests>();

                foreach ( var waybill in waybillList)
                {
                    pickUpRequest.Waybill = waybill;
                    var newRequest = Mapper.Map<MobilePickUpRequests>(pickUpRequest);
                    mobilePickUpRequests.Add(newRequest);
                }
                _uow.MobilePickUpRequests.AddRange(mobilePickUpRequests);
            }
            else
            {
                request.ForEach(x => x.Status = pickUpRequest.Status);
            }
            await _uow.CompleteAsync();
        }

        public async Task<List<MobilePickUpRequestsDTO>> GetAllMobilePickUpRequests()
        {
            try
            {
                var userid = await _userservice.GetCurrentUserId();
                var mobilerequests = await _uow.MobilePickUpRequests.GetMobilePickUpRequestsAsync(userid);
                foreach(var item in mobilerequests)
                {
                    if(item.PreShipment.ServiceCentreAddress !=null)
                    {
                        item.PreShipment.ReceiverLocation.Longitude = item.PreShipment.serviceCentreLocation.Longitude;
                        item.PreShipment.ReceiverLocation.Latitude = item.PreShipment.serviceCentreLocation.Latitude;
                        item.PreShipment.ReceiverAddress = item.PreShipment.ServiceCentreAddress;
                    }
                }
                return mobilerequests;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Partnerdto> GetMonthlyTransactions()
        {
            try
            {
                var CurrencyCode = "";
                var CurrencySymbol = "";
                var userid = await _userservice.GetCurrentUserId();
                var user = await _userservice.GetUserById(userid);
                var Country = await _uow.Country.GetAsync(s => s.CountryId == user.UserActiveCountryId);
                if(Country !=null)
                {
                    CurrencyCode = Country.CurrencyCode;
                    CurrencySymbol = Country.CurrencySymbol;
                }
                
                var mobilerequests = await _uow.MobilePickUpRequests.GetMobilePickUpRequestsAsyncMonthly(userid);
                foreach (var item in mobilerequests)
                {
                    if (item.PreShipment.ServiceCentreAddress != null)
                    {
                        item.PreShipment.ReceiverLocation.Longitude = item.PreShipment.serviceCentreLocation.Longitude;
                        item.PreShipment.ReceiverLocation.Latitude = item.PreShipment.serviceCentreLocation.Latitude;
                        item.PreShipment.ReceiverAddress = item.PreShipment.ServiceCentreAddress;
                    }
                }
                var Count = await _uow.MobilePickUpRequests.FindAsync(x => x.UserId == userid && x.DateCreated.Month == DateTime.Now.Month && x.DateCreated.Year == DateTime.Now.Year && x.Status == "Delivered");
                int TotalDelivery = Count.Count();
                var TotalEarnings =  await _uow.PartnerTransactions.FindAsync(s => s.UserId == userid && s.DateCreated.Month == DateTime.Now.Month && s.DateCreated.Year == DateTime.Now.Year);
                var TotalEarning = TotalEarnings.Sum(x =>x.AmountReceived);
                var totaltransactions = new Partnerdto
                {
                  CurrencyCode = CurrencyCode,
                  CurrencySymbol = CurrencySymbol,
                  MonthlyDelivery = mobilerequests,
                  TotalDelivery = TotalDelivery,
                  MonthlyTransactions = TotalEarning
                };
                return totaltransactions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest, string userId)
        {
                try
                {
                    //var userId = await _userservice.GetCurrentUserId();
                    var MobilePickupRequests = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == PickUpRequest.Waybill && s.UserId == userId && s.Status != MobilePickUpRequestStatus.Rejected.ToString());
                    if (MobilePickupRequests == null)
                    {
                        throw new GenericException("Pickup Request Does Not Exist");
                    }
                    MobilePickupRequests.Status = PickUpRequest.Status;
                    await _uow.CompleteAsync();
                }
                catch (Exception)
                {
                    throw;
                }
        }

        public async Task UpdatePreShipmentMobileStatus(List<string> waybillList, string status)
        {
            try
            {
                var preshipmentmobile = _uow.PreShipmentMobile.GetAllAsQueryable().Where(s => waybillList.Contains(s.Waybill)).ToList();
                preshipmentmobile.ForEach(u => u.shipmentstatus = status);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateMobilePickUpRequestsForWaybillList(List<string> waybills, string userId, string status)
        {
            try
            {
                //why doing the filtering by status again
                var MobilePickupRequests = _uow.MobilePickUpRequests.GetAllAsQueryable().Where(s => waybills.Contains(s.Waybill) && s.UserId == userId
                            && s.Status != MobilePickUpRequestStatus.Rejected.ToString()).ToList();
                MobilePickupRequests.ForEach(u => u.Status = status);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
