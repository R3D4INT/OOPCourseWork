using Core.Enums;
using System.Globalization;
using System.Text.Json;
using DAL.Interfaces;

namespace DAL.Implementations;

public class HttpRequests : IHttpRequests
{
    public async Task<List<double>> GetHistoricalPricesFromBinance(NameOfCoin name, string periodOfTime)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var url = $"https://api.binance.com/api/v3/klines?symbol={name}USDT&interval={periodOfTime}&limit=15";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<List<List<object>>>(responseBody);
                if (data == null)
                {
                    throw new Exception("Failed to deserialize data from file");
                }

                return data.Select(item => double.Parse(item[4].ToString(), CultureInfo.InvariantCulture)).ToList();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get price history of {name}");
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to get price history {ex.Message}");
            }
        }
    }
    public async Task<double> GetPriceFromBinance(NameOfCoin name)
    {
        using (var client = new HttpClient())
        {
            try
            {
                var url = $"https://api.binance.com/api/v3/ticker/price?symbol={name}USDT";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);
                var price = data["price"];
                return double.Parse(price, CultureInfo.InvariantCulture);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to get price from Binance");
            }
        }
    }
}