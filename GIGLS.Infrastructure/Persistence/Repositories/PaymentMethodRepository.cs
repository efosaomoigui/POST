using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod, GIGLSContext>, IPaymentMethodRepository
    {
        private GIGLSContext _context;
        public PaymentMethodRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<PaymentMethodDTO>> GetPaymentMethodByUserActiveCountry(int countryId)
        {
            try
            {
                var  paymentMethods = _context.PaymentMethod.AsQueryable();
                if (countryId > 0)
                {
                    paymentMethods = paymentMethods.Where(x => x.CountryId.Equals(countryId) && x.IsActive == true);
                }

                var transferDetailsDto = GetListOfPaymentMethods(paymentMethods.OrderBy(x =>x.PaymentMethodId));

                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task<List<PaymentMethodDTO>> GetListOfPaymentMethods(IQueryable<PaymentMethod> paymentMethods)
        {
            var paymentMethodDto = from p in paymentMethods
                              orderby p.DateCreated descending
                              select new PaymentMethodDTO
                              {
                                  PaymentMethodId = p.PaymentMethodId,
                                  PaymentMethodName = p.PaymentMethodName,
                                  IsActive = p.IsActive,
                              };
            return Task.FromResult(paymentMethodDto.ToList());
        }
    }
}
