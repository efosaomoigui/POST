using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
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
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO.Customers;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.User;
using VehicleType = GIGLS.Core.Domain.VehicleType;
using Hangfire;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.DTO.Utility;

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
        private readonly IPartnerService _partnerService;
        private readonly ICustomerService _customerService;
        private readonly IGiglgoStationService _giglgoStationService;


        public PreShipmentMobileService(IUnitOfWork uow, IShipmentService shipmentService, IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, IUserServiceCentreMappingService userServiceCentre, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPricingService pricingService, IWalletService walletService, IWalletTransactionService walletTransactionService,
            IUserService userService, ISpecialDomesticPackageService specialdomesticpackageservice, IMobileShipmentTrackingService mobiletrackingservice,
            IMobilePickUpRequestsService mobilepickuprequestservice, IDomesticRouteZoneMapService domesticroutezonemapservice, ICategoryService categoryservice, ISubCategoryService subcategoryservice,
            IPartnerTransactionsService partnertransactionservice, IGlobalPropertyService globalPropertyService, IMobileRatingService mobileratingService, IMessageSenderService messageSenderService,
            IHaulageService haulageService, IHaulageDistanceMappingService haulageDistanceMappingService, IPartnerService partnerService, ICustomerService customerService, IGiglgoStationService giglgoStationService)
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
            _partnerService = partnerService;
            _customerService = customerService;
            _giglgoStationService = giglgoStationService;



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
                else if (newPreShipment.IsEligible == false)
                {
                    var carPickUprice = new GlobalPropertyDTO();
                    if (newPreShipment.IsCodNeeded == false)
                    {
                        carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceNoCodAmount, newPreShipment.CountryId);
                    }
                    else
                    {
                        carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceCodAmount, newPreShipment.CountryId);
                    }
                    var remainingamount = (Convert.ToDecimal(carPickUprice.Value) - newPreShipment.CurrentWalletAmount);
                    message = $"You are not yet eligible to create shipment on the Platform. You need a minimum balance of {preShipment.CurrencySymbol}{carPickUprice.Value}, please fund your wallet with additional {preShipment.CurrencySymbol}{remainingamount} to complete the process. Thank you";
                    newPreShipment.Waybill = "";

                    throw new GenericException(message);

                }
                return new { waybill = newPreShipment.Waybill, message = message, IsBalanceSufficient, Zone = zoneid.ZoneId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<PreShipmentMobileDTO> CreatePreShipment(PreShipmentMobileDTO preShipmentDTO)
        {
            try
            {
                if(preShipmentDTO.VehicleType == null || preShipmentDTO.VehicleType == "")
                {
                    throw new GenericException("Please select a vehicle type");
                }
                var PreshipmentPriceDTO = new MobilePriceDTO();
                // get the current user info
                var currentUserId = await _userService.GetCurrentUserId();
                preShipmentDTO.UserId = currentUserId;
                var user = await _userService.GetUserById(currentUserId);
                var Country = await _uow.Country.GetCountryByStationId(preShipmentDTO.SenderStationId);
                preShipmentDTO.CountryId = Country.CountryId;
                var customer = await _uow.Company.GetAsync(s => s.CustomerCode == user.UserChannelCode);
                if(customer != null)
                {
                    if (customer.IsEligible != true)
                    {
                        preShipmentDTO.IsEligible = false;
                        preShipmentDTO.IsCodNeeded = customer.isCodNeeded;
                        preShipmentDTO.CurrencySymbol = Country.CurrencySymbol;
                        preShipmentDTO.CurrentWalletAmount = Convert.ToDecimal(customer.WalletAmount);
                        return preShipmentDTO;
                    }
                }
               
                if (preShipmentDTO.VehicleType.ToLower() == Vehicletype.Truck.ToString().ToLower())
                {
                    PreshipmentPriceDTO = await GetHaulagePrice(new HaulagePriceDTO
                    {
                        Haulageid = (int)preShipmentDTO.Haulageid,
                        DepartureStationId = preShipmentDTO.SenderStationId,
                        DestinationStationId = preShipmentDTO.ReceiverStationId
                    });
                    preShipmentDTO.GrandTotal = (decimal)PreshipmentPriceDTO.GrandTotal;
                }
                else
                {
                    //var exists = await _uow.Company.ExistAsync(s => s.CustomerCode == user.UserChannelCode);
                    if (user.UserChannelType == UserChannelType.Ecommerce || customer != null)
                    {
                        preShipmentDTO.Shipmentype = ShipmentType.Ecommerce;
                    }
                    preShipmentDTO.IsFromShipment = true;
                    PreshipmentPriceDTO = await GetPrice(preShipmentDTO);
                }

                var wallet = await _walletService.GetWalletBalance();
                if (wallet.Balance >= Convert.ToDecimal(PreshipmentPriceDTO.GrandTotal))
                {
                    var price = (wallet.Balance - Convert.ToDecimal(PreshipmentPriceDTO.GrandTotal));

                    var gigGOServiceCenter = await _userService.GetGIGGOServiceCentre();

                    //generate waybill
                    var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber, gigGOServiceCenter.Code);
                    preShipmentDTO.Waybill = waybill;


                    if (preShipmentDTO.PreShipmentItems.Count == 0)
                    {
                        throw new GenericException("Shipment Items cannot be empty");
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
                    preShipmentDTO.DiscountValue = PreshipmentPriceDTO.Discount;
                    _uow.PreShipmentMobile.Add(newPreShipment);
                    await _uow.CompleteAsync();
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = newPreShipment.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MCRT
                    });
                    
                    var transaction = new WalletTransactionDTO
                    {
                        WalletId = wallet.WalletId,
                        CreditDebitType = CreditDebitType.Debit,
                        Amount = newPreShipment.GrandTotal,
                        ServiceCentreId = gigGOServiceCenter.ServiceCentreId,
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
                throw;
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

                var zoneid = await _domesticroutezonemapservice.GetZoneMobile(preShipment.SenderStationId, preShipment.ReceiverStationId);
                
                var Price = 0.0M;
                var amount = 0.0M;
                var IndividualPrice = 0.0M;
                decimal DeclaredValue = 0.0M;

                var Country = await _uow.Country.GetCountryByStationId(preShipment.SenderStationId);
                preShipment.CountryId = Country.CountryId;
                
                //undo comment when App is updated
                if (zoneid.ZoneId == 1 && preShipment.ReceiverLocation != null && preShipment.SenderLocation != null)
                {
                    var ShipmentCount = preShipment.PreShipmentItems.Count();

                    amount = await CalculateGeoDetailsBasedonLocation(preShipment);
                    IndividualPrice = (amount / ShipmentCount);
                }
                
                //Get the customer Type
                var userChannelCode = await _userService.GetUserChannelCode();
                var userChannel = await _uow.Company.GetAsync(x => x.CustomerCode == userChannelCode);

                if (userChannel != null)
                {
                    preShipment.Shipmentype = ShipmentType.Ecommerce;
                }

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
                        Weight = preShipmentItem.Weight,
                        SpecialPackageId = (int)preShipmentItem.SpecialPackageId,
                        ShipmentType = preShipmentItem.ShipmentType,
                        CountryId = preShipment.CountryId  //Nigeria
                    };
                    
                    if (preShipmentItem.ShipmentType == ShipmentType.Special)
                    {
                        if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                        {
                            PriceDTO.DeliveryOptionId = 4;
                        }

                        preShipmentItem.CalculatedPrice = await _pricingService.GetMobileSpecialPrice(PriceDTO);
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                    }
                    else if (preShipmentItem.ShipmentType == ShipmentType.Regular)
                    {
                        if(preShipmentItem.Weight == 0)
                        {
                            throw new GenericException("Item weight cannot be zero");
                        }
                        
                        if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                        {
                            preShipmentItem.CalculatedPrice = await _pricingService.GetMobileEcommercePrice(PriceDTO);
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                        }
                        else
                        {
                            preShipmentItem.CalculatedPrice = await _pricingService.GetMobileRegularPrice(PriceDTO);
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                        }
                        //preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + IndividualPrice;
                    }

                    var vatForPreshipment = (preShipmentItem.CalculatedPrice * 0.05M);

                    if (!string.IsNullOrEmpty(preShipmentItem.Value))
                    {
                        DeclaredValue += Convert.ToDecimal(preShipmentItem.Value);
                        var DeclaredValueForPreShipment = Convert.ToDecimal(preShipmentItem.Value);
                        if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                        {
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + (DeclaredValueForPreShipment * 0.01M);
                        }
                        else
                        {
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + (DeclaredValueForPreShipment * 0.01M) + vatForPreshipment;
                        }
                        preShipment.IsdeclaredVal = true;
                    }
                    else
                    {
                        if (preShipment.Shipmentype != ShipmentType.Ecommerce)
                        {
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + vatForPreshipment;
                        }
                    }
                    
                    preShipmentItem.CalculatedPrice = (decimal)Math.Round((double)preShipmentItem.CalculatedPrice);
                    Price += (decimal)preShipmentItem.CalculatedPrice;
                };
                
                var DiscountPercent = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DiscountPercentage, preShipment.CountryId);
                var Percentage = (Convert.ToDecimal(DiscountPercent.Value));
                var PercentageTobeUsed = ((100M - Percentage) / 100M);

                decimal EstimatedDeclaredPrice = Convert.ToDecimal(DeclaredValue);
                preShipment.DeliveryPrice = Price * PercentageTobeUsed;
                preShipment.InsuranceValue = (EstimatedDeclaredPrice * 0.01M);
                preShipment.CalculatedTotal = (double)(preShipment.DeliveryPrice);
                preShipment.CalculatedTotal = Math.Round((double)preShipment.CalculatedTotal);
                preShipment.Value = DeclaredValue;
                var discount = Math.Round(Price - (decimal)preShipment.CalculatedTotal);
                preShipment.DiscountValue = discount;

                var Pickuprice = await GetPickUpPrice(preShipment.VehicleType, preShipment.CountryId, preShipment.UserId);
                var PickupValue = Convert.ToDecimal(Pickuprice);

                var IsWithinProcessingTime = await WithinProcessingTime(preShipment.CountryId);

                var returnprice = new MobilePriceDTO()
                {
                    MainCharge = (decimal)preShipment.CalculatedTotal,
                    PickUpCharge = PickupValue,
                    InsuranceValue = preShipment.InsuranceValue,
                    GrandTotal = ((decimal)preShipment.CalculatedTotal + PickupValue),
                    PreshipmentMobile = preShipment,
                    CurrencySymbol = Country.CurrencySymbol,
                    CurrencyCode = Country.CurrencyCode,
                    IsWithinProcessingTime = IsWithinProcessingTime,
                    Discount = discount
                };
                return returnprice;
            }
            catch (Exception)
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

                if (filterOptionsDto.StartDate == null & filterOptionsDto.EndDate == null && filterOptionsDto.fromGigGoDashboard == false)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                else if (filterOptionsDto.StartDate == null && filterOptionsDto.EndDate == null && filterOptionsDto.fromGigGoDashboard == true)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                //Excluding It Test
                string[] testUserId = { "2932eb15-aa30-462c-89f0-7247670f504b", "ab3722d7-57f3-4e6e-a32d-1580315b7da6", "e67d50c2-953a-44b2-bbcd-c38fadef237f", "b476fea8-84e4-4c5b-ac51-2efd68526fdc"};

                var allShipmentsResult = allShipments.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate &&
                                            !testUserId.Contains(s.UserId) && s.SenderName != "it_test test");

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
                                                              CustomerCode = r.CustomerCode,
                                                              VehicleType = r.VehicleType
                                                          }).ToList();

                return await Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());

            }
            catch (Exception)
            {
                throw;
            }

        }

        //GIG Go Dashboard
        public async Task<GIGGoDashboardDTO> GetDashboardInfo(BaseFilterCriteria filterCriteria)
        {
            try
            {
                var shipment = await GetShipments(filterCriteria);

                //To get Partners details
                var partners = await _partnerService.GetPartners();
                var internals = partners.Where(s => s.PartnerType == PartnerType.InternalDeliveryPartner).Count();
                var externals = partners.Where(s => s.PartnerType == PartnerType.DeliveryPartner && s.IsActivated == true).Count();

                //To get shipment details
                var all = shipment.Count();
                var accepted = shipment.Where(s => s.shipmentstatus == "Assigned for Pickup").Count();
                var cancelled = shipment.Where(s => s.shipmentstatus == "Cancelled").Count();
                var pickedUp = shipment.Where(s => s.shipmentstatus == "PickedUp").Count();
                var delivered = shipment.Where(s => s.shipmentstatus == "Delivered").Count();
                var sum = shipment.Sum(s => s.GrandTotal);

                //To get partner earnings
                var partnerEarnings = await _uow.PartnerTransactions.GetPartnerTransactionByDate(filterCriteria);
                var sumPartnerEarnings = partnerEarnings.Sum(s => s.AmountReceived);


                var result = new GIGGoDashboardDTO
                {
                    ExternalPartners = externals,
                    InternalPartners = internals,

                    AcceptedShipments = accepted,
                    CancelledShipments = cancelled,
                    PickedupShipments = pickedUp,
                    DeliveredShipments = delivered,
                    ShipmentRequests = all,
                    TotalRevenue = sum,
                    PartnerEarnings = sumPartnerEarnings

                };

                return result;

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
                var Shipmentdto = new PreShipmentMobileDTO();
                var shipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == waybill, "PreShipmentItems,SenderLocation,ReceiverLocation,serviceCentreLocation");
                if (shipment != null)
                {
                    var currentUser = await _userService.GetCurrentUserId();
                    var user = await _uow.User.GetUserById(currentUser);
                    Shipmentdto = Mapper.Map<PreShipmentMobileDTO>(shipment);
                    if (user.UserChannelType.ToString() == UserChannelType.Partner.ToString() || user.SystemUserRole == "Dispatch Rider")
                    {
                        if (shipment.ServiceCentreAddress != null)
                        {
                            Shipmentdto.ReceiverLocation = new LocationDTO
                            {
                                Latitude = shipment.serviceCentreLocation.Latitude,
                                Longitude = shipment.serviceCentreLocation.Longitude
                            };
                            Shipmentdto.ReceiverAddress = shipment.ServiceCentreAddress;
                        }
                    } 

                    var country = await _uow.Country.GetCountryByStationId(shipment.SenderStationId);
                    if (country != null)
                    {
                        Shipmentdto.CurrencyCode = country.CurrencyCode;
                        Shipmentdto.CurrencySymbol = country.CurrencySymbol;
                    }
                }
                else
                {
                    var agilityshipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill, "ShipmentItems");
                    if (agilityshipment != null)
                    {
                        Shipmentdto = Mapper.Map<PreShipmentMobileDTO>(agilityshipment);

                        CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), agilityshipment.CustomerType);
                        var CustomerDetails = await _customerService.GetCustomer(agilityshipment.CustomerId, customerType);

                        Shipmentdto.SenderAddress = CustomerDetails.Address;
                        Shipmentdto.SenderName = CustomerDetails.Name;
                        Shipmentdto.SenderPhoneNumber = CustomerDetails.PhoneNumber;

                        Shipmentdto.PreShipmentItems = new List<PreShipmentItemMobileDTO>();

                        foreach (var shipments in agilityshipment.ShipmentItems)
                        {
                            var item = Mapper.Map<PreShipmentItemMobileDTO>(shipments);
                            item.ItemName = shipments.Description;
                            item.ImageUrl = "";
                            Shipmentdto.PreShipmentItems.Add(item);
                        }

                        var country = await _uow.Country.GetAsync(agilityshipment.DepartureCountryId);
                        if (country != null)
                        {
                            Shipmentdto.CurrencyCode = country.CurrencyCode;
                            Shipmentdto.CurrencySymbol = country.CurrencySymbol;
                        }
                    }
                    else
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} does not exist");
                    }
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
                var mobileShipment = await _uow.PreShipmentMobile.FindAsync(x => x.CustomerCode.Equals(user.UserChannelCode), "PreShipmentItems,SenderLocation,ReceiverLocation");

                List<PreShipmentMobileDTO> shipmentDto = (from r in mobileShipment
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
                                                              CustomerCode = r.CustomerCode,
                                                              VehicleType = r.VehicleType,
                                                              ReceiverLocation = new LocationDTO
                                                              {
                                                                  Longitude = r.ReceiverLocation.Longitude,
                                                                  Latitude = r.ReceiverLocation.Latitude
                                                              },
                                                              SenderLocation = new LocationDTO
                                                              {
                                                                  Longitude = r.SenderLocation.Longitude,
                                                                  Latitude = r.SenderLocation.Latitude
                                                              }
                                                          }).OrderByDescending(x => x.DateCreated).Take(20).ToList();
                
                var agilityShipment = await GetPreShipmentForEcommerce(user.UserChannelCode);

                //added agility shipment to Giglgo list of shipments.
                foreach (var shipment in shipmentDto)
                {

                    if (agilityShipment.Exists(s => s.Waybill == shipment.Waybill))
                    {
                        var s = agilityShipment.Where(x => x.Waybill == shipment.Waybill).FirstOrDefault();
                        agilityShipment.Remove(s);
                    }
                    else
                    {
                        var partnerId = await _uow.MobilePickUpRequests.GetAsync(r => r.Waybill == shipment.Waybill && (r.Status == MobilePickUpRequestStatus.Delivered.ToString()));
                        if (partnerId != null)
                        {
                            var partneruser = await _uow.User.GetUserById(partnerId.UserId);
                            if (partneruser != null)
                            {
                                shipment.PartnerFirstName = partneruser.FirstName;
                                shipment.PartnerLastName = partneruser.LastName;
                                shipment.PartnerImageUrl = partneruser.PictureUrl;
                            }
                        }

                        shipment.IsRated = false;

                        var rating = await _uow.MobileRating.GetAsync(j => j.Waybill == shipment.Waybill);
                        if (rating != null)
                        {
                            shipment.IsRated = rating.IsRatedByCustomer;
                        }

                        var country = await _uow.Country.GetCountryByStationId(shipment.SenderStationId);
                        if (country != null)
                        {
                            shipment.CurrencyCode = country.CurrencyCode;
                            shipment.CurrencySymbol = country.CurrencySymbol;
                        }
                    }                  
                }

                var newlist = shipmentDto.Union(agilityShipment);
                return await Task.FromResult(newlist.OrderByDescending(x => x.DateCreated).Take(20).ToList());
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
                //Exclude those package from showing on gig go
                int[] excludePackage = new int[] { 11, 12, 13, 14, 15, 16, 17, 32, 33, 34, 35, 36, 37, 38 };

                var result = _uow.SpecialDomesticPackage.GetAll().Where(x => !excludePackage.Contains(x.SpecialDomesticPackageId));

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
                                       Weight = s.SubCategory.Weight,
                                       WeightRange = s.SubCategory.WeightRange
                                   }
                               };
                return await Task.FromResult(packages);
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
                if (pickuprequest.UserId == null)
                {
                    pickuprequest.UserId = await _userService.GetCurrentUserId();
                }

                var newPreShipment = new PreShipmentMobileDTO();
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "PreShipmentItems,SenderLocation,ReceiverLocation,serviceCentreLocation");
                
                if (preshipmentmobile == null)
                {
                    throw new GenericException("Shipment does not exist"); 
                }
                else
                {
                    if (pickuprequest.Status == MobilePickUpRequestStatus.Rejected.ToString() || pickuprequest.Status == MobilePickUpRequestStatus.TimedOut.ToString())
                    {
                        var request = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == pickuprequest.Waybill && s.UserId == pickuprequest.UserId
                        && (s.Status == MobilePickUpRequestStatus.Rejected.ToString() || s.Status == MobilePickUpRequestStatus.TimedOut.ToString()));

                        if (request == null)
                        {
                            await _mobilepickuprequestservice.AddMobilePickUpRequests(pickuprequest);
                        }
                        else
                        {
                            throw new GenericException($"Shipment with waybill number: {pickuprequest.Waybill} already exists");
                        }
                    }
                    else if (preshipmentmobile.shipmentstatus == "Shipment created" || preshipmentmobile.shipmentstatus == MobilePickUpRequestStatus.Processing.ToString())
                    {
                        pickuprequest.Status = MobilePickUpRequestStatus.Accepted.ToString();
                        await _mobilepickuprequestservice.AddMobilePickUpRequests(pickuprequest);
                    }
                    else
                    {
                        throw new GenericException($"Shipment has already been accepted..");
                    }

                    if (pickuprequest.ServiceCentreId != null)
                    {
                        var DestinationServiceCentreId = await _uow.ServiceCentre.GetAsync(s => s.Code == pickuprequest.ServiceCentreId);
                        preshipmentmobile.ServiceCentreAddress = DestinationServiceCentreId.Address;
                        var Locationdto = new LocationDTO
                        {
                            Latitude = DestinationServiceCentreId.Latitude,
                            Longitude = DestinationServiceCentreId.Longitude
                        };
                        var LOcation = Mapper.Map<Location>(Locationdto);
                        preshipmentmobile.serviceCentreLocation = LOcation;
                    }

                    if (pickuprequest.Status == MobilePickUpRequestStatus.Accepted.ToString())
                    {
                        preshipmentmobile.shipmentstatus = "Assigned for Pickup";

                        await ScanMobileShipment(new ScanDTO
                        {
                            WaybillNumber = pickuprequest.Waybill,
                            ShipmentScanStatus = ShipmentScanStatus.MAPT
                        });

                    }

                    //if (pickuprequest.Status == MobilePickUpRequestStatus.TimedOut.ToString())
                    //{
                    //    preshipmentmobile.shipmentstatus = "Shipment created";
                    //}

                    newPreShipment = Mapper.Map<PreShipmentMobileDTO>(preshipmentmobile);

                    if (pickuprequest.ServiceCentreId != null)
                    {
                        newPreShipment.ReceiverAddress = preshipmentmobile.ServiceCentreAddress;
                        newPreShipment.ReceiverLocation.Latitude = preshipmentmobile.serviceCentreLocation.Latitude;
                        newPreShipment.ReceiverLocation.Longitude = preshipmentmobile.serviceCentreLocation.Longitude;
                    }

                    var Country = await _uow.Country.GetCountryByStationId(preshipmentmobile.SenderStationId);

                    if (Country != null)
                    {
                        newPreShipment.CurrencyCode = Country.CurrencyCode;
                        newPreShipment.CurrencySymbol = Country.CurrencySymbol;
                    }

                    await _uow.CompleteAsync();
                }

                return newPreShipment;
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
                var userId = await _userService.GetCurrentUserId();

                await _mobilepickuprequestservice.UpdateMobilePickUpRequests(pickuprequest, userId);

                if (pickuprequest.Status == MobilePickUpRequestStatus.Confirmed.ToString())
                {
                    await ConfirmMobilePickupRequest(pickuprequest, userId);
                }

                else if (pickuprequest.Status == MobilePickUpRequestStatus.Delivered.ToString())
                {
                    await DeliveredMobilePickupRequest(pickuprequest, userId);
                }

                else if (pickuprequest.Status == MobilePickUpRequestStatus.Cancelled.ToString())
                {
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.SSC
                    });
                }

                else if (pickuprequest.Status == MobilePickUpRequestStatus.Dispute.ToString())
                {
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Dispute.ToString();
                    pickuprequest.Status = MobilePickUpRequestStatus.Dispute.ToString();
                    await _mobilepickuprequestservice.UpdateMobilePickUpRequests(pickuprequest, userId);
                    await _uow.CompleteAsync();
                }
                else if(pickuprequest.Status == MobilePickUpRequestStatus.Rejected.ToString())
                {
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Processing.ToString();
                    await _uow.CompleteAsync();
                }

                else if (pickuprequest.Status == MobilePickUpRequestStatus.LogVisit.ToString())
                {
                    await LogVisitMobilePickupRequest(pickuprequest, userId);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ConfirmMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest, string userId)
        {
            try
            {
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "PreShipmentItems,SenderLocation,ReceiverLocation");
                if (preshipmentmobile == null)
                {
                    throw new GenericException("Shipment item does not exist");
                }

                int destinationServiceCentreId = 0;
                int departureServiceCentreId = 0;

                //shipment witin a state 
                if (preshipmentmobile.ZoneMapping == 1)
                {
                    var gigGOServiceCentre = await _userService.GetGIGGOServiceCentre();
                    destinationServiceCentreId = gigGOServiceCentre.ServiceCentreId;
                    departureServiceCentreId = gigGOServiceCentre.ServiceCentreId;
                }
                else
                {
                    //shipment outside a state -- Inter State Shipment
                    var DepartureStation = await _uow.Station.GetAsync(s => s.StationId == preshipmentmobile.SenderStationId);
                    departureServiceCentreId = DepartureStation.SuperServiceCentreId;

                    var DestinationStation = await _uow.Station.GetAsync(s => s.StationId == preshipmentmobile.ReceiverStationId);
                    destinationServiceCentreId = DestinationStation.SuperServiceCentreId;
                }

                var CustomerId = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);

                int customerid = 0;
                if (CustomerId != null)
                {
                    customerid = CustomerId.IndividualCustomerId;
                }
                else
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
                    DestinationServiceCentreId = destinationServiceCentreId,
                    DepartureServiceCentreId = departureServiceCentreId,
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
                        Quantity = s.Quantity

                    }).ToList()
                };
                var status = await _shipmentService.AddShipmentFromMobile(MobileShipment);
                preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.PickedUp.ToString();
                preshipmentmobile.IsConfirmed = true;

                await _uow.CompleteAsync();

                await ScanMobileShipment(new ScanDTO
                {
                    WaybillNumber = pickuprequest.Waybill,
                    ShipmentScanStatus = ShipmentScanStatus.MSHC
                });

                var item = Mapper.Map<PreShipmentMobileDTO>(preshipmentmobile);
                //await CheckDeliveryTimeAndSendMail(item);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeliveredMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest, string userId)
        {
            try
            {
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "SenderLocation,ReceiverLocation");
                if (preshipmentmobile == null)
                {
                    throw new GenericException("Shipment item does not exist");
                }
                //preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Delivered.ToString();
                preshipmentmobile.IsDelivered = true;

                if (preshipmentmobile.ZoneMapping == 1)
                {
                    var Partnerpaymentfordelivery = new PartnerPayDTO
                    {
                        ShipmentPrice = preshipmentmobile.GrandTotal,
                        ZoneMapping = (int)preshipmentmobile.ZoneMapping
                    };

                    decimal price = await _partnertransactionservice.GetPriceForPartner(Partnerpaymentfordelivery);
                    var partneruser = await _userService.GetUserById(userId);
                    var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partneruser.UserChannelCode);
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Delivered.ToString();
                    if (wallet != null)
                    {
                        wallet.Balance = wallet.Balance + price;
                    }

                    var partnertransactions = new PartnerTransactionsDTO
                    {
                        Destination = preshipmentmobile.ReceiverAddress,
                        Departure = preshipmentmobile.SenderAddress,
                        AmountReceived = price,
                        Waybill = preshipmentmobile.Waybill
                    };

                    await _uow.CompleteAsync();

                    var id = await _partnertransactionservice.AddPartnerPaymentLog(partnertransactions);
                    var defaultServiceCenter = await _userService.GetGIGGOServiceCentre();
                    var transaction = new WalletTransactionDTO
                    {
                        WalletId = wallet.WalletId,
                        CreditDebitType = CreditDebitType.Credit,
                        Amount = price,
                        ServiceCentreId = defaultServiceCenter.ServiceCentreId,
                        Waybill = preshipmentmobile.Waybill,
                        Description = "Credit for Delivered Shipment Request",
                        PaymentType = PaymentType.Online,
                        UserId = userId
                    };
                    var walletTransaction = await _walletTransactionService.AddWalletTransaction(transaction);

                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MAHD
                    });

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
                    if (preshipmentmobile.shipmentstatus == MobilePickUpRequestStatus.OnwardProcessing.ToString())
                    {
                        var Pickuprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId, preshipmentmobile.UserId = null);
                        pickuprequest.Status = MobilePickUpRequestStatus.Delivered.ToString();
                        await _mobilepickuprequestservice.UpdateMobilePickUpRequests(pickuprequest, userId);
                        //var Pickupprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId, preshipmentmobile.UserId=null);
                        var Partner = new PartnerPayDTO
                        {
                            ShipmentPrice = preshipmentmobile.GrandTotal,
                            PickUprice = Pickuprice
                        };
                        decimal price = await _partnertransactionservice.GetPriceForPartner(Partner);
                        var partneruser = await _userService.GetUserById(userId);
                        var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partneruser.UserChannelCode);
                        wallet.Balance = wallet.Balance + price;
                        var partnertransactions = new PartnerTransactionsDTO
                        {
                            Destination = preshipmentmobile.ReceiverAddress,
                            Departure = preshipmentmobile.SenderAddress,
                            AmountReceived = price,
                            Waybill = preshipmentmobile.Waybill,
                            UserId = userId

                        };
                        await _partnertransactionservice.AddPartnerPaymentLog(partnertransactions);

                        var defaultServiceCenter = await _userService.GetGIGGOServiceCentre();
                        var transaction = new WalletTransactionDTO
                        {
                            WalletId = wallet.WalletId,
                            CreditDebitType = CreditDebitType.Credit,
                            Amount = price,
                            ServiceCentreId = defaultServiceCenter.ServiceCentreId,
                            Waybill = preshipmentmobile.Waybill,
                            Description = "Credit for Delivered Shipment Request",
                            PaymentType = PaymentType.Online,
                            UserId = userId
                        };
                        var walletTransaction = await _walletTransactionService.AddWalletTransaction(transaction);
                        return;
                    }
                    else
                    {
                        pickuprequest.Status = MobilePickUpRequestStatus.Confirmed.ToString();
                        await _mobilepickuprequestservice.UpdateMobilePickUpRequests(pickuprequest, userId);
                        throw new GenericException("This is an interstate delivery, drop at assigned service centre!!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task LogVisitMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest, string userId)
        {
            try
            {
                var mobileRequest = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                if (mobileRequest == null)
                {
                    throw new GenericException("Shipment item does not exist in Pickup");
                }
                else
                {
                    mobileRequest.Status = MobilePickUpRequestStatus.Visited.ToString();
                }
                var preshipmentMobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                if (preshipmentMobile == null)
                {
                    throw new GenericException("Shipment item does not exist");
                }
                preshipmentMobile.shipmentstatus = MobilePickUpRequestStatus.Visited.ToString();

                await _uow.CompleteAsync();

                var user = await _userService.GetUserByChannelCode(preshipmentMobile.CustomerCode);

                var emailMessageExtensionDTO = new MobileMessageDTO()
                {
                    SenderName = user.FirstName + " " + user.LastName,
                    SenderEmail = user.Email,
                    WaybillNumber = preshipmentMobile.Waybill,
                    SenderPhoneNumber = preshipmentMobile.SenderPhoneNumber
                };

                var smsMessageExtensionDTO = new MobileMessageDTO()
                {
                    SenderName = preshipmentMobile.ReceiverName,
                    WaybillNumber = preshipmentMobile.Waybill,
                    SenderPhoneNumber = preshipmentMobile.ReceiverPhoneNumber
                };

                await _messageSenderService.SendGenericEmailMessage(MessageType.MATD, emailMessageExtensionDTO);
                await _messageSenderService.SendMessage(MessageType.MATD, EmailSmsType.SMS, smsMessageExtensionDTO);

            }
            catch (Exception ex)
            {
                throw ex;
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
                    if (preShipment != null)
                    {
                        //throw new GenericException("PreShipmentItem Does Not Exist");
                        preshipment.CalculatedPrice = preShipment.CalculatedPrice;
                        preshipment.Description = preShipment.Description;
                        preshipment.EstimatedPrice = preShipment.EstimatedPrice;
                        preshipment.ImageUrl = preShipment.ImageUrl;
                        preshipment.IsVolumetric = preShipment.IsVolumetric;
                        preshipment.ItemName = preShipment.ItemName;
                        preshipment.ItemType = preShipment.ItemType;
                        preshipment.Length = preShipment.Length;
                        preshipment.Quantity = preShipment.Quantity;
                        preshipment.Weight = (double)preShipment.Weight;
                        preshipment.Width = preShipment.Width;
                        preshipment.Height = preShipment.Height;
                    }
                }
                await _uow.CompleteAsync();
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
                var dictionaryCategories = await GetDictionaryCategories(Categories, Subcategories);
                var WeightDictionaryCategories = await GetWeightRangeDictionaryCategories(Categories, Subcategories);
                var result = new SpecialResultDTO
                {
                    Specialpackages = packages,
                    Categories = Categories,
                    SubCategories = Subcategories,
                    DictionaryCategory = dictionaryCategories,
                    WeightRangeDictionaryCategory = WeightDictionaryCategories
                };
                return result;
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get all special packages.");
            }
        }

        private async Task<Dictionary<string, List<string>>> GetDictionaryCategories(List<CategoryDTO> categories, List<SubCategoryDTO> subcategories)
        {
            try
            {
                Dictionary<string, List<string>> finalDictionary = new Dictionary<string, List<string>>
                {
                    //1. category
                    { "CATEGORY", categories.Select(s => s.CategoryName).OrderBy(s => s).ToList() }
                };

                //2. subcategory
                //var Subcategories = await GetSubCategories();
                foreach (var category in categories)
                {
                    var listOfSubcategory = subcategories.Where(s => s.Category.CategoryId == category.CategoryId).
                        Select(s => s.SubCategoryName).Distinct().ToList();

                    //add to dictionary
                    finalDictionary.Add(category.CategoryName, listOfSubcategory);
                }

                //3. subsubcategory
                //var specialDomesticPackages = await GetSpecialDomesticPackages();
                foreach (var subcategory in subcategories)
                {
                    var list = new List<decimal>();
                    var listOfWeights = subcategories.Where(s => s.SubCategoryName == subcategory.SubCategoryName).
                        Select(s => s.Weight.ToString()).ToList();

                    //add to dictionary
                    if (!finalDictionary.ContainsKey(subcategory.SubCategoryName))
                    {
                        finalDictionary.Add(subcategory.SubCategoryName, listOfWeights);
                    }
                }
                return await Task.FromResult(finalDictionary);
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get the categorization of special packages.");
            }
        }

        private async Task<Dictionary<string, List<string>>> GetDictionaryCategories()
        {
            try
            {
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

        private async Task<Dictionary<string, List<string>>> GetWeightRangeDictionaryCategories(List<CategoryDTO> categories, List<SubCategoryDTO> subcategories)
        {
            try
            {
                Dictionary<string, List<string>> finalDictionary = new Dictionary<string, List<string>>
                {
                    //1. category
                    { "CATEGORY", categories.Select(s => s.CategoryName).OrderBy(s => s).ToList() }
                };

                //2. subcategory
                foreach (var category in categories)
                {
                    var listOfSubcategory = subcategories.Where(s => s.Category.CategoryId == category.CategoryId).
                        Select(s => s.SubCategoryName).Distinct().ToList();

                    //add to dictionary
                    finalDictionary.Add(category.CategoryName, listOfSubcategory);
                }

                //3. subsubcategory
                foreach (var subcategory in subcategories)
                {
                    var list = new List<decimal>();
                    var listOfWeights = subcategories.Where(s => s.SubCategoryName == subcategory.SubCategoryName).
                        Select(s => s.WeightRange).ToList();

                    //add to dictionary
                    if (!finalDictionary.ContainsKey(subcategory.SubCategoryName))
                    {
                        finalDictionary.Add(subcategory.SubCategoryName, listOfWeights);
                    }
                }
                return await Task.FromResult(finalDictionary);
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get the categorization of special packages.");
            }
        }

        private async Task<Dictionary<string, List<string>>> GetWeightRangeDictionaryCategories()
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
                        Select(s => s.WeightRange.ToString()).ToList();

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
                var shipments = await _uow.PreShipmentMobile.FindAsync(s => s.UserId == user && s.shipmentstatus == MobilePickUpRequestStatus.Dispute.ToString(), "PreShipmentItems");
                var shipment = shipments.OrderByDescending(s => s.DateCreated);
                var newPreShipment = Mapper.Map<List<PreShipmentMobileDTO>>(shipment);
                foreach (var Shipment in newPreShipment)
                {
                    var country = await _uow.Country.GetCountryByStationId(Shipment.SenderStationId);
                    Shipment.CurrencyCode = country.CurrencyCode;
                    Shipment.CurrencySymbol = country.CurrencySymbol;
                }
                return newPreShipment;
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get shipments in dispute.");
            }
        }
        public async Task<SummaryTransactionsDTO> GetPartnerWalletTransactions()
        {
            try
            {
                var CurrencyCode = "";
                var CurrencySymbol = "";
                var user = await _userService.GetCurrentUserId();
                var transactions = await _uow.PartnerTransactions.FindAsync(s => s.UserId == user);
                var partneruser = await _userService.GetUserById(user);
                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partneruser.UserChannelCode);
                transactions = transactions.OrderByDescending(s => s.DateCreated);
                var TotalTransactions = Mapper.Map<List<PartnerTransactionsDTO>>(transactions);
                var Country = await _uow.Country.GetAsync(s => s.CountryId == partneruser.UserActiveCountryId);
                if (Country != null)
                {
                    CurrencyCode = Country.CurrencyCode;
                    CurrencySymbol = Country.CurrencySymbol;
                }
                var summary = new SummaryTransactionsDTO
                {
                    CurrencySymbol = CurrencySymbol,
                    CurrencyCode = CurrencyCode,
                    WalletBalance = wallet.Balance,
                    Transactions = TotalTransactions
                };
                return summary;
            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to get partner's transactions.");
            }
        }

        public async Task<object> ResolveDisputeForMobile(PreShipmentMobileDTO preShipment)
        {
            try
            {
                var preshipmentmobilegrandtotal = await _uow.PreShipmentMobile.GetAsync(s => s.PreShipmentMobileId == preShipment.PreShipmentMobileId);

                if (preshipmentmobilegrandtotal.shipmentstatus == MobilePickUpRequestStatus.PickedUp.ToString() || preshipmentmobilegrandtotal.shipmentstatus == MobilePickUpRequestStatus.Delivered.ToString())
                {
                    throw new GenericException("This Shipment cannot be placed in Dispute, because it has been" + " " + preshipmentmobilegrandtotal.shipmentstatus);
                }

                foreach (var id in preShipment.DeletedItems)
                {
                    var preshipmentitemmobile = await _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentItemMobileId == id && s.PreShipmentMobileId == preShipment.PreShipmentMobileId);
                    preshipmentitemmobile.IsCancelled = true;
                    _uow.PreShipmentItemMobile.Remove(preshipmentitemmobile);
                }

                foreach (var item in preShipment.PreShipmentItems)
                {
                    var preshipmentitemmobile = await _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentItemMobileId == item.PreShipmentItemMobileId && s.PreShipmentMobileId == preShipment.PreShipmentMobileId);
                    preshipmentitemmobile.Quantity = item.Quantity;
                    preshipmentitemmobile.Value = item.Value;
                    preshipmentitemmobile.Weight = (double)item.Weight;
                    preshipmentitemmobile.Description = item.Description;
                    preshipmentitemmobile.Height = item.Height;
                    preshipmentitemmobile.ImageUrl = item.ImageUrl;
                    preshipmentitemmobile.ItemName = item.ItemName;
                    preshipmentitemmobile.Length = item.Length;
                }

                var PreshipmentPriceDTO = await GetPrice(preShipment);
                preshipmentmobilegrandtotal.shipmentstatus = MobilePickUpRequestStatus.Resolved.ToString();
                var difference = (preshipmentmobilegrandtotal.GrandTotal - PreshipmentPriceDTO.GrandTotal);
                
                var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preShipment.CustomerCode);

                if (difference < 0.00M)
                {
                    var newdiff = Math.Abs((decimal)difference);
                    if (newdiff > updatedwallet.Balance)
                    {
                        throw new GenericException("Insufficient Wallet Balance");
                    }
                }

                updatedwallet.Balance = updatedwallet.Balance + (decimal)difference;

                var pickuprequests = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preshipmentmobilegrandtotal.Waybill && s.Status == MobilePickUpRequestStatus.Dispute.ToString());
                if (pickuprequests != null)
                {
                    pickuprequests.Status = MobilePickUpRequestStatus.Resolved.ToString();
                }
                await _uow.CompleteAsync();
                return new { IsResolved = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> CancelShipment(string Waybill)
        {
            try
            {
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == Waybill);
                if (preshipmentmobile == null)
                {
                    throw new GenericException("Shipment does not exist");
                }
                                
                var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);

                var pickuprequests = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preshipmentmobile.Waybill && (s.Status != MobilePickUpRequestStatus.Rejected.ToString() || s.Status != MobilePickUpRequestStatus.TimedOut.ToString()));
                if (pickuprequests != null)
                {
                    var pickuprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId, preshipmentmobile.UserId = null);
                    updatedwallet.Balance = ((updatedwallet.Balance + preshipmentmobile.GrandTotal) - Convert.ToDecimal(pickuprice));
                    var Partnersprice = (0.4M * Convert.ToDecimal(pickuprice));
                    
                    var rider = await _userService.GetUserById(pickuprequests.UserId);
                    var riderWallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == rider.UserChannelCode);
                    riderWallet.Balance = riderWallet.Balance + Partnersprice;
                    
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Cancelled.ToString();
                    pickuprequests.Status = MobilePickUpRequestStatus.Cancelled.ToString();

                    var partnertransactions = new PartnerTransactionsDTO
                    {
                        Destination = preshipmentmobile.ReceiverAddress,
                        Departure = preshipmentmobile.SenderAddress,
                        AmountReceived = Partnersprice,
                        Waybill = preshipmentmobile.Waybill
                    };
                    await _partnertransactionservice.AddPartnerPaymentLog(partnertransactions);
                }
                else
                {
                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Cancelled.ToString();
                    updatedwallet.Balance = ((updatedwallet.Balance + preshipmentmobile.GrandTotal));
                }
                await _uow.CompleteAsync();
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
                }
                await _uow.CompleteAsync();
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
                if (user.UserChannelType == UserChannelType.Ecommerce)
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

        public async Task<PartnerDTO> CreatePartner(string CustomerCode)
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
                        partnerDTO.UserActiveCountryId = user.UserActiveCountryId;


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
                        partnerDTO.UserActiveCountryId = user.UserActiveCountryId;
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
                        partner.UserActiveCountryId = user.UserActiveCountryId;
                    }
                    else
                    {
                        partner.PartnerType = PartnerType.DeliveryPartner;
                        partner.UserActiveCountryId = user.UserActiveCountryId;
                    }

                }
                await _uow.CompleteAsync();
                var latestpartner = await _uow.Partner.GetAsync(s => s.PartnerCode == CustomerCode);
                var finalPartner = Mapper.Map<PartnerDTO>(latestpartner);
                return finalPartner;
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
                var number = await _uow.DeliveryNumber.GetAsync(s => s.Number.ToLower() == detail.DeliveryNumber.ToLower());
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
                    var VehicleDetails = await _uow.VehicleType.FindAsync(s => s.Partnercode == partnerdto.PartnerCode);
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
            return await Task.FromResult(userActiveCountryId);
        }

        public async Task<decimal> GetPickUpPrice(string vehicleType, int CountryId, string UserId)
        {
            try
            {
                var PickUpPrice = 0.0M;
                if (vehicleType != null)
                {
                    if (UserId == null)
                    {
                        PickUpPrice = await GetPickUpPriceForIndividual(vehicleType, CountryId);
                    }
                    else
                    {

                        var user = await _userService.GetUserById(UserId);
                        var exists = await _uow.Company.ExistAsync(s => s.CustomerCode == user.UserChannelCode);
                        if (user.UserChannelType == UserChannelType.Ecommerce || exists)
                        {
                            PickUpPrice = await GetPickUpPriceForEcommerce(vehicleType, CountryId);
                        }
                        else
                        {
                            PickUpPrice = await GetPickUpPriceForIndividual(vehicleType, CountryId);
                        }
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
                string filename = images.PartnerFullName + "_" + images.FileType.ToString() + ".png";
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
                var result = await _uow.Shipment.GetAsync(s => s.DeliveryNumber == DeliveryNumber || s.Waybill == DeliveryNumber);
                if (result != null)
                {
                    ShipmentSummaryDetails = await GetPartnerDetailsFromWaybill(result.Waybill);
                    var details = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == result.Waybill, "PreShipmentItems");
                    if (details == null)
                    {
                        throw new GenericException("Shipment cannot be found in PreshipmentMobile");
                    }
                    var PreShipmentdto = Mapper.Map<PreShipmentMobileDTO>(details);
                    ShipmentSummaryDetails.shipmentdetails = PreShipmentdto;
                    return ShipmentSummaryDetails;
                }
                else if (result == null)
                {
                    var details = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == DeliveryNumber, "PreShipmentItems");
                    if (details == null)
                    {
                        throw new GenericException("Shipment cannot be found in PreshipmentMobile");
                    }
                    else
                    {
                        ShipmentSummaryDetails = await GetPartnerDetailsFromWaybill(details.Waybill);
                        var PreShipmentdto = Mapper.Map<PreShipmentMobileDTO>(details);
                        ShipmentSummaryDetails.shipmentdetails = PreShipmentdto;
                        return ShipmentSummaryDetails;
                    }

                }
                else
                {
                    throw new GenericException("Shipment cannot be found");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> ApproveShipment(ApproveShipmentDTO detail)
        {
            try
            {
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == detail.WaybillNumber, "PreShipmentItems,SenderLocation,ReceiverLocation");
                var Pickupprice = await GetPickUpPrice(preshipmentmobile.VehicleType, preshipmentmobile.CountryId, preshipmentmobile.UserId);
                var user = await _userService.GetUserByChannelCode(preshipmentmobile.CustomerCode);

                if (preshipmentmobile.ZoneMapping != 1)
                {
                    if (preshipmentmobile.IsApproved != true)
                    {
                        var CustomerId = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);

                        int customerid = 0;
                        if (CustomerId != null)
                        {
                            customerid = CustomerId.IndividualCustomerId;
                        }
                        else
                        {
                            var companyid = await _uow.Company.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                            customerid = companyid.CompanyId;
                        }
                        var DepartureCountryId = await GetCountryByServiceCentreId(detail.SenderServiceCentreId);
                        var DestinationCountryId = await GetCountryByServiceCentreId(detail.ReceiverServiceCentreId);
                        var MobileShipment = new ShipmentDTO
                        {
                            Waybill = preshipmentmobile.Waybill,
                            ReceiverName = preshipmentmobile.ReceiverName,
                            ReceiverPhoneNumber = preshipmentmobile.ReceiverPhoneNumber,
                            ReceiverEmail = preshipmentmobile.ReceiverEmail,
                            ReceiverAddress = preshipmentmobile.ReceiverAddress,
                            DeliveryOptionId = 2,
                            GrandTotal = preshipmentmobile.GrandTotal,
                            Insurance = preshipmentmobile.InsuranceValue,
                            Vat = preshipmentmobile.Vat,
                            SenderAddress = preshipmentmobile.SenderAddress,
                            IsCashOnDelivery = false,
                            CustomerCode = preshipmentmobile.CustomerCode,
                            DestinationServiceCentreId = detail.ReceiverServiceCentreId,
                            DepartureServiceCentreId = detail.SenderServiceCentreId,
                            CustomerId = customerid,
                            UserId = user.Id,
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
                            ShipmentPickupPrice = Pickupprice,
                            DestinationCountryId = DestinationCountryId,
                            DepartureCountryId = DepartureCountryId,
                            ShipmentItems = preshipmentmobile.PreShipmentItems.Select(s => new ShipmentItemDTO
                            {
                                Description = s.Description,
                                IsVolumetric = s.IsVolumetric,
                                Weight = s.Weight,
                                Nature = s.ItemType,
                                Price = (decimal)s.CalculatedPrice,
                                Quantity = s.Quantity

                            }).ToList()
                        };
                        var status = await _shipmentService.AddShipmentFromMobile(MobileShipment);

                        preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.OnwardProcessing.ToString();
                        preshipmentmobile.IsApproved = true;

                        //add scan status into Mobiletracking and Shipmenttracking
                        await ScanMobileShipment(new ScanDTO
                        {
                            WaybillNumber = detail.WaybillNumber,
                            ShipmentScanStatus = ShipmentScanStatus.MSVC
                        });

                        await _shipmentService.ScanShipment(new ScanDTO
                        {
                            WaybillNumber = detail.WaybillNumber,
                            ShipmentScanStatus = ShipmentScanStatus.ARO
                        });
                        await _uow.CompleteAsync();
                    }
                    else
                    {
                        throw new GenericException("Shipment has already been approved!!!");
                    }
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
                        Name = user.FirstName + " " + user.LastName,
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
            try
            {
                decimal price = 0;
                
                //check haulage exists
                var haulage = await _haulageService.GetHaulageById(haulagePricingDto.Haulageid);
                if (haulage == null)
                {
                    throw new GenericException("The Tonne specified has not been mapped");
                }
                
                var country = await _uow.Country.GetCountryByStationId(haulagePricingDto.DestinationStationId);
                var IsWithinProcessingTime = await WithinProcessingTime(country.CountryId);
                var DiscountPercent = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DiscountPercentage, country.CountryId);

                var Percentage = (Convert.ToDecimal(DiscountPercent.Value));
                var PercentageTobeUsed = ((100M - Percentage) / 100M);
                var discount = (1 - PercentageTobeUsed);
                
                //get the distance based on the stations
                var haulageDistanceMapping = await _haulageDistanceMappingService.GetHaulageDistanceMappingForMobile(haulagePricingDto.DepartureStationId, haulagePricingDto.DestinationStationId);
                int distance = haulageDistanceMapping.Distance;

                //set the default distance to 1
                if (distance == 0)
                {
                    distance = 1;
                }

                //Get Haulage Maximum Fixed Distance
                var maximumFixedDistanceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.HaulageMaximumFixedDistance, country.CountryId);
                int maximumFixedDistance = int.Parse(maximumFixedDistanceObj.Value);

                //calculate price for the haulage
                if (distance <= maximumFixedDistance)
                {
                    price = haulage.FixedRate;
                }
                else
                {
                    //1. get the fixed rate and substract the maximum fixed distance from distance
                    distance = distance - maximumFixedDistance;

                    //2. multiply the remaining distance with the additional pate
                    price = haulage.FixedRate + distance * haulage.AdditionalRate;
                }

                return new MobilePriceDTO
                {
                    GrandTotal = Math.Round(price * PercentageTobeUsed),
                    CurrencySymbol = country.CurrencySymbol,
                    CurrencyCode = country.CurrencyCode,
                    IsWithinProcessingTime = IsWithinProcessingTime,
                    Discount = Math.Round(price * discount)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditProfile(UserDTO user)
        {
            try
            {
                string currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);

                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                //User.PhoneNumber = user.PhoneNumber;
                //User.Email = user.Email;
                currentUser.PictureUrl = user.PictureUrl;
                await _userService.UpdateUser(currentUser.Id, currentUser);

                string userChannelType = user.UserChannelType.ToString();
                user.UserChannelCode = currentUser.UserChannelCode;

                if (userChannelType == UserChannelType.Partner.ToString()
                    || userChannelType == UserChannelType.IndividualCustomer.ToString()
                    || userChannelType == UserChannelType.Ecommerce.ToString())
                {
                    await UpdatePartner(user);
                    await UpdateCustomer(user);
                    await UpdateCompany(user);
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
                        PartnerName = user.FirstName + " " + user.LastName,
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

        private async Task<bool> WithinProcessingTime(int CountryId)
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
        public async Task DetermineShipmentstatus(string waybill, int countryid, DateTime ExpectedTimeOfDelivery)
        {
            var status = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == waybill && (s.Status != MobilePickUpRequestStatus.Rejected.ToString() || s.Status != MobilePickUpRequestStatus.TimedOut.ToString()));
            if (status != null)
            {
                if (status.Status != MobilePickUpRequestStatus.Delivered.ToString())
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
            var country = await _uow.Country.GetCountryByStationId(item.SenderStationId);
            var additionaltime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.AddedMinutesForDeliveryTime, country.CountryId);
            var addedtime = int.Parse(additionaltime.Value);
            var totaltime = time + addedtime;
            var NewTime = DateTime.Now.AddMinutes(totaltime);
            BackgroundJob.Schedule(() => DetermineShipmentstatus(item.Waybill, country.CountryId, NewTime), TimeSpan.FromMinutes(totaltime));

        }

        public async Task<object> CancelShipmentWithNoCharge(string Waybill, string Userchanneltype)
        {
            try
            {
                var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == Waybill);

                if(preshipmentmobile == null)
                {
                    throw new GenericException("Shipment cannot be found");
                }

                if (Userchanneltype == UserChannelType.IndividualCustomer.ToString())
                {
                    if (preshipmentmobile.shipmentstatus == "Shipment created" || preshipmentmobile.shipmentstatus == MobilePickUpRequestStatus.Rejected.ToString() || preshipmentmobile.shipmentstatus == MobilePickUpRequestStatus.TimedOut.ToString())
                    {
                        preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Cancelled.ToString();
                        
                        var defaultServiceCenter = await _userService.GetGIGGOServiceCentre();
                        var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                        updatedwallet.Balance = ((updatedwallet.Balance + preshipmentmobile.GrandTotal));
                        
                        var transaction = new WalletTransactionDTO
                        {
                            WalletId = updatedwallet.WalletId,
                            CreditDebitType = CreditDebitType.Credit,
                            Amount = preshipmentmobile.GrandTotal,
                            ServiceCentreId = defaultServiceCenter.ServiceCentreId,
                            Waybill = preshipmentmobile.Waybill,
                            Description = "Cancelled Shipment",
                            PaymentType = PaymentType.Online,
                            UserId = preshipmentmobile.UserId
                        };

                        var walletTransaction = await _walletTransactionService.AddWalletTransaction(transaction);
                        await ScanMobileShipment(new ScanDTO
                        {
                            WaybillNumber = Waybill,
                            ShipmentScanStatus = ShipmentScanStatus.MSCC
                        });
                    }
                    else
                    {
                        throw new GenericException("Shipment cannot be cancelled because it has been assigned.");
                    }
                }
                //FOR PARTNER TRYING TO CANCEL  A SHIPMENT
                else
                {
                    var pickuprequests = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preshipmentmobile.Waybill && s.Status == MobilePickUpRequestStatus.Accepted.ToString());

                    if(pickuprequests != null)
                    {
                        pickuprequests.Status = MobilePickUpRequestStatus.Processing.ToString();
                    }

                    preshipmentmobile.shipmentstatus = MobilePickUpRequestStatus.Processing.ToString();
                }
                await _uow.CompleteAsync();
                return new { IsCancelled = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<PreShipmentSummaryDTO> GetPartnerDetailsFromWaybill(string Waybill)
        {
            var ShipmentSummaryDetails = new PreShipmentSummaryDTO();
            var partner = await _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == Waybill && (s.Status != MobilePickUpRequestStatus.Rejected.ToString() || s.Status != MobilePickUpRequestStatus.TimedOut.ToString()));
            if (partner != null)
            {
                var Partnerdetails = await _uow.Partner.GetAsync(s => s.UserId == partner.UserId);
                var userdetails = await _userService.GetUserById(partner.UserId);
                if (Partnerdetails == null)
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
            return ShipmentSummaryDetails;
        }

        private async Task UpdatePartner(UserDTO user)
        {
            var partner = await _uow.Partner.GetAsync(s => s.PartnerCode == user.UserChannelCode);
            if (partner != null)
            {
                partner.FirstName = user.FirstName;
                partner.LastName = user.LastName;
                //partner.Email = user.Email;
                partner.PictureUrl = user.PictureUrl;
            }
        }
        private async Task UpdateCustomer(UserDTO user)
        {
            var customer = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == user.UserChannelCode);
            if (customer != null)
            {
                customer.FirstName = user.FirstName;
                customer.LastName = user.LastName;
                //customer.Email = user.Email;
                customer.PictureUrl = user.PictureUrl;
            }
        }
        private async Task UpdateCompany(UserDTO user)
        {
            var company = await _uow.Company.GetAsync(s => s.CustomerCode == user.UserChannelCode);
            if (company != null)
            {
                company.FirstName = user.FirstName;
                company.LastName = user.LastName;
                //company.Email = user.Email;
                company.Name = user.Organisation;
            }
        }
        private async Task<int> GetCountryByServiceCentreId(int ServicecentreId)
        {
            int CountryId = 0;
            var ServiceCentre = await _uow.ServiceCentre.GetAsync(s => s.ServiceCentreId == ServicecentreId);
            if (ServiceCentre != null)
            {
                var Country = await _uow.Country.GetCountryByStationId(ServiceCentre.StationId);
                if (Country != null)
                {
                    CountryId = Country.CountryId;
                }
            }
            return CountryId;
        }

        private async Task<decimal> CalculateGeoDetailsBasedonLocation(PreShipmentMobileDTO item)
        {
            var FixedDistance = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GiglgoMaximumFixedDistance, item.CountryId);
            var FixedPriceForTime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GiglgoFixedPriceForTime, item.CountryId);
            var FixedPriceForDistance = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GiglgoFixedPriceForDistance, item.CountryId);

            var FixedDistanceValue = int.Parse(FixedDistance.Value);
            var FixedPriceForTimeValue = Convert.ToDecimal(FixedPriceForTime.Value);
            var FixedPriceForDistanceValue = Convert.ToDecimal(FixedPriceForDistance.Value);

            var Location = new LocationDTO
            {
                DestinationLatitude = (double)item.ReceiverLocation.Latitude,
                DestinationLongitude = (double)item.ReceiverLocation.Longitude,
                OriginLatitude = (double)item.SenderLocation.Latitude,
                OriginLongitude = (double)item.SenderLocation.Longitude
            };

            RootObject details = await _partnertransactionservice.GetGeoDetails(Location);
            var time = (details.rows[0].elements[0].duration.value / 60);
            var distance = (details.rows[0].elements[0].distance.value / 1000);

            decimal amount = time * FixedPriceForTimeValue;

            if (distance > FixedDistanceValue)
            {
                var distancedifference = (distance - FixedDistanceValue);
                amount += distancedifference * FixedPriceForDistanceValue;
            }

            return amount;
        }

        public async Task<List<PreShipmentMobileDTO>> GetPreShipmentForEcommerce(string userChannelCode)
        {
            try
            {
                var shipment = await _uow.Shipment.FindAsync(x => x.CustomerCode.Equals(userChannelCode) && x.IsCancelled == false);
                
                List<PreShipmentMobileDTO> shipmentDto = (from r in shipment
                                                          select new PreShipmentMobileDTO()
                                                          {
                                                              PreShipmentMobileId = r.ShipmentId,
                                                              Waybill = r.Waybill,
                                                              ActualDateOfArrival = r.ActualDateOfArrival,
                                                              DateCreated = r.DateCreated,
                                                              DateModified = r.DateModified,
                                                              ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                              ReceiverAddress = r.ReceiverAddress,
                                                              SenderAddress = r.SenderAddress,
                                                              ReceiverCountry = r.ReceiverCountry,
                                                              ReceiverEmail = r.ReceiverEmail,
                                                              ReceiverName = r.ReceiverName,
                                                              ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                              ReceiverState = r.ReceiverState,
                                                              UserId = r.UserId,
                                                              Value = r.Value,
                                                              GrandTotal = r.GrandTotal,
                                                              CustomerCode = r.CustomerCode,
                                                              DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                              shipmentstatus = "Shipment",
                                                              CustomerId = r.CustomerId,
                                                              CustomerType = r.CustomerType,
                                                              CountryId = r.DepartureCountryId
                                                          }).OrderByDescending(x => x.DateCreated).Take(20).ToList();

                foreach (var shipments in shipmentDto)
                {
                    var country = await _uow.Country.GetAsync(shipments.CountryId);

                    if (country != null)
                    {
                        shipments.CurrencyCode = country.CurrencyCode;
                        shipments.CurrencySymbol = country.CurrencySymbol;
                    }

                    if (shipments.CustomerType == "Individual")
                    {
                        shipments.CustomerType = CustomerType.IndividualCustomer.ToString();
                    }

                    if (shipments.SenderAddress == null)
                    {
                        CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipments.CustomerType);
                        var CustomerDetails = await _customerService.GetCustomer(shipments.CustomerId, customerType);
                        shipments.SenderAddress = CustomerDetails.Address;
                        shipments.SenderName = CustomerDetails.Name;
                    }
                }

                return await Task.FromResult(shipmentDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<decimal> GetPickUpPriceForIndividual(string vehicleType, int CountryId)
        {
            var PickUpPrice = 0.0M;
            if (vehicleType.ToLower() == Vehicletype.Car.ToString().ToLower())
            {
                var carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.CarPickupPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(carPickUprice.Value));
            }
            else if (vehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower())
            {
                var bikePickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikePickUpPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(bikePickUprice.Value));
            }
            else if (vehicleType.ToLower() == Vehicletype.Van.ToString().ToLower())
            {
                var vanPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.VanPickupPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(vanPickUprice.Value));
            }
            return PickUpPrice;
        }
        private async Task<decimal> GetPickUpPriceForEcommerce(string vehicleType, int CountryId)
        {
            var PickUpPrice = 0.0M;
            if (vehicleType.ToLower() == Vehicletype.Car.ToString().ToLower())
            {
                var carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceCarPickupPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(carPickUprice.Value));
            }
            else if (vehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower())
            {
                var bikePickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceBikePickUpPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(bikePickUprice.Value));
            }
            else if (vehicleType.ToLower() == Vehicletype.Van.ToString().ToLower())
            {
                var vanPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceVanPickupPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(vanPickUprice.Value));
            }
            return PickUpPrice;
        }

        public async Task<List<GiglgoStationDTO>> GetGoStations()
        {
            var stations = await _giglgoStationService.GetGoStations();
            return stations;

        }

    }
}