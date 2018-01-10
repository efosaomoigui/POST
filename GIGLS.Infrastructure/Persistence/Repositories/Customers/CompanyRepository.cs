using System.Collections.Generic;
using System.Data.Entity;
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

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Customers
{
    public class CompanyRepository : Repository<Company, GIGLSContext>, ICompanyRepository
    {
        public CompanyRepository(GIGLSContext context): base(context)
        {

        }

        public Task<List<CompanyDTO>> GetCompanies()
        {
            try
            {
                var companies = Context.Company.ToList();
                var companiesDto = Mapper.Map<List<CompanyDTO>>(companies);
                return Task.FromResult(companiesDto);
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

    }
}