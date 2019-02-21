using GIGLS.Core.Domain.Expenses;
using GIGLS.Core.IRepositories.Expenses;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Expenses
{
    public class ExpenseTypeRepository : Repository<ExpenseType, GIGLSContext>, IExpenseTypeRepository
    {
        public ExpenseTypeRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
