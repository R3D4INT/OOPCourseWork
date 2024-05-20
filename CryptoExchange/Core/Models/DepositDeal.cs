using Core.Enums;
using Core.Models.BaseModels;
using Core.Models.Persons;

namespace Core.Models;

public class DepositDeal : BaseEntity 
{
    public int PeriodInMonth { get; set; }
    public double AmountInUSDT { get; set; }
    public double ExpectableIncome { get; set; }
    public double MonthIncomeInPercents { get; set; }
    public NameOfCoin Coin { get; set; }
    public DateTime TimeOfOpen { get; set; }
    public DateTime CloseTime { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
}