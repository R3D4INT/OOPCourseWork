using DAL.Interfaces;
using System.Linq.Expressions;
using Core.Enums;
using System.Globalization;
using System.Text.Json;
using BLL.Interfaces.IServiceInterfaces;
using Core.Models.Wallets;

namespace BLL.Implementations;

public class CoinService : GenericService<Coin>, ICoinService
{
    private readonly IHttpRequests _httpRequests;
    public CoinService(IGenericRepository<Coin> repository, IHttpRequests httpRequests) :
        base(repository)
    {
        _httpRequests = httpRequests;
    }
    public async Task<Coin> UpdatePrice(Guid id)
    {
        try
        {
            var coin = await GetSingleByCondition(coin => coin.Id == id);
            var currentPrice = await _httpRequests.GetPriceFromBinance(coin.Name);
            coin.Price = currentPrice;
            await Update(coin, e => e.Id == id);
            return coin;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to change price. Exception {ex.Message}");
        }
    }

    public async Task<List<double>> GetPriceHistory(NameOfCoin coin, string periodOfTime)
    {
        try
        {
            var coinHistory = await _httpRequests.GetHistoricalPricesFromBinance(coin, periodOfTime);
            return coinHistory;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}