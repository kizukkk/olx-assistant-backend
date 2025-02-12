using System.Globalization;
using System.Text.RegularExpressions;

public static class CurrencyConverter
{
    private static decimal _usdToUahRate;
    private static DateTime _lastFetched;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    public static async Task<decimal> ConvertUAH2USD(decimal uah)
    {
        decimal rate = await GetUAH2USDRateAsync();
        return uah / rate;
    }

    public static async Task<decimal> ConvertUSD2UAH(decimal usd)
    {
        decimal rate = await GetUAH2USDRateAsync();
        return usd * rate;
    }

    private static async Task<decimal> GetUAH2USDRateAsync()
    {
        if (_usdToUahRate != 0 && (DateTime.Now - _lastFetched) < CacheDuration)
        {
            return _usdToUahRate;
        }

        _usdToUahRate = await FetchUSD2UAHRateFromApi();
        _lastFetched = DateTime.Now;

        return _usdToUahRate;
    }

    private static async Task<decimal> FetchUSD2UAHRateFromApi()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode=USD&json");
            string responseBody = await response.Content.ReadAsStringAsync();
            
            var stringRate = Regex.Match(responseBody, @"rate\"":(\d+.\d+)").Groups[1].Value;
            
            return decimal.Parse(stringRate, CultureInfo.InvariantCulture);
        }
    }
}
