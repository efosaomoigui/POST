using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Expenses;
using GIGLS.Core.DTO.Expenses;
using GIGLS.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Expenses
{
    public interface IExpenditureRepository : IRepository<Expenditure>
    {
        Task<List<ExpenditureDTO>> GetExpenditures(ExpenditureFilterCriteria expenditureFilterCriteria, int[] serviceCentreIds);
    }
}
