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
                var companies = Context.Company;
                var companydto = from r in companies
                                 select new CompanyDTO()
                                 {
                                    Address = r.Address,
                                    City = r.City,
                                    CompanyId = r.CompanyId,
                                    CompanyType = r.CompanyType,
                                    Discount = r.Discount,
                                    Email = r.Email,
                                    Industry = r.Industry,
                                    PhoneNumber = r.PhoneNumber,
                                    RcNumber = r.RcNumber,
                                    State = r.State,
                                     CompanyStatus = r.CompanyStatus,
                                    DateModified = r.DateModified,
                                    DateCreated = r.DateCreated,
                                    Name = r.Name
                                };
                return Task.FromResult(companydto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}