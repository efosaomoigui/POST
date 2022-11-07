using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Fleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IFleetPartnerTransactionRepository : IRepository<FleetPartnerTransaction>
    {
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerTransaction(string partnercode);
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerCreditTransaction(string partnercode);
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerDebitTransaction(string partnercode);
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerTransactionByDateRange(string partnercode, FleetFilterCriteria filterCriteria);
    }
}
