using GIGLS.Core.IServices.Expenses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Expenses;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain.Expenses;
using System.Linq;

namespace GIGLS.Services.Implementation.Expenses
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IUnitOfWork _uow;

        public ExpenseTypeService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddExpenseType(ExpenseTypeDTO expenseDto)
        {
            try
            {
                if(await _uow.ExpenseType.ExistAsync(c => c.ExpenseTypeName.ToLower().Equals(expenseDto.ExpenseTypeName.Trim().ToLower())))
                {
                    throw new GenericException("Expense Type information already exist");
                }
                var expense = Mapper.Map<ExpenseType>(expenseDto);
                _uow.ExpenseType.Add(expense);
                await _uow.CompleteAsync();
                return new { Id = expense.ExpenseTypeId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteExpenseType(int expenseTypeId)
        {
            try
            {
                var expense = await _uow.ExpenseType.GetAsync(expenseTypeId);
                if(expense ==  null)
                {
                    throw new GenericException("Expense Type information does not exist");
                }
                _uow.ExpenseType.Remove(expense);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ExpenseTypeDTO> GetExpenseTypeById(int expenseTypeId)
        {
            try
            {
                var expense = await _uow.ExpenseType.GetAsync(expenseTypeId);
                if (expense == null)
                {
                    throw new GenericException("Expense Type information does not exist");
                }
                var expenseDto = Mapper.Map<ExpenseTypeDTO>(expense);
                return expenseDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ExpenseTypeDTO>> GetExpenseTypes()
        {
            try
            {
                var expense = _uow.ExpenseType.GetAll().ToList();
                return Task.FromResult(Mapper.Map<List<ExpenseTypeDTO>>(expense));                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateExpenseType(int expenseTypeId, ExpenseTypeDTO expenseDto)
        {
            try
            {
                var expense = await _uow.ExpenseType.GetAsync(expenseTypeId);
                if (expense == null)
                {
                    throw new GenericException("Expense Type information does not exist");
                }
                expense.ExpenseTypeName = expenseDto.ExpenseTypeName;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}