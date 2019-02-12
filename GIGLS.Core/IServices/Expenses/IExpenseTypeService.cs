using GIGLS.Core.DTO.Expenses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Expenses
{
    public interface IExpenseTypeService : IServiceDependencyMarker
    {
        Task<List<ExpenseTypeDTO>> GetExpenseTypes();
        Task<ExpenseTypeDTO> GetExpenseTypeById(int expenseTypeId);
        Task<object> AddExpenseType(ExpenseTypeDTO expenseDto);
        Task UpdateExpenseType(int expenseTypeId, ExpenseTypeDTO expenseDto);
        Task DeleteExpenseType(int expenseTypeId);
    }
}
