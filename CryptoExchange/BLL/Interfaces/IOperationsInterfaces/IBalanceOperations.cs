namespace BLL.Interfaces.IOperationsInterfaces;

public interface IBalanceOperations
{
    Task IncreaseBalance(Guid userId, double amount);
    Task DecreaseBalance(Guid userId, double amount);
}