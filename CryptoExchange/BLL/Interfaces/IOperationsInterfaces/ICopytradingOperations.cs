using Core.Enums;
using Core.Models.Persons;

namespace BLL.Interfaces.IOperationsInterfaces;

public interface ICopyTradingOperations
{
    Task<User> BecomeAvailableForCopyTrading(Guid userId);
    Task<User> BecomeUnAvailableForCopyTradingTradingContract(Guid userId);
    Task<List<User>> GetAvailableCopyTraders();
    Task FollowSomebody(Guid userId, Guid userForFollowId);
    Task<User> ModifyCopyTradingBalance(Guid idOfUser, double amount, bool isIncrease);
}