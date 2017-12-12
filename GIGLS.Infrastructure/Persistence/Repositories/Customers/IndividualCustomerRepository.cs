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
                                      CustomerCode = s.CustomerCode
                                      //select all shipments of the customer                              
                                  };
                return Task.FromResult(customerdto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
