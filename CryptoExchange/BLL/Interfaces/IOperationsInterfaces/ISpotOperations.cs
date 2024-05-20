using Core.Enums;

namespace BLL.Interfaces.IOperationsInterfaces;

public interface ISpotOperations
{
    Task BuyCoin(Guid userId, NameOfCoin coin, double amount);
    Task SellCoin(Guid userId, NameOfCoin coin, double amount);
    Task ConvertCurrency(Guid idOfUser, NameOfCoin CoinForConvert, NameOfCoin imWhichCoinConvert,
        double amountOfCoinForConvert);
}