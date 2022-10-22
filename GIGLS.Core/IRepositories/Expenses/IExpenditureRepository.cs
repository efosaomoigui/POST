using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Expenses;
using POST.Core.DTO.Expenses;
using POST.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Expenses
{
    public interface IExpenditureRepository : IRepository<Expenditure>
    {
        Task<List<ExpenditureDTO>> GetExpenditures(ExpenditureFilterCriteria expenditureFilterCriteria, int[] serviceCentreIds);
    }
}
