using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerTransactionsService : IPartnerTransactionsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public PartnerTransactionsService(IUnitOfWork uow, IUserService userService)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<RootObject> GetGeoDetails(LocationDTO location)
        {
            var Response = new RootObject();
            try
            {
                var GoogleURL = ConfigurationManager.AppSettings["DistanceURL"];
                var GoogleApiKey = ConfigurationManager.AppSettings["DistanceApiKey"];
                var finalURL = $"{GoogleURL}{GoogleApiKey}&units=metric&origins={location.OriginLatitude},{location.OriginLongitude}&destinations={location.DestinationLatitude},{location.DestinationLongitude}";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalURL);
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream result = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(result);
                    string responseFromServer = reader.ReadToEnd();
                    Response = JsonConvert.DeserializeObject<RootObject>(responseFromServer);
                    
                }
            }
            catch (Exception ex)
            {
                // An exception occurred making the REST call
                throw ex;
            }

            return await Task.FromResult(Response);
        }

        public async Task<decimal> GetPriceForPartner (PartnerPayDTO partnerpay)
        {
            var distance = Convert.ToDecimal(partnerpay.Distance);
            var actualdistance = distance / 1000;
            var TotalAmountBasedonDistance = actualdistance * 3;
            var Time = Convert.ToDecimal(partnerpay.Time);
            var actualTimeinMinutes = Time / 60;
            var TotalAmountBasedonTime = actualTimeinMinutes * 2;
            var TotalAmountBasedonShipment = partnerpay.ShipmentPrice * 0.05M;
            var TotalPrice = TotalAmountBasedonDistance + TotalAmountBasedonTime + TotalAmountBasedonShipment;
            return await Task.FromResult(TotalPrice);
        }
        public async Task<object> AddPartnerPaymentLog(PartnerTransactionsDTO walletPaymentLogDto)
        {
            
            walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();
            var walletPaymentLog = Mapper.Map<PartnerTransactions>(walletPaymentLogDto);
            _uow.PartnerTransactions.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.PartnerTransactionsID };
        }
    }
}
