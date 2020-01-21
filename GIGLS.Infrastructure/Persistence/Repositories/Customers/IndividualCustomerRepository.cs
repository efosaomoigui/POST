using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IRepositories.Customers;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using GIGLS.Core.DTO;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Customers
{
    public class IndividualCustomerRepository : Repository<IndividualCustomer, GIGLSContext>, IIndividualCustomerRepository
    {
        private GIGLSContext _context;

        public IndividualCustomerRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<IndividualCustomerDTO>> GetIndividualCustomers()
        {
            try
            {
                var customers = _context.IndividualCustomer;

                var customerdto = from s in customers
                                  select new IndividualCustomerDTO
                                  {
                                      FirstName = s.FirstName,
                                      LastName = s.LastName,
                                      Email = s.Email,
                                      Address = s.Address,
                                      City = s.City,
                                      Gender = s.Gender,
                                      PictureUrl = s.PictureUrl,
                                      PhoneNumber = s.PhoneNumber,
                                      State = s.State,
                                      DateCreated = s.DateCreated,
                                      DateModified = s.DateModified,
                                      CustomerCode = s.CustomerCode,
                                      UserActiveCountryId = s.UserActiveCountryId
                                      //select all shipments of the customer                              
                                  };
                return Task.FromResult(customerdto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<IndividualCustomerDTO>> GetIndividualCustomers(string searchData)
        {
            try
            {
                var customers = _context.IndividualCustomer.Where(x => x.PhoneNumber.Contains(searchData) || 
                x.FirstName.Contains(searchData) || x.LastName.Contains(searchData)
                || x.CustomerCode.Contains(searchData) || x.Email.Contains(searchData));
                var individualDto = from s in customers
                                   select new IndividualCustomerDTO
                                   {
                                       FirstName = s.FirstName,
                                       LastName = s.LastName,
                                       Email = s.Email,
                                       Address = s.Address,
                                       City = s.City,
                                       Gender = s.Gender,
                                       PictureUrl = s.PictureUrl,
                                       PhoneNumber = s.PhoneNumber,
                                       State = s.State,
                                       DateCreated = s.DateCreated,
                                       DateModified = s.DateModified,
                                       CustomerCode = s.CustomerCode,
                                       UserActiveCountryId = s.UserActiveCountryId,
                                       Country = _context.Country.Where(x => x.CountryId == s.UserActiveCountryId).Select(x => new CountryDTO
                                       {
                                           CountryId = x.CountryId,
                                           CountryName = x.CountryName,
                                           CurrencySymbol = x.CurrencySymbol,
                                           CurrencyCode = x.CurrencyCode,
                                           PhoneNumberCode = x.PhoneNumberCode
                                       }).FirstOrDefault()
                                   };
                return Task.FromResult(individualDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IndividualCustomerDTO> GetIndividualCustomerByIdWithCountry(int customerId)
        {
            try
            {
                var individuals = _context.IndividualCustomer.Where(x => x.IndividualCustomerId == customerId);
                var individualDto = from s in individuals
                                   select new IndividualCustomerDTO
                                   {
                                       FirstName = s.FirstName,
                                       LastName = s.LastName,
                                       Email = s.Email,
                                       Address = s.Address,
                                       City = s.City,
                                       Gender = s.Gender,
                                       PictureUrl = s.PictureUrl,
                                       PhoneNumber = s.PhoneNumber,
                                       State = s.State,
                                       DateCreated = s.DateCreated,
                                       DateModified = s.DateModified,
                                       CustomerCode = s.CustomerCode,
                                       UserActiveCountryId = s.UserActiveCountryId,
                                       Country = _context.Country.Where(x => x.CountryId == s.UserActiveCountryId).Select(x => new CountryDTO
                                       {
                                           CountryId = x.CountryId,
                                           CountryName = x.CountryName,
                                           CurrencySymbol = x.CurrencySymbol,
                                           CurrencyCode = x.CurrencyCode,
                                           PhoneNumberCode = x.PhoneNumberCode
                                       }).FirstOrDefault()
                                   };
                return Task.FromResult(individualDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
