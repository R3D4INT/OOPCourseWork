using Core.Models;
using Core.Models.Persons;

namespace BLL.Interfaces.IServiceInterfaces;

public interface ISupportService : IGenericService<Support>
{
    Task<Support> CreateNewSupport(Guid idOfUser);
    Task TakeTicket(Guid idOfSupport, Guid idOfTicket);
    Task CloseTicket(Guid ticketId, Guid supportId);
    Task<List<Ticket>> GetOpenTickets();
}