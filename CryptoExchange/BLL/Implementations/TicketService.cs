using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces.IServiceInterfaces;
using Core.Enums;
using Core.Models;
using DAL.Interfaces;

namespace BLL.Implementations
{
    public class TicketService : GenericService<Ticket>, ITicketService
    {
        private readonly IMessageService _messageService;
        public TicketService(IGenericRepository<Ticket> repository, IMessageService messageService) :
            base(repository)
        {
            _messageService = messageService;
        }
        public async Task<Ticket> CreateTicket(Guid userId)
        {
            try
            {
                var ticket = new Ticket() { ChatHistory = new List<Message>(), Status = Status.Open, Id = Guid.NewGuid(), UserId = userId };
                await Add(ticket);
                return ticket;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create ticket. {e.Message}");
            }
        }
        public async Task SendMessageToTicket(Guid idOfTicket, Guid author, string valueOfMessage)
        {
            try
            {
                if (author == null || valueOfMessage == null)
                {
                    throw new Exception("Value of string or user were empty");
                }
                var ticket = await GetSingleByCondition(e => e.Id == idOfTicket);
                if (ticket == null)
                {
                    throw new Exception("Failed to get ticket");
                }

                ticket.ChatHistory = await _messageService.GetChatHistoryOfTicket(ticket.Id);
                var message = await _messageService.CreateMessage(valueOfMessage, author, idOfTicket);
                ticket.ChatHistory.Add(message);
                await Update(ticket, e => e.Id == ticket.Id);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to send message {e.Message}");
            }
        }
        public async Task<Ticket> GetTicketById(Guid idOfTicket)
        {
            try
            {
                var ticket = await GetSingleByCondition(e => e.Id == idOfTicket);
                if (ticket == null)
                {
                    throw new Exception("Failed to get ticket");
                }
                ticket.ChatHistory = await _messageService.GetChatHistoryOfTicket(ticket.Id);
                return ticket;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get ticket");
            }
        }
    }
}
