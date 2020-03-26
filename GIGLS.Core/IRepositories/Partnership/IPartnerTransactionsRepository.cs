using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IPartnerTransactionsRepository : IRepository<PartnerTransactions>
    {
        Task<List<PartnerTransactionsDTO>> GetPartnerTransactionByDate(BaseFilterCriteria filterCriteria);
        Task<List<FleetPartnerTransactionsDTO>> GetRecentFivePartnerTransactionsForFleet(string fleetPartnerCode);
        Task<List<FleetPartnerTransactionsDTO>> GetPartnerTransactionsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode);
        Task<List<object>> GetPartnerEarningsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode);
        Task<List<PartnerTransactionsDTO>> GetPartnerTransactionByUser(string userId);
    }
}
