using BLL.Implementations;
using Core.Models;

namespace BLL.Interfaces.IServiceInterfaces;

public interface ITicketService : IGenericService<Ticket>
{
    Task<Ticket> CreateTicket(Guid userId);
    Task SendMessageToTicket(Guid idOfTicket, Guid userId, string valueOfMessage);
    Task<Ticket> GetTicketById(Guid idOfTicket);
}