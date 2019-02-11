using GIGLS.Core.DTO.Expenses;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Expenses;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Account
{

    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/expensetype")]
    public class ExpenseTypeController : BaseWebApiController
    {
        private readonly IExpenseTypeService _expenseService;

        public ExpenseTypeController(IExpenseTypeService expenseService) : base(nameof(ExpenseTypeController))
        {
            _expenseService = expenseService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ExpenseTypeDTO>>> GetExpenseTypes()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var expenses = await _expenseService.GetExpenseTypes();
                return new ServiceResponse<IEnumerable<ExpenseTypeDTO>>
                {
                    Object = expenses
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{expenseTypeId:int}")]
        public async Task<IServiceResponse<ExpenseTypeDTO>> GetExpenseTypeById(int expenseTypeId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var expense = await _expenseService.GetExpenseTypeById(expenseTypeId);

                return new ServiceResponse<ExpenseTypeDTO>
                {
                    Object = expense
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddExpenseType(ExpenseTypeDTO expenseDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var expense = await _expenseService.AddExpenseType(expenseDto);
                return new ServiceResponse<object>
                {
                    Object = expense
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{expenseTypeId:int}")]
        public async Task<IServiceResponse<object>> UpdateExpenseType(int expenseTypeId, ExpenseTypeDTO expenseDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _expenseService.UpdateExpenseType(expenseTypeId, expenseDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{expenseTypeId:int}")]
        public async Task<IServiceResponse<bool>> DeleteExpenseType(int expenseTypeId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _expenseService.DeleteExpenseType(expenseTypeId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
