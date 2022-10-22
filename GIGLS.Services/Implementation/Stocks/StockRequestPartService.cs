using System;
using System.Threading.Tasks;
using POST.Core.DTO.Stocks;
using POST.Core.IServices.Stocks;

namespace POST.Services.Implementation.Stocks
{
    public class StockRequestPartService : IStockRequestPartService
    {
        public Task<object> AddStockRequestPart(StockRequestPartDTO stock)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStockRequestPart(int stockId)
        {
            throw new NotImplementedException();
        }

        public Task<StockRequestPartDTO> GetStockRequestPartById(int stockId)
        {
            throw new NotImplementedException();
        }

        public Task<StockRequestPartDTO> GetStockRequestParts()
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockRequestPart(int stockId, StockRequestPartDTO stock)
        {
            throw new NotImplementedException();
        }
    }
}
