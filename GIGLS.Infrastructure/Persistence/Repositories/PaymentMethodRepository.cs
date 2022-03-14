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

                var transferDetailsDto = GetListOfPaymentMethods(paymentMethods);

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
                              orderby p.DateCreated ascending
                              select new PaymentMethodDTO
                              {
                                  PaymentMethodId = p.PaymentMethodId,
                                  PaymentMethodName = p.PaymentMethodName,
                                  IsActive = p.IsActive,
                              };
            return Task.FromResult(paymentMethodDto.OrderBy(x => x.PaymentMethodId).ToList());
        }

        public Task<List<PaymentMethodNewDTO>> GetPaymentMethods()
        {
            try
            {
                var paymentMethods = _context.PaymentMethod;
                var paymentdto = from p in paymentMethods
                                orderby p.DateCreated ascending
                                select new PaymentMethodNewDTO
                                {
                                    PaymentMethodId = p.PaymentMethodId,
                                    PaymentMethodName = p.PaymentMethodName,
                                    IsActive = p.IsActive,
                                    DateCreated = p.DateCreated,
                                    DateModified = p.DateModified,
                                    CountryId = p.CountryId,
                                };

                return Task.FromResult(paymentdto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PaymentMethodNewDTO> GetPaymentMethodById(int paymentMethodId)
        {
            var method = Context.PaymentMethod.Where(x => x.PaymentMethodId == paymentMethodId);
            var methodDto = GetPaymentMethod(method);
            return methodDto;
        }

        private Task<PaymentMethodNewDTO> GetPaymentMethod(IQueryable<PaymentMethod> methods)
        {
            var methoddto = from m in methods
                              select new PaymentMethodNewDTO
                              {
                                  PaymentMethodId = m.PaymentMethodId,
                                  PaymentMethodName = m.PaymentMethodName,
                                  IsActive = m.IsActive,
                                  CountryId = m.CountryId,
                                  DateCreated = m.DateCreated,
                                  DateModified = m.DateModified,
                              };
            return Task.FromResult(methoddto.FirstOrDefault());
        }
    }
}
