using BLL.Interfaces.IOperationsInterfaces;
using Core.Enums;
using Core.Models;
using Core.Models.BaseModels;
using Core.Models.Persons;
using Core.Models.Wallets;

namespace BLL.Interfaces.IServiceInterfaces;

public interface IUserService : IGenericService<User>, ISpotOperations, ICopyTradingOperations, IBalanceOperations, IDepositDeals
{
    Task<double> GetTotalWalletBalance(Guid id);
    Task<User> CreateNewUser(ProfileBase profile);
    Task<Ticket> CreateTicket(Guid userId);
    Task<Ticket> GetTicketById(Guid idOfTicket);
    Task SendMessageToTicketChat(Guid idOfTicket, Guid idOfAuthorOfMessage, string valueOfMessage);
    Task<List<Ticket>> GetAllMyTickets(Guid userId);
    Task<Wallet> GetMyWallet(Guid userId);
    Task<Ticket> GetChatHistory(Guid ticketId);
}