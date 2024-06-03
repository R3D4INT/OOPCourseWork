using BLL.Interfaces.IServiceInterfaces;
using Core.Enums;
using Core.Models;
using Core.Models.BaseModels;
using Core.Models.Persons;
using DAL.Interfaces;

namespace BLL.Implementations;

public class SupportService : GenericService<Support>, ISupportService
{
    private readonly IWalletService _walletService;
    
    private readonly ITicketService _ticketService;

    private readonly IMessageService _messageService;

    private readonly IUserService _userService;

    public SupportService(IGenericRepository<Support> repository, IWalletService walletService,
        ITicketService ticketService, IMessageService messageService, IUserService userService) :
        base(repository)
    {
        _walletService = walletService;
        _ticketService = ticketService;
        _messageService = messageService;
        _userService = userService;
    }
    public async Task<Support> CreateNewSupport(Guid idOfUser)
    {
        try
        {
            var user = await _userService.GetSingleByCondition(e => e.Id == idOfUser);
            if (user == null)
            {
                throw new Exception("Failed to get user");
            }

            var support = new Support(user, 0, 1500);

            await _userService.Delete(e => e.Id == idOfUser);
            await Add(support);
            return support;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create support {e.Message}");
        }
    } 
    public async Task TakeTicket(Guid idOfSupport, Guid idOfTicket)
    {
        try
        {
            var ticket = await _ticketService.GetSingleByCondition(e => e.Id == idOfTicket);
            if (ticket == null)
            {
                throw new Exception("Ticket is null");
            }

            var support = await GetSingleByCondition(e => e.Id == idOfSupport);
            if (support == null)
            {
                throw new Exception("Support is null");
            }
            ticket.Status = Status.InProcess;
            await _ticketService.Update(ticket, e => e.Id == ticket.Id);
            support.TicketInProgressId = ticket.Id;
            await Update(support, e => e.Id == support.Id);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to take ticket {e.Message}");
        }
    }
    public async Task CloseTicket(Guid ticketId, Guid supportId)
    {
        try
        {
            var ticket = await _ticketService.GetSingleByCondition(e => e.Id == ticketId);
            if (ticket == null)
            {
                throw new Exception("Failed to get ticket");
            }
            ticket.Status = Status.Closed;
            await _ticketService.Update(ticket, e => e.Id == ticket.Id);
            var support = await GetSingleByCondition(e => e.Id == supportId);
            if (support == null)
            {
                throw new Exception("Failed to get support");
            }

            support.TicketInProgress = null;
            support.TicketInProgressId = null;
            support.Experience++;
            await Update(support, e => e.Id == support.Id);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to close ticket {e.Message}");
        }
    }
    public async Task<List<Ticket>> GetOpenTickets()
    {
        try
        {
            var tickets = await _ticketService.GetListByCondition(e => e.Status == Status.Open);
            var ticketsList = tickets.ToList();
            for (var i = 0; i < ticketsList.Count(); i++)
            {
                ticketsList[i].ChatHistory = await _messageService.GetChatHistoryOfTicket(ticketsList[i].Id);
            }
            return (List<Ticket>)tickets;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to get open tickets {e.Message}");
        }
    }
    
}