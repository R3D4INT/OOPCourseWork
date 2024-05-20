using BLL.Interfaces.IServiceInterfaces;
using Core.Models;
using DAL.Interfaces;

namespace BLL.Implementations;

public class MessageService : GenericService<Message>, IMessageService
{
    public MessageService(IGenericRepository<Message> repository) :
        base(repository)
    {
    }

    public async Task<Message> CreateMessage(string valueOfMessage, Guid authorId, Guid idOfTicket)
    {
        try
        {
            var message = new Message() { AuthorId = authorId, Value = valueOfMessage, Id = Guid.NewGuid(), TicketId = idOfTicket};

            await Add(message);
            return message;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create message. {e.Message}");
        }
    }

    public async Task<List<Message>> GetChatHistoryOfTicket(Guid ticketId)
    {
        try
        {
            var chatHistory = await GetListByCondition(e => e.TicketId == ticketId);
            return (List<Message>)chatHistory;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}