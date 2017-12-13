﻿using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CashOnDeliveryBalance
{
    public interface ICashOnDeliveryBalanceService : IServiceDependencyMarker
    {
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalances();
        Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceById(int cashOnDeliveryBalanceId);
        Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceByWallet(string walletNumber);
        Task AddCashOnDeliveryBalance(CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO);
        Task UpdateCashOnDeliveryBalance(int cashOnDeliveryBalanceId, CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO);
        Task RemoveCashOnDeliveryBalance(int cashOnDeliveryBalanceId);
    }

}
