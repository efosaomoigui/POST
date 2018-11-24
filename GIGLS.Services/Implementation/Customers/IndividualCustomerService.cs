using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using System.Collections.Generic;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Infrastructure;
using AutoMapper;
using System.Linq;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Customers
{
    public class IndividualCustomerService : IIndividualCustomerService
    {
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IUserService _userService;

        public IndividualCustomerService(INumberGeneratorMonitorService numberGeneratorMonitorService,
            IWalletService walletService, IPasswordGenerator passwordGenerator, IUserService userService, IUnitOfWork uow)
        {
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _passwordGenerator = passwordGenerator;
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IndividualCustomerDTO> AddCustomer(IndividualCustomerDTO customer)
        {
            try
            {
                if (await _uow.IndividualCustomer.ExistAsync(c => c.PhoneNumber == customer.PhoneNumber.Trim()))
                {
                    throw new GenericException($"Individual Customer Phone Number {customer.PhoneNumber } Already Exist");
                }

                //if (await _uow.IndividualCustomer.ExistAsync(c => c.Email == customer.Email.Trim()))
                //{
                //    throw new GenericException($"Individual Customer Email {customer.Email } Already Exist");
                //}

                var newCustomer = Mapper.Map<IndividualCustomer>(customer);

                //generate customer code
                var customerCode = await _numberGeneratorMonitorService.GenerateNextNumber(
                    NumberGeneratorType.CustomerCodeIndividual);
                newCustomer.CustomerCode = customerCode;

                _uow.IndividualCustomer.Add(newCustomer);

                await _uow.CompleteAsync();

                // add customer to a wallet
                await _walletService.AddWallet(new WalletDTO
                {
                    CustomerId = newCustomer.IndividualCustomerId,
                    CustomerType = CustomerType.IndividualCustomer,
                    CustomerCode = newCustomer.CustomerCode,
                    CompanyType = CustomerType.IndividualCustomer.ToString()
                });

                // add to user table for login
                //1. If userEmail is null, use CustomerCode
                if(String.IsNullOrEmpty(newCustomer.Email))
                {
                    newCustomer.Email = newCustomer.CustomerCode;
                }
                try
                {
                    var password = await _passwordGenerator.Generate();
                    var result = await _userService.AddUser(new Core.DTO.User.UserDTO()
                    {
                        ConfirmPassword = password,
                        Department = CustomerType.IndividualCustomer.ToString(),
                        DateCreated = DateTime.Now,
                        Designation = CustomerType.IndividualCustomer.ToString(),
                        Email = newCustomer.Email,
                        FirstName = newCustomer.FirstName,
                        LastName = newCustomer.LastName,
                        Organisation = CustomerType.IndividualCustomer.ToString(),
                        Password = password,
                        PhoneNumber = newCustomer.PhoneNumber,
                        UserType = UserType.Regular,
                        Username = newCustomer.CustomerCode,
                        UserChannelCode = newCustomer.CustomerCode,
                        UserChannelPassword = password,
                        UserChannelType = UserChannelType.IndividualCustomer,
                        PasswordExpireDate = DateTime.Now
                    });
                }
                catch (Exception)
                {
                    // do nothing
                }

                return Mapper.Map<IndividualCustomerDTO>(newCustomer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCustomer(int customerId)
        {
            try
            {
                var customer = await _uow.IndividualCustomer.GetAsync(customerId);
                if (customer == null)
                {
                    throw new GenericException("Individual Customer Inforamtion does not exist");
                }
                _uow.IndividualCustomer.Remove(customer);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IndividualCustomerDTO> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _uow.IndividualCustomer.GetAsync(customerId);
                if (customer == null)
                {
                    throw new GenericException("Individual Customer information does not exist");
                }

                IndividualCustomerDTO individual = Mapper.Map<IndividualCustomerDTO>(customer);
                return individual;
            }
            catch (Exception)
            {
                throw;
            }
        }
              
        public async Task<IndividualCustomerDTO> GetCustomerByPhoneNumber(string phoneNumber)
        {
            try
            {
                var customer = await _uow.IndividualCustomer.GetAsync(x => x.PhoneNumber.Contains(phoneNumber));

                //if (customer == null)
                //{
                //    throw new GenericException("Individual Customer information does not exist");
                //}

                IndividualCustomerDTO individual = Mapper.Map<IndividualCustomerDTO>(customer);
                return individual;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<IndividualCustomerDTO>> GetIndividualCustomers()
        {
            try
            {
                List<IndividualCustomerDTO> customers = _uow.IndividualCustomer.GetAll().Select(s => new IndividualCustomerDTO
                {
                    IndividualCustomerId = s.IndividualCustomerId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Address = s.Address,
                    City = s.City,
                    Gender = s.Gender,
                    PictureUrl = s.PictureUrl,
                    PhoneNumber = s.PhoneNumber,
                    State = s.State,
                    DateModified = s.DateModified,
                    DateCreated = s.DateCreated,
                    CustomerCode = s.CustomerCode
                }).ToList();

                return await Task.FromResult(customers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCustomer(int customerId, IndividualCustomerDTO customerDto)
        {
            var customer = await _uow.IndividualCustomer.GetAsync(customerId);
            if (customer == null || customerDto.IndividualCustomerId != customerId)
            {
                throw new GenericException("Individual Customer information does not exist");
            }
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.Address = customerDto.Address;
            customer.City = customerDto.City;
            customer.Gender = customerDto.Gender;
            customer.PictureUrl = customerDto.PictureUrl;
            //work on the picture later
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.State = customerDto.State;
            _uow.Complete();
        }

        public async Task<List<IndividualCustomerDTO>> GetIndividualCustomers(string searchData)
        {
            return await _uow.IndividualCustomer.GetIndividualCustomers(searchData);
        }

        public async Task<IndividualCustomerDTO> GetCustomerByCode(string customerCode)
        {
            try
            {
                var customer = await _uow.IndividualCustomer.GetAsync(x => x.CustomerCode.ToLower() == customerCode.ToLower());
                if (customer == null)
                {
                    return new IndividualCustomerDTO { };
                }
                IndividualCustomerDTO individual = Mapper.Map<IndividualCustomerDTO>(customer);
                return individual;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
