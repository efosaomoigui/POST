using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

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
                GoogleApiKey = Decrypt(GoogleApiKey);
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
            var TotalPrice = 0.0M;
            if (partnerpay.ZoneMapping == 1)
            {
                var TotalAmount = (partnerpay.ShipmentPrice + partnerpay.PickUprice);
                var amount = (0.8M * TotalAmount);
                TotalPrice = Convert.ToDecimal(string.Format("{0:F2}", amount));
            }
            else
            {
                var distance = Convert.ToDecimal(partnerpay.Distance);
                var actualdistance = distance / 1000;
                var TotalAmountBasedonDistance = actualdistance * 3;
                var Time = Convert.ToDecimal(partnerpay.Time);
                var actualTimeinMinutes = Convert.ToDecimal(string.Format("{0:F2}", (Time / 60)));
                var TotalAmountBasedonTime = actualTimeinMinutes * 2;
                var TotalAmountBasedonShipment = partnerpay.ShipmentPrice * 0.05M;
                var Totalprice = TotalAmountBasedonDistance + TotalAmountBasedonTime + TotalAmountBasedonShipment;
                Totalprice = Convert.ToDecimal(string.Format("{0:F2}", Totalprice));
                var Sumofpickupandgooglapicalc =  0.8M * (Totalprice + partnerpay.ShipmentPrice);
                TotalPrice = Convert.ToDecimal(string.Format("{0:F2}", Sumofpickupandgooglapicalc));
            }
            return await Task.FromResult(TotalPrice);
        }

        public async Task<object> AddPartnerPaymentLog(PartnerTransactionsDTO walletPaymentLogDto)
        {
            
            walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();
            if(walletPaymentLogDto.IsFromServiceCentre)
            {
                var Partnerid = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == walletPaymentLogDto.Waybill && s.Status != MobilePickUpRequestStatus.Rejected.ToString());
                walletPaymentLogDto.UserId = Partnerid.UserId;
            }
            var walletPaymentLog = Mapper.Map<PartnerTransactions>(walletPaymentLogDto);
            _uow.PartnerTransactions.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.PartnerTransactionsID };
        }
        public string Encrypt(string clearText)
            {
                string EncryptionKey = "abc123";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
        public string Decrypt(string cipherText)
            {
                string EncryptionKey = "abc123";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
    }
}
