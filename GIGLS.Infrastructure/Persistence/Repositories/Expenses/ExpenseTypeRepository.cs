using POST.Core.Domain.Expenses;
using POST.Core.IRepositories.Expenses;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Expenses
{
    public class ExpenseTypeRepository : Repository<ExpenseType, GIGLSContext>, IExpenseTypeRepository
    {
        public ExpenseTypeRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
