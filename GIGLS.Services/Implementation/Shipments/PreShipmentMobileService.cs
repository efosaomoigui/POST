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


        public PreShipmentMobileService(IUnitOfWork uow, IShipmentService shipmentService, IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, IUserServiceCentreMappingService userServiceCentre, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPricingService pricingService, IWalletService walletService, IWalletTransactionService walletTransactionService,
            IUserService userService, ISpecialDomesticPackageService specialdomesticpackageservice, IMobileShipmentTrackingService mobiletrackingservice,
            IMobilePickUpRequestsService mobilepickuprequestservice, IDomesticRouteZoneMapService domesticroutezonemapservice, ICategoryService categoryservice, ISubCategoryService subcategoryservice,
            IPartnerTransactionsService partnertransactionservice, IGlobalPropertyService globalPropertyService)
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
                throw;
            }
        }

        private async Task<PreShipmentMobileDTO> CreatePreShipment(PreShipmentMobileDTO preShipmentDTO)
        {
            // get the current user info
            var currentUser = await _userService.GetCurrentUserId();
            preShipmentDTO.UserId = currentUser;

            var PreshipmentPriceDTO = await GetPrice(preShipmentDTO);

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

        public async Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
            var user = await _userService.GetUserById(preShipment.UserId);
            var Price = 0.0M;
            decimal DeclaredValue = 0.0M;
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
                    ShipmentType = preShipmentItem.ShipmentType
                };

                if (preShipmentItem.ShipmentType == ShipmentType.Ecommerce)
                {
                    preShipmentItem.CalculatedPrice = await _pricingService.GetMobileEcommercePrice(PriceDTO);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                }
                if (preShipmentItem.ShipmentType == ShipmentType.Special)
                {
                    preShipmentItem.CalculatedPrice = await _pricingService.GetMobileSpecialPrice(PriceDTO);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                }
                else if (preShipmentItem.ShipmentType == ShipmentType.Regular)
                {
                    preShipmentItem.CalculatedPrice = await _pricingService.GetMobileRegularPrice(PriceDTO);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                }
                if (!string.IsNullOrEmpty(preShipmentItem.Value))
                {
                    DeclaredValue += Convert.ToDecimal(preShipmentItem.Value);
                    var DeclaredValueForPreShipment = Convert.ToDecimal(preShipmentItem.Value);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + (DeclaredValueForPreShipment * 0.01M);
                    preShipment.IsdeclaredVal = true;
                }
                Price += (decimal)preShipmentItem.CalculatedPrice;
            };
            decimal EstimatedDeclaredPrice = Convert.ToDecimal(DeclaredValue);
            preShipment.DeliveryPrice = Price;
            preShipment.Vat = (decimal)(preShipment.DeliveryPrice * 0.05M);
            preShipment.InsuranceValue = (EstimatedDeclaredPrice * 0.01M);
            preShipment.CalculatedTotal = (double)(preShipment.DeliveryPrice + preShipment.Vat + preShipment.InsuranceValue);
            preShipment.CalculatedTotal = Math.Round((double)preShipment.CalculatedTotal / 100d, 0) * 100;
            preShipment.Value = DeclaredValue;
            var returnprice = new MobilePriceDTO()
            {
                PreshipmentMobile = preShipment,
                DeliveryPrice = preShipment.DeliveryPrice,
                Vat = preShipment.Vat,
                InsuranceValue = preShipment.InsuranceValue,
                GrandTotal = (decimal)preShipment.CalculatedTotal
            };
            return returnprice;
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


        public async Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages()
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
        public async Task<List<CategoryDTO>> GetCategories()
        {
            var categories = await _categoryservice.GetCategories();
            return categories;
        }
        public async Task<List<SubCategoryDTO>> GetSubCategories()
        {
            var subcategories = await _subcategoryservice.GetSubCategories();
            return subcategories;
        }

        public async Task ScanMobileShipment(ScanDTO scan)
        {
            // verify the waybill number exists in the system

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
        public async Task<PreShipmentMobile> GetMobileShipmentForScan(string waybill)
        {
            var shipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill.Equals(waybill));
            return shipment;
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
                pickuprequest.Status = MobilePickUpRequestStatus.Accepted.ToString();
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
                    await _mobilepickuprequestservice.AddMobilePickUpRequests(pickuprequest);
                    preshipmentmobile.shipmentstatus = "Assigned for Pickup";
                    await _uow.CompleteAsync();
                    var newPreShipment = Mapper.Map<PreShipmentMobileDTO>(preshipmentmobile);

                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MAPT
                    });
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
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MSHC
                    });
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "PreShipmentItems");
                    var DepartureStation = await _uow.Station.GetAsync(s => s.StationId == preshipmentmobile.SenderStationId);
                    var DestinationStation = await _uow.Station.GetAsync(s => s.StationId == preshipmentmobile.ReceiverStationId);
                    var CustomerId = await _uow.IndividualCustomer.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                   
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
                        CustomerId = CustomerId.IndividualCustomerId,
                        UserId = userId,
                        PickupOptions = PickupOptions.HOMEDELIVERY,
                        IsdeclaredVal = preshipmentmobile.IsdeclaredVal,
                        ShipmentPackagePrice = preshipmentmobile.GrandTotal,
                        ApproximateItemsWeight = 0.00,
                        ReprintCounterStatus = false,
                        CustomerType = preshipmentmobile.CustomerType,
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
                    preshipmentmobile.shipmentstatus = "Picked up";
                    preshipmentmobile.IsConfirmed = true;
                    await _uow.CompleteAsync();


                }
                if (pickuprequest.Status == MobilePickUpRequestStatus.Delivered.ToString())
                {
                    
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "SenderLocation,ReceiverLocation");
                    if (preshipmentmobile.ZoneMapping == 1)
                    {
                        await ScanMobileShipment(new ScanDTO
                        {
                            WaybillNumber = pickuprequest.Waybill,
                            ShipmentScanStatus = ShipmentScanStatus.MAHD
                        });
                    }
                    else
                    {
                        await ScanMobileShipment(new ScanDTO
                        {
                            WaybillNumber = pickuprequest.Waybill,
                            ShipmentScanStatus = ShipmentScanStatus.MSVC
                        });

                    }
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
                        ShipmentPrice = (decimal)preshipmentmobile.CalculatedTotal
                    };
                    decimal price = await _partnertransactionservice.GetPriceForPartner(Partner);
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
                    preshipmentmobile.shipmentstatus = "Shipment Delivered";
                    await _uow.CompleteAsync();
                }
                if (pickuprequest.Status == MobilePickUpRequestStatus.Rejected.ToString())
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
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<Dictionary<string, List<string>>> GetDictionaryCategories()
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
                if(!finalDictionary.ContainsKey(subcategory.SubCategoryName))
                {
                    finalDictionary.Add(subcategory.SubCategoryName, listOfWeights);
                }

            }

            return finalDictionary;
        }

        public async Task<List<PreShipmentMobileDTO>> GetDisputePreShipment()
        {
            var user = await _userService.GetCurrentUserId();

            var shipments = _uow.PreShipmentMobile.FindAsync(s => s.UserId == user && s.shipmentstatus == MobilePickUpRequestStatus.Dispute.ToString(), "PreShipmentItems").Result;
            var shipment = shipments.OrderByDescending(s => s.DateCreated);
            var newPreShipment = Mapper.Map<List<PreShipmentMobileDTO>>(shipment);
            return newPreShipment;

        }
        public async Task<SummaryTransactionsDTO> GetPartnerWalletTransactions()
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
        
        public async Task<object> ResolveDisputeForMobile(PreShipmentMobileDTO preShipment)
        {
            try
            {
                var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preShipment.CustomerCode);
                var preshipmentmobilegrandtotal = _uow.PreShipmentMobile.GetAsync(s => s.PreShipmentMobileId == preShipment.PreShipmentMobileId).Result;
                foreach (var id in preShipment.DeletedItems.ToList())
                {
                    var preshipmentitemmobile = _uow.PreShipmentItemMobile.GetAsync(s =>s.PreShipmentItemMobileId == id && s.PreShipmentMobileId == preShipment.PreShipmentMobileId).Result;
                    preshipmentitemmobile.IsCancelled = true;
                    _uow.PreShipmentItemMobile.Remove(preshipmentitemmobile);
                }

                var PreshipmentPriceDTO = await GetPrice(preShipment);
                preshipmentmobilegrandtotal.shipmentstatus = "Resolved";
                var difference = ((decimal)preshipmentmobilegrandtotal.CalculatedTotal - PreshipmentPriceDTO.GrandTotal);
                if(difference < 0.00M)
                {
                    var newdiff = Math.Abs((decimal)difference);
                    if(newdiff > updatedwallet.Balance)
                    {
                        throw new GenericException("Insufficient Wallet Balance");
                    }
                }
                updatedwallet.Balance = updatedwallet.Balance + (decimal)difference;
                var pickuprequests = _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preShipment.Waybill).Result;
                pickuprequests.Status = "Resolved";
                await _uow.CompleteAsync();
                return new { IsResolved = true};
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
                var preshipmentmobile = _uow.PreShipmentMobile.GetAsync(s => s.Waybill == Waybill,"PreShipmentItems").Result;
                if(preshipmentmobile==null)
                {
                    throw new GenericException("Shipment does not exist");
                }
                var pickuprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PickUpPrice);
                var updatedwallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == preshipmentmobile.CustomerCode);
                foreach (var id in preshipmentmobile.PreShipmentItems.ToList())
                {
                    var preshipmentitmmobile = _uow.PreShipmentItemMobile.GetAsync(s => s.PreShipmentMobileId == preshipmentmobile.PreShipmentMobileId && s.PreShipmentItemMobileId == id.PreShipmentItemMobileId).Result;
                    preshipmentitmmobile.IsCancelled = true;
                    _uow.PreShipmentItemMobile.Remove(preshipmentitmmobile);
                }
                var pickuprequests = _uow.MobilePickUpRequests.GetAsync(s => s.Waybill == preshipmentmobile.Waybill).Result;
                if (pickuprequests != null)
                {
                    pickuprequests.Status = "Cancelled";
                    updatedwallet.Balance = ((updatedwallet.Balance + (decimal)preshipmentmobile.CalculatedTotal) - Convert.ToDecimal(pickuprice.Value));
                    preshipmentmobile.shipmentstatus = "Cancelled";
                    await _uow.CompleteAsync();
                }
                else
                {
                    preshipmentmobile.shipmentstatus = "Cancelled";
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

    }
}