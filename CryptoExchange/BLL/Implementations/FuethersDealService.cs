using Core.Models;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using BLL.Interfaces.IServiceInterfaces;
using Core.Models.Wallets;

namespace BLL.Implementations
{
    public class FuethersDealService : GenericService<FuethersDeal>, IFuethersDealService
    {
        private readonly ICoinService _coinService;

        private readonly IUserService _userService;
        public FuethersDealService(IGenericRepository<FuethersDeal> repository, ICoinService coinService, IUserService userService) :
            base(repository)
        {
            _coinService = coinService;
            _userService = userService;
        }
        public async Task<FuethersDeal> CreateDeal(Coin coin, TypeOfFuetersDeal typeOfFuetersDeal,
            int leverage, Guid userId, double stopLoss, double takeProfit, double marginValue, double amount)
        {
            try
            {
                await _coinService.Update(coin, e => e.Id == coin.Id);
                var updatedCoin = await _coinService.GetSingleByCondition(e => e.Id == coin.Id);
                var user = await _userService.GetSingleByCondition(e => e.Id == userId);
                var FuethersDeal = new FuethersDeal();
                    FuethersDeal.CoinId = updatedCoin.Id;
                    FuethersDeal.EnterPrice = updatedCoin.Price;
                    FuethersDeal.Id = Guid.NewGuid();
                    FuethersDeal.Leverage = leverage;
                    FuethersDeal.UserId = userId;
                    FuethersDeal.StopLoss = stopLoss;
                    FuethersDeal.TakeProfit = takeProfit;
                    FuethersDeal.TypeOfDeal = typeOfFuetersDeal;
                    FuethersDeal.TypeOfMargin = TypeOfMargin.Isolate;
                    FuethersDeal.MarginValue = marginValue;
                    FuethersDeal.Status = Status.InProcess;
                    FuethersDeal.Amount = amount;
                await Add(FuethersDeal);
                user.Balance -= marginValue;
                foreach (var followerId in user.FollowersIds)
                {
                    var follower = await _userService.GetSingleByCondition(e => e.Id == followerId);
                    var FollowerDeal = new FuethersDeal();
                    FollowerDeal.CoinId = updatedCoin.Id;
                    FollowerDeal.EnterPrice = updatedCoin.Price;
                    FollowerDeal.Id = Guid.NewGuid();
                    FollowerDeal.Leverage = leverage;
                    FollowerDeal.UserId = follower.Id;
                    FollowerDeal.StopLoss = stopLoss;
                    FollowerDeal.TakeProfit = takeProfit;
                    FollowerDeal.TypeOfDeal = typeOfFuetersDeal;
                    FollowerDeal.TypeOfMargin = TypeOfMargin.Isolate;
                    FollowerDeal.MarginValue = marginValue;
                    FollowerDeal.Status = Status.InProcess;
                    FollowerDeal.Amount = amount;
                    follower.BalanceForCopyTrading -= marginValue;
                    await Add(FollowerDeal);
                    await _userService.Update(follower, e => e.Id == follower.Id);
                }
                await _userService.Update(user, e => e.Id == user.Id);
                return FuethersDeal;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<FuethersDeal> CheckFuethersDeal(Guid dealId)
        {
            try
            {
                var deal = await GetSingleByCondition(e => e.Id == dealId);
                if (deal == null)
                {
                    throw new Exception("Failed to get deal");
                }
                var coin = await _coinService.GetSingleByCondition(e => e.Id == deal.CoinId);
                var dealIncome = (deal.EnterPrice - coin.Price) / deal.Leverage * deal.Amount;
                var user = await _userService.GetSingleByCondition(e => e.Id == deal.UserId);
                if (dealIncome < 0 && +dealIncome >= deal.MarginValue)
                {
                    deal.Status = Status.Closed;
                    deal.MarginValue = 0;
                    user.Balance += deal.MarginValue;
                    await _userService.Update(user, e => e.Id == user.Id);
                }
                if (coin.Price <= deal.StopLoss)
                {
                    deal.Status = Status.Closed;
                    deal.MarginValue += dealIncome;
                    user.Balance += deal.MarginValue;
                    await _userService.Update(user, e => e.Id == user.Id);
                }

                if (coin.Price >= deal.TakeProfit)
                {
                    deal.Status = Status.Closed;
                    user.Balance += deal.MarginValue + dealIncome;
                    await _userService.Update(user, e => e.Id == user.Id);
                }

                await Update(deal, e => e.Id == deal.Id);
                return deal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<FuethersDeal> CloseDeal(Guid dealId)
        {
            try
            {
                var deal = await GetSingleByCondition(e => e.Id == dealId);
                if (deal == null)
                {
                    throw new Exception("Failed to get deal");
                }

                deal = await CheckFuethersDeal(deal.Id);
                if (deal.Status == Status.Closed)
                {
                    return deal;
                }

                var coin = await _coinService.GetSingleByCondition(e => e.Id == deal.CoinId);
                var dealIncome = (deal.EnterPrice - coin.Price) / deal.Leverage * deal.Amount;
                var user = await _userService.GetSingleByCondition(e => e.Id == deal.UserId);
                deal.MarginValue += dealIncome;
                user.Balance += deal.MarginValue;
                deal.Status = Status.Closed;
                await _userService.Update(user, e => e.Id == user.Id);
                await Update(deal, e => e.Id == deal.Id);
                return deal;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to close the Deal {e.Message}");
            }
        }

        public async Task<List<FuethersDeal>> GetAllFuethersDealsForUser(Guid userId)
        {
            try
            {
                var deals = await GetListByCondition(e => e.UserId == userId);
                return (List<FuethersDeal>)deals;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get deals for this user");
            }
        }
    }
}
