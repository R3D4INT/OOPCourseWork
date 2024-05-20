using Core.Enums;
using Core.Models;
using Core.Models.Persons;

namespace BLL.Interfaces.IServiceInterfaces;

public interface IDepositService : IGenericService<DepositDeal>
{
    Task<DepositDeal> OpenDeal(NameOfCoin coin, double amountInUsdt, int period, double monthIncome, User user);
    Task<List<DepositDeal>> GetAllMyDeals(Guid idOfUser);
    Task<List<DepositDeal>> GetAllMyOpenDeals(Guid idOfUser);
    Task<DepositDeal> CloseTheDeal(Guid dealId);
}