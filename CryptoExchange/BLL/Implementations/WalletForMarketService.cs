using System.ComponentModel;
using Core.Enums;
using DAL.Interfaces;
using System.Globalization;
using System.Text.Json;
using BLL.Interfaces.IOperationsInterfaces;
using Core.Models.Wallets;

namespace BLL.Implementations;

public class WalletForMarketService : GenericService<WalletForMarket>, IMarketWalletService
{
    private readonly IHttpRequests _httpRequests;
    public WalletForMarketService(IGenericRepository<WalletForMarket> repository, IHttpRequests httpRequests) :
        base(repository)
    {
        _httpRequests = httpRequests;
    }

    public async Task<WalletForMarket> CreateWalletForMarket()
    {
        try
        {
            var wallets = await GetListByCondition(e => e.Id != Guid.Empty);
            if (wallets != null)
            {
                return wallets.ElementAt(0);
            }
            else
            {
                var wallet = new WalletForMarket();
                var seedPhrase = new SeedPhrase();
                wallet.AmountOfCoins = new List<Coin>();
                seedPhrase.SeedPhraseValues = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                wallet.SeedPhrase = seedPhrase;
                foreach (NameOfCoin name in Enum.GetValues(typeof(NameOfCoin)))
                {
                    var coin = new Coin { Name = name, Price = await _httpRequests.GetPriceFromBinance(name), Amount = double.MaxValue, Id = Guid.NewGuid() };
                    wallet.AmountOfCoins.Add(coin);
                }

                var walletId = Guid.NewGuid();
                wallet.Id = walletId;
                WalletForMarket.CurrentId = walletId;
                await Add(wallet);
                return await GetSingleByCondition(e => e.Id == WalletForMarket.CurrentId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("Failed To Create Market Wallet");
        }
    }
}
