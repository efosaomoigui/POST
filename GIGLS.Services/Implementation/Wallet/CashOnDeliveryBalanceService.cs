using GIGLS.Core.IServices.CashOnDeliveryBalance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CashOnDeliveryBalanceService : ICashOnDeliveryBalanceService
    {
        public Task AddCashOnDeliveryBalance(CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO)
        {
            throw new NotImplementedException();
        }

        public Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceById(int cashOnDeliveryBalanceId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalanceByWallet(string walletNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalances()
        {
            throw new NotImplementedException();
        }

        public Task RemoveCashOnDeliveryBalance(int cashOnDeliveryBalanceId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCashOnDeliveryBalance(int cashOnDeliveryBalanceId, CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO)
        {
            throw new NotImplementedException();
        }
    }
}
