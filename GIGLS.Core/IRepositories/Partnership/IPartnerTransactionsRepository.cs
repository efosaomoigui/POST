using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Partnership;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using POST.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IPartnerTransactionsRepository : IRepository<PartnerTransactions>
    {
        Task<List<PartnerTransactionsDTO>> GetPartnerTransactionByDate(BaseFilterCriteria filterCriteria);
        Task<List<FleetPartnerTransactionsDTO>> GetRecentFivePartnerTransactionsForFleet(string fleetPartnerCode);
        Task<List<FleetPartnerTransactionsDTO>> GetPartnerTransactionsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode);
        Task<List<object>> GetPartnerEarningsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode);
        Task<List<PartnerTransactionsDTO>> GetPartnerTransactionByUser(string userId);
        Task<List<ExternalPartnerTransactionsPaymentDTO>> GetExternalPartnerTransactionsForPayment(ShipmentCollectionFilterCriteria filterCriteria);
    }
}
