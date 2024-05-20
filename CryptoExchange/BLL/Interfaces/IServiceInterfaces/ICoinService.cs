using Core.Enums;
using Core.Models.Wallets;

namespace BLL.Interfaces.IServiceInterfaces;

public interface ICoinService : IGenericService<Coin>
{
    Task<Coin> UpdatePrice(Guid id);
    Task<List<double>> GetPriceHistory(NameOfCoin coin, string periodOfTime);
}