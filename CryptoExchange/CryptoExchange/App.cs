using Core.Enums;
using Core.Models;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Reflection;
using System.Runtime.InteropServices;
using Core.Models.BaseModels;
using Microsoft.Extensions.DependencyInjection;
using BLL.Interfaces.IServiceInterfaces;
using BLL.Interfaces.IOperationsInterfaces;
using Core.Models.Persons;

namespace UI;

public class App
{
    private readonly IUserService _userService;

    private readonly ICoinService _coinService;

    private readonly IMarketWalletService _marketWalletService;

    private readonly ISupportService _supportService;

    private readonly IFuethersDealService _futhersDealService;

    private readonly IAdminService _adminService;
    public App(IUserService userService, ICoinService coinService, IMarketWalletService marketWalletService, ISeedPhraseService seedPhraseService,
        IWalletService walletService, ISupportService supportService, ITicketService ticketService, IFuethersDealService fuethersDealService, IAdminService adminService)
    {
        _userService = userService;
        _coinService = coinService;
        _marketWalletService = marketWalletService;
        _supportService = supportService;
        _futhersDealService = fuethersDealService;
        _adminService = adminService;
    }

    public async Task Run()
    {
        //var user = await _userService.CreateNewUser(await CreateUserInfo());
        //user.Balance = 80000;
        //await _userService.Update(user, e => e.Id == user.Id);
        //var deal = await _userService.OpenDepositDeal(NameOfCoin.BNB, 5000, user.Id, 11);
        //var deal2 = await _userService.OpenDepositDeal(NameOfCoin.BNB, 5000, user.Id, 1);
        //var deal3 = await _userService.OpenDepositDeal(NameOfCoin.BNB, 5000, user.Id, 21);
        //var deals = await _userService.GetAllDealsForUser(user.Id);
        //deal2 = await _userService.CloseTheDeal(deal2.Id);
        //var dealForTest =
        //    await _futhersDealService.GetSingleByCondition(e => e.Id == Guid.Parse("17896c4c-2ecc-4551-bd67-592e0da2ad13"));
        //dealForTest.StopLoss = 681000;
        //await _futhersDealService.Update(dealForTest, e => e.Id == dealForTest.Id);
        //var result = await _futhersDealService.CheckFuethersDeal(dealForTest.Id);
        //var testUser = await _userService.CreateNewUser(await CreateUserInfo());
        //var firstDeal = await _futhersDealService.CreateDeal(
        //    testUser.Wallet.AmountOfCoins.Find(e => e.Name == NameOfCoin.BTC), TypeOfFuetersDeal.Long,
        //    TypeOfOpeningTheDeal.MarketPrice, 1000, testUser, 67000, 69000, 100, 1);
        //var secondDeal = await _futhersDealService.CreateDeal(
        //    testUser.Wallet.AmountOfCoins.Find(e => e.Name == NameOfCoin.BTC), TypeOfFuetersDeal.Short,
        //    TypeOfOpeningTheDeal.MarketPrice, 1000, testUser, 67000, 69000, 100, 1);
        //secondDeal = await _futhersDealService.CheckFuethersDeal(secondDeal.Id);
        //firstDeal = await _futhersDealService.CloseDeal(firstDeal.Id);
        //var newTestUser = await _userService.CreateNewUser(await CreateUserInfo());
        //var marketWallet =  await _marketWalletService.CreateWalletForMarket();
        User user = new User();
        Support support = new Support();
        Admin admin = new Admin();
        var loggedIn = false;
        var walletForMarket = await _marketWalletService.CreateWalletForMarket();
        List<Ticket> tickets = null;
        Ticket ticket;
        var fuethersDeals = new List<FuethersDeal>();
        DepositDeal myCurrentDepositDeal;
        var depositDeals = new List<DepositDeal>();
        while (true)
        {
            try
            {
                while (!loggedIn)
                {
                    var resulOfAuth = await Auth();
                    if (resulOfAuth == null)
                    {
                        Console.WriteLine("This user does not exist");
                        throw new CustomAttributeFormatException("This user does not exist");
                    }
                    else
                    {
                        user = resulOfAuth;
                        if (user.IsBanned)
                        {
                            Console.WriteLine("You are banned ba ba ba");
                            throw new InvalidOperationException("You are banned by");
                        }

                        Console.WriteLine("Successfully");
                        if (user.Role == Role.Support)
                        {
                            support = (Support)user;
                        }

                        if (user.Role == Role.Admin)
                        {
                            admin = new Admin(user);
                        }

                        loggedIn = true;
                    }
                }

                while (true)
                {
                    Console.WriteLine("What do you want to do?");
                    Console.WriteLine("Manage my cryptocurrency - 1");
                    Console.WriteLine("Open or manage ticket - 2");
                    Console.WriteLine("Update price of the coin or get price history - 3");
                    Console.WriteLine("Open or close fuethers deal - 4");
                    Console.WriteLine("Withdraw balance or make deposit of money - 5");
                    Console.WriteLine("Menu of deposits - 6");
                    Console.WriteLine("Copy trading abilities - 7");
                    if (user.Role == Role.Support)
                    {
                        Console.WriteLine("Manage support abilities - 8");
                    }

                    if (user.Role == Role.Admin)
                    {
                        Console.WriteLine("Manage support abilities - 8");
                        Console.WriteLine("Manage admin abilities - 9");
                    }

                    switch (GetIntValue())
                    {
                        case (1):
                        {
                            Console.WriteLine("What do you want to do?");
                            Console.WriteLine("Sell some coins - 1");
                            Console.WriteLine("Buy some coins- 2");
                            Console.WriteLine("Convert some coins - 3");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine("Okay, enter name of the coin: BTC ETH LTC BNB SOL XRP");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coin);
                                    await SellCoin(user, coin);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine("Okay, enter name of the coin: BTC ETH LTC BNB SOL XRP");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coin);
                                    await BuyCoin(user, coin);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                case (3):
                                {
                                    user.Wallet = await _userService.GetMyWallet(user.Id);
                                            Console.WriteLine("Okay here is your amount of each coin and it's current price");
                                    foreach (var coin in user.Wallet.AmountOfCoins)
                                    {
                                        await _coinService.UpdatePrice(coin.Id);
                                        Console.WriteLine(
                                            $"Amount of {coin.Name} is {coin.Amount}. Current price is {coin.Price}");
                                    }

                                    Console.WriteLine(
                                        "Firstly enter coin for convert, after this amount for convert, and lately coin in which convert");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coinForConvert);
                                    var amount = GetDoubleValueFromConsole();
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coinInWhichConvert);
                                    await _userService.ConvertCurrency(user.Id, coinForConvert, coinInWhichConvert,
                                        amount);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                default:
                                    Console.WriteLine("Something went wrong try again");
                                    break;
                            }

                            break;
                        }
                        case (2):
                        {
                            Console.WriteLine("What do you want to do?");
                            Console.WriteLine("Create new ticket - 1");
                            Console.WriteLine("Send message to ticket - 2");
                            Console.WriteLine("Get ticket by id - 3");
                            Console.WriteLine("Get all my tickets - 4");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    var id = Guid.NewGuid();
                                    ticket = await _userService.CreateTicket(user.Id);
                                    Console.WriteLine($"Successfully, id of ticket is {id}");
                                    break;
                                }
                                case (2):
                                {
                                    var ticketsForOperation = await _userService.GetAllMyTickets(user.Id);
                                    Console.WriteLine("Okay your open tickets are:");
                                    var i = 0;
                                    foreach (var tick in ticketsForOperation)
                                    {
                                        Console.WriteLine(
                                            $"Ticket with id {tick.Id} with {tick.ChatHistory.Count} messages - {i}");
                                        i++;
                                    }

                                    Console.WriteLine("Enter the number of the ticket");
                                    var ticketForOperation = ticketsForOperation[GetIntValue()];
                                    foreach (var messages in ticketForOperation.ChatHistory)
                                    {
                                        var authorUser =
                                            await _userService.GetSingleByCondition(e => e.Id == messages.AuthorId);
                                        Console.WriteLine(
                                            $"{authorUser.Name} send message with: {messages.Value}. His role is {authorUser.Role}");
                                    }

                                    Console.WriteLine("Enter the message of the ticket");
                                    var message = Console.ReadLine();
                                    await _userService.SendMessageToTicketChat(ticketForOperation.Id, user.Id, message);
                                    break;
                                }
                                case (3):
                                {
                                    Console.WriteLine("All possible variants for you:");
                                    tickets = await _userService.GetAllMyTickets(user.Id);
                                    for (var i = 0; i < tickets.Count; i++)
                                    {
                                        Console.WriteLine(
                                            $"Ticket with id {tickets[i].Id}. If you want chose it write {i}");
                                    }

                                    var choosedTicket = tickets[GetIntValue()];
                                    ticket = await _userService.GetTicketById(choosedTicket.Id);
                                    break;
                                }
                                case (4):
                                {
                                    Console.WriteLine("Okay here are your tickets");
                                    tickets = await _userService.GetAllMyTickets(user.Id);
                                    foreach (var elem in tickets)
                                    {
                                        Console.WriteLine(
                                            $"Ticket with id {elem.Id}, it's status is {elem.Status}, and it has {elem.ChatHistory.Count} messages in chat");
                                    }

                                    break;
                                }
                                default:
                                    Console.WriteLine("Something went wrong try again");
                                    break;
                            }

                            break;
                        }
                        case (3):
                        {
                            Console.WriteLine("Nice choose next step");
                            Console.WriteLine("Update coin price - 1");
                            Console.WriteLine("Get coin price history - 2");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine("Okay choose the coin to update BTC ETH LTC BNB SOL XRP");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coin);
                                    user.Wallet = await _userService.GetMyWallet(user.Id);
                                    var coinForUpdate = user.Wallet.AmountOfCoins.Find(e => e.Name == coin);
                                    coinForUpdate = await _coinService.UpdatePrice(coinForUpdate.Id);
                                    Console.WriteLine($"Updated, current price is {coinForUpdate.Price}");
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine(
                                        "Okay choose the coin to get price history BTC ETH LTC BNB SOL XRP");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coin);
                                    Console.WriteLine("Enter a period of time like one string");
                                    Console.WriteLine(
                                        "Variants: 1m, 3m, 5m, 15m, 30m, 1h, 2h, 4h, 6h, 8h, 12h, 1d, 3d, 1w, 1M");
                                    user.Wallet = await _userService.GetMyWallet(user.Id);
                                            var periodOfTimeInput = Console.ReadLine();
                                    int periodOfTimeInMinutes;

                                    switch (periodOfTimeInput)
                                    {
                                        case "1m":
                                            periodOfTimeInMinutes = 1;
                                            break;
                                        case "3m":
                                            periodOfTimeInMinutes = 3;
                                            break;
                                        case "5m":
                                            periodOfTimeInMinutes = 5;
                                            break;
                                        case "15m":
                                            periodOfTimeInMinutes = 15;
                                            break;
                                        case "30m":
                                            periodOfTimeInMinutes = 30;
                                            break;
                                        case "1h":
                                            periodOfTimeInMinutes = 60;
                                            break;
                                        case "2h":
                                            periodOfTimeInMinutes = 120;
                                            break;
                                        case "4h":
                                            periodOfTimeInMinutes = 240;
                                            break;
                                        case "6h":
                                            periodOfTimeInMinutes = 360;
                                            break;
                                        case "8h":
                                            periodOfTimeInMinutes = 480;
                                            break;
                                        case "12h":
                                            periodOfTimeInMinutes = 720;
                                            break;
                                        case "1d":
                                            periodOfTimeInMinutes = 1440;
                                            break;
                                        case "3d":
                                            periodOfTimeInMinutes = 4320;
                                            break;
                                        case "1w":
                                            periodOfTimeInMinutes = 10080;
                                            break;
                                        case "1M":
                                            periodOfTimeInMinutes = 43800;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid input. Please try again.");
                                            return;
                                    }

                                    var priceHistory = await _coinService.GetPriceHistory(coin, periodOfTimeInput);
                                    var timeStamp = DateTime.Now;

                                    for (var i = priceHistory.Count - 1; i >= 0; i--)
                                    {
                                        Console.WriteLine($"Price at {timeStamp} is {priceHistory[i]}");
                                        timeStamp = timeStamp.AddMinutes(-periodOfTimeInMinutes);
                                    }

                                    await DisplayGraph(priceHistory);
                                    break;
                                }
                                default:
                                    Console.WriteLine("Something went wrong try again");
                                    break;
                            }

                            break;
                        }
                        case (4):
                        {
                            Console.WriteLine("Okay, what do you want to do?");
                            Console.WriteLine("Open the deal - 1");
                            Console.WriteLine("Check the deal - 2");
                            Console.WriteLine("Close the deal - 3");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine(
                                        "Okay, enter coin in which you want to create deal BTC ETH LTC BNB SOL XRP");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coinForConvert);
                                    user.Wallet = await _userService.GetMyWallet(user.Id);
                                    var coin = user.Wallet.AmountOfCoins.Where(e => e.Name == coinForConvert);
                                    var updatedCoin = await _coinService.UpdatePrice(coin.ElementAt(0).Id);
                                    Console.WriteLine("Enter type of the deal Short or Long");
                                    Enum.TryParse(Console.ReadLine(), out TypeOfFuetersDeal typeOfDeal);
                                    Console.WriteLine("Write the leverage of the deal, from 1 to 100");
                                    var leverage = GetIntValue();
                                    Console.WriteLine(
                                        $"Current price of the coin is {updatedCoin.Price}. Enter the stop loss, and after this take profit");
                                    var stopLoss = GetDoubleValueFromConsole();
                                    var takeProfit = GetDoubleValueFromConsole();
                                    Console.WriteLine(
                                        $"Enter how much money you want to spend on margin. Your current balance is {user.Balance}");
                                    var marginValue = GetDoubleValueFromConsole();
                                    Console.WriteLine(
                                        $"Enter the amount of the coin which you want to buy for the deal, max value is ${user.Balance / updatedCoin.Price}");
                                    var amount = GetDoubleValueFromConsole();
                                    await _futhersDealService.CreateDeal(updatedCoin, typeOfDeal, leverage,
                                        user.Id, stopLoss, takeProfit, marginValue, amount);
                                    fuethersDeals = await _futhersDealService.GetAllFuethersDealsForUser(user.Id);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine("Okay your current deals are");
                                    fuethersDeals = await _futhersDealService.GetAllFuethersDealsForUser(user.Id);
                                    for (var i = 0; i < fuethersDeals.Count; i++)
                                    {
                                        Console.WriteLine(
                                            $"Deal with id {fuethersDeals[i].Id}, type {fuethersDeals[i].TypeOfDeal} - {i}");
                                    }

                                    var dealManager = GetIntValue();
                                    await _futhersDealService.CheckFuethersDeal(fuethersDeals[dealManager].Id);
                                    fuethersDeals[dealManager] =
                                        await _futhersDealService.GetSingleByCondition(e =>
                                            e.Id == fuethersDeals[dealManager].Id);
                                    break;
                                }
                                case (3):
                                {
                                    Console.WriteLine("Okay your current deals are");
                                    fuethersDeals = await _futhersDealService.GetAllFuethersDealsForUser(user.Id);
                                    for (var i = 0; i < fuethersDeals.Count; i++)
                                    {
                                        Console.WriteLine(
                                            $"Deal with id {fuethersDeals[i].Id}, type {fuethersDeals[i].TypeOfDeal} - {i}");
                                    }

                                    var dealManager = GetIntValue();
                                    await _futhersDealService.CloseDeal(fuethersDeals[dealManager].Id);
                                    fuethersDeals[dealManager] =
                                        await _futhersDealService.GetSingleByCondition(e =>
                                            e.Id == fuethersDeals[dealManager].Id);
                                    break;
                                }
                            }

                            break;
                        }
                        case (5):
                        {
                            Console.WriteLine(
                                "Okay you want to make deposit or withdraw, for first one write 1 for other 2");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine(
                                        $"Okay how much you want to deposit, just write amount. Your current balance is {user.Balance}");
                                    var amount = GetDoubleValueFromConsole();
                                    if (amount < 0)
                                    {
                                        Console.WriteLine("Ah you stupid");
                                        break;
                                    }

                                    await _userService.IncreaseBalance(user.Id, amount);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine(
                                        $"Okay how much you want to withdraw, just write amount. Your balance is {user.Balance}");
                                    var amount = GetDoubleValueFromConsole();
                                    if (amount < 0 || amount > user.Balance)
                                    {
                                        Console.WriteLine("Ah you stupid");
                                        break;
                                    }

                                    await _userService.DecreaseBalance(user.Id, amount);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }

                            }

                            break;
                        }
                        case (6):
                        {
                            Console.WriteLine("Okay what do you want to do?");
                            Console.WriteLine("Open the deposit - 1");
                            Console.WriteLine("Try to close the deposit - 2");
                            Console.WriteLine("Get all my deposits - 3");
                            Console.WriteLine("Get all my open deposits (without closed ones) - 4");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine(
                                        "Okay, enter name of the coin for deposit: BTC ETH LTC BNB SOL XRP");
                                    Enum.TryParse(Console.ReadLine().ToUpper(), out NameOfCoin coin);
                                    Console.WriteLine(
                                        $"Your current balance is {user.Balance}, how much do you deposit");
                                    var amountInUsdtForDeposit = GetDoubleValueFromConsole();
                                    Console.WriteLine(
                                        "Pick the period of deposit in month, be attentive you can't change it");
                                    var period = GetIntValue();
                                    if (period < 0)
                                    {
                                        Console.WriteLine("Man what are you doing");
                                        break;
                                    }

                                    myCurrentDepositDeal =
                                        await _userService.OpenDepositDeal(coin, amountInUsdtForDeposit, user.Id,
                                            period);
                                    depositDeals.Add(myCurrentDepositDeal);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine("Okay, there are your deposits, pick one of them");
                                    for (var i = 0; i < depositDeals.Count; i++)
                                    {
                                        Console.WriteLine(
                                            $"Deal with id {depositDeals[i].Id}, opened {depositDeals[i].TimeOfOpen} - {i}");
                                    }

                                    var indexOfdeal = GetIntValue();
                                    if (indexOfdeal < 0)
                                    {
                                        Console.WriteLine("Man what are you doing");
                                        break;
                                    }

                                    await _userService.CloseTheDeal(depositDeals[indexOfdeal].Id);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    break;
                                }
                                case (3):
                                {
                                    Console.WriteLine("Okay, there are your deposits");
                                    for (var i = 0; i < depositDeals.Count; i++)
                                    {
                                        Console.WriteLine(
                                            $"Deal with id {depositDeals[i].Id}, opened {depositDeals[i].TimeOfOpen}.");
                                    }

                                    depositDeals = await _userService.GetAllDealsForUser(user.Id);
                                    break;
                                }
                                case (4):
                                {
                                    Console.WriteLine("Okay, there are your open deposits");
                                    for (var i = 0; i < depositDeals.Count; i++)
                                    {
                                        Console.WriteLine(
                                            $"Deal with id {depositDeals[i].Id}, opened {depositDeals[i].TimeOfOpen}. It closes {depositDeals[i].CloseTime}");
                                    }

                                    depositDeals = await _userService.GetAllOpenDealsForUser(user.Id);
                                    break;
                                }
                            }

                            break;
                        }
                        case (7):
                        {
                            Console.WriteLine("Okay what do you want to do?");
                            Console.WriteLine("Change copy trading balance - 1");
                            Console.WriteLine("Become available for copy trading - 2");
                            Console.WriteLine("Become unavailable for copy trading- 3");
                            Console.WriteLine("Follow somebody - 4");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine(
                                        "Okay if you want increase write 1, if you want decrease write 2");
                                    var input = GetIntValue();
                                    if (input == 1)
                                    {
                                        Console.WriteLine(
                                            $"Write amount which you want send to copy trading balance. Your balance is {user.Balance}, your copy trading balance is {user.BalanceForCopyTrading}");
                                        var amount = GetDoubleValueFromConsole();
                                        user = await _userService.ModifyCopyTradingBalance(user.Id, amount, true);
                                    }

                                    if (input == 2)
                                    {
                                        Console.WriteLine(
                                            $"Write amount which you want sell from copy trading balance. Your balance is {user.Balance}, your copy trading balance is {user.BalanceForCopyTrading}");
                                        var amount = GetDoubleValueFromConsole();
                                        user = await _userService.ModifyCopyTradingBalance(user.Id, amount, false);
                                    }

                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine(
                                        "Now you are available for copy trading, anybody can copy your deposits");
                                    user = await _userService.BecomeAvailableForCopyTrading(user.Id);
                                    break;
                                }
                                case (3):
                                {
                                    Console.WriteLine(
                                        "Now you are not available for copy trading, nobody can copy your deposits");
                                    user = await _userService.BecomeUnAvailableForCopyTradingTradingContract(user.Id);
                                    break;
                                }
                                case (4):
                                {
                                    var traders = await _userService.GetAvailableCopyTraders();
                                    
                                    if (traders.Count == 0 || (traders.Count == 1 && traders[0].Id == user.Id))
                                    {
                                        Console.WriteLine("There are no available traders now");
                                        break;
                                    }
                                    Console.WriteLine("Okay there are all available for trading users");
                                    for (var i = 0; i < traders.Count; i++)
                                    {
                                        if (traders[i].Id != user.Id)
                                        {
                                            Console.WriteLine(
                                                $"Trader {traders[i].Name} {traders[i].Surname}, his balance is {traders[i].Balance}, if you want to pick him write {i}");
                                        }
                                    }

                                    var input = GetIntValue();
                                    await _userService.FollowSomebody(user.Id, traders[input].Id);
                                    Console.WriteLine("Nice you successfully followed him");
                                    break;
                                }
                            }

                            break;
                        }
                        case (8):
                        {
                            if (user.Role == Role.User)
                            {
                                Console.WriteLine("Ah ah ah cheater go away");
                                break;
                            }

                            Console.WriteLine($"Hi choose what you want to do as Support");
                            Console.WriteLine("Create new Support - 1");
                            Console.WriteLine("Take ticket - 2");
                            Console.WriteLine("Close ticket - 3");
                            Console.WriteLine("Get open tickets - 4");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    var availableUsers = await _userService.GetListByCondition(e => e.IsBanned == false && e.Id != user.Id && e.Role == Role.User);
                                    var availableUsers1 = availableUsers.ToList();
                                    Console.WriteLine("All available users:");
                                    for (var i = 0; i < availableUsers1.Count(); i++)
                                    {
                                        Console.WriteLine($"First name - {availableUsers1[i].Name} Surname - {availableUsers1[i].Surname} Id - {availableUsers1[i].Id}  if you to choose him write {i}");
                                    }

                                    var input = GetIntValue();
                                    Console.WriteLine("Sure now he is a support");
                                            availableUsers1[input] = await _supportService.CreateNewSupport(availableUsers1[input].Id);
                                    Console.WriteLine(
                                        $"His salary is {support.Salary} and his experience is {support.Experience}");
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine(
                                        "Ok, here are all open ticket choose number of which you want to take");
                                    tickets = await _supportService.GetOpenTickets();
                                    for (var i = 0; i < tickets.Count; i++)
                                    {
                                        var opener =
                                            await _userService.GetSingleByCondition(e => e.Id == tickets[i].UserId);
                                        Console.WriteLine(
                                            $"Ticket with id {tickets[i].Id}, user which opened is {opener.Name} {opener.Surname} - {i}");
                                    }

                                    var ticketPlace = GetIntValue();
                                    await _supportService.TakeTicket(support.Id, tickets[ticketPlace].Id);
                                    support = await _supportService.GetSingleByCondition(e => e.Id == support.Id);
                                    break;
                                }
                                case (3):
                                {
                                    support.TicketInProgress = await _userService.GetTicketById(support.TicketInProgressId.Value);
                                    Console.WriteLine(
                                        $"Ok, your ticket in progress is {support.TicketInProgress.Id}, " +
                                        $"it has {support.TicketInProgress.ChatHistory.Count} messages in chat history");
                                    Console.WriteLine("Are you sure that you want to close it?");
                                    var input = Console.ReadLine();
                                    if (input == "yes")
                                    {
                                        await _supportService.CloseTicket(support.TicketInProgress.Id,
                                            user.Id);
                                    }

                                    support = await _supportService.GetSingleByCondition(e => e.Id == support.Id);
                                    break;
                                }
                                case (4):
                                {
                                    Console.WriteLine("Sure there are all open tickets at this moment");
                                    tickets = await _supportService.GetOpenTickets();
                                    foreach (var ticketInProgress in tickets)
                                    {
                                        var opener =
                                            await _userService.GetSingleByCondition(
                                                e => e.Id == ticketInProgress.UserId);
                                        Console.WriteLine(
                                            $"Id of ticket is {ticketInProgress.Id}, chat history has {ticketInProgress.ChatHistory.Count}, " +
                                            $"user who opened it is {opener.Name} {opener.Surname}");
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case (9):
                        {
                            if (user.Role != Role.Admin)
                            {
                                Console.WriteLine("Ah ah ah cheater go away");
                                break;
                            }

                            Console.WriteLine($"Hi choose what you want to do as Admin");
                            Console.WriteLine("Ban somebody - 1");
                            Console.WriteLine("Unban somebody - 2");
                            Console.WriteLine("Delete somebody profile- 3");
                            Console.WriteLine("Change profile info - 4");
                            Console.WriteLine("Create new admin - 5");
                            switch (GetIntValue())
                            {
                                case (1):
                                {
                                    Console.WriteLine("Nice lets ban somebody");
                                    var users = await _userService.GetListByCondition(e => e.IsBanned == false && e.Id != admin.Id);
                                    var usersCollection = users.ToList();
                                    for (int i = 0; i < usersCollection.Count(); i++)
                                    {
                                        Console.WriteLine(
                                            $"User {usersCollection[i].Name} {usersCollection[i].Surname} with id {usersCollection[i].Id} - {i}");
                                    }

                                    Console.WriteLine("Enter the user for ban");
                                    var userForBan = GetIntValue();
                                    await _adminService.BanUser(usersCollection[userForBan].Id, admin.Id);
                                    admin = await _adminService.GetSingleByCondition(e => e.Id == admin.Id);
                                    break;
                                }
                                case (2):
                                {
                                    Console.WriteLine("Nice lets unban somebody");
                                    var users = await _userService.GetListByCondition(e => e.IsBanned == true);
                                    var usersCollection = users.ToList();
                                    for (int i = 0; i < usersCollection.Count(); i++)
                                    {
                                        Console.WriteLine(
                                            $"User {usersCollection[i].Name} {usersCollection[i].Surname} with id {usersCollection[i].Id} - {i}");
                                    }

                                    Console.WriteLine("Enter the user for ban");
                                    var userForUnban = GetIntValue();
                                    await _adminService.UnbanUserAccount(usersCollection[userForUnban].Id, admin.Id);
                                    admin = await _adminService.GetSingleByCondition(e => e.Id == admin.Id);
                                    break;
                                }
                                case (3):
                                {
                                    Console.WriteLine("Nice there are all accounts");
                                    var users = await _userService.GetListByCondition(e => e != null && e.Id != admin.Id);
                                    var usersCollection = users.ToList();
                                    for (int i = 0; i < usersCollection.Count(); i++)
                                    {
                                        Console.WriteLine(
                                            $"User {usersCollection[i].Name} {usersCollection[i].Surname} with id {usersCollection[i].Id} - {i}");
                                    }

                                    Console.WriteLine("Enter the user which account you want to delete");
                                    var userForDeleting = GetIntValue();
                                    await _adminService.DeleteUserAccount(usersCollection[userForDeleting].Id,
                                        admin.Id);
                                    admin = await _adminService.GetSingleByCondition(e => e.Id == admin.Id);
                                    break;
                                }
                                case (4):
                                {
                                    Console.WriteLine("Okay yours current info is:");
                                    Console.WriteLine(
                                        $"Name - {user.Name} \nSurname - {user.Surname} \nEmail - {user.Email} \nAge - {user.Age}" +
                                        $" \nCountry - {user.Country} \nPhone - {user.PhoneNumber} \nGender - {user.Gender}");
                                    Console.WriteLine("Okay let's change user info");
                                    var newProfileBase = await CreateUserInfo();
                                    await _adminService.ChangeUserInfo(user.Id, newProfileBase, admin.Id);
                                    user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
                                    admin = await _adminService.GetSingleByCondition(e => e.Id == admin.Id);
                                    break;
                                }
                                case (5):
                                {
                                    Console.WriteLine(
                                        "Nice there are all accounts, choose which do you want to make an admin");
                                    var users = await _userService.GetListByCondition(e => e != null && e.Id != admin.Id);
                                    var usersCollection = users.ToList();
                                    for (int i = 0; i < usersCollection.Count(); i++)
                                    {
                                        Console.WriteLine(
                                            $"User {usersCollection[i].Name} {usersCollection[i].Surname} with id {usersCollection[i].Id} - {i}");
                                    }

                                    Console.WriteLine("Enter the user which you want to make an admin");
                                    var userForDeleting = GetIntValue();
                                    user = await _adminService.CreateAdmin(usersCollection[userForDeleting].Id);
                                    Console.WriteLine("Sure now he is an admin");
                                    Console.WriteLine(
                                        $"His salary is {(user as Admin).Salary} and his experience is {(user as Admin).Experience}");
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                break;
            }
            catch (CustomAttributeFormatException e)
            {
                Console.WriteLine("Do you want to create a new user?");
                if (Console.ReadLine() == "yes")
                {
                    var newUserBase = await CreateUserInfo();
                    user = await _userService.CreateNewUser(newUserBase);
                    Console.WriteLine("Your seed phrase is: ");
                    foreach (var word in user.Wallet.SeedPhrase.SeedPhraseValues)
                    {
                        Console.Write($"{word} ");
                    }

                    Console.WriteLine("");
                    loggedIn = true;
                }
                else
                {
                    Console.WriteLine("Okay bye");
                    break;
                }
            }
        }
    }
    private async Task<ProfileBase> CreateUserInfo()
    {
        Console.WriteLine("Enter your name");
        var name = Console.ReadLine();
        Console.WriteLine("Enter your surname");
        var surname = Console.ReadLine();
        Console.WriteLine("Enter your email");
        var email = Console.ReadLine();
        Console.WriteLine("Enter your age");
        var age = GetIntValue();
        Console.WriteLine("Enter your county");
        var country = Console.ReadLine();
        Console.WriteLine("Enter your phone");
        var phone = Console.ReadLine();
        Console.WriteLine("Enter your address");
        var address = Console.ReadLine();
        Console.WriteLine("Choose your gender");
        for (var i = 0; i < Enum.GetValues(typeof(Gender)).Length; i++)
        {
            Console.WriteLine($"{Enum.GetValues(typeof(Gender)).GetValue(i)} - {i}");
        }
        var genderInput = GetIntValue();
        var id = Guid.NewGuid();
        var profile = new ProfileBase()
        {
            Name = name,
            Surname = surname,
            Email = email,
            Age = age,
            Gender = (Gender)genderInput,
            Id = id,
            Income = 0,
            Role = Role.User,
            FollowersIds = new List<Guid>(),
            Country = country,
            PhoneNumber = phone,
            Adress = address
        };
        return profile;
    }
    private async Task<User> Auth()
    {
        try
        {
            Console.WriteLine("Enter your email");
            var email = Console.ReadLine();
            Console.WriteLine("Enter your seed phrase with space between each word and anything else");
            var seedPhraseString = Console.ReadLine();


            var seedPhrase = new List<string>(seedPhraseString.Split(' '));
            if (seedPhrase.Count != 12)
            {
                return null;
            }
            var users = await _userService.GetListByCondition(e => e.Id != null);
            foreach (var user in users)
            {
                if (email != user.Email)
                {
                    continue;
                }
                var isAuth = true;
                var wallet = await _userService.GetMyWallet(user.Id);
                for (var i = 0; i < seedPhrase.Count; i++)
                {
                    if (seedPhrase[i] != wallet.SeedPhrase.SeedPhraseValues[i])
                    {
                        isAuth = false;
                        break;
                    }
                }

                if (isAuth)
                {
                    if (user.Role == Role.User)
                    {
                        return user;
                    }

                    if (user.Role == Role.Support)
                    {
                        return await _supportService.GetSingleByCondition(e => e.Id == user.Id);
                    }

                    if (user.Role == Role.Admin)
                    {
                        return await _adminService.GetSingleByCondition(e => e.Id == user.Id);
                    }
                }
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to auth");
            return null;
        }
    }
    private async Task SellCoin(User user, NameOfCoin coinName)
    {
        user.Wallet = await _userService.GetMyWallet(user.Id);
        var coin = user.Wallet.AmountOfCoins
            .Where(e => e.Name == coinName).ElementAt(0);
        Console.Write($"Good, enter amount which you want to sell, your amount of this {coin.Amount}");
        coin = await _coinService.UpdatePrice(coin.Id);
        Console.WriteLine($" and current price is {coin.Price}");
        var amount = GetDoubleValueFromConsole();
        await _userService.SellCoin(user.Id, coinName, amount);
    }
    private async Task BuyCoin(User user, NameOfCoin coinName)
    {
        user.Wallet = await _userService.GetMyWallet(user.Id);
        var coin = user.Wallet.AmountOfCoins
            .Where(e => e.Name == coinName).ElementAt(0);
        coin = await _coinService.UpdatePrice(coin.Id);
        Console.WriteLine(
            $"Good, enter amount which you want to buy, your balance is {user.Balance} " +
            $"and current price is {coin.Price}");
        var amount = GetDoubleValueFromConsole();
        await _userService.BuyCoin(user.Id, coinName, amount);
        user = await _userService.GetSingleByCondition(e => e.Id == user.Id);
    }
    private async Task DisplayGraph(List<double> priceHistory)
    {
        var graph = new List<List<char>>();
        int maxColumnHeight = 0;

        for (var i = priceHistory.Count - 2; i >= 0; i--) 
        {
            double percentIncrease = ((priceHistory[i] - priceHistory[i + 1]) / priceHistory[i + 1]) * 100;

            int numberOfSymbols = Math.Abs((int)(percentIncrease / 0.32));
            if (numberOfSymbols == 0)
            {
                numberOfSymbols = 1;
            }
            var column = new List<char>();
            for (int j = 0; j < numberOfSymbols; j++)
            {
                if (percentIncrease > 0)
                {
                    column.Add('^'); 
                }
                else if (percentIncrease < 0)
                {
                    column.Add('v'); 
                }
                else
                {
                    column.Add('-'); 
                }
            }
            graph.Add(column);

            if (column.Count > maxColumnHeight)
            {
                maxColumnHeight = column.Count;
            }
        }
        var updatedGraph = new List<List<char>>();
        for (var o = 0; o < graph.Count; o++)
        {
            updatedGraph.Add(new List<char>());
        }

        for (var f = 0; f < graph[0].Count; f++)
        {
            updatedGraph[0].Add(graph[0][f]);
        }
        for (var i = 1; i < graph.Count; i++)
        {
            for (var j = 0; j < graph[i - 1].Count; j++)
            {
                if (graph[i - 1][j] == ' ' || graph[i - 1][j] == 'v')
                {
                    updatedGraph[i].Add(' ');
                }
                if (graph[i - 1][j] == '^')
                {
                    break;
                }
            }

            for (var k = 0; k < graph[i].Count; k++)
            {
                if (graph[i][k] == '^')
                {
                    updatedGraph[i].Add(graph[i][k]);
                }
                updatedGraph[i].Add(graph[i][k]);
            }
        }


        for (int i = 0; i < maxColumnHeight; i++)
        {
            for (int j = 0; j < updatedGraph.Count; j++)
            {
                if (i < updatedGraph[j].Count)
                {
                    Console.Write(updatedGraph[j][i]);
                }
                else
                {
                    Console.Write(' ');
                }
            }
            Console.WriteLine();
        }
    }
    private double GetDoubleValueFromConsole()
    {
        return Convert.ToDouble(Console.ReadLine(), new System.Globalization.CultureInfo("en-US"));
    }
    private int GetIntValue()
    {
        return Convert.ToInt32(Console.ReadLine());
    }
}