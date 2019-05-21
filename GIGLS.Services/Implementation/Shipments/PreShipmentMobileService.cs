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
             
        public PreShipmentMobileService(IUnitOfWork uow,IShipmentService shipmentService,IDeliveryOptionService deliveryService,
            IServiceCentreService centreService,IUserServiceCentreMappingService userServiceCentre,INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPricingService pricingService,IWalletService walletService,IWalletTransactionService walletTransactionService,
            IUserService userService, ISpecialDomesticPackageService specialdomesticpackageservice, IMobileShipmentTrackingService mobiletrackingservice,
            IMobilePickUpRequestsService mobilepickuprequestservice)
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
            MapperConfig.Initialize();
        }

        public async Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment)
        {
            try
            {
                var newPreShipment = await CreatePreShipment(preShipment);
                await _uow.CompleteAsync();
                bool IsBalanceSufficient = true;
                string message = "Shipment created successfully";               

                if (newPreShipment.IsBalanceSufficient == false)
                {
                    message = "Insufficient Wallet Balance";
                    IsBalanceSufficient = false;
                }

                return new { waybill = newPreShipment.Waybill, message = message, IsBalanceSufficient};
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
                    SpecialPackageId = preShipmentItem.SpecialPackageId,
                    ShipmentType = preShipmentItem.ShipmentType
                };
              
                if (preShipmentItem.ShipmentType ==ShipmentType.Ecommerce)
                {
                    preShipmentItem.CalculatedPrice = await _pricingService.GetMobileEcommercePrice(PriceDTO);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                }
                if(preShipmentItem.ShipmentType == ShipmentType.Special)
                {
                    preShipmentItem.CalculatedPrice = await _pricingService.GetMobileSpecialPrice(PriceDTO);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                }
                else if(preShipmentItem.ShipmentType ==ShipmentType.Regular)
                {
                    preShipmentItem.CalculatedPrice = await _pricingService.GetMobileRegularPrice(PriceDTO);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                }
                if (!string.IsNullOrEmpty(preShipmentItem.Value))
                {
                    DeclaredValue += Convert.ToDecimal(preShipmentItem.Value);
                    preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + Convert.ToDecimal(preShipmentItem.Value);
                    preShipment.IsdeclaredVal = true;
                }
                Price += (decimal)preShipmentItem.CalculatedPrice;
            };
            int EstimatedDeclaredPrice = Convert.ToInt32(DeclaredValue);
            preShipment.DeliveryPrice = Price;
            preShipment.Vat = (decimal)(Convert.ToInt32(preShipment.DeliveryPrice) * 0.05);
            preShipment.InsuranceValue = (decimal)(EstimatedDeclaredPrice * 0.01);
            preShipment.CalculatedTotal = (double)(preShipment.DeliveryPrice + preShipment.Vat + preShipment.InsuranceValue);
            preShipment.CalculatedTotal = Math.Round((double)preShipment.CalculatedTotal / 100d, 0) * 100;
            preShipment.Value = DeclaredValue;
            var returnprice = new MobilePriceDTO()
            {
                PreshipmentMobile = preShipment,
                DeliveryPrice = preShipment.DeliveryPrice,
                Vat= preShipment.Vat,
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


        public async Task<List<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages()
        {
            var specialpackages = await _specialdomesticpackageservice.GetSpecialDomesticPackages();
            var special = specialpackages.ToList();
            special.Add(new SpecialDomesticPackageDTO
            {
                Name = "Other",
                Status = true,
                SpecialDomesticPackageType = SpecialDomesticPackageType.Special
            });
            return special;
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
                },  scan.ShipmentScanStatus);
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
               var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill, "PreShipmentItems");
               if (preshipmentmobile != null)
               {
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
                else {
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
                if (pickuprequest.Status == MobilePickUpRequestStatus.Confirmed.ToString())
                {
                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MSHC
                    });
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
                    preshipmentmobile.shipmentstatus = "Picked up";
                    await _uow.CompleteAsync();

                }
                if(pickuprequest.Status == MobilePickUpRequestStatus.Delivered.ToString())
                {
                   await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = pickuprequest.Waybill,
                        ShipmentScanStatus = ShipmentScanStatus.AHD
                    });
                    var preshipmentmobile = await _uow.PreShipmentMobile.GetAsync(s => s.Waybill == pickuprequest.Waybill);
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
                return true;

            }
            catch
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
    }
}