using Core.Models;

namespace BLL.Interfaces.IServiceInterfaces;

public interface IMessageService : IGenericService<Message>
{
    Task<Message> CreateMessage(string valueOfMessage, Guid author, Guid idOfTicket);

    Task<List<Message>> GetChatHistoryOfTicket(Guid ticketId);
}