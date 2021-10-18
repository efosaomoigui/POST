using GIGLS.Core.DTO;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface ITicketMannService : IServiceDependencyMarker
    {
        Task<string> GetToken();
        Task<MerchantSalesDTO> GetMerchantSalesSummary(DateFilterCriteria filter);
    }
}
