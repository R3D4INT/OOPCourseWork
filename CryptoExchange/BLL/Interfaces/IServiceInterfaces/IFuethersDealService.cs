using Core.Enums;
using Core.Models;
using Core.Models.Wallets;

namespace BLL.Interfaces.IServiceInterfaces;

public interface IFuethersDealService : IGenericService<FuethersDeal>
{
    Task<FuethersDeal> CreateDeal(Coin coin, TypeOfFuetersDeal typeOfFuetersDeal,
        int leverage, Guid userId, double stopLoss, double takeProfit, double marginValue, double amount);
    Task<FuethersDeal> CheckFuethersDeal(Guid dealId);
    Task<FuethersDeal> CloseDeal(Guid dealId);
    Task<List<FuethersDeal>> GetAllFuethersDealsForUser(Guid userId);
}