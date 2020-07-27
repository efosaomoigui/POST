using System.Collections.Generic;
using System.Linq;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.IRepositories.Customers;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Core.DTO.Customers;
using GIGL.GIGLS.Core.Domain;
using System.Threading.Tasks;
using System;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Customers
{
    public class CompanyRepository : Repository<Company, GIGLSContext>, ICompanyRepository
    {
        private GIGLSContext _context; 

        public CompanyRepository(GIGLSContext context): base(context)
        {
            _context = context;
        }

        public Task<List<CompanyDTO>> GetCompanies()
        {
            try
            {
                var companies = _context.Company;
                var companiesDto = from c in companies
                                   join w in _context.Wallets on c.CustomerCode equals w.CustomerCode
                                   join co in _context.Country on c.UserActiveCountryId equals co.CountryId
                                   select new CompanyDTO
                                   {
                                       CompanyId = c.CompanyId,
                                       Name = c.Name,
                                       RcNumber = c.RcNumber,
                                       Email = c.Email,
                                       City = c.City,
                                       State = c.State,
                                       Address = c.Address,
                                       isCodNeeded = c.isCodNeeded,
                                       PhoneNumber = c.PhoneNumber,
                                       Industry = c.Industry,
                                       CompanyType = c.CompanyType,
                                       CompanyStatus = c.CompanyStatus,
                                       Discount = c.Discount,
                                       SettlementPeriod = c.SettlementPeriod,
                                       CustomerCode = c.CustomerCode,
                                       CustomerCategory = c.CustomerCategory,
                                       ReturnOption = c.ReturnOption,
                                       ReturnServiceCentre = c.ReturnServiceCentre,
                                       ReturnServiceCentreName = Context.ServiceCentre.Where(x => x.ServiceCentreId == c.ReturnServiceCentre).Select(x => x.Name).FirstOrDefault(),
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       WalletBalance = w.Balance,
                                       UserActiveCountryId = c.UserActiveCountryId,
                                       Country = new CountryDTO
                                       {
                                           CountryId = co.CountryId,
                                           CountryName = co.CountryName,
                                           CurrencySymbol = co.CurrencySymbol,
                                           CurrencyCode = co.CurrencyCode,
                                           PhoneNumberCode = co.PhoneNumberCode
                                       },
                                       UserActiveCountryName = co.CountryName
                                   };
                return Task.FromResult(companiesDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanyDTO>> GetCompanies(BaseFilterCriteria filterCriteria)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-30);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                var companies = Context.Company.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);
                var companiesDto = from c in companies
                                   join w in _context.Wallets on c.CustomerCode equals w.CustomerCode
                                   join co in _context.Country on c.UserActiveCountryId equals co.CountryId
                                   select new CompanyDTO
                                   {
                                       CompanyId = c.CompanyId,
                                       Name = c.Name,
                                       RcNumber = c.RcNumber,
                                       Email = c.Email,
                                       City = c.City,
                                       State = c.State,
                                       Address = c.Address,
                                       isCodNeeded = c.isCodNeeded,
                                       PhoneNumber = c.PhoneNumber,
                                       Industry = c.Industry,
                                       CompanyType = c.CompanyType,
                                       CompanyStatus = c.CompanyStatus,
                                       Discount = c.Discount,
                                       SettlementPeriod = c.SettlementPeriod,
                                       CustomerCode = c.CustomerCode,
                                       CustomerCategory = c.CustomerCategory,
                                       ReturnOption = c.ReturnOption,
                                       ReturnServiceCentre = c.ReturnServiceCentre,
                                       ReturnServiceCentreName = Context.ServiceCentre.Where(x => x.ServiceCentreId == c.ReturnServiceCentre).Select(x => x.Name).FirstOrDefault(),
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       WalletBalance = w.Balance,
                                       UserActiveCountryId = c.UserActiveCountryId,
                                       Country = new CountryDTO
                                       {
                                           CountryId = co.CountryId,
                                           CountryName = co.CountryName,
                                           CurrencySymbol = co.CurrencySymbol,
                                           CurrencyCode = co.CurrencyCode,
                                           PhoneNumberCode = co.PhoneNumberCode
                                       },
                                       UserActiveCountryName = co.CountryName
                                   };
                return Task.FromResult(companiesDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EcommerceAgreementDTO>> GetPendingEcommerceRequest(BaseFilterCriteria filterCriteria)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-30);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                var companies = _context.EcommerceAgreement.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.Status == EcommerceAgreementStatus.Pending);
                var companiesDto = from c in companies
                                   select new EcommerceAgreementDTO
                                   {
                                       EcommerceAgreementId = c.EcommerceAgreementId,
                                       BusinessEmail = c.BusinessEmail,
                                       BusinessOwnerName = c.BusinessOwnerName,
                                       OfficeAddress = c.OfficeAddress,
                                       ContactName = c.ContactName,
                                       ContactEmail = c.ContactEmail,
                                       ContactPhoneNumber = c.ContactPhoneNumber,
                                       AgreementDate = c.AgreementDate,
                                       Status = c.Status,
                                       City = c.City,
                                       State = c.State,
                                       CountryId = c.CountryId,
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                   };
                return await Task.FromResult(companiesDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanyDTO>> GetCompanies(CompanyType companyType,  CustomerSearchOption searchOption)
        {
            try
            {
                var companies = Context.Company.Where(x => x.CompanyType == companyType && 
                (   x.Name.Contains(searchOption.SearchData) || x.PhoneNumber.Contains(searchOption.SearchData) || x.Email.Contains(searchOption.SearchData))).ToList();
                var companiesDto = Mapper.Map<List<CompanyDTO>>(companies);
                return Task.FromResult(companiesDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public Task<CompanyDTO> GetCompanyById(int companyId)
        {
            try
            {
                var companies = _context.Company.Where(x => x.CompanyId == companyId);
                var companiesDto = from c in companies
                                   select new CompanyDTO
                                   {
                                       CompanyId = c.CompanyId,
                                       Name = c.Name,
                                       RcNumber = c.RcNumber,
                                       Email = c.Email,
                                       City = c.City,
                                       State = c.State,
                                       Address = c.Address,
                                       PhoneNumber = c.PhoneNumber,
                                       Industry = c.Industry,
                                       CompanyType = c.CompanyType,
                                       CompanyStatus = c.CompanyStatus,
                                       Discount = c.Discount,
                                       SettlementPeriod = c.SettlementPeriod,
                                       CustomerCode = c.CustomerCode,
                                       CustomerCategory = c.CustomerCategory,
                                       ReturnOption = c.ReturnOption,
                                       ReturnServiceCentre = c.ReturnServiceCentre,
                                       ReturnServiceCentreName = _context.ServiceCentre.Where(x => x.ServiceCentreId == c.ReturnServiceCentre).Select(x => x.Name).FirstOrDefault(),
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       isCodNeeded = c.isCodNeeded,
                                       UserActiveCountryId = c.UserActiveCountryId,
                                       ContactPersons = _context.CompanyContactPerson.Where(x => x.CompanyId == c.CompanyId).Select(p => new CompanyContactPersonDTO
                                       {
                                           CompanyContactPersonId = p.CompanyContactPersonId,
                                           Designation = p.Designation,
                                           Email = p.Email,
                                           FirstName = p.FirstName,
                                           LastName = p.LastName,
                                           PhoneNumber = p.PhoneNumber,
                                           DateCreated = p.DateCreated,
                                           DateModified = p.DateModified,
                                           CompanyId = p.CompanyId
                                       }).ToList(),
                                       Country = _context.Country.Where(x => x.CountryId == c.UserActiveCountryId).Select(x =>  new CountryDTO
                                       {
                                           CountryId = x.CountryId,
                                           CountryName = x.CountryName,
                                           CurrencySymbol = x.CurrencySymbol,
                                           CurrencyCode = x.CurrencyCode,
                                           PhoneNumberCode = x.PhoneNumberCode
                                       }).FirstOrDefault()
                                   };
                return Task.FromResult(companiesDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EcommerceAgreementDTO> GetPendingEcommerceRequestById(int companyId)
        {
            try
            {
                var companies = _context.EcommerceAgreement.Where(x => x.EcommerceAgreementId == companyId);
                var companiesDto = from c in companies
                                   select new EcommerceAgreementDTO
                                   {
                                       EcommerceAgreementId = c.EcommerceAgreementId,
                                       BusinessEmail = c.BusinessEmail,
                                       BusinessOwnerName = c.BusinessOwnerName,
                                       OfficeAddress = c.OfficeAddress,
                                       ContactName = c.ContactName,
                                       ContactEmail = c.ContactEmail,
                                       ContactPhoneNumber = c.ContactPhoneNumber,
                                       AgreementDate = c.AgreementDate,
                                       City = c.City,
                                       State = c.State,
                                       CountryId = c.CountryId,
                                       ReturnAddress = c.ReturnAddress,
                                       Industry =c.NatureOfBusiness,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       IsCod = c.IsCod,
                                       BankName = c.BankName,
                                       AccountName = c.AccountName,
                                       AccountNumber = c.AccountNumber,
                                       IsApi = c.IsApi
                                   };

                return await Task.FromResult(companiesDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<CompanyDTO> GetCompanyByCode(string customerCode)
        {
            try
            {
                var companies = _context.Company.Where(x => x.CustomerCode.ToLower() == customerCode.ToLower());
                var companiesDto = from c in companies
                                   select new CompanyDTO
                                   {
                                       CompanyId = c.CompanyId,
                                       Name = c.Name,
                                       RcNumber = c.RcNumber,
                                       Email = c.Email,
                                       City = c.City,
                                       State = c.State,
                                       Address = c.Address,
                                       PhoneNumber = c.PhoneNumber,
                                       Industry = c.Industry,
                                       CompanyType = c.CompanyType,
                                       CompanyStatus = c.CompanyStatus,
                                       Discount = c.Discount,
                                       isCodNeeded = c.isCodNeeded,
                                       SettlementPeriod = c.SettlementPeriod,
                                       CustomerCode = c.CustomerCode,
                                       CustomerCategory = c.CustomerCategory,
                                       ReturnOption = c.ReturnOption,
                                       ReturnServiceCentre = c.ReturnServiceCentre,
                                       ReturnServiceCentreName = _context.ServiceCentre.Where(x => x.ServiceCentreId == c.ReturnServiceCentre).Select(x => x.Name).FirstOrDefault(),
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       UserActiveCountryId = c.UserActiveCountryId,
                                       ContactPersons = _context.CompanyContactPerson.Where(x => x.CompanyId == c.CompanyId).Select(p => new CompanyContactPersonDTO
                                       {
                                           CompanyContactPersonId = p.CompanyContactPersonId,
                                           Designation = p.Designation,
                                           Email = p.Email,
                                           FirstName = p.FirstName,
                                           LastName = p.LastName,
                                           PhoneNumber = p.PhoneNumber,
                                           DateCreated = p.DateCreated,
                                           DateModified = p.DateModified,
                                           CompanyId = p.CompanyId
                                       }).ToList(),
                                       Country = _context.Country.Where(x => x.CountryId == c.UserActiveCountryId).Select(x => new CountryDTO
                                       {
                                           CountryId = x.CountryId,
                                           CountryName = x.CountryName,
                                           CurrencySymbol = x.CurrencySymbol,
                                           CurrencyCode = x.CurrencyCode,
                                           PhoneNumberCode = x.PhoneNumberCode
                                       }).FirstOrDefault()
                                   };
                return Task.FromResult(companiesDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<CompanyDTO> GetCompanyByIdWithCountry(int companyId)
        {
            try
            {
                var companies = _context.Company.Where(x => x.CompanyId == companyId);
                var companiesDto = from c in companies
                                   select new CompanyDTO
                                   {
                                       CompanyId = c.CompanyId,
                                       Name = c.Name,
                                       RcNumber = c.RcNumber,
                                       Email = c.Email,
                                       City = c.City,
                                       State = c.State,
                                       Address = c.Address,
                                       PhoneNumber = c.PhoneNumber,
                                       Industry = c.Industry,
                                       CompanyType = c.CompanyType,
                                       CompanyStatus = c.CompanyStatus,
                                       Discount = c.Discount,
                                       SettlementPeriod = c.SettlementPeriod,
                                       CustomerCode = c.CustomerCode,
                                       CustomerCategory = c.CustomerCategory,
                                       ReturnOption = c.ReturnOption,
                                       ReturnServiceCentre = c.ReturnServiceCentre,
                                       ReturnServiceCentreName = _context.ServiceCentre.Where(x => x.ServiceCentreId == c.ReturnServiceCentre).Select(x => x.Name).FirstOrDefault(),
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       isCodNeeded = c.isCodNeeded,
                                       UserActiveCountryId = c.UserActiveCountryId,
                                       Country = _context.Country.Where(x => x.CountryId == c.UserActiveCountryId).Select(x => new CountryDTO
                                       {
                                           CountryId = x.CountryId,
                                           CountryName = x.CountryName,
                                           CurrencySymbol = x.CurrencySymbol,
                                           CurrencyCode = x.CurrencyCode,
                                           PhoneNumberCode = x.PhoneNumberCode
                                       }).FirstOrDefault()
                                   };
                return Task.FromResult(companiesDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<EcommerceWalletDTO> GetWalletDetailsForCompany(int companyId)
        {
            try
            {
                var company = _context.Company.Where(x => x.CompanyId == companyId);
                var companiesDto = from c in company
                                   join w in _context.Wallets on c.CustomerCode equals w.CustomerCode
                                   join co in _context.Country on c.UserActiveCountryId equals co.CountryId
                                   select new EcommerceWalletDTO
                                   {
                                       WalletBalance = w.Balance,
                                       Country = _context.Country.Where(x => x.CountryId == c.UserActiveCountryId).Select(x => new CountryDTO
                                       {
                                           CountryId = x.CountryId,
                                           CountryName = x.CountryName,
                                           CurrencySymbol = x.CurrencySymbol,
                                           CurrencyCode = x.CurrencyCode,
                                           PhoneNumberCode = x.PhoneNumberCode
                                       }).FirstOrDefault(),
                                   };
                return Task.FromResult(companiesDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanyDTO>> GetCompanyByEmail(string email)
        {
            try
            {
                if (email != null)
                    email = email.ToLower();

                var companies = _context.Company.Where(x => x.Email.ToLower() == email || x.CustomerCode.ToLower() == email || x.Name.Contains(email) || x.PhoneNumber.Contains(email));
                var companiesDto = from c in companies
                                   join w in _context.Wallets on c.CustomerCode equals w.CustomerCode
                                   join co in _context.Country on c.UserActiveCountryId equals co.CountryId
                                   select new CompanyDTO
                                   {
                                       CompanyId = c.CompanyId,
                                       Name = c.Name,
                                       RcNumber = c.RcNumber,
                                       Email = c.Email,
                                       City = c.City,
                                       State = c.State,
                                       Address = c.Address,
                                       isCodNeeded = c.isCodNeeded,
                                       PhoneNumber = c.PhoneNumber,
                                       Industry = c.Industry,
                                       CompanyType = c.CompanyType,
                                       CompanyStatus = c.CompanyStatus,
                                       Discount = c.Discount,
                                       SettlementPeriod = c.SettlementPeriod,
                                       CustomerCode = c.CustomerCode,
                                       CustomerCategory = c.CustomerCategory,
                                       ReturnOption = c.ReturnOption,
                                       ReturnServiceCentre = c.ReturnServiceCentre,
                                       ReturnServiceCentreName = Context.ServiceCentre.Where(x => x.ServiceCentreId == c.ReturnServiceCentre).Select(x => x.Name).FirstOrDefault(),
                                       ReturnAddress = c.ReturnAddress,
                                       DateCreated = c.DateCreated,
                                       DateModified = c.DateModified,
                                       WalletBalance = w.Balance,
                                       UserActiveCountryId = c.UserActiveCountryId,
                                       Country = new CountryDTO
                                       {
                                           CountryId = co.CountryId,
                                           CountryName = co.CountryName,
                                           CurrencySymbol = co.CurrencySymbol,
                                           CurrencyCode = co.CurrencyCode,
                                           PhoneNumberCode = co.PhoneNumberCode
                                       },
                                       UserActiveCountryName = co.CountryName
                                   };
                return Task.FromResult(companiesDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}