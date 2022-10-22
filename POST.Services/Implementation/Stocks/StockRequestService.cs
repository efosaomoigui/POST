using System;
using System.Threading.Tasks;
using POST.Core.DTO.Stocks;
using POST.Core.IServices.Stocks;

namespace POST.Services.Implementation.Stocks
{
    public class StockRequestService : IStockRequestService
    {
        public Task<object> AddStockRequest(StockRequestDTO stock)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStockRequest(int stockId)
        {
            throw new NotImplementedException();
        }

        public Task<StockRequestDTO> GetStockRequestById(int stockId)
        {
            throw new NotImplementedException();
        }

        public Task<StockRequestDTO> GetStockRequests()
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockRequest(int stockId, StockRequestDTO stock)
        {
            throw new NotImplementedException();
        }
    }
}
