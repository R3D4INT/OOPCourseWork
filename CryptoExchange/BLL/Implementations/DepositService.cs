using BLL.Interfaces.IServiceInterfaces;
using Core.Enums;
using Core.Models;
using Core.Models.Persons;
using DAL.Interfaces;

namespace BLL.Implementations;

public class DepositService : GenericService<DepositDeal>, IDepositService
{
    public DepositService(IGenericRepository<DepositDeal> repository) :
        base(repository)
    {
    }
    public async Task<DepositDeal> OpenDeal(NameOfCoin coin, double amountInUsdt, int period, double monthIncome, User user)
    {
        try
        {
            double expectableIncome = 0;
            for (var i = 0; i < period; i++)
            {
                expectableIncome += amountInUsdt / 100 * monthIncome;
            }
            var depositDeal = new DepositDeal() {AmountInUSDT = amountInUsdt, Coin = coin, Id = Guid.NewGuid(),
                ExpectableIncome = expectableIncome, MonthIncomeInPercents = monthIncome, PeriodInMonth = period, UserId = user.Id,
                TimeOfOpen = DateTime.Now, CloseTime = DateTime.Now.AddMonths(period), Status = Status.InProcess
            };
            await Add(depositDeal);
            return depositDeal;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create deal {e.Message}");
        }
    }
    public async Task<List<DepositDeal>> GetAllMyDeals(Guid idOfUser)
    {
        try
        {
            var result = await GetListByCondition(e => e.UserId == idOfUser);
            if (result == null)
            {
                throw new Exception("No deals for this user");
            }

            return (List<DepositDeal>)result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<List<DepositDeal>> GetAllMyOpenDeals(Guid idOfUser)
    {
        try
        {
            var result = await GetListByCondition(e => e.UserId == idOfUser && e.CloseTime > DateTime.Now);
            if (result == null)
            {
                throw new Exception("No deals for this user");
            }

            return (List<DepositDeal>)result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<DepositDeal> CloseTheDeal(Guid dealId)
    {
        try
        {
            var deal = await GetSingleByCondition(e => e.Id == dealId);
            if (deal == null)
            {
                throw new Exception("Failed to get deal");
            }

            if (deal.CloseTime > DateTime.Now)
            {
                throw new Exception("Deal can not be closed right now");
            }

            deal.Status = Status.Closed;
            await Update(deal, e => e.Id == deal.Id);
            return deal;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to close the deal {e.Message}");
        }
    }
}