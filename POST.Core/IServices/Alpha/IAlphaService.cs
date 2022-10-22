using POST.Core.DTO.Alpha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IServices.Alpha
{
    public interface IAlphaService : IServiceDependencyMarker
    {
        Task<string> GetToken();
        Task<bool> UpdateUserSubscription(AlphaSubscriptionUpdateDTO payload);
        Task<bool> UpdateOrderStatus(AlphaUpdateOrderStatusDTO payload);
    }
}
