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

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerService : IPartnerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly ICompanyService _companyService;

        public PartnerService(IUnitOfWork uow, IWalletService walletService, 
            INumberGeneratorMonitorService numberGeneratorMonitorService, ICompanyService companyService)
        {
            _uow = uow;
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _companyService = companyService;
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

            if(partner == null)
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
                return riderRates;
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

                    var dlvrd = pickupRequests.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.Status == MobilePickUpRequestStatus.Delivered.ToString() && x.Waybill == item.Waybill);
                    if (dlvrd != null)
                    {
                        dtat += (int)(dlvrd.DateCreated - createdDay.DateCreated).TotalMinutes;
                    }
                    var assigned = mobiletracking.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.Status == ShipmentScanStatus.MAPT.ToString() && x.Waybill == item.Waybill);
                    if (assigned != null)
                    {
                        //atat += (int)(assigned.DateCreated - createdDay.DateCreated).TotalMinutes;
                        atat += (int)(assigned.DateCreated).Minute;
                    }

                    var picked = mobiletracking.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.Status == ShipmentScanStatus.MSHC.ToString() && x.Waybill == item.Waybill);
                    if (picked != null && assigned != null)
                    {
                        ptat += (int)(picked.DateCreated - assigned.DateCreated).TotalMinutes;
                        //ptat += (int)picked.DateCreated.Minute - assigned.DateCreated.Minute;
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
                    rider.AssignTAT = atat/ userTransac.Count; 
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
    }
}
