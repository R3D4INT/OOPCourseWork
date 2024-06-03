using BLL.Interfaces.IServiceInterfaces;
using Core.Models.Wallets;

namespace BLL.Interfaces.IOperationsInterfaces;

public interface IMarketWalletService : IGenericService<WalletForMarket>
{
    Task<WalletForMarket> CreateWalletForMarket();
}