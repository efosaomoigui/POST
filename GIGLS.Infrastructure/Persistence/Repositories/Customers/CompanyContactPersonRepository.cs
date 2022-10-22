using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Customers;
using POST.Core.IRepositories.Customers;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Customers
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
