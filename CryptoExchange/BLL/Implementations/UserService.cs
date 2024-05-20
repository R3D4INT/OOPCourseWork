using Core.Models;
using DAL.Interfaces;
using System.Linq.Expressions;
using System.Threading.Channels;
using Core.Enums;
using Core.Models.BaseModels;
using BLL.Interfaces.IServiceInterfaces;
using Core.Models.Persons;
using Core.Models.Wallets;

namespace BLL.Implementations;

public class UserService : GenericService<User>, IUserService
{
    private readonly IWalletService _walletService;

    private readonly ITicketService _ticketService;

    private readonly IMessageService _messageService;

    private readonly IDepositService _depositService;
    public UserService(IGenericRepository<User> repository, IWalletService walletService,
        ITicketService ticketService, IMessageService messageService, IDepositService depositDealService) :
        base(repository)
    {
        _walletService = walletService;
        _ticketService = ticketService;
        _messageService = messageService;
        _depositService = depositDealService;
    }
    public async Task<double> GetTotalWalletBalance(Guid id)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == id);
            if (user == null)
            {
                throw new Exception($"Failed to get wallet");
            }

            var wallet = user.Wallet;
            double totalBalance = 0;
            foreach (var coin in wallet.AmountOfCoins)
            {
                totalBalance += coin.Amount * coin.Price;
            }

            return totalBalance;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to change price. Exception {ex.Message}");
        }
    }
    public virtual async Task<User> CreateNewUser(ProfileBase profile)
    {
        try
        {
            if (profile == null)
            {
                throw new Exception($"Empty profile info");
            }

            var user = new User(profile.Name, profile.Surname, profile.PhoneNumber, profile.Adress, profile.Age,
                profile.Email, profile.Country, profile.Gender,
                profile.Role, profile.Income, profile.Id, await _walletService.CreateWallet(), profile.FollowersIds);
            if (user == null)
            {
                throw new Exception("Failed to create user");
            }
            user.WalletId = user.Wallet.Id;
            await Add(user);
            return user;
        }
        catch (Exception e)
        {
            throw new Exception($"Empty profile info {e.Message}");
        }
    }
    public async Task BuyCoin(Guid userId, NameOfCoin coin, double amount)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("Failed to get user");
            }

            user.Wallet = await GetMyWallet(user.Id);
            var coinInWallet = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coin);
            if (coinInWallet == null)
            {
                throw new Exception($"Coin {coin} not found in user's wallet");
            }

            var moneyForThisOperation = coinInWallet.Price * amount;

            if (moneyForThisOperation > user.Balance)
            {
                throw new Exception("Balance is less than required");
            }

            user.Balance += -moneyForThisOperation;
            coinInWallet.Amount += amount;
            await _walletService.UpdateCoin(coinInWallet);
            await Update(user, e => e.Id == userId);
        }
        catch (Exception e)
        {
            throw new Exception($"failed to buy Coin {coin}. {e.Message}");
        }
    }
    public async Task SellCoin(Guid userId, NameOfCoin coin, double amount)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            user.Wallet = await GetMyWallet(user.Id);
            if (user == null)
            {
                throw new Exception("Failed to get user");
            }

            var coinInWallet = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coin);
            if (coinInWallet == null)
            {
                throw new Exception($"Coin {coin} not found in user's wallet");
            }

            if (coinInWallet.Amount < amount)
            {
                throw new Exception($"Amount of coin in wallet is less than you want to sell");
            }
            var moneyIncomeAfterOperation = coinInWallet.Price * amount;
            user.Balance += moneyIncomeAfterOperation;
            coinInWallet.Amount -= amount;
            await _walletService.UpdateCoin(coinInWallet);
            await Update(user, e => e.Id == user.Id);
        }
        catch (Exception e)
        {
            throw new Exception($"{e.Message}");
        }
    }
    public async Task IncreaseBalance(Guid userId, double amount)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("failed to get user");
            }
            user.Balance += amount;
            await Update(user, e => e.Id == userId);
        }
        catch (Exception e)
        {
            throw new Exception($"Fail to increase balance {e.Message}");
        }
    }
    public async Task DecreaseBalance(Guid userId, double amount)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("failed to get user");
            }
            user.Balance += -amount;
            await Update(user, e => e.Id == userId);
        }
        catch (Exception e)
        {
            throw new Exception($"Fail to decrease balance {e.Message}");
        }
    }
    public async Task<Ticket> CreateTicket(Guid userId)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("Failed to get user from database");
            }
            var ticket = await _ticketService.CreateTicket(userId);
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task SendMessageToTicketChat(Guid idOfTicket, Guid idOfAuthorOfMessage, string valueOfMessage)
    {
        try
        {
            if (idOfAuthorOfMessage == null)
            {
                throw new Exception("Empty author of ticket");
            }
            await _ticketService.SendMessageToTicket(idOfTicket, idOfAuthorOfMessage, valueOfMessage);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to get user {e.Message}");
        }
    }
    public async Task<List<Ticket>> GetAllMyTickets(Guid userId)
    {
        try
        {
            var tickets = await _ticketService.GetListByCondition(e => e.UserId == userId);
            var updatedTicketsWithChatHistory = tickets.ToList();
            for (var i = 0; i < updatedTicketsWithChatHistory.Count(); i++)
            {
                updatedTicketsWithChatHistory[i].ChatHistory = await _messageService.GetChatHistoryOfTicket(updatedTicketsWithChatHistory[i].Id);
            }
            if (tickets == null)
            {
                throw new Exception("Failed to get tickets");
            }

            return updatedTicketsWithChatHistory;
        }
        catch (Exception e)
        {
            throw new Exception($"Error {e.Message}");
        }
    }

    public async Task<Ticket> GetChatHistory(Guid ticketId)
    {
        try
        {
            var ticket = await _ticketService.GetSingleByCondition(e => e.Id == ticketId);
            ticket.ChatHistory = await _messageService.GetChatHistoryOfTicket(ticketId);
            return ticket;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<DepositDeal> OpenDepositDeal(NameOfCoin coin, double amountInUSDT, Guid userId, int period)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (amountInUSDT > user.Balance)
            {
                throw new Exception("Impossible to create deal, increase your balance");
            }
            user.Balance -= amountInUSDT;
            await Update(user, e => e.Id == user.Id);
            var monthIncome = 0.0;
            switch (period)
            {
                case int n when (n >= 0 && n < 3):
                    monthIncome = 11.3;
                    break;
                case int n when (n >= 3 && n < 6):
                    monthIncome = 12.5;
                    break;
                case int n when (n >= 6 && n < 12):
                    monthIncome = 13.1;
                    break;
                case int n when (n >= 12):
                    monthIncome = 14.4;
                    break;
                default:
                    throw new Exception("Period was incorrect");
                    break;
            }

            var deal = await _depositService.OpenDeal(coin, amountInUSDT, period, monthIncome, user);
            if (deal == null)
            {
                throw new Exception("Failed to create deal");
            }
            foreach (var followerId in user.FollowersIds)
            {
                var follower = await GetSingleByCondition(e => e.Id == followerId);
                follower.Wallet = await GetMyWallet(followerId);
                follower.BalanceForCopyTrading -= amountInUSDT;
                await Update(follower, e => e.Id == follower.Id);
                await _depositService.OpenDeal(coin, amountInUSDT, period, monthIncome, follower);
            }
            return deal;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create new deal {e.Message}");
        }
    }
    public async Task<List<DepositDeal>> GetAllDealsForUser(Guid idOfUser)
    {
        try
        {
            if (idOfUser == null)
            {
                throw new Exception("Empty id in input");
            }
            var result = await _depositService.GetAllMyDeals(idOfUser);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to get deals {e.Message}");
        }
    }
    public async Task<List<DepositDeal>> GetAllOpenDealsForUser(Guid idOfUser)
    {
        try
        {
            if (idOfUser == null)
            {
                throw new Exception("Empty id in input");
            }
            var result = await _depositService.GetAllMyOpenDeals(idOfUser);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to get deals {e.Message}");
        }
    }
    public async Task<DepositDeal> CloseTheDeal(Guid idOfTheDeal)
    {
        try
        {
            var deal = await _depositService.GetSingleByCondition(e => e.Id == idOfTheDeal);
            var user = await GetSingleByCondition(e => e.Id == deal.UserId);
            if (user == null || deal == null)
            {
                throw new Exception("Failed to get info");
            }

            deal = await _depositService.CloseTheDeal(deal.Id);
            user.Balance += deal.ExpectableIncome;
            await _depositService.Update(deal, e => e.Id == deal.Id);
            await Update(user, e => e.Id == user.Id);
            return deal;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to close deal {e.Message}");
        }
    }
    public async Task ConvertCurrency(Guid idOfUser, NameOfCoin CoinForConvert, NameOfCoin imWhichCoinConvert,
        double amountOfCoinForConvert)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == idOfUser);
            user.Wallet = await GetMyWallet(user.Id);
            var coinForConvert = user.Wallet.AmountOfCoins.FirstOrDefault(e => e.Name == CoinForConvert);
            var coinToConvertInto = user.Wallet.AmountOfCoins.FirstOrDefault(e => e.Name == imWhichCoinConvert);

            if (coinForConvert == null || coinToConvertInto == null)
            {
                throw new Exception("One of the coins was not found in the wallet.");
            }

            if (coinForConvert.Amount < amountOfCoinForConvert)
            {
                throw new Exception("Not enough coins for conversion.");
            }

            double amountAfterConversion = (coinForConvert.Price / coinToConvertInto.Price) * amountOfCoinForConvert;

            coinForConvert.Amount -= amountOfCoinForConvert;
            coinToConvertInto.Amount += amountAfterConversion;
            await _walletService.UpdateCoin(coinForConvert);
            await _walletService.UpdateCoin(coinToConvertInto);
            await Update(user, e => e.Id == user.Id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to convert coin: {ex.Message}");
        }
    }
    public async Task<Ticket> GetTicketById(Guid idOfTicket)
    {
        try
        {
            var ticket = await _ticketService.GetTicketById(idOfTicket);
            if (ticket == null)
            {
                throw new Exception("Failed to get ticket");
            }

            return ticket;
        }
        catch (Exception e)
        {
            throw new Exception("Failed to get ticket");
        }
    }
    public async Task<User> BecomeAvailableForCopyTrading(Guid userId)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("user is null");
            }
            user.IsAvailableForCopyTrade = true;
            await Update(user, e => e.Id == user.Id);
            return user;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to update copy trading {e.Message}");
        }
    }
    public async Task<User> BecomeUnAvailableForCopyTradingTradingContract(Guid userId)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("user is null");
            }
            user.IsAvailableForCopyTrade = false;
            await Update(user, e => e.Id == user.Id);
            return user;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to update copy trading {e.Message}");
        }
    }
    public async Task<List<User>> GetAvailableCopyTraders()
    {
        try
        {
            var users = await GetListByCondition(e => e.IsAvailableForCopyTrade == true);
            return (List<User>)users;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to get copy traders {e.Message}");
        }
    }
    public async Task FollowSomebody(Guid userId, Guid userForFollowId)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("Incorrect user id ");
            }
            var userForFollow = await GetSingleByCondition(e => e.Id == userForFollowId);
            userForFollow.FollowersIds.Add(user.Id);
            await Update(userForFollow, e => e.Id == userForFollowId);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to follow user");
        }
    }
    public async Task<User> ModifyCopyTradingBalance(Guid idOfUser, double amount, bool isIncrease)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == idOfUser);

            if (isIncrease)
            {
                if (user.Balance < amount)
                {
                    throw new Exception("Insufficient balance for increase operation");
                }
                user.BalanceForCopyTrading += amount;
                user.Balance -= amount;
            }
            else
            {
                if (user.BalanceForCopyTrading < amount)
                {
                    throw new Exception("Insufficient balance for decrease operation");
                }
                user.BalanceForCopyTrading -= amount;
                user.Balance += amount;
            }

            await Update(user, e => e.Id == user.Id);
            return user;
        }
        catch (Exception e)
        {
            throw new Exception("Failed to modify balance");
        }
    }

    public async Task<Wallet> GetMyWallet(Guid userId)
    {
        try
        {
            var user = await GetSingleByCondition(e => e.Id == userId);
            if (user == null)
            {
                throw new Exception("Failed to get user");
            }

            var wallet = await _walletService.GetWallet(user.WalletId);
            return wallet;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}