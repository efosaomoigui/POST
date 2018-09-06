using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.BankSettlement
{
    public class CODSettlementSheetRepository : Repository<CODSettlementSheet, GIGLSContext>, ICODSettlementSheetRepository
    {
        public CODSettlementSheetRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<CODSettlementSheetDTO>> GetCODSettlementSheetsAsync(int[] serviceCentreIds)
        {
            var codSettlementSheets = Context.CODSettlementSheet.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                //codSettlementSheets = codSettlementSheets.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var codSettlementSheetDTO = from codSettlementSheet in codSettlementSheets
                                        select new CODSettlementSheetDTO
                                        {
                                            CODSettlementSheetId = codSettlementSheet.CODSettlementSheetId,
                                            Waybill = codSettlementSheet.Waybill,
                                            Amount = codSettlementSheet.Amount,
                                            DateCreated = codSettlementSheet.DateCreated,
                                            DateSettled = codSettlementSheet.DateSettled,
                                            ReceivedCOD = codSettlementSheet.ReceivedCOD,
                                            ReceiverAgentId = codSettlementSheet.ReceiverAgentId,
                                            ReceiverAgent = Context.Users.Where(d => d.Id == codSettlementSheet.ReceiverAgentId).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                            CollectionAgentId = codSettlementSheet.CollectionAgentId,
                                            CollectionAgent = Context.Users.Where(d => d.Id == codSettlementSheet.CollectionAgentId).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                        };
            return Task.FromResult(codSettlementSheetDTO.ToList());
        }
    }
}
