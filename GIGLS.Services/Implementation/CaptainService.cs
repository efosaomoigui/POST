﻿using GIGLS.Core;
using GIGLS.Core.DTO.Captains;
using GIGLS.Core.IServices.User;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using System;
using GIGLS.Core.IServices.Utility;
using GIGLS.Services.Implementation.Messaging;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using GIGLS.Core.DTO;
using GIGLS.CORE.DTO.Report;
using System.Linq;
using AutoMapper;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.IMessage;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Core.IServices.MessagingLog;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Fleets;
using System.Data.Entity;
using GIGLS.Core.IServices.Fleets;

namespace GIGLS.Services.Implementation
{
    public class CaptainService : ICaptainService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly MessageSenderService _messageSenderService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPartnerService _partnerService;
        private readonly IFleetPartnerRepository _fleetPartnerRepository;
        private readonly IEmailService _emailService;
        private readonly IEmailSendLogService _iEmailSendLogService;
        private readonly IFleetTripService _fleetTripService;

        public CaptainService(IUserService userService, IUnitOfWork uow, IPasswordGenerator passwordGenerator, MessageSenderService messageSenderService, INumberGeneratorMonitorService numberGeneratorMonitorService, IPartnerService partnerService, IFleetTripService fleetTripService)
        {
            _userService = userService;
            _uow = uow;
            _passwordGenerator = passwordGenerator;
            _messageSenderService = messageSenderService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _partnerService = partnerService;
            _fleetTripService = fleetTripService;
        }


        public async Task<object> RegisterCaptainAsync(RegCaptainDTO captainDTO)
        {
            var currentUserRole = await GetCurrentUserRoleAsync();

            if (currentUserRole == "Administrator" || currentUserRole == "Admin" || currentUserRole == "CaptainManagement" || currentUserRole == "FleetCoordinator")
            {
                var confirmUser = await _uow.User.GetUserByEmail(captainDTO.Email);
                var captain = await _uow.Partner.GetPartnerByEmail(captainDTO.Email);

                // confirm if captain exist in partner table
                if (captain.Any())
                {
                    throw new GenericException($"Captain with email {captainDTO.Email} already exist");
                }

                var systemUsers = await _userService.GetSystemUsers();
                var systemUser = systemUsers.FirstOrDefault(x => x.FirstName.Trim() == "Captain");
                
                if (systemUser == null)
                {
                    throw new GenericException("Unable to add to role at the moment, pls contact the admin");
                }

                string password = await _passwordGenerator.Generate();
                
                var user = new GIGL.GIGLS.Core.Domain.User
                {
                    Organisation = captainDTO.Organisation,
                    Status = (int)UserStatus.Active,
                    DateCreated = DateTime.Now.Date,
                    DateModified = DateTime.Now.Date,
                    Department = captainDTO.Department,
                    Designation = captainDTO.Designation,
                    Email = captainDTO.Email,
                    FirstName = captainDTO.FirstName,
                    LastName = captainDTO.LastName,
                    Gender = captainDTO.Gender,
                    UserName = captainDTO.Email,
                    PhoneNumber = captainDTO.PhoneNumber,
                    UserType = captainDTO.UserType,
                    IsActive = true,
                    PictureUrl = captainDTO.PictureUrl,
                    SystemUserRole = systemUser.FirstName,
                    SystemUserId = systemUser.Id,
                    PasswordExpireDate = DateTime.Now
                };
                user.Id = Guid.NewGuid().ToString();                

                // partner
                var partnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Partner);
                var partner = new Partner()
                {
                    Address = captainDTO.Address,
                    CaptainAccountName = captainDTO.AccountName,
                    CaptainAccountNumber = captainDTO.AccountNumber,
                    CaptainBankName = captainDTO.BankName,
                    DateCreated = DateTime.Now.Date,
                    Email = captainDTO.Email,
                    PictureUrl = captainDTO?.PictureUrl,
                    DateModified = DateTime.Now.Date,
                    FirstName = captainDTO.FirstName,
                    LastName = captainDTO.LastName,
                    PhoneNumber = captainDTO.PhoneNumber,
                    PartnerType = PartnerType.Captain,
                    UserId = user.Id,
                    IsDeleted = false,
                    IsActivated = true,
                    PartnerCode = partnerCode,
                    ActivityDate = DateTime.Now.Date,
                    Age = captainDTO.Age,
                    PartnerName = captainDTO.FirstName + " " + captainDTO.LastName,
                };

                // confirm if user does not exist in user table
                if (confirmUser == null)
                {
                    var result = await _uow.User.RegisterUser(user, password);
                    if (!result.Succeeded)
                    {
                        var errors = "";
                        foreach (var error in result.Errors)
                        {
                            errors += error + "\n";
                        }
                        throw new GenericException($"{errors}");
                    }
                }

                _uow.Partner.Add(partner);
                await _uow.CompleteAsync();

                // send mail
                var partnerDto = new PartnerDTO()
                {
                    Email = user.Email,
                    Password = password,
                    PartnerCode = partnerCode,
                };
                await _messageSenderService.SendGenericEmailMessage(MessageType.CAPTEMAIL, partnerDto);

                return new { id = user.Id, password = password, email = user.Email };
            } 
            
            throw new GenericException("You are not authorized to use this feature");
        }

        public async Task<IReadOnlyList<ViewCaptainsDTO>> GetCaptainsByDateAsync(DateTime? date)
        {
            DateFilterCriteria dateFilterCriteria = new DateFilterCriteria();
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var captains = await _uow.CaptainRepository.GetAllCaptainsByDateAsync(date);
                    return captains;
                }

                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> GetCaptainByIdAsync(int partnerId)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var captain = await _uow.CaptainRepository.GetCaptainByIdAsync(partnerId);
                    var user = await _userService.GetUserById(captain.UserId);

                    var captainDetails = new
                    {
                        PartnerId = partnerId,
                        Status = user.Status == 1 ? "Active" : "Inactive",
                        EmploymentDate = captain.DateCreated,
                        AssignedVehicleType = captain.VehicleType,
                        AssignedVehicleNumber = captain.VehicleLicenseNumber,
                        CaptainAge = captain.Age,
                        CaptainCode = captain.PartnerCode,
                        CaptainName = captain.FirstName + " " + captain.LastName,
                        FirstName = captain.FirstName,
                        LastName = captain.LastName,
                        CaptainPhoneNumber = captain.PhoneNumber,
                        Email = captain.Email,
                        PictureUrl = captain.PictureUrl,
                        Organization = user.Organisation,
                        Department = user.Department,
                        Designation = user.Designation,
                        Address = captain.Address,
                        CaptainBankName = captain.CaptainBankName,
                        CaptainAccountNumber = captain.CaptainAccountNumber,
                        CaptainAccountName = captain.CaptainAccountName,
                    };
                    return captainDetails;
                }
                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCaptainByIdAsync(int captainId)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var captain = await _uow.CaptainRepository.GetCaptainByIdAsync(captainId);
                    if (captain == null)
                    {
                        throw new GenericException($"Captain with Id {captain.PartnerId} and name {captain.FirstName} {captain.LastName} does not exist");
                    }

                    _uow.CaptainRepository.Remove(captain);
                    captain.IsActivated = false;
                    await _uow.CompleteAsync();
                }
                else
                {
                    throw new GenericException("You are not authorized to use this feature");
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EditCaptainAsync(UpdateCaptainDTO partner)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var captain = await _uow.CaptainRepository.GetCaptainByIdAsync(partner.PartnerId);
                    if (captain == null)
                    {
                        throw new GenericException($"Captain with Id {captain.PartnerId} does not exist");
                    }

                    var user = await _userService.GetUserById(captain.UserId);
                    if (user == null) 
                    { 
                        throw new GenericException($"Captain's user information not exist"); 
                    }
                    // update user
                    user.LastName = partner.LastName;
                    user.FirstName = partner.FirstName;
                    user.Email = partner.Email;
                    user.PhoneNumber = partner.CaptainPhoneNumber;
                    user.Username = partner.Email;
                    user.Status = partner.Status.ToLower() == "active" ? 1 : 0;

                    await _userService.UpdateUser(user.Id, user);

                    // update captain
                    captain.PartnerName = partner.FirstName + " " + partner.LastName;
                    captain.FirstName = partner.FirstName;
                    captain.LastName = partner.LastName;
                    captain.Email = partner.Email;
                    captain.PhoneNumber = partner.CaptainPhoneNumber;
                    captain.DateModified = DateTime.Now;
                    captain.Age = partner.CaptainAge;
                    captain.VehicleType = partner.AssignedVehicleType;
                    captain.VehicleLicenseNumber = partner.AssignedVehicleNumber;
                    captain.PictureUrl = partner.PictureUrl;

                    await _uow.CompleteAsync();
                }
                else
                {
                    throw new GenericException("You are not authorized to use this feature");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RegisterVehicleAsync(RegisterVehicleDTO vehicleDTO)
        {
            var currentUserRole = await GetCurrentUserRoleAsync();
            if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
            {
                if (await _uow.Fleet.ExistAsync(c => c.RegistrationNumber.ToLower() == vehicleDTO.RegistrationNumber.Trim().ToLower()))
                {
                    throw new GenericException($"Fleet/Vehicle with Registration Number: {vehicleDTO.RegistrationNumber} already exist!");
                }

                var fleetModel = await _uow.FleetModel.FindAsync(x => x.ModelName == vehicleDTO.VehicleType.ToUpper() || x.ModelName == vehicleDTO.VehicleName.ToUpper());
                if(!fleetModel.Any())
                {
                    throw new GenericException($"The chosen Vehicle Model for Vehicle type: {vehicleDTO.VehicleType} does not exist!");
                }

                var partner = await _uow.Partner.GetPartnerByEmail(vehicleDTO.PartnerEmail);

                FleetType outType;
                if(!(Enum.TryParse(vehicleDTO.VehicleType.Replace(" ",""), out outType)))
                {
                    throw new GenericException($"The chosen vehicle type: {vehicleDTO.VehicleType} not yet available!");
                }
                
                FleetType fleetType = (FleetType)Enum.Parse(typeof(FleetType), vehicleDTO.VehicleType.Replace(" ",""));
                if(fleetType == null)
                {
                    throw new GenericException($"The chosen vehicle type: {vehicleDTO.VehicleType} does not exist!");
                }

                VehicleFixedStatus isFixed = (VehicleFixedStatus)Enum.Parse(typeof(VehicleFixedStatus), vehicleDTO.IsFixed);

                Fleet newFleet = new Fleet()
                {
                    RegistrationNumber = vehicleDTO.RegistrationNumber,
                    Capacity = vehicleDTO.VehicleCapacity,
                    DateCreated = vehicleDTO.DateOfCommission,
                    DateModified = DateTime.Now,
                    IsDeleted = false,
                    Status = vehicleDTO.Status == "Active" ? true : false,
                    Partner = partner[0],
                    PartnerId = partner[0].PartnerId,
                    FleetType = fleetType,
                    FleetName = vehicleDTO.VehicleName,
                    ModelId = fleetModel.FirstOrDefault().MakeId,
                    FleetModel = fleetModel.FirstOrDefault(),
                    EnterprisePartnerId = vehicleDTO.VehicleOwner,
                    IsFixed = isFixed,
                };

                _uow.Fleet.Add(newFleet);

                partner[0].VehicleType = newFleet.FleetType.ToString();
                partner[0].VehicleLicenseNumber = newFleet.RegistrationNumber;

                await _uow.CompleteAsync();
                return true;
            }
            else
            {
                throw new GenericException("You are not authorized to use this feature");
            }
        }

        public async Task<IReadOnlyList<CaptainDetailsDTO>> GetAllCaptainsAsync()
        {
            var currentUserRole = await GetCurrentUserRoleAsync();
            if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
            {
                var captains = await _uow.CaptainRepository.GetAllCaptainsAsync();
                //var captainUserInfo = await _userService.GetUserByEmail(ca)
                var captainsDto = captains.Select(x => new CaptainDetailsDTO
                {
                    Status = "Active",
                    CaptainAge = x.Age,
                    CaptainCode = x.PartnerCode,
                    CaptainName = $"{x.FirstName} {x.LastName}",
                    CaptainLastName = x.LastName,
                    CaptainFirstName = x.FirstName,
                    CaptainPhoneNumber = x.PhoneNumber,
                    AssignedVehicleName = null,
                    AssignedVehicleNumber = x.VehicleLicenseNumber,
                    Email = x.Email,
                    EmploymentDate = x.DateCreated,
                    PartnerId = x.PartnerId,
                    PictureUrl = x.PictureUrl
                }).ToList();
                return captainsDto;
            }
            else
            {
                throw new GenericException("You are not authorized to use this feature");
            }
        }

        public async Task<IReadOnlyList<VehicleDTO>> GetVehiclesByDateAsync(DateTime? date)
        {
            DateFilterCriteria dateFilterCriteria = new DateFilterCriteria();
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var vehicles = await _uow.CaptainRepository.GetAllVehiclesByDateAsync(date);
                    return vehicles;
                }

                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> GetVehicleByIdAsync(int fleetId)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var vehicle = await _uow.Fleet.GetAsync(fleetId);
                    var owner = await _userService.GetUserById(vehicle.EnterprisePartnerId);
                    var partner = await _uow.Partner.GetAsync(vehicle.PartnerId);

                    var vehicleDetails = new VehicleDetailsDTO
                    {
                        FleetId = vehicle.FleetId,
                        Status = vehicle.Status == true ? "Active" : "Inactive",
                        FleetName = vehicle.FleetName,
                        AssignedCaptain = $"{partner.FirstName} {partner.LastName}",
                        RegistrationNumber = vehicle.RegistrationNumber,
                        VehicleAge = (int)(DateTime.Now - vehicle.DateCreated).TotalDays,
                        VehicleOwner = $"{owner.FirstName} {owner.LastName}",
                        VehicleType = vehicle.FleetType.ToString(),
                        Capacity = vehicle.Capacity,
                        PartnerId = vehicle.PartnerId,
                        VehicleOwnerId = owner.Id,
                        IsFixed = vehicle.IsFixed.ToString(),
                    };
                    return vehicleDetails;
                }
                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteVehicleByIdAsync(int fleetId)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var fleet = await _uow.Fleet.GetAsync(fleetId);
                    if (fleet == null)
                    {
                        throw new GenericException($"Vehicle with Id {fleetId} does not exist");
                    }

                    _uow.Fleet.Remove(fleet);
                    fleet.Status = false;
                    await _uow.CompleteAsync();

                    return true;
                }
                else
                {
                    throw new GenericException("You are not authorized to use this feature");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditVehicleAsync(VehicleDetailsDTO vehicle)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var fleet = await _uow.Fleet.GetAsync(vehicle.FleetId);
                    if (fleet == null)
                    {
                        throw new GenericException($"Vehicle with Id {vehicle.FleetId} does not exist");
                    }

                    FleetType fleetType = (FleetType)Enum.Parse(typeof(FleetType), vehicle.VehicleType);
                    VehicleFixedStatus isFixed = (VehicleFixedStatus)Enum.Parse(typeof(VehicleFixedStatus), vehicle.IsFixed);

                    var partner = await _uow.Partner.GetAsync(vehicle.PartnerId);
                    var today = DateTime.Now;

                    // update vehicle
                    fleet.Status = vehicle.Status == "Active" ? true : false; 
                    fleet.Capacity = vehicle.Capacity;
                    fleet.FleetName = vehicle.FleetName;
                    fleet.RegistrationNumber = vehicle.RegistrationNumber;
                    fleet.DateModified = DateTime.Now;
                    fleet.DateCreated = today.AddDays(-1 * vehicle.VehicleAge);
                    fleet.Partner = partner;
                    fleet.PartnerId = partner.PartnerId;
                    fleet.EnterprisePartnerId = vehicle.VehicleOwnerId;
                    fleet.FleetType = fleetType;
                    fleet.IsFixed = isFixed;

                    await _uow.CompleteAsync();
                    return true;
                }
                else
                {
                    throw new GenericException("You are not authorized to use this feature");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<VehicleDetailsDTO>> GetAllVehiclesAsync()
        {
            var currentUserRole = await GetCurrentUserRoleAsync();
            if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
            {
                var vehicles = _uow.Fleet.GetAll("Partner");
                var vehicledetails = vehicles.ToList();

                var vehiclesDto = vehicles.Select(x => new VehicleDetailsDTO
                {
                    Status = "Active",
                    VehicleAge = (int)(DateTime.Now - x.DateCreated).TotalDays,
                    FleetId = x.FleetId,
                    FleetName = x.FleetName,
                    PartnerId = x.PartnerId,
                    AssignedCaptain = x.Partner.PartnerName,
                    RegistrationNumber = x.RegistrationNumber,
                    VehicleType = x.FleetType.ToString(),
                    VehicleOwnerId = x.EnterprisePartnerId,
                    //VehicleOwner = await _userService.GetUserById(x.FleetOwner).Result.FirstName + " " + _userService.GetUserById(x.FleetOwner).Result.LastName,
                    Capacity = x.Capacity,
                    IsFixed = x.IsFixed.ToString()
                }).ToList();
                return vehiclesDto;
            }
            else
            {
                throw new GenericException("You are not authorized to use this feature");
            }
        }

        public async Task<VehicleDetailsDTO> GetVehicleByRegistrationNumberAsync(string regNum)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var vehicle = await _uow.CaptainRepository.GetVehicleByRegistrationNumberAsync(regNum);
                    return vehicle;
                }
                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<VehicleAnalyticsDto> GetVehicleAnalyticsAsync(string vehicleNumber)
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();                

                if (currentUserRole == "CaptainManagement" || currentUserRole == "Admin" || currentUserRole == "Administrator" || currentUserRole == "FleetCoordinator")
                {
                    var vehicle = await _uow.CaptainRepository.GetVehicleByRegistrationNumberAsync(vehicleNumber);
                    var fleetTrip = await _uow.FleetTrip.FindAsync(x => x.FleetRegistrationNumber == vehicleNumber);

                    if (vehicle != null)
                    {
                        var formatter = new System.Globalization.CultureInfo("HA-LATN-NG");
                        formatter.NumberFormat.CurrencySymbol = "₦";

                        return new VehicleAnalyticsDto
                        {
                            VehicleAge = vehicle.VehicleAge,
                            TotalExpenses = fleetTrip.Sum(x => x.FuelCosts + x.DispatchAmount).ToString("c", formatter),
                            VehicleAssignedCaptain = vehicle.AssignedCaptain,
                            VehicleCurrentLocation = "",
                            TotalNumberOfTrip = fleetTrip.Count(),
                            TotalRevenueGenerated = fleetTrip.Sum(x => x.TripAmount).ToString("c", formatter),
                        };
                    }
                    throw new GenericException($"Vehicle with registration number: {vehicleNumber} does not exist");
                    
                }
                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<string> GetCurrentUserRoleAsync()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            return currentUser.SystemUserRole;
        }
    }
}