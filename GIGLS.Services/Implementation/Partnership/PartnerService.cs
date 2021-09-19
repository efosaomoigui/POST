using GIGLS.Core.IServices.Partnership;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core;
using GIGLS.Core.IServices.Wallet;
using System.Collections.Generic;
using AutoMapper;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
using System;
using System.Linq;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using System.Net;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Node;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerService : IPartnerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly ICompanyService _companyService;
        private readonly INodeService _nodeService;
        private readonly IUserService _userService;

        public PartnerService(IUnitOfWork uow, IWalletService walletService,
            INumberGeneratorMonitorService numberGeneratorMonitorService, ICompanyService companyService, INodeService nodeService, IUserService userService)
        {
            _uow = uow;
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _companyService = companyService;
            _nodeService = nodeService;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<PartnerDTO>> GetPartners()
        {
            var partners = await _uow.Partner.GetPartnersAsync();
            return partners;
        }


        public async Task<IEnumerable<VehicleTypeDTO>> GetVerifiedPartners(string fleetCode)
        {
            var partners = await _uow.Partner.GetPartnersAsync(fleetCode, true);
            return partners;
        }

        public async Task<List<PartnerDTO>> GetPartnersByDate(BaseFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            var allpartners = _uow.Partner.GetAllAsQueryable();

            if (filterCriteria.StartDate == null & filterCriteria.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }
            var allPartnersResult = allpartners.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

            List<PartnerDTO> partnerDTO = (from r in allPartnersResult
                                           select new PartnerDTO()
                                           {
                                               PartnerId = r.PartnerId,
                                               PartnerName = r.PartnerName,
                                               PartnerCode = r.PartnerCode,
                                               PartnerType = r.PartnerType
                                           }).ToList();
            return await Task.FromResult(partnerDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<PartnerDTO> GetPartnerById(int partnerId)
        {
            var partner = await _uow.Partner.GetAsync(partnerId);

            if (partner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            else
            {
                PartnerDTO partnerDto = Mapper.Map<PartnerDTO>(partner);
                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
                if (wallet != null)
                {
                    partnerDto.WalletBalance = wallet.Balance;
                    partnerDto.WalletId = wallet.WalletId;
                }

                var country = await _uow.Country.GetAsync(s => s.CountryId == partner.UserActiveCountryId);
                if (country != null)
                {
                    partnerDto.CurrencySymbol = country.CurrencySymbol;

                }
                return partnerDto;
            }
        }

        public async Task<PartnerDTO> GetPartnerByCode(string partnerCode)
        {
            var partnerDto = new PartnerDTO();
            var partner = await _uow.Partner.GetAsync(x => x.PartnerCode == partnerCode);

            if (partner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            else
            {
                partnerDto = Mapper.Map<PartnerDTO>(partner);
                var Wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
                var Country = await _uow.Country.GetAsync(s => s.CountryId == partner.UserActiveCountryId);
                if (Wallet != null)
                {
                    partnerDto.WalletBalance = Wallet.Balance;
                    partnerDto.WalletId = Wallet.WalletId;
                }
                if (Country != null)
                {
                    partnerDto.CurrencySymbol = Country.CurrencySymbol;

                }
            }
            return partnerDto;
        }

        public async Task<object> AddPartner(PartnerDTO partnerDto)
        {
            partnerDto.PartnerName = partnerDto.PartnerName.Trim();

            if (await _uow.Partner.ExistAsync(v => v.PartnerName.ToLower() == partnerDto.PartnerName.ToLower()))
            {
                throw new GenericException("PARTNER_EXISTS");
            }

            var partnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Partner);

            var partner = Mapper.Map<Partner>(partnerDto);
            partner.PartnerCode = partnerCode;
            _uow.Partner.Add(partner);
            await _uow.CompleteAsync();
            return new { id = partner.PartnerId };
        }

        public async Task UpdatePartner(int partnerId, PartnerDTO partner)
        {
            var existingParntner = await _uow.Partner.GetAsync(partnerId);

            if (existingParntner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }

            existingParntner.FirstName = partner.FirstName.Trim();
            existingParntner.LastName = partner.LastName.Trim();
            existingParntner.Email = partner.Email.Trim();
            existingParntner.PartnerName = partner.PartnerName.Trim();
            existingParntner.Address = partner.Address;
            existingParntner.OptionalPhoneNumber = partner.OptionalPhoneNumber;
            existingParntner.PartnerType = partner.PartnerType;
            existingParntner.PhoneNumber = partner.PhoneNumber;
            await _uow.CompleteAsync();
        }

        public async Task RemovePartner(int partnerId)
        {
            var existingPartner = await _uow.Partner.GetAsync(partnerId);

            if (existingPartner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            _uow.Partner.Remove(existingPartner);
            await _uow.CompleteAsync();
        }

        public async Task<IEnumerable<PartnerDTO>> GetExternalDeliveryPartners()
        {
            var partners = await _uow.Partner.GetExternalPartnersAsync();
            return partners;
        }

        public async Task<List<ExternalPartnerTransactionsPaymentDTO>> GetExternalPartnerTransactionsForPayment(ShipmentCollectionFilterCriteria shipmentCollectionFilterCriteria)
        {
            var partnersTransaction = await _uow.PartnerTransactions.GetExternalPartnerTransactionsForPayment(shipmentCollectionFilterCriteria);

            return partnersTransaction;
        }

        public async Task<IEnumerable<VehicleTypeDTO>> GetUnVerifiedPartners(ShipmentCollectionFilterCriteria filterCriteria)
        {
            string fleetCode = null;

            var partners = await _uow.Partner.GetPartnersAsync(fleetCode, false, filterCriteria);
            return partners;
        }

        //Send a mail to a partner that has not yet been verified
        public async Task ContactUnverifiedPartner(string email)
        {
            var partner = await _uow.Partner.GetAsync(x => x.Email == email);

            if (partner == null)
            {
                throw new GenericException($"Partner with email {email} does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            //Send Mail
            var messageDTO = new CompanyMessagingDTO
            {
                Email = partner.Email,
                Name = partner.PartnerName,
                UserChannelType = UserChannelType.Partner,
                PhoneNumber = partner.PhoneNumber
            };

            await _companyService.SendMessageToNewSignUps(messageDTO);
            partner.Contacted = true;

            await _uow.CompleteAsync();

        }
        public async Task<IEnumerable<VehicleTypeDTO>> GetVerifiedByRangePartners(ShipmentCollectionFilterCriteria filterCriteria)
        {
            string fleetCode = null;
            List<VehicleTypeDTO> partners = new List<VehicleTypeDTO>();

            if (filterCriteria.StartDate != null)
            {
                partners = await _uow.Partner.GetPartnersAsync(fleetCode, true, filterCriteria);
            }
            else
            {
                partners = await _uow.Partner.GetPartnersAsync(fleetCode, true);
            }
            partners = partners.Where(x => x.IsVerified == true).ToList();
            if (partners.Any())
            {
                var today = DateTime.Now;
                foreach (var item in partners)
                {
                    var months = 12 * (today.Year - item.ActivityDate.Year) + today.Month - item.ActivityDate.Month;
                    var monthsApart = Math.Abs(months);
                    if (monthsApart <= 1)
                    {
                        item.Active = true;
                    }
                }

            }
            return partners.OrderByDescending(x => x.DateModified);
        }

        public async Task DeactivatePartner(int partnerId)
        {
            var existingPartner = await _uow.Partner.GetAsync(partnerId);

            if (existingPartner == null)
            {
                throw new GenericException("PARTNER_DOES_NOT_EXIST");
            }
            existingPartner.IsActivated = false;
            await _uow.CompleteAsync();
        }

        public async Task<List<PartnerTransactionsDTO>> RiderRatings(PaginationDTO pagination)
        {
            int totalCount;
            var transactions = new List<PartnerTransactions>();
            pagination.StartDate = pagination.StartDate.Value.ToUniversalTime();
            pagination.StartDate = pagination.StartDate.Value.AddHours(12).AddMinutes(00);
            pagination.EndDate = pagination.EndDate.Value.ToUniversalTime();
            pagination.EndDate = pagination.EndDate.Value.AddHours(23).AddMinutes(59);
            if (!String.IsNullOrEmpty(pagination.FilterOption))
            {

                var partner = await _uow.Partner.GetAsync(x => x.Email.ToLower() == pagination.FilterOption.ToLower() || x.FirstName.Contains(pagination.FilterOption) || x.LastName.Contains(pagination.FilterOption));
                if (partner != null)
                {
                    transactions = _uow.PartnerTransactions.Query(x => x.UserId == partner.UserId && x.DateCreated >= pagination.StartDate && x.DateCreated <= pagination.EndDate).Select().ToList();
                }
            }
            else
            {
                transactions = _uow.PartnerTransactions.Query(x => x.DateCreated >= pagination.StartDate && x.DateCreated <= pagination.EndDate).Select().ToList();
            }
            var partnerTransactionsDTO = Mapper.Map<List<PartnerTransactionsDTO>>(transactions);
            return partnerTransactionsDTO;
        }


        public async Task<List<RiderRateDTO>> GetRidersRatings(PaginationDTO pagination)
        {
            try
            {
                var riderRates = new List<RiderRateDTO>();
                if (pagination.PageSize < 1)
                {
                    pagination.PageSize = 200;
                }
                if (pagination.Page < 1)
                {
                    pagination.Page = 1;
                }

                var transactions = await RiderRatings(pagination);
                var partnerIds = transactions.Select(x => x.UserId).ToList();
                var partners = _uow.Partner.GetAllAsQueryable().Where(x => partnerIds.Contains(x.UserId)).ToList();
                var waybills = transactions.Select(x => x.Waybill).ToList();
                var mobileshipments = _uow.PreShipmentMobile.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();
                var pickupRequests = _uow.MobilePickUpRequests.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();
                var mobiletracking = _uow.MobileShipmentTracking.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();
                var mobilerating = _uow.MobileRating.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();

                var groupedTransaction = transactions.GroupBy(x => x.UserId).ToList();
                foreach (var item in groupedTransaction)
                {
                    var riderRate = new RiderRateDTO();
                    var partner = partners.Where(x => x.UserId == item.Key).FirstOrDefault();
                    if (partner != null)
                    {
                        riderRate.PartnerName = partner.PartnerName;
                        riderRate.PartnerID = partner.UserId;
                        riderRate.PartnerEmail = partner.Email;
                        riderRate.PartnerType = partner.PartnerType.ToString();
                        riderRate.Status = partner.ActivityStatus;
                        riderRate.LastSeen = partner.ActivityDate;
                        var rider = await MapRiderOATAT(riderRate, transactions, mobileshipments, pickupRequests, mobiletracking, mobilerating);
                        if (rider != null)
                        {
                            riderRates.Add(rider);
                        }
                    }
                }
                return riderRates.OrderByDescending(x => x.LastSeen).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<RiderRateDTO> MapRiderOATAT(RiderRateDTO rider, List<PartnerTransactionsDTO> transactions, List<PreShipmentMobile> mobileshipments, List<MobilePickUpRequests> pickupRequests, List<MobileShipmentTracking> mobiletracking, List<MobileRating> mobilerating)
        {
            var userTransac = transactions.Where(x => x.UserId == rider.PartnerID).ToList();
            int dtat = 0;
            int atat = 0;
            int ptat = 0;
            double avrating = 0;
            int count = 0;
            if (userTransac.Any())
            {
                foreach (var item in userTransac)
                {
                    var createdDay = mobileshipments.FirstOrDefault(x => x.Waybill == item.Waybill);

                    var dlvrd = mobiletracking.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => (x.Status == ShipmentScanStatus.MAHD.ToString() || x.Status == ShipmentScanStatus.MSVC.ToString()) && x.Waybill == item.Waybill);
                    var picked = mobiletracking.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.Status == ShipmentScanStatus.MSHC.ToString() && x.Waybill == item.Waybill);
                    var assigned = mobiletracking.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.Status == ShipmentScanStatus.MAPT.ToString() && x.Waybill == item.Waybill);

                    if (dlvrd != null)
                    {
                        dtat += (int)(dlvrd.DateCreated - createdDay.DateCreated).TotalHours;
                    }

                    if (picked != null && assigned != null)
                    {
                        ptat += (int)(picked.DateCreated - assigned.DateCreated).TotalHours;
                    }
                    if (assigned != null && picked != null)
                    {
                        atat += (int)(picked.DateCreated - assigned.DateCreated).TotalHours;
                    }

                    var waybillRating = mobilerating.Where(x => x.Waybill == item.Waybill).FirstOrDefault();
                    if (waybillRating != null && waybillRating.PartnerRating != null)
                    {
                        count++;
                        avrating += waybillRating.PartnerRating.Value;
                    }
                }
                if (atat > 0)
                {
                    rider.AssignTAT = atat / userTransac.Count;
                }
                if (dtat > 0)
                {
                    rider.DeliveryTAT = dtat / userTransac.Count;
                }
                if (ptat > 0)
                {
                    rider.PickupTAT = ptat / userTransac.Count;
                }
                if (atat > 0 || ptat > 0)
                {
                    rider.AverageAssignTAT = (atat + ptat) / userTransac.Count;
                }
                if (dtat > 0 || ptat > 0)
                {
                    rider.AverageDeliveryTAT = (dtat + ptat) / userTransac.Count;
                }
                var oatat = atat + dtat + ptat;
                if (oatat > 0)
                {
                    rider.AverageOATAT = oatat / userTransac.Count;
                }
                rider.Trip = userTransac.Count;
                if (count > 0)
                {
                    rider.Rate = avrating / count;
                }
            }
            return rider;
        }

        public async Task UpdatePartnerEmailPhoneNumber(PartnerUpdateDTO update)
        {
            if (string.IsNullOrEmpty(update.OldEmail))
            {
                throw new GenericException("Partner Email is invalid");
            }

            update.OldEmail = string.IsNullOrEmpty(update.OldEmail) ? update.OldEmail : update.OldEmail.Trim();
            update.NewEmail = string.IsNullOrEmpty(update.NewEmail) ? update.NewEmail : update.NewEmail.Trim();
            update.OldPhoneNumber = string.IsNullOrEmpty(update.OldPhoneNumber) ? update.OldPhoneNumber : update.OldPhoneNumber.Trim();
            update.NewPhoneNumber = string.IsNullOrEmpty(update.NewPhoneNumber) ? update.NewPhoneNumber : update.NewPhoneNumber.Trim();

            var subOldNumber = string.IsNullOrEmpty(update.OldPhoneNumber) ? update.OldPhoneNumber : update.OldPhoneNumber.Substring(1);
            var subNewNumber = string.IsNullOrEmpty(update.NewPhoneNumber) ? update.NewPhoneNumber : update.NewPhoneNumber.Substring(1);
            var countryCode = "";
            var prefixNewNumber = "";
            var partners = await _uow.Partner.GetPartnerByEmail(update.OldEmail);

            if (partners.Count == 0)
            {
                throw new GenericException("Partner Information Not Found!", $"{(int)HttpStatusCode.NotFound}");
            }

            
            foreach (var partner in partners)
            {
                if (partner.UserActiveCountryId != 0)
                {
                    countryCode = _uow.Country.GetAllAsQueryable().Where(x => x.CountryId == partner.UserActiveCountryId).Select(x => new { x.PhoneNumberCode }).FirstOrDefault().PhoneNumberCode;
                }

                if (!string.IsNullOrEmpty(update.NewEmail) && partner.Email.Contains(update.OldEmail))
                {
                    partner.Email = update.NewEmail;
                }

                if (!string.IsNullOrEmpty(update.NewPhoneNumber) && !string.IsNullOrEmpty(update.OldPhoneNumber))
                {
                    prefixNewNumber = $"{countryCode}{subNewNumber}";

                    if (!string.IsNullOrEmpty(partner.PhoneNumber))
                    {
                        if ( partner.PhoneNumber.Contains(subOldNumber))
                        {
                            partner.PhoneNumber = prefixNewNumber;
                        }
                    }
                    else
                    {
                        partner.PhoneNumber = prefixNewNumber;
                    }
                }
            }

            var users = await _uow.User.GetPartnerUsersByEmail(update.OldEmail);

            if (users.Count != 0)
            {
                foreach (var user in users)
                {
                    if (user.UserActiveCountryId != 0)
                    {
                        countryCode = _uow.Country.GetAllAsQueryable().Where(x => x.CountryId == user.UserActiveCountryId).Select(x => new { x.PhoneNumberCode }).FirstOrDefault().PhoneNumberCode;
                    }

                    if (!string.IsNullOrEmpty(update.NewEmail) && user.Email.Contains(update.OldEmail))
                    {
                        user.Email = update.NewEmail;
                    }

                    if (!string.IsNullOrEmpty(update.NewPhoneNumber) && !string.IsNullOrEmpty(update.OldPhoneNumber))
                    {
                        prefixNewNumber = $"{countryCode}{subNewNumber}";

                        if (!string.IsNullOrEmpty(user.PhoneNumber))
                        {
                            if (user.PhoneNumber.Contains(subOldNumber))
                            {
                                user.PhoneNumber = prefixNewNumber;
                            }
                        }
                        else
                        {
                            user.PhoneNumber = prefixNewNumber;
                        }
                    }
                }
            }

            await _uow.CompleteAsync();
        }

        //Assign a Shipment to a Partner
        public async Task<bool> AssignShipmentToPartner(ShipmentAssignmentDTO partnerInfo)
        {
            try
            {
                var result = false;

                var preshipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == partnerInfo.Waybill);
                if (preshipment == null)
                {
                    throw new GenericException($"This waybill {partnerInfo.Waybill} does not exist");
                }

                if (preshipment.shipmentstatus != "Shipment created")
                {
                    throw new GenericException($"This waybill {partnerInfo.Waybill} status has to Shipment Created to perform this action.");
                }
                if (string.IsNullOrWhiteSpace(partnerInfo.Email))
                {
                    throw new GenericException($"Partner Email is required.");
                }

                var partner = await _uow.Partner.GetAsync(x => x.Email == partnerInfo.Email);
                if (partner == null)
                {
                    throw new GenericException($"This partner  does not exist");
                }

                var nodePayload = new AcceptShipmentPayload()
                {
                    WaybillNumber = preshipment.Waybill,
                    PartnerId = partner.PartnerId.ToString(),
                    PartnerInfo = new PartnerPayload()
                    {
                        FullName = partner.PartnerName,
                        PhoneNumber = partner.PhoneNumber,
                        VehicleType = partnerInfo.VehicleType,
                        ImageUrl = partner.PictureUrl
                    },

                };
                await _nodeService.AssignShipmentToPartner(nodePayload);

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CaptainTransactionDTO>> GetCaptainTransactions(PaginationDTO pagination)
        {
            try
            {
                var transactions = new List<CaptainTransactionDTO>();
                var captains = await _uow.User.GetCaptains();
                if (captains.Any())
                {
                    if (pagination != null && pagination.StartDate == null && pagination.EndDate == null)
                    {
                        var now = DateTime.Now;
                        DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                        DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                        pagination.StartDate = firstDay;
                        pagination.EndDate = DateTime.Now;
                    }
                    else if (pagination != null && pagination.StartDate != null && pagination.EndDate == null)
                    {
                        pagination.EndDate = DateTime.Now;
                    }

                    if (pagination != null && pagination.StartDate != null && pagination.EndDate != null)
                    {
                        pagination.StartDate = pagination.StartDate.Value.ToUniversalTime();
                        pagination.StartDate = pagination.StartDate.Value.AddHours(12).AddMinutes(00);
                        pagination.EndDate = pagination.EndDate.Value.ToUniversalTime();
                        pagination.EndDate = pagination.EndDate.Value.AddHours(23).AddMinutes(59);
                    }
                    var captainIds = captains.Select(x => x.Id).ToList();
                    var walletTransactions = _uow.PartnerTransactions.GetAllAsQueryable().Where(x => captainIds.Contains(x.UserId) && x.DateCreated >= pagination.StartDate && x.DateCreated <= pagination.EndDate).GroupBy(x => x.UserId).ToList();
                    if (walletTransactions.Any())
                    {
                        transactions = (from r in walletTransactions
                                        select new CaptainTransactionDTO()
                                        {
                                            PartnerCode = captains.FirstOrDefault(x => x.Id == r.Key).UserChannelCode,
                                            PartnerName = $"{captains.FirstOrDefault(x => x.Id == r.Key).FirstName} {captains.FirstOrDefault(x => x.Id == r.Key).LastName}",
                                            PartnerEmail = captains.FirstOrDefault(x => x.Id == r.Key).Email,
                                            PartnerType = captains.FirstOrDefault(x => x.Id == r.Key).SystemUserRole,
                                            Amount = r.Sum(x => x.AmountReceived),

                                        }).ToList();

                    }
                }
                return transactions;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
