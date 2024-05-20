using Core.Enums;
using Core.Models.BaseModels;

namespace Core.Models.Wallets;

public class Coin : BaseEntity
{
    public NameOfCoin Name { get; set; }
    public double Price { get; set; }
    public double Amount { get; set; } = 0;
    public Guid WalletId { get; set; }
}