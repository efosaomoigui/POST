using System;
using System.Threading.Tasks;
using POST.Core.DTO.Stocks;
using POST.Core.IServices.Stocks;

namespace POST.Services.Implementation.Stocks
{
    public class StockSupplyDetailsService : IStockSupplyDetailsService
    {
        public Task<object> AddStockSupplyDetail(StockSupplyDetailsDTO stock)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStockSupplyDetail(int stockId)
        {
            throw new NotImplementedException();
        }

        public Task<StockSupplyDetailsDTO> GetStockSupplyDetailById(int stockId)
        {
            throw new NotImplementedException();
        }

        public Task<StockSupplyDetailsDTO> GetStockSupplyDetails()
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockSupplyDetail(int stockId, StockSupplyDetailsDTO stock)
        {
            throw new NotImplementedException();
        }
    }
}
