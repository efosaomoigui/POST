using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var Count = await _uow.MobilePickUpRequests.FindAsync(x => x.UserId == userid && x.DateCreated.Month == DateTime.Now.Month && x.DateCreated.Year == DateTime.Now.Year && x.Status =="Delivered");
                foreach (var item in mobilerequests)
                {
                    if (item.PreShipment.ServiceCentreAddress != null)
                    {
                        item.PreShipment.ReceiverLocation.Longitude = item.PreShipment.serviceCentreLocation.Longitude;
                        item.PreShipment.ReceiverLocation.Latitude = item.PreShipment.serviceCentreLocation.Latitude;
                        item.PreShipment.ReceiverAddress = item.PreShipment.ServiceCentreAddress;
                    }
                }
                var TotalDelivery = Count.Count();
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
            catch (Exception)
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
    }
}
