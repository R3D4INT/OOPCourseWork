using Core.Enums;

namespace DAL.Interfaces;

public interface IHttpRequests
{
    Task<List<double>> GetHistoricalPricesFromBinance(NameOfCoin name, string periodOfTime);
    Task<double> GetPriceFromBinance(NameOfCoin name);
}