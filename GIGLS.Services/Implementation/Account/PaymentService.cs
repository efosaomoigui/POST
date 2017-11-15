using GIGLS.Core;
using GIGLS.Core.IServices.Account;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;

        public PaymentService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> ConfirmPayment(string waybill)
        {
            return new { result = true };
        }

    }
}
