using Core.Enums;
using Core.Models;
using Core.Models.BaseModels;
using Core.Models.Wallets;

namespace BLL.Interfaces.IServiceInterfaces;

public interface IWalletService : IGenericService<Wallet>
{
    Task<Wallet> CreateWallet();
    Task<Wallet> GetWallet(Guid walletId);
    Task UpdateCoin(Coin coin);
}