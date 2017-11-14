using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IRepositories.Customers;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Customers
{
    public class CompanyContactPersonRepository : Repository<CompanyContactPerson, GIGLSContext>, ICompanyContactPersonRepository
    {
        private GIGLSContext _context;
        public CompanyContactPersonRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<CompanyContactPersonDTO>> GetCompanyContactPerson()
        {
            try
            {
                var persons = _context.CompanyContactPerson;

                var persondto = from s in persons
                                  select new CompanyContactPersonDTO
                                  {
                                      FirstName = s.FirstName,
                                      LastName = s.LastName,
                                      Email = s.Email,
                                      CompanyContactPersonId = s.CompanyContactPersonId,
                                      Designation = s.Designation,
                                      DateCreated = s.DateCreated,
                                      DateModified = s.DateModified,
                                      PhoneNumber = s.PhoneNumber,
                                      CompanyId = s.CompanyId        
                                  };
                return Task.FromResult(persondto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
