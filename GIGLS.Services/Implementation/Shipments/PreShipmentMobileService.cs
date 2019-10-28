using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.DTO.Partnership;
using System.Configuration;
using GIGLS.Core.IMessage;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO.Customers;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain.Partnership;
using System.IO;
using System.Drawing;
using System.Web;
using GIGLS.Core.DTO.User;
using VehicleType = GIGLS.Core.Domain.VehicleType;
using Hangfire;

namespace GIGLS.Services.Implementation.Shipments
{
    public class PreShipmentMobileService : IPreShipmentMobileService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
        private readonly IDeliveryOptionService _deliveryService;
        private readonly IServiceCentreService _centreService;
        private readonly IUserServiceCentreMappingService _userServiceCentre;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPricingService _pricingService;
        private readonly IWalletService _walletService;
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IUserService _userService;
        private readonly ISpecialDomesticPackageService _specialdomesticpackageservice;
        private readonly IMobileShipmentTrackingService _mobiletrackingservice;
        private readonly IMobilePickUpRequestsService _mobilepickuprequestservice;
        private readonly IDomesticRouteZoneMapService _domesticroutezonemapservice;
        private readonly ICategoryService _categoryservice;
        private readonly ISubCategoryService _subcategoryservice;
        private readonly IPartnerTransactionsService _partnertransactionservice;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IMobileRatingService _mobileratingService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IHaulageService _haulageService;
        private readonly IHaulageDistanceMappingService _haulageDistanceMappingService;

        public PreShipmentMobileService(IUnitOfWork uow, IShipmentService shipmentService, IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, IUserServiceCentreMappingService userServiceCentre, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPricingService pricingService, IWalletService walletService, IWalletTransactionService walletTransactionService,
            IUserService userService, ISpecialDomesticPackageService specialdomesticpackageservice, IMobileShipmentTrackingService mobiletrackingservice,
            IMobilePickUpRequestsService mobilepickuprequestservice, IDomesticRouteZoneMapService domesticroutezonemapservice, ICategoryService categoryservice, ISubCategoryService subcategoryservice,
            IPartnerTransactionsService partnertransactionservice, IGlobalPropertyService globalPropertyService, IMobileRatingService mobileratingService, IMessageSenderService messageSenderService,
            IHaulageService haulageService, IHaulageDistanceMappingService haulageDistanceMappingService)
        {
            _uow = uow;
            _shipmentService = shipmentService;
            _deliveryService = deliveryService;
            _centreService = centreService;
            _userServiceCentre = userServiceCentre;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _pricingService = pricingService;
            _walletService = walletService;
            _walletTransactionService = walletTransactionService;
            _userService = userService;
            _specialdomesticpackageservice = specialdomesticpackageservice;
            _mobiletrackingservice = mobiletrackingservice;
            _mobilepickuprequestservice = mobilepickuprequestservice;
            _domesticroutezonemapservice = domesticroutezonemapservice;
            _subcategoryservice = subcategoryservice;
            _categoryservice = categoryservice;
            _partnertransactionservice = partnertransactionservice;
            _globalPropertyService = globalPropertyService;
            _mobileratingService = mobileratingService;
            _messageSenderService = messageSenderService;
            _haulageService = haulageService;
            _haulageDistanceMappingService = haulageDistanceMappingService;

            MapperConfig.Initialize();
        }

        public async Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment)
        {
            try
            {
                //null DateCreated
                preShipment.DateCreated = DateTime.Now;
                var zoneid = await _domesticroutezonemapservice.GetZoneMobile(preShipment.SenderStationId, preShipment.ReceiverStationId);
                preShipment.ZoneMapping = zoneid.ZoneId;
                var newPreShipment = await CreatePreShipment(preShipment);
                await _uow.CompleteAsync();
                bool IsBalanceSufficient = true;
                string message = "Shipment created successfully";
                if (newPreShipment.IsBalanceSufficient == false)
                {
                    message = "Insufficient Wallet Balance";
                    IsBalanceSufficient = false;
                }
                return new { waybill = newPreShipment.Waybill, message = message, IsBalanceSufficient, Zone = zoneid.ZoneId };
            }
            catch (Exception)
            {
                throw new GenericException("An error occurred while attempting to add preshipment.");
            }
        }

        private async Task<PreShipmentMobileDTO> CreatePreShipment(PreShipmentMobileDTO preShipmentDTO)
        {
            try
            {
                var PreshipmentPriceDTO = new MobilePriceDTO();
                // get the current user info
                var currentUserId = await _userService.GetCurrentUserId();
                preShipmentDTO.UserId = currentUserId;
                var user = await _userService.GetUserById(currentUserId);
                var Country = await _uow.Country.GetCountryByStationId(preShipmentDTO.SenderStationId);
                preShipmentDTO.CountryId = Country.CountryId;
                 if (preShipmentDTO.VehicleType.ToLower() == Vehicletype.Truck.ToString().ToLower())
                 {
                        PreshipmentPriceDTO = await GetHaulagePrice(new HaulagePriceDTO
                        {
                            Haulageid = (int)preShipmentDTO.Haulageid,
                            DepartureStationId = preShipmentDTO.SenderStationId,
                            DestinationStationId = preShipmentDTO.ReceiverStationId
                        });
                        preShipmentDTO.CalculatedTotal = (double)PreshipmentPriceDTO.GrandTotal;
                  }
                  else
                  {
                        PreshipmentPriceDTO = await GetPrice(preShipmentDTO);
                  }
               
                var wallet = await _walletService.GetWalletBalance();
                if (wallet.Balance >= Convert.ToDecimal(PreshipmentPriceDTO.GrandTotal))
                {
                    var price = (wallet.Balance - Convert.ToDecimal(PreshipmentPriceDTO.GrandTotal));
                    var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber);
                    preShipmentDTO.Waybill = waybill;
                    if (preShipmentDTO.PreShipmentItems.Count == 0)
                    {
                        throw new GenericException("Shipment Items cannot be empty");
                    }
                    foreach (var item in preShipmentDTO.PreShipmentItems)
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            preShipmentDTO.Value += Convert.ToDecimal(item.Value);
                            preShipmentDTO.IsdeclaredVal = true;
                        }

                    }
                    var newPreShipment = Mapper.Map<PreShipmentMobile>(preShipmentDTO);

                    if (user.UserChannelType == UserChannelType.Ecommerce)
                    {
                        newPreShipment.CustomerType = CustomerType.Company.ToString();
                        newPreShipment.CompanyType = CompanyType.Ecommerce.ToString();
                    }
                    else
                    {
                        newPreShipment.CustomerType = "Individual";
                        newPreShipment.CompanyType = CustomerType.IndividualCustomer.ToString();
                    }
                    newPreShipment.IsConfirmed = false;
                    newPreShipment.IsDelivered = false;
                    newPreShipment.shipmentstatus = "Shipment created";
                    newPreShipment.DateCreated = DateTime.Now;
                    newPreShipment.GrandTotal = (decimal)PreshipmentPriceDTO.GrandTotal;
                    preShipmentDTO.IsBalanceSufficient = true;
                    _uow.PreShipmentMobile.Add(newPreShipment);
                    await _uow.CompleteAsync();
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = newPreShipment.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MCRT
                    });
                    var defaultServiceCenter = await _userService.GetDefaultServiceCenter();

                    var transaction = new WalletTransactionDTO
                    {
                        WalletId = wallet.WalletId,
                        CreditDebitType = CreditDebitType.Debit,
                        Amount = (decimal)newPreShipment.CalculatedTotal,
                        ServiceCentreId = defaultServiceCenter.ServiceCentreId,
                        Waybill = waybill,
                        Description = "Payment for Shipment",
                        PaymentType = PaymentType.Online,
                        UserId = newPreShipment.UserId
                    };
                    var walletTransaction = await _walletTransactionService.AddWalletTransaction(transaction);
                    var updatedwallet = await _uow.Wallet.GetAsync(wallet.WalletId);
                    updatedwallet.Balance = price;
                    await _uow.CompleteAsync();
                    return preShipmentDTO;
                }
                else
                {
                    preShipmentDTO.IsBalanceSufficient = false;
                    return preShipmentDTO;
                }
            }
            catch
            {
                throw new GenericException("An error occurred while attempting to create shipment.");
            }
        }

        public async Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
            try
            {
                if (preShipment.PreShipmentItems.Count() == 0)
                {
                    throw new GenericException("No Preshipitem was added");
                }
                //var ShipmentCount = preShipment.PreShipmentItems.Count();
                var Price = 0.0M;
                decimal DeclaredValue = 0.0M;
                var Country = await _uow.Country.GetCountryByStationId(preShipment.SenderStationId);
                preShipment.CountryId = Country.CountryId;
                var Pickuprice = await GetPickUpPrice(preShipment.VehicleType,preShipment.CountryId);
                var PickupValue = Convert.ToDecimal(Pickuprice);
                var IsWithinProcessingTime = await WithinProcessingTime(preShipment.CountryId);
                foreach (var preShipmentItem in preShipment.PreShipmentItems)
                {
                    if (preShipmentItem.Quantity == 0)
                    {
                        throw new GenericException("Quantity cannot be zero");
                    }
                    var PriceDTO = new PricingDTO
                    {
                        DepartureStationId = preShipment.SenderStationId,
                        DestinationStationId = preShipment.ReceiverStationId,
                        Weight = (decimal)preShipmentItem.Weight,
                        SpecialPackageId = (int)preShipmentItem.SpecialPackageId,
                        ShipmentType = preShipmentItem.ShipmentType,
                        CountryId = preShipment.CountryId  //Nigeria
                    };
                    if (preShipmentItem.ShipmentType == ShipmentType.Ecommerce)
                    {
                        preShipmentItem.CalculatedPrice = await _pricingService.GetMobileEcommercePrice(PriceDTO);
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                        //preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                    }
                    if (preShipmentItem.ShipmentType == ShipmentType.Special)
                    {
                        preShipmentItem.CalculatedPrice = await _pricingService.GetMobileSpecialPrice(PriceDTO);
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                        //preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                    }
                    else if (preShipmentItem.ShipmentType == ShipmentType.Regular)
                    {
                        preShipmentItem.CalculatedPrice = await _pricingService.GetMobileRegularPrice(PriceDTO);
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                        //preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                    }
                    var vatForPreshipment = (preShipmentItem.CalculatedPrice * 0.05M);
                    if (!string.IsNullOrEmpty(preShipmentItem.Value))
                    {
                        DeclaredValue += Convert.ToDecimal(preShipmentItem.Value);
                        var DeclaredValueForPreShipment = Convert.ToDecimal(preShipmentItem.Value);
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + (DeclaredValueForPreShipment * 0.01M) + vatForPreshipment;
                        preShipmentItem.CalculatedPrice = (decimal)Math.Round((double)preShipmentItem.CalculatedPrice);
                        preShipment.IsdeclaredVal = true;
                    }
                    else
                    {
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + vatForPreshipment;
                        preShipmentItem.CalculatedPrice = (decimal)Math.Round((double)preShipmentItem.CalculatedPrice);
                    }
                    Price += (decimal)preShipmentItem.CalculatedPrice;
                };
                decimal EstimatedDeclaredPrice = Convert.ToDecimal(DeclaredValue);
                preShipment.DeliveryPrice = Price;
                preShipment.InsuranceValue = (EstimatedDeclaredPrice * 0.01M);
                preShipment.CalculatedTotal = (double)(preShipment.DeliveryPrice);
                preShipment.CalculatedTotal = Math.Round((double)preShipment.CalculatedTotal);
                preShipment.Value = DeclaredValue;
                var returnprice = new MobilePriceDTO()
                {
                    MainCharge = (decimal)preShipment.CalculatedTotal,
                    PickUpCharge = PickupValue,
                    InsuranceValue = preShipment.InsuranceValue,
                    GrandTotal = ((decimal)preShipment.CalculatedTotal + PickupValue),
                    PreshipmentMobile = preShipment,
                    CurrencySymbol = Country.CurrencySymbol,
                    CurrencyCode = Country.CurrencyCode,
                    IsWithinProcessingTime = IsWithinProcessingTime
                };
                return returnprice;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<PreShipmentMobileDTO>> GetShipments(BaseFilterCriteria filterOptionsDto)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterOptionsDto.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;
                var allShipments = _uow.PreShipmentMobile.GetAllAsQueryable();

                if (filterOptionsDto.StartDate == null & filterOptionsDto.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }
                var allShipmentsResult = allShipments.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

                List<PreShipmentMobileDTO> shipmentDto = (from r in allShipmentsResult
                                                          select new PreShipmentMobileDTO()
                                                          {
                                                              PreShipmentMobileId = r.PreShipmentMobileId,
                                                              Waybill = r.Waybill,
                                                              CustomerType = r.CustomerType,
                                                              ActualDateOfArrival = r.ActualDateOfArrival,
                                                              DateCreated = r.DateCreated,
                                                              DateModified = r.DateModified,
                                                              ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                              ReceiverAddress = r.ReceiverAddress,
                                                              SenderAddress = r.SenderAddress,
                                                              SenderPhoneNumber = r.SenderPhoneNumber,
                                                              ReceiverCity = r.ReceiverCity,
                                                              ReceiverCountry = r.ReceiverCountry,
                                                              ReceiverEmail = r.ReceiverEmail,
                                                              ReceiverName = r.ReceiverName,
                                                              ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                              ReceiverState = r.ReceiverState,
                                                              SenderName = r.SenderName,
                                                              UserId = r.UserId,
                                                              Value = r.Value,
                                                              shipmentstatus = r.shipmentstatus,
                                                              GrandTotal = r.GrandTotal,
                                                              DeliveryPrice = r.DeliveryPrice,
                                                              CalculatedTotal = r.CalculatedTotal,
                                                              InsuranceValue = r.InsuranceValue,
                                                              DiscountValue = r.DiscountValue,
                                                              CompanyType = r.CompanyType,
                                                              CustomerCode = r.CustomerCode
                                                          }).ToList();

                return await Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill)
        {
            try
            {
                var shipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == waybill, "PreShipmentItems,SenderLocation,ReceiverLocation");
                var Shipmentdto = Mapper.Map<PreShipmentMobileDTO>(shipment);
                if (shipment == null)
                {
                    throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
                }
                return await Task.FromResult(Shipmentdto);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser()
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserId();
                var user = await _uow.User.GetUserById(currentUser);
                var shipment = await _uow.PreShipmentMobile.FindAsync(x => x.CustomerCode.Equals(user.UserChannelCode), "PreShipmentItems");

                List<PreShipmentMobileDTO> shipmentDto = (from r in shipment
                                                          select new PreShipmentMobileDTO()
                                                          {
                                                              PreShipmentMobileId = r.PreShipmentMobileId,
                                                              Waybill = r.Waybill,
                                                              ActualDateOfArrival = r.ActualDateOfArrival,
                                                              DateCreated = r.DateCreated,
                                                              DateModified = r.DateModified,
                                                              ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                              ReceiverAddress = r.ReceiverAddress,
                                                              SenderAddress = r.SenderAddress,
                                                              SenderPhoneNumber = r.SenderPhoneNumber,
                                                              ReceiverCountry = r.ReceiverCountry,
                                                              SenderStationId = r.SenderStationId,
                                                              ReceiverStationId = r.ReceiverStationId,
                                                              ReceiverEmail = r.ReceiverEmail,
                                                              ReceiverName = r.ReceiverName,
                                                              ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                              ReceiverState = r.ReceiverState,
                                                              SenderName = r.SenderName,
                                                              UserId = r.UserId,
                                                              Value = r.Value,
                                                              shipmentstatus = r.shipmentstatus,
                                                              GrandTotal = r.GrandTotal,
                                                              DeliveryPrice = r.DeliveryPrice,
                                                              CalculatedTotal = r.CalculatedTotal,
                                                              CustomerCode = r.CustomerCode
                                                          }).ToList();
                foreach (var Shipment in shipmentDto)
                {
                    var PartnerId = await _uow.MobilePickUpRequests.GetAsync(r => r.Waybill == Shipment.Waybill && r.Status != MobilePickUpRequestStatus.Rejected.ToString());
                    if (PartnerId != null)
                    {
                        var partneruser = await _uow.User.GetUserById(PartnerId.UserId);
                        if (partneruser != null)
                        {
                            Shipment.PartnerFirstName = partneruser.FirstName;
                            Shipment.PartnerLastName = partneruser.LastName;
                            Shipment.PartnerImageUrl = partneruser.PictureUrl;
                        }
                    }
                    var rating = await _uow.MobileRating.GetAsync(j => j.Waybill == Shipment.Waybill);
                    if (rating != null)
                    {
                        Shipment.IsRated = rating.IsRatedByCustomer;
                    }
                    else
                    {
                        Shipment.IsRated = false;
                    }
                    var country = await _uow.Country.GetCountryByStationId(Shipment.SenderStationId);
                    if (country != null)
                    {
                        Shipment.CurrencyCode = country.CurrencyCode;
                        Shipment.CurrencySymbol = country.CurrencySymbol;
                    }
                }
                return await Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages()
        {
            try
            {
                var result = _uow.SpecialDomesticPackage.GetAll();
                var packages = from s in result
                               select new SpecialDomesticPackageDTO
                               {
                                   SpecialDomesticPackageId = s.SpecialDomesticPackageId,
                                   SpecialDomesticPackageType = s.SpecialDomesticPackageType,
                                   Name = s.Name,
                                   Status = s.Status,
                                   SubCategory = new SubCategoryDTO
                                   {
                                       SubCategoryName = s.SubCategory.SubCategoryName,
                                       Category = new CategoryDTO
                                       {
                                           CategoryId = s.SubCategory.Category.CategoryId,
                                           CategoryName = s.SubCategory.Category.CategoryName
                                       },
                                       SubCategoryId = s.SubCategory.SubCategoryId,
                                       CategoryId = s.SubCategory.CategoryId,
                                       Weight = s.SubCategory.Weight
                                   }


                               };
                return packages;
            }
            catch(Exception)
            {
                throw new GenericException("Please an error occured while getting Special Packages");
            }
        }
        public async Task<List<CategoryDTO>> GetCategories()
        {
            try
            {
                var categories = await _categoryservice.GetCategories();
                return categories;
            }
            catch(Exception)
            {
                throw new GenericException("Please an error occured while getting categories.Please try again");
            }
        }
        public async Task<List<SubCategoryDTO>> GetSubCategories()
        {
            try
            {
                var subcategories = await _subcategoryservice.GetSubCategories();
                return subcategories;
            }
            catch(Exception)
            {
                throw new GenericException("Please an error occurred while getting sub-categories.Please try again");
            }
        }

        public async Task ScanMobileShipment(ScanDTO scan)
        {
            // verify the waybill number exists in the system
            try
            {
                var shipment = await GetMobileShipmentForScan(scan.WaybillNumber);
                string scanStatus = scan.ShipmentScanStatus.ToString();
                if (shipment != null)
                {
                    await _mobiletrackingservice.AddMobileShipmentTracking(new MobileShipmentTrackingDTO
                    {
                        DateTime = DateTime.Now,
                        Status = scanStatus,
                        Waybill = scan.WaybillNumber,
                    }, scan.ShipmentScanStatus);
                }
            }
            catch(Exception)
            {
                throw new GenericException("Please an error occurred while trying to scan shipment.");
            }


        }
        public async Task<PreShipmentMobile> GetMobileShipmentForScan(string waybill)
        {
            try
            {
                var shipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill.Equals(waybill));
                if (shipment == null)
                {
                    throw new GenericException("Waybill does not exist");
                }
                return shipment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MobileShipmentTrackingHistoryDTO> TrackShipment(string waybillNumber)
        {
            try
            {
                var result = await _mobiletrackingservice.GetMobileShipmentTrackings(waybillNumber);
                return result;
            }
            catch
            {
                throw new GenericException("Error: You cannot track this waybill number.");
            }
        }
        public async Task<PreShipmentMobileDTO> AddMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest)
        {
            try
            {
                var userId = await _userService.GetCurrentUserId();
                pickuprequest.UserId = userId;
                if (pickuprequest.Status == MobilePickUpRequestStatus.Rejected.ToString())
                {
                    var request = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == pickuprequest.Waybill && s.UserId == userId && s.Status == MobilePickUpRequestStatus.Rejected.ToString());
                    if (request == null)
                    {
                        await _mobilepickuprequestservice.AddMobilePickUpRequests(pickuprequest);
                    }
                    else
                    {
                        throw new GenericException($"Shipment with waybill number: {pickuprequest.Waybill} exists already");
                    }
                }
                else
                {
                    pickuprequest.Status = MobilePickUpRequestStatus.Accepted.ToString();
                    await _mobilepickuprequestservice.AddMobilePickUpRequests(pickuprequest);
                }
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "PreShipmentItems,SenderLocation,ReceiverLocation");
                if (preshipmentmobile != null)
                {
                    if (pickuprequest.ServiceCentreId != null)
                    {

                        var DestinationServiceCentreId = await _uow.ServiceCentre.GetAsync(s => s.Code == pickuprequest.ServiceCentreId);
                        preshipmentmobile.ReceiverAddress = DestinationServiceCentreId.Address;
                        preshipmentmobile.ReceiverLocation.Latitude = DestinationServiceCentreId.Latitude;
                        preshipmentmobile.ReceiverLocation.Longitude = DestinationServiceCentreId.Longitude;

                    }
                    if (pickuprequest.Status == MobilePickUpRequestStatus.Accepted.ToString())
                    {
                        preshipmentmobile.shipmentstatus = "Assigned for Pickup";
                    }
                    else
                    {
                        preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Processing.ToString();
                    }
                    
                    var newPreShipment = Mapper.Map<PreShipmentMobileDTO>(preshipmentmobile);

                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MAPT
                    });
                    await _uow.CompleteAsync();
                    return newPreShipment;
                }
                else
                {
                    throw new GenericException("Waybill Does Not Exist");
                }
                
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> UpdateMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest)
        {
            try
            {
                await _mobilepickuprequestservice.UpdateMobilePickUpRequests(pickuprequest);
                var userId = await _userService.GetCurrentUserId();
                if (pickuprequest.Status == MobilePickUpRequestStatus.Confirmed.ToString())
                {
                    var customerid = 0;
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MSHC
                    });
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "PreShipmentItems,SenderLocation,ReceiverLocation");
                    var DepartureStation = await _uow.Station.GetAsync(s => s.StationId == preshipmentmobile.SenderStationId);
                    var DestinationStation = await _uow.Station.GetAsync(s => s.StationId == preshipmentmobile.ReceiverStationId);
                    var CustomerId = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                    if (CustomerId != null)
                    {
                        customerid = CustomerId.IndividualCustomerId;
                    }
                    if (CustomerId == null)
                    {
                        var companyid = await _uow.Company.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                        customerid = companyid.CompanyId;
                    }
                    
                    var MobileShipment = new ShipmentDTO
                    {
                        Waybill = preshipmentmobile.Waybill,
                        ReceiverName = preshipmentmobile.ReceiverName,
                        ReceiverPhoneNumber = preshipmentmobile.ReceiverPhoneNumber,
                        ReceiverEmail = preshipmentmobile.ReceiverEmail,
                        ReceiverAddress = preshipmentmobile.ReceiverAddress,
                        DeliveryOptionId = 1,
                        GrandTotal = preshipmentmobile.GrandTotal,
                        Insurance = preshipmentmobile.InsuranceValue,
                        Vat = preshipmentmobile.Vat,
                        SenderAddress = preshipmentmobile.SenderAddress,
                        IsCashOnDelivery = false,
                        CustomerCode = preshipmentmobile.CustomerCode,
                        DestinationServiceCentreId = DestinationStation.SuperServiceCentreId,
                        DepartureServiceCentreId = DepartureStation.SuperServiceCentreId,
                        CustomerId = customerid,
                        UserId = userId,
                        PickupOptions = PickupOptions.HOMEDELIVERY,
                        IsdeclaredVal = preshipmentmobile.IsdeclaredVal,
                        ShipmentPackagePrice = preshipmentmobile.GrandTotal,
                        ApproximateItemsWeight = 0.00,
                        ReprintCounterStatus = false,
                        CustomerType = preshipmentmobile.CustomerType,
                        CompanyType = preshipmentmobile.CompanyType,
                        Value = preshipmentmobile.Value,
                        PaymentStatus = PaymentStatus.Paid,
                        IsFromMobile = true,
                        ShipmentItems = preshipmentmobile.PreShipmentItems.Select(s => new ShipmentItemDTO
                        {
                            Description = s.Description,
                            IsVolumetric = s.IsVolumetric,
                            Weight = s.Weight,
                            Nature = s.ItemType,
                            Price = (decimal)s.CalculatedPrice,
                            Quantity = s.Quantity,
                            //Length = (double)s.Length,
                            //Width = (double)s.Width,
                            //Height = (double)s.Height

                        }).ToList()
                    };
                    var status = await _shipmentService.AddShipmentFromMobile(MobileShipment);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.PickedUp.ToString();
                    preshipmentmobile.IsConfirmed = true;
                    var item = Mapper.Map<PreShipmentMobileDTO>(preshipmentmobile);
                    await CheckDeliveryTimeAndSendMail(item);
                    await _uow.CompleteAsync();
                }
                if (pickuprequest.Status == MobilePickUpRequestStatus.Delivered.ToString())
                {
                    
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "SenderLocation,ReceiverLocation");
                    var Pickuprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId);
                    if (preshipmentmobile.ZoneMapping == 1)
                    {
                        await ScanMobileShipment(new ScanDTO
                        {
                            WaybillNumber = pickuprequest.Waybill,
                            ShipmentScanStatus = ShipmentScanStatus.MAHD
                        });
                        var Partnerpaymentfordelivery = new PartnerPayDTO
                        {
                            PickUprice = Pickuprice,
                            ShipmentPrice = (decimal)preshipmentmobile.CalculatedTotal,
                            ZoneMapping = (int)preshipmentmobile.ZoneMapping
                        };

                        decimal price = await _partnertransactionservice.GetPriceForPartner(Partnerpaymentfordelivery);
                        var partneruser = await _userService.GetUserById(userId);
                        var wallet = _uow.Wallet.GetAsync(s => s.CustomerCode == partneruser.UserChannelCode).Result;
                        wallet.Balance = wallet.Balance + price;
                        var partnertransactions = new PartnerTransactionsDTO
                        {
                            Destination = preshipmentmobile.ReceiverAddress,
                            Departure = preshipmentmobile.SenderAddress,
                            AmountReceived = price,
                            Waybill = preshipmentmobile.Waybill
                        };
                        var id = await _partnertransactionservice.AddPartnerPaymentLog(partnertransactions);
                        preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Delivered.ToString();
                        await _uow.CompleteAsync();
                        var messageextensionDTO = new MobileMessageDTO()
                        {
                            SenderName = preshipmentmobile.ReceiverName,
                            WaybillNumber = preshipmentmobile.Waybill,
                            SenderPhoneNumber = preshipmentmobile.ReceiverPhoneNumber
                        };
                        await _messageSenderService.SendMessage(MessageType.OKC, EmailSmsType.SMS, messageextensionDTO);
                    }
                    else
                    {
                        throw new GenericException("This shipment is an interstate delivery, drop at the assigned service centre");
                    }
                    
                }
                if (pickuprequest.Status == MobilePickUpRequestStatus.Cancelled.ToString())
                {
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.SSC
                    });
                }
                if (pickuprequest.Status == MobilePickUpRequestStatus.Dispute.ToString())
                {
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Dispute.ToString();
                    await _uow.CompleteAsync();
                }
                if (pickuprequest.Status == MobilePickUpRequestStatus.LogVisit.ToString())
                {
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                    var Mobilerequest = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                    Mobilerequest.Status = MobilePickUpRequestStatus.Visited.ToString();
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Visited.ToString();
                    await _uow.CompleteAsync();
                    var user = await _userService.GetUserByChannelCode(preshipmentmobile.CustomerCode);
                    //send Email to Sender's Email Address
                    var messageExtensionDTO = new MobileMessageDTO()
                    {
                        SenderName = user.FirstName + " " + user.LastName,
                        SenderEmail = user.Email,
                        WaybillNumber = preshipmentmobile.Waybill,
                        SenderPhoneNumber = preshipmentmobile.SenderPhoneNumber
                    };
                    await _messageSenderService.SendGenericEmailMessage(MessageType.MATD, messageExtensionDTO);
                    //send SMS to Receiver's Phone Number
                    var messageextensionDTO = new MobileMessageDTO()
                    {
                        SenderName = preshipmentmobile.ReceiverName,
                        WaybillNumber = preshipmentmobile.Waybill,
                        SenderPhoneNumber = preshipmentmobile.ReceiverPhoneNumber
                    };
                    await _messageSenderService.SendMessage(MessageType.MATD, EmailSmsType.SMS, messageExtensionDTO);

                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MobilePickUpRequestsDTO>> GetMobilePickupRequest()
        {
            try
            {
                var mobilerequests = await _mobilepickuprequestservice.GetAllMobilePickUpRequests();
                return mobilerequests;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdatePreShipmentMobileDetails(List<PreShipmentItemMobileDTO> preshipmentmobile)
        {
            try
            {
                foreach (var preShipment in preshipmentmobile)
                {
                    var preshipment = await _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentMobileId == preShipment.PreShipmentMobileId && s.PreShipmentItemMobileId == preShipment.PreShipmentItemMobileId);
                    if (preShipment == null)
                    {
                        throw new GenericException("PreShipmentItem Does Not Exist");
                    }
                    preshipment.CalculatedPrice = preShipment.CalculatedPrice;
                    preshipment.Description = preShipment.Description;
                    preshipment.EstimatedPrice = preShipment.EstimatedPrice;
                    preshipment.ImageUrl = preShipment.ImageUrl;
                    preshipment.IsVolumetric = preShipment.IsVolumetric;
                    preshipment.ItemName = preShipment.ItemName;
                    preshipment.ItemType = preShipment.ItemType;
                    preshipment.Length = preShipment.Length;
                    preshipment.Quantity = preShipment.Quantity;
                    preshipment.Weight = preShipment.Weight;
                    preshipment.Width = preShipment.Width;
                    preshipment.Height = preShipment.Height;
                    await _uow.CompleteAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialResultDTO> GetSpecialPackages()
        {
            try
            {
                var packages = await GetSpecialDomesticPackages();
                var Categories = await GetCategories();
                var Subcategories = await GetSubCategories();
                var dictionaryCategories = await GetDictionaryCategories();
                var result = new SpecialResultDTO
                {
                    Specialpackages = packages.ToList(),
                    Categories = Categories,
                    SubCategories = Subcategories,
                    DictionaryCategory = dictionaryCategories

                };
                return result;
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get all special packages.");
            }
        }

        private async Task<Dictionary<string, List<string>>> GetDictionaryCategories()
        {
            try
            {
                //
                Dictionary<string, List<string>> finalDictionary = new Dictionary<string, List<string>>();

                //1. category
                var Categories = await GetCategories();
                finalDictionary.Add("CATEGORY", Categories.Select(s => s.CategoryName).OrderBy(s => s).ToList());


                //2. subcategory
                var Subcategories = await GetSubCategories();
                foreach (var category in Categories)
                {
                    var listOfSubcategory = Subcategories.Where(s => s.Category.CategoryId == category.CategoryId).
                        Select(s => s.SubCategoryName).Distinct().ToList();

                    //add to dictionary
                    finalDictionary.Add(category.CategoryName, listOfSubcategory);

                }


                //3. subsubcategory
                //var specialDomesticPackages = await GetSpecialDomesticPackages();
                foreach (var subcategory in Subcategories)
                {
                    var list = new List<decimal>();
                    var listOfWeights = Subcategories.Where(s => s.SubCategoryName == subcategory.SubCategoryName).
                        Select(s => s.Weight.ToString()).ToList();

                    //add to dictionary
                    if (!finalDictionary.ContainsKey(subcategory.SubCategoryName))
                    {
                        finalDictionary.Add(subcategory.SubCategoryName, listOfWeights);
                    }
                }
                return finalDictionary;
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get the categorization of special packages.");
            }
        }

        public async Task<List<PreShipmentMobileDTO>> GetDisputePreShipment()
        {
            try
            {
                var user = await _userService.GetCurrentUserId();
                var shipments = _uow.PreShipmentMobile.FindAsync(s => s.UserId == user && s.shipmentstatus == MobilePickUpRequestStatus.Dispute.ToString(), "PreShipmentItems").Result;
                var shipment = shipments.OrderByDescending(s => s.DateCreated);
                var newPreShipment = Mapper.Map<List<PreShipmentMobileDTO>>(shipment);
                foreach(var Shipment in newPreShipment)
                {
                    var country = await _uow.Country.GetCountryByStationId(Shipment.SenderStationId);
                    Shipment.CurrencyCode = country.CurrencyCode;
                    Shipment.CurrencySymbol = country.CurrencySymbol;
                }
                return newPreShipment;
            }
            catch(Exception)
            {
                throw new GenericException("Please an error occurred while trying to get shipments in dispute.");
            }
        }
        public async Task<SummaryTransactionsDTO> GetPartnerWalletTransactions()
        {
            try
            {
                var user = await _userService.GetCurrentUserId();
                var transactions = _uow.PartnerTransactions.FindAsync(s => s.UserId == user).Result;
                var partneruser = await _userService.GetUserById(user);
                var wallet = _uow.Wallet.GetAsync(s => s.CustomerCode == partneruser.UserChannelCode).Result;
                transactions = transactions.OrderByDescending(s => s.DateCreated);
                var TotalTransactions = Mapper.Map<List<PartnerTransactionsDTO>>(transactions);
                var summary = new SummaryTransactionsDTO
                {
                    WalletBalance = wallet.Balance,
                    Transactions = TotalTransactions
                };
                return summary;
            }
            catch(Exception)
            {
                throw new GenericException("Please an error occurred while trying to get partne's transactions.");
            }
        }

        public async Task<object> ResolveDisputeForMobile(PreShipmentMobileDTO preShipment)
        {
            try
            {
                var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preShipment.CustomerCode);
                var preshipmentmobilegrandtotal = _uow.PreShipmentMobile.GetAsync(s => s.PreShipmentMobileId == preShipment.PreShipmentMobileId).Result;
                if (preshipmentmobilegrandtotal.shipmentstatus == MobilePickUpRequestStatus.PickedUp.ToString() || preshipmentmobilegrandtotal.shipmentstatus == MobilePickUpRequestStatus.Delivered.ToString())
                {
                    throw new GenericException("This Shipment cannot be placed in Dispute, because it has been" + " " + preshipmentmobilegrandtotal.shipmentstatus);
                }
                foreach (var id in preShipment.DeletedItems.ToList())
                {
                    var preshipmentitemmobile = _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentItemMobileId == id && s.PreShipmentMobileId == preShipment.PreShipmentMobileId).Result;
                    preshipmentitemmobile.IsCancelled = true;
                    _uow.PreShipmentItemMobile.Remove(preshipmentitemmobile);
                }
                foreach (var item in preShipment.PreShipmentItems)
                {
                    var preshipmentitemmobile = _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentItemMobileId == item.PreShipmentItemMobileId && s.PreShipmentMobileId == preShipment.PreShipmentMobileId).Result;
                    preshipmentitemmobile.Quantity = item.Quantity;
                    preshipmentitemmobile.Value = item.Value;
                    preshipmentitemmobile.Weight = item.Weight;
                    preshipmentitemmobile.Description = item.Description;
                    preshipmentitemmobile.Height = item.Height;
                    preshipmentitemmobile.ImageUrl = item.ImageUrl;
                    preshipmentitemmobile.ItemName = item.ItemName;
                    preshipmentitemmobile.Length = item.Length;
                }

                var PreshipmentPriceDTO = await GetPrice(preShipment);
                preshipmentmobilegrandtotal.shipmentstatus = MobilePickUpRequestStatus.Resolved.ToString();
                var difference = ((decimal)preshipmentmobilegrandtotal.CalculatedTotal - PreshipmentPriceDTO.GrandTotal);
                if (difference < 0.00M)
                {
                    var newdiff = Math.Abs((decimal)difference);
                    if (newdiff > updatedwallet.Balance)
                    {
                        throw new GenericException("Insufficient Wallet Balance");
                    }
                }
                updatedwallet.Balance = updatedwallet.Balance + (decimal)difference;
                var pickuprequests = _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preshipmentmobilegrandtotal.Waybill).Result;
                pickuprequests.Status = MobilePickUpRequestStatus.Resolved.ToString();
                await _uow.CompleteAsync();
                return new { IsResolved = true };
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public async Task<object> CancelShipment(string Waybill)
        {
            var CountryId = await GetCountryId();
            var userActiveCountryId = CountryId;
            try
            {
                userActiveCountryId = await _userService.GetUserActiveCountryId();
            }
            catch (Exception ex) { }

            try
            {
                var preshipmentmobile = _uow.PreShipmentMobile.GetAsync(s => s.Waybill == Waybill, "PreShipmentItems").Result;
                if (preshipmentmobile == null)
                {
                    throw new GenericException("Shipment does not exist");
                }
                var pickuprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId);
                var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                foreach (var id in preshipmentmobile.PreShipmentItems.ToList())
                {
                    var preshipmentitmmobile = _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentMobileId == preshipmentmobile.PreShipmentMobileId && s.PreShipmentItemMobileId == id.PreShipmentItemMobileId).Result;
                    preshipmentitmmobile.IsCancelled = true;
                    _uow.PreShipmentItemMobile.Remove(preshipmentitmmobile);
                }
                var pickuprequests = _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preshipmentmobile.Waybill && s.Status != MobilePickUpRequestStatus.Rejected.ToString()).Result;
                if (pickuprequests != null)
                {
                    var user = await _userService.GetUserById(pickuprequests.UserId);
                    pickuprequests.Status = MobilePickUpRequestStatus.Cancelled.ToString();
                    updatedwallet.Balance = ((updatedwallet.Balance + (decimal)preshipmentmobile.CalculatedTotal) - Convert.ToDecimal(pickuprice));
                    var Partnersprice = (0.4M * Convert.ToDecimal(pickuprice));
                    var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == user.UserChannelCode);
                    wallet.Balance = wallet.Balance + Partnersprice;
                    var partnertransactions = new PartnerTransactionsDTO
                    {
                        Destination = preshipmentmobile.ReceiverAddress,
                        Departure = preshipmentmobile.SenderAddress,
                        AmountReceived = Partnersprice,
                        Waybill = preshipmentmobile.Waybill
                    };
                    var id = await _partnertransactionservice.AddPartnerPaymentLog(partnertransactions);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Cancelled.ToString();
                    await _uow.CompleteAsync();
                }
                else
                {
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Cancelled.ToString();
                    updatedwallet.Balance = ((updatedwallet.Balance + (decimal)preshipmentmobile.CalculatedTotal));
                    await _uow.CompleteAsync();
                }
                return new { IsCancelled = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> AddRatings(MobileRatingDTO rating)
        {
            try
            {
                var user = await _userService.GetCurrentUserId();
                var partneruser = await _userService.GetUserById(user);
                var existingrating = await _uow.MobileRating.GetAsync(s => s.Waybill == rating.Waybill);
                if (existingrating != null)
                {
                    if (rating.UserChannelType == UserChannelType.IndividualCustomer.ToString())
                    {
                        if (existingrating.IsRatedByCustomer == true)
                        {
                            throw new GenericException("Customer has rated this Partner already!");
                        }
                        existingrating.CustomerCode = partneruser.UserChannelCode;
                        existingrating.PartnerRating = rating.Rating;
                        existingrating.CommentByCustomer = rating.Comment;
                        existingrating.IsRatedByCustomer = true;
                        existingrating.DateCustomerRated = DateTime.Now;
                    }
                    if (rating.UserChannelType == UserChannelType.Partner.ToString())
                    {
                        if (existingrating.IsRatedByPartner == true)
                        {
                            throw new GenericException("Partner has rated this Customer already!");
                        }
                        existingrating.PartnerCode = partneruser.UserChannelCode;
                        existingrating.CustomerRating = rating.Rating;
                        existingrating.CommentByPartner = rating.Comment;
                        existingrating.IsRatedByPartner = true;
                        existingrating.DatePartnerRated = DateTime.Now;
                    }
                    await _uow.CompleteAsync();
                }
                else
                {
                    if (rating.UserChannelType == UserChannelType.IndividualCustomer.ToString())
                    {
                        rating.CustomerCode = partneruser.UserChannelCode;
                        rating.PartnerRating = rating.Rating;
                        rating.CommentByCustomer = rating.Comment;
                        rating.IsRatedByCustomer = true;
                        rating.DateCustomerRated = DateTime.Now;
                    }
                    if (rating.UserChannelType == UserChannelType.Partner.ToString())
                    {
                        rating.PartnerCode = partneruser.UserChannelCode;
                        rating.CustomerRating = rating.Rating;
                        rating.CommentByPartner = rating.Comment;
                        rating.IsRatedByPartner = true;
                        rating.DatePartnerRated = DateTime.Now;
                    }
                    var ratings = Mapper.Map<MobileRating>(rating);
                    _uow.MobileRating.Add(ratings);
                    await _uow.CompleteAsync();
                }
                return new { IsRated = true };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Partnerdto> GetMonthlyPartnerTransactions()
        {
            try
            {
                var mobilerequests = await _mobilepickuprequestservice.GetMonthlyTransactions();
                return mobilerequests;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateCustomer(string CustomerCode)
        {
            try
            {
                var user = await _userService.GetUserByChannelCode(CustomerCode);
                if(user.UserChannelType == UserChannelType.Ecommerce)
                {
                    return true;
                }
                var customer = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == CustomerCode);
                if (customer == null)
                {
                    var customerDTO = new IndividualCustomerDTO
                    {
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.Password,
                        CustomerCode = user.UserChannelCode,
                        PictureUrl = user.PictureUrl,
                        userId = user.Id,
                        IsRegisteredFromMobile = true
                        //added this to pass channelcode };
                    };
                    var individualCustomer = Mapper.Map<IndividualCustomer>(customerDTO);
                    _uow.IndividualCustomer.Add(individualCustomer);
                    await _uow.CompleteAsync();
                }
                return true; ;
            }
            catch
            {
                throw new GenericException("An error occurred while trying to create a customer record(L).");
            }
        }

        public async Task<string> CreatePartner(string CustomerCode)
        {
            try
            {
                var partnerDTO = new PartnerDTO();
                var user = await _userService.GetUserByChannelCode(CustomerCode);
                var partner = await _uow.Partner.GetAsync(s => s.PartnerCode == CustomerCode);
                if (partner == null)
                {
                    if (user.SystemUserRole == "Dispatch Rider")
                    {
                        partnerDTO.PartnerType = PartnerType.InternalDeliveryPartner;
                        partnerDTO.PartnerName = user.FirstName + "" + user.LastName;
                        partnerDTO.PartnerCode = user.UserChannelCode;
                        partnerDTO.FirstName = user.FirstName;
                        partnerDTO.LastName = user.LastName;
                        partnerDTO.Email = user.Email;
                        partnerDTO.PhoneNumber = user.PhoneNumber;
                        partnerDTO.UserId = user.Id;
                        partnerDTO.IsActivated = true;
                        

                    }
                    else
                    {
                        partnerDTO.PartnerType = PartnerType.DeliveryPartner;
                        partnerDTO.PartnerName = user.FirstName + "" + user.LastName;
                        partnerDTO.PartnerCode = user.UserChannelCode;
                        partnerDTO.FirstName = user.FirstName;
                        partnerDTO.LastName = user.LastName;
                        partnerDTO.Email = user.Email;
                        partnerDTO.PhoneNumber = user.PhoneNumber;
                        partnerDTO.UserId = user.Id;
                        partnerDTO.IsActivated = false;
                    }
                     var FinalPartner = Mapper.Map<Partner>(partnerDTO);
                    _uow.Partner.Add(FinalPartner);
                }
                else
                {
                    if (user.SystemUserRole == "Dispatch Rider")
                    {
                        partner.PartnerType = PartnerType.InternalDeliveryPartner;
                        partner.IsActivated = true;
                    }
                    else
                    {
                        partner.PartnerType = PartnerType.DeliveryPartner;
                    }

                }
                await _uow.CompleteAsync();
                var latestpartner = await _uow.Partner.GetAsync(s => s.PartnerCode == CustomerCode);
                return latestpartner.PartnerType.ToString();
            }
            catch
            {
                throw new GenericException("An error occurred while trying to create a Partner record(L)."); ;
            }
        }

        public async Task<bool> UpdateDeliveryNumber(MobileShipmentNumberDTO detail)
        {
            try
            {
                var userId = await _userService.GetCurrentUserId();
                var number = await _uow.DeliveryNumber.GetAsync(s => s.Number == detail.DeliveryNumber);
                if (number == null)
                {
                    throw new GenericException("Delivery Number does not exist");
                }
                else
                {
                    if (number.IsUsed == true)
                    {
                        throw new GenericException("Delivery Number has been used ");
                    }
                    else
                    {
                        number.IsUsed = true;
                        number.UserId = userId;
                        var shipment = await _uow.Shipment.GetAsync(s => s.Waybill == detail.WayBill);
                        if (shipment != null)
                        {
                            shipment.DeliveryNumber = detail.DeliveryNumber;
                            await _uow.CompleteAsync();
                        }
                        else
                        {
                            throw new GenericException("Waybill does not exist in Shipments");
                        }
                    }
                }
                return true; 
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> deleterecord(string detail)
        {
            try
            {
                var user = await _userService.GetUserByEmail(detail);
                if (user != null)
                {
                    var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == user.UserChannelCode);
                    if (wallet != null)
                    {
                        var transactions = await _uow.WalletTransaction.FindAsync(s => s.WalletId == wallet.WalletId);
                        if (transactions.Count() > 0)
                        {
                            foreach (var transaction in transactions.ToList())
                            {
                                _uow.WalletTransaction.Remove(transaction);
                            }
                        }
                        _uow.Wallet.Remove(wallet);
                    }
                    var userDTO = await _uow.User.Remove(user.Id);
                }
                var Customer = await _uow.IndividualCustomer.GetAsync(s => s.Email == detail);
                if (Customer != null)
                {
                    _uow.IndividualCustomer.Remove(Customer);
                }
                var partner = await _uow.Partner.GetAsync(s => s.Email == detail);
                if (partner != null)
                {
                    var vehicles = await _uow.VehicleType.FindAsync(s => s.Partnercode == partner.PartnerCode);
                    if (vehicles.Count() > 0)
                    {
                        foreach (var vehicle in vehicles)
                        {
                            _uow.VehicleType.Remove(vehicle);
                        }
                    }
                    _uow.Partner.Remove(partner);
                }
                await _uow.CompleteAsync();
                return true; ;
            }
            catch
            {
                throw new GenericException("An error occurred while attempting to delete record.");
            }
        }

        public async Task<bool> VerifyPartnerDetails(PartnerDTO partner)
        {
            try
            {
                //to get images information
                var images = new ImageDTO();
                var Partner = await _uow.Partner.GetAsync(s => s.Email == partner.Email);
                if (Partner != null)
                {
                    Partner.Address = partner.Address;
                    Partner.Email = partner.Email;
                    Partner.FirstName = partner.FirstName;
                    Partner.IsActivated = true;
                    Partner.LastName = partner.LastName;
                    Partner.OptionalPhoneNumber = partner.OptionalPhoneNumber;
                    Partner.PartnerName = partner.PartnerName;
                    Partner.PhoneNumber = partner.PhoneNumber;
                    Partner.PictureUrl = partner.PictureUrl;
                    Partner.AccountName = partner.AccountName;
                    Partner.AccountNumber = partner.AccountNumber;
                    Partner.BankName = partner.BankName;
                    Partner.VehicleLicenseExpiryDate = partner.VehicleLicenseExpiryDate;
                    images.PartnerFullName = partner.FirstName + partner.LastName;
                    if (partner.VehicleLicenseImageDetails != null && !partner.VehicleLicenseImageDetails.Contains("agilityblob"))
                    {
                        images.FileType = ImageFileType.VehicleLicense;
                        images.ImageString = partner.VehicleLicenseImageDetails;
                        partner.VehicleLicenseImageDetails = await LoadImage(images);
                    }
                    Partner.VehicleLicenseImageDetails = partner.VehicleLicenseImageDetails;
                    Partner.VehicleLicenseNumber = partner.VehicleLicenseNumber;
                    if (partner.VehicleTypeDetails.Count() == 0)
                    {
                        throw new GenericException("Partner does not any Vehicle attached. Kindly review!!");
                    }
                    foreach (var vehicle in partner.VehicleTypeDetails)
                    {
                        if (vehicle.VehicleInsurancePolicyDetails != null && !vehicle.VehicleInsurancePolicyDetails.Contains("agilityblob"))
                        {
                            images.FileType = ImageFileType.VehiceInsurancePolicy;
                            images.ImageString = vehicle.VehicleInsurancePolicyDetails;
                            vehicle.VehicleInsurancePolicyDetails = await LoadImage(images);
                        }

                        if (vehicle.VehicleRoadWorthinessDetails != null && !vehicle.VehicleRoadWorthinessDetails.Contains("agilityblob"))
                        {
                            images.FileType = ImageFileType.VehiceRoadWorthiness;
                            images.ImageString = vehicle.VehicleRoadWorthinessDetails;
                            vehicle.VehicleRoadWorthinessDetails = await LoadImage(images);
                        }

                        if (vehicle.VehicleParticularsDetails != null && !vehicle.VehicleParticularsDetails.Contains("agilityblob"))
                        {
                            images.FileType = ImageFileType.VehicleParticulars;
                            images.ImageString = vehicle.VehicleParticularsDetails;
                            vehicle.VehicleParticularsDetails = await LoadImage(images);
                        }
                        var VehicleDetails = await _uow.VehicleType.GetAsync(s => s.VehicleTypeId == vehicle.VehicleTypeId && s.Partnercode == partner.PartnerCode);
                        if (VehicleDetails != null)
                        {
                            VehicleDetails.VehicleInsurancePolicyDetails = vehicle.VehicleInsurancePolicyDetails;
                            VehicleDetails.VehicleRoadWorthinessDetails = vehicle.VehicleRoadWorthinessDetails;
                            VehicleDetails.VehicleParticularsDetails = vehicle.VehicleParticularsDetails;
                            VehicleDetails.VehiclePlateNumber = vehicle.VehiclePlateNumber;
                            VehicleDetails.Vehicletype = vehicle.Vehicletype;
                            VehicleDetails.IsVerified = vehicle.IsVerified;
                        }
                        else
                        {
                            var Vehicle = Mapper.Map<VehicleType>(vehicle);
                            _uow.VehicleType.Add(Vehicle);
                        }
                    }
                    await _uow.CompleteAsync();
                }
                else
                {
                    throw new GenericException("Partner Information does not exist!");
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PartnerDTO> GetPartnerDetails(string Email)
        {
            var partnerdto = new PartnerDTO();
            try
            {
                var Partner = await _uow.Partner.GetAsync(s => s.Email == Email);
                if (Partner != null)
                {
                    partnerdto = Mapper.Map<PartnerDTO>(Partner);
                        var VehicleDetails = await _uow.VehicleType.FindAsync(s =>s.Partnercode == partnerdto.PartnerCode);
                        if (VehicleDetails != null)
                        {
                          var vehicles = Mapper.Map<List<VehicleTypeDTO>>(VehicleDetails);
                          partnerdto.VehicleTypeDetails = vehicles;
                        }
                }
                else
                {
                    throw new GenericException("Partner Information does not exist!");
                }
                return partnerdto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateReceiverDetails(PreShipmentMobileDTO receiver)
        {
            try
            {
                var shipment = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == receiver.Waybill);
                if (shipment != null)
                {
                    shipment.ActualReceiverFirstName = receiver.ActualReceiverFirstName;
                    shipment.ActualReceiverLastName = receiver.ActualReceiverLastName;
                    shipment.ActualReceiverPhoneNumber = receiver.ActualReceiverPhoneNumber;
                }
                else
                {
                    throw new GenericException("Shipment Information does not exist!");
                }
                await _uow.CompleteAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetCountryId()
        {
            int userActiveCountryId = 1;
            return userActiveCountryId;
        }

        public async Task<decimal> GetPickUpPrice(string vehicleType,int CountryId)
        {
            try
            {
                var PickUpPrice = 0.0M;
                if (vehicleType != null)
                {
                   
                    if (vehicleType.ToLower() == Vehicletype.Car.ToString().ToLower())
                    {
                        var carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.CarPickupPrice, CountryId);
                        PickUpPrice = (Convert.ToDecimal(carPickUprice.Value));
                    }
                    if (vehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower())
                    {
                        var bikePickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikePickUpPrice, CountryId);
                        PickUpPrice = (Convert.ToDecimal(bikePickUprice.Value));
                    }
                    if (vehicleType.ToLower() == Vehicletype.Van.ToString().ToLower())
                    {
                        var vanPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.VanPickupPrice, CountryId);
                        PickUpPrice = (Convert.ToDecimal(vanPickUprice.Value));
                    }
                    
                }
                return PickUpPrice;
            }
            catch
            {
                throw new GenericException("Please an error occurred while getting PickupPrice!!");
            }
        }

        //method for converting a base64 string to an image and saving to Azure
        public async Task<string> LoadImage(ImageDTO images)
        {
            try
            {
                //To get only the base64 string
                var baseString = images.ImageString.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(baseString);
                string filename = images.PartnerFullName + "_"  + images.FileType.ToString() + ".png";
               //Save to AzureBlobStorage
               var blobname = await AzureBlobServiceUtil.UploadAsync(bytes, filename);

                return await Task.FromResult(blobname);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<Uri>> DisplayImages()
        {
            var result = await AzureBlobServiceUtil.DisplayAll();

            return result;
        }

        public async Task<PreShipmentSummaryDTO> GetShipmentDetailsFromDeliveryNumber(string DeliveryNumber)
        {
            try
            {
                var ShipmentSummaryDetails = new PreShipmentSummaryDTO();
                var result = await _uow.Shipment.GetAsync(s => s.DeliveryNumber == DeliveryNumber);
                var partner = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == result.Waybill && s.Status != MobilePickUpRequestStatus.Rejected.ToString());
                if (partner != null)
                {
                    var Partnerdetails = await _uow.Partner.GetAsync(s => s.UserId == partner.UserId);
                    var userdetails = await _userService.GetUserById(partner.UserId);
                    if (Partnerdetails ==null)
                    {
                        throw new GenericException("Partner Details does not exist!!");
                    }
                    else
                    {
                        var partnerinfo = Mapper.Map<PartnerDTO>(Partnerdetails);
                        ShipmentSummaryDetails.partnerdetails = partnerinfo;
                        ShipmentSummaryDetails.partnerdetails.PictureUrl = userdetails.PictureUrl;
                    }
                }
                else
                {
                    throw new GenericException("No Partner has accepted shipment with this Delivery Number");
                }
                if (result != null)
                {
                    var details = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == result.Waybill, "PreShipmentItems");
                    if(details ==null)
                    {
                        throw new GenericException("Shipment with this delivery number cannot be found in PreshipmentMobile");
                    }
                    var PreShipmentdto = Mapper.Map<PreShipmentMobileDTO>(details);
                    ShipmentSummaryDetails.shipmentdetails = PreShipmentdto;
                    return ShipmentSummaryDetails;
                }
                else
                {
                    throw new GenericException("Shipment with this delivery number cannot be found");
                }
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public async Task<bool> ApproveShipment(string waybillNumber)
        {
            try
            {
              
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == waybillNumber, "SenderLocation,ReceiverLocation");
                var Pickupprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId);
                var Partnerid = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == waybillNumber && s.Status != MobilePickUpRequestStatus.Rejected.ToString());
                await ScanMobileShipment(new ScanDTO
                {
                   WaybillNumber = waybillNumber,
                   ShipmentScanStatus = ShipmentScanStatus.MSVC
                });
                if (preshipmentmobile.ZoneMapping != 1)
                {
                    var Location = new LocationDTO
                    {
                        DestinationLatitude = (double)preshipmentmobile.ReceiverLocation.Latitude,
                        DestinationLongitude = (double)preshipmentmobile.ReceiverLocation.Longitude,
                        OriginLatitude = (double)preshipmentmobile.SenderLocation.Latitude,
                        OriginLongitude = (double)preshipmentmobile.SenderLocation.Longitude
                    };

                    RootObject details = await _partnertransactionservice.GetGeoDetails(Location);
                    var Partner = new PartnerPayDTO
                    {
                        Distance = details.rows[0].elements[0].distance.value.ToString(),
                        Time = details.rows[0].elements[0].duration.value.ToString(),
                        ShipmentPrice = (decimal)preshipmentmobile.CalculatedTotal,
                        PickUprice = Pickupprice
                    };
                    decimal price = await _partnertransactionservice.GetPriceForPartner(Partner);
                    var partneruser = await _userService.GetUserById(Partnerid.UserId);
                    var wallet = _uow.Wallet.GetAsync(s => s.CustomerCode == partneruser.UserChannelCode).Result;
                    wallet.Balance = wallet.Balance + price;
                    var partnertransactions = new PartnerTransactionsDTO
                    {
                        Destination = preshipmentmobile.ReceiverAddress,
                        Departure = preshipmentmobile.SenderAddress,
                        AmountReceived = price,
                        Waybill = preshipmentmobile.Waybill
                    };
                    var id = await _partnertransactionservice.AddPartnerPaymentLog(partnertransactions);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.OnwardProcessing.ToString();
                    await _uow.CompleteAsync();
                }
                else
                {
                    throw new GenericException("This shipment is not an interstate delivery,take to the assigned receiver's location");
                }
               return true;
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateCompany(string CustomerCode)
        {
            try
            {
                var user = await _userService.GetUserByChannelCode(CustomerCode);
                var company = await _uow.Company.GetAsync(s => s.CustomerCode == CustomerCode);
                if (company == null)
                {
                    var companydto = new CompanyDTO
                    {
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Name = user.Organisation,
                        CompanyType = CompanyType.Ecommerce,
                        Password = user.Password,
                        CustomerCode = user.UserChannelCode,
                        IsRegisteredFromMobile = true,
                    };
                    var Company = Mapper.Map<Company>(companydto);
                    _uow.Company.Add(Company);
                    await _uow.CompleteAsync();
                }
                return true; ;
            }
            catch
            {
                throw new GenericException("An error occurred while trying to create company(L).M");
            }
        }

        public async Task<MobilePriceDTO> GetHaulagePrice(HaulagePriceDTO haulagePricingDto)
        {
            decimal price = 0;

            var departureStationId = haulagePricingDto.DepartureStationId;
            var destinationStationId = haulagePricingDto.DestinationStationId;
            var haulageid = haulagePricingDto.Haulageid;
            var country = await _uow.Country.GetCountryByStationId(departureStationId);
            var IsWithinProcessingTime = await WithinProcessingTime(country.CountryId);

            //check haulage exists
            var haulage = await _haulageService.GetHaulageById(haulageid);
            if (haulage == null)
            {
                throw new GenericException("The Tonne specified has not been mapped");
            }

            //get the distance based on the stations
            var haulageDistanceMapping = await _haulageDistanceMappingService.GetHaulageDistanceMappingForMobile(departureStationId, destinationStationId);
            var distance = haulageDistanceMapping.Distance;

            //set the default distance to 1
            if (distance == 0)
            {
                distance = 1;
            }
            //Get Haulage Maximum Fixed Distance
            var userActiveCountryId = country.CountryId;
            var maximumFixedDistanceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.HaulageMaximumFixedDistance, userActiveCountryId);
            int maximumFixedDistance = int.Parse(maximumFixedDistanceObj.Value);

            //calculate price for the haulage
            if (distance <= maximumFixedDistance)
            {
                price = haulage.FixedRate;
            }
            else
            {
                //1. get the fixed rate and substract the maximum fixed distance from distance
                decimal fixedRate = haulage.FixedRate;
                distance = distance - maximumFixedDistance;

                //2. multiply the remaining distance with the additional pate
                price = fixedRate + distance * haulage.AdditionalRate;
            }

            return new MobilePriceDTO{ GrandTotal = price,
                CurrencySymbol = country.CurrencySymbol,
                CurrencyCode = country.CurrencyCode,
                IsWithinProcessingTime = IsWithinProcessingTime
            };
        }


        public async Task<bool> EditProfile (UserDTO user)
        {
            try
            {
                var User = await _userService.GetUserByEmail(user.Email);
                User.FirstName = user.FirstName;
                User.LastName = user.LastName;
                User.PhoneNumber = user.PhoneNumber;
                User.Email = user.Email;
                User.PictureUrl = user.PictureUrl;
                await _userService.UpdateUser(User.Id, User);
                if (user.UserChannelType.ToString() == UserChannelType.Partner.ToString())
                {
                    var partner = await _uow.Partner.GetAsync(s => s.PartnerCode == user.UserChannelCode);
                    partner.FirstName = user.FirstName;
                    partner.LastName = user.LastName;
                    partner.Email = user.Email;
                    partner.PictureUrl = user.PictureUrl;
                    
                }
                if (user.UserChannelType.ToString() == UserChannelType.IndividualCustomer.ToString())
                {
                    var customer = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == user.UserChannelCode);
                    customer.FirstName = user.FirstName;
                    customer.LastName = user.LastName;
                    customer.Email = user.Email;
                    customer.PictureUrl = user.PictureUrl;
                }
                if (user.UserChannelType.ToString() == UserChannelType.Ecommerce.ToString())
                {
                    var company = await _uow.Company.GetAsync(s => s.CustomerCode == user.UserChannelCode);
                    company.FirstName = user.FirstName;
                    company.LastName = user.LastName;
                    company.Email = user.Email;
                }
                await _uow.CompleteAsync();
                return true;
            }
            catch
            {
                throw new GenericException("Please an error occurred while updating profile.");
            }
        }

        public async Task<bool> UpdateVehicleProfile(UserDTO user)
        {
            try
            {
                var partner = await _uow.Partner.GetAsync(s => s.PartnerCode == user.UserChannelCode);
                if (partner == null)
                {
                    var partnerDTO = new PartnerDTO
                    {
                        PartnerType = PartnerType.InternalDeliveryPartner,
                        PartnerName = user.FirstName + "" + user.LastName,
                        PartnerCode = user.UserChannelCode,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserId = user.Id,
                        IsActivated = false,
                    };
                    var FinalPartner = Mapper.Map<Partner>(partnerDTO);
                   _uow.Partner.Add(FinalPartner);
                }
                foreach (var vehicle in user.VehicleType)
                {
                    var Vehicle = new VehicleTypeDTO
                    {
                        Vehicletype = vehicle.ToUpper(),
                        Partnercode = user.UserChannelCode
                    };
                    var vehicletype = Mapper.Map<VehicleType>(Vehicle);
                    _uow.VehicleType.Add(vehicletype);
                }
            }
            catch
            {
                throw new GenericException("Please an error occurred while updating profile.");
            }
            await _uow.CompleteAsync();
            return true;
        }

        private async Task<bool> WithinProcessingTime (int CountryId)
        {
            bool IsWithinTime = false;
            var Startime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PickUpStartTime, CountryId); 
            var Endtime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PickUpEndTime, CountryId); ;
            TimeSpan start = new TimeSpan(Convert.ToInt32(Startime.Value), 0, 0);
            TimeSpan end = new TimeSpan(Convert.ToInt32(Endtime.Value), 0, 0);
            //TimeSpan now = new TimeSpan(19, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now > start && now < end)
            {
               IsWithinTime = true;
            }
            return IsWithinTime;

        }
        private async Task<int> CalculateTimeBasedonLocation(PreShipmentMobileDTO item)
        {
            var Location = new LocationDTO
            {
                DestinationLatitude = (double)item.ReceiverLocation.Latitude,
                DestinationLongitude = (double)item.ReceiverLocation.Longitude,
                OriginLatitude = (double)item.SenderLocation.Latitude,
                OriginLongitude = (double)item.SenderLocation.Longitude
            };
            RootObject details = await _partnertransactionservice.GetGeoDetails(Location);
            var time = (details.rows[0].elements[0].duration.value / 60);
            return time;

        }
        public async Task DetermineShipmentstatus(string waybill, int countryid,DateTime ExpectedTimeOfDelivery)
        {
            var status = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == waybill && s.Status != MobilePickUpRequestStatus.Rejected.ToString());
            if(status!=null)
            {
                if(status.Status == MobilePickUpRequestStatus.Confirmed.ToString())
                {
                    var time = string.Format("{0:f}", ExpectedTimeOfDelivery);
                    var Email = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EmailForDeliveryTime, countryid);
                    var message = new MobileMessageDTO
                    {
                        SenderEmail = Email.Value,
                        WaybillNumber = waybill,
                        ExpectedTimeofDelivery = time
                    };
                    await _messageSenderService.SendGenericEmailMessage(MessageType.UDM, message);
                }
            }

        }
        private async Task CheckDeliveryTimeAndSendMail(PreShipmentMobileDTO item)
        {
            var time = await CalculateTimeBasedonLocation(item);
            var country =await _uow.Country.GetCountryByStationId(item.SenderStationId);
            var additionaltime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.AddedMinutesForDeliveryTime, country.CountryId);
            var addedtime = int.Parse(additionaltime.Value);
            var totaltime = time + addedtime;
            var NewTime = DateTime.Now.AddMinutes(totaltime);
            BackgroundJob.Schedule(() => DetermineShipmentstatus(item.Waybill, country.CountryId, NewTime), TimeSpan.FromMinutes(totaltime));

        }



    }


}