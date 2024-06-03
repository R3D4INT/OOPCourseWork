using Core.Enums;
using Core.Models;

namespace BLL.Interfaces.IOperationsInterfaces;

public interface IDepositDeals
{
    Task<DepositDeal> OpenDepositDeal(NameOfCoin coin, double amountInUSDT, Guid userId, int period);
    Task<List<DepositDeal>> GetAllDealsForUser(Guid idOfUser);
    Task<List<DepositDeal>> GetAllOpenDealsForUser(Guid idOfUser);
    Task<DepositDeal> CloseTheDeal(Guid idOfTheDeal);
}