using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IPaymentMethodService : IServiceDependencyMarker
    {
        Task<bool> AddPaymentMethod(PaymentMethodNewDTO paymentMethodDTO);
        Task<List<PaymentMethodDTO>> GetPaymentMethodByUserActiveCountry();
        Task<List<PaymentMethodDTO>> GetPaymentMethodByUserActiveCountry(int countryid);
        Task UpdatePaymentMethodStatus(int paymentMethodId, bool status);
        Task<List<PaymentMethodNewDTO>> GetPaymentMethods();
        Task DeletePaymentMethod(int paymentMethodId);
        Task UpdatePaymentMethod(int paymentMethodId, PaymentMethodNewDTO paymentMethodDto);
        Task<PaymentMethodNewDTO> GetPaymentMethodById(int paymentMethodId);
    }
}
