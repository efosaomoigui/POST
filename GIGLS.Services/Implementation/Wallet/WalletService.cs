using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _uow;

        public WalletService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<WalletDTO>> GetWallets()
        {
            var wallets = await _uow.Wallet.GetWalletsAsync();
            return wallets;
        }
        

        public async Task<Core.Domain.Wallet.Wallet> GetWalletById(int walletid)
        {
            var wallet = await _uow.Wallet.GetAsync(walletid);

            if (wallet == null)
            {
                throw new Exception("WALLET_NUMBER_EXHAUSTED");
            }
            return wallet;
        }

        public async Task AddWallet(WalletDTO wallet)
        {
            wallet.WalletNumber = wallet.WalletNumber.Trim();

            var walletNo = wallet.WalletNumber.ToLower();

            if (await _uow.Wallet.ExistAsync(v => v.WalletNumber.ToLower() == walletNo))
            {
                throw new Exception("WALLET_EXIST");
            }

            _uow.Wallet.Add(new Core.Domain.Wallet.Wallet
            {
                WalletId = wallet.WalletId,
                WalletNumber = wallet.WalletNumber,
                Balance = wallet.Balance
            });
            await _uow.CompleteAsync();
        }

        public async Task UpdateWallet(int walletId, WalletDTO wallet)
        {
            var wall = await _uow.Wallet.GetAsync(walletId);

            if (wall == null)
            {
                throw new Exception("WALLET_NOT_EXIST");
            }

            wall.WalletNumber = wallet.WalletNumber.Trim();
            wall.Balance = wallet.Balance;
            await _uow.CompleteAsync();
        }

        public async Task RemoveWallet(int walletId)
        {
            var wall = await _uow.Wallet.GetAsync(walletId);

            if (wall == null)
            {
                throw new Exception("WALLET_NOT_EXIST");
            }

            _uow.Wallet.Remove(wall);
            await _uow.CompleteAsync();
        }

        public async Task<WalletNumber> GenerateNextValidWalletNumber()
        {
            //1. Get the last wallet number
            var walletNumber = await _uow.WalletNumber.GetLastValidWalletNumber();

            // At this point, walletNumber can only be null if it's the first time we're
            // creating a wallet. If that's the case, we assume our wallet PAN to be "0".
            var walletPan = walletNumber?.WalletPan ?? "0";

            //2. Increment and pad walletPan to get the next available wallet number
            var number = long.Parse(walletPan) + 1;
            var numberStr = number.ToString("0000000000");

            //3. Return New Wallet Number
            return new WalletNumber
            {
                WalletPan = numberStr,
                IsActive = true
            };
        }

    }
}
