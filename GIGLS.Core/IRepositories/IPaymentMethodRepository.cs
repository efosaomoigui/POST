using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
        Task<List<PaymentMethodDTO>> GetPaymentMethodByUserActiveCountry(int countryId);
        Task<List<PaymentMethodNewDTO>> GetPaymentMethods();
        Task<PaymentMethodNewDTO> GetPaymentMethodById(int paymentMethodId);
    }
}
