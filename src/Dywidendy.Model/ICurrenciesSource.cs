using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dywidendy.Model
{
    public interface ICurrenciesSource
    {
         Task<CurrencyRate> GetByDate(string code, DateTime date);
    }

    public class NbpCurrenciesSource : ICurrenciesSource
    {
        public async Task<CurrencyRate> GetByDate(string code, DateTime date)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.nbp.pl/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                for (int i = 0; i < 10; i++)
                {
                    var response =
                    await client.GetAsync("api/exchangerates/tables/A/" + date.ToString("yyyy-MM-dd"));

                    if (response.IsSuccessStatusCode)
                    {
                        var product = await response.Content.ReadAsAsync<NbpApiCurrenciesResponse[]>();
                        return product[0].Rates.Single(p => p.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                    }
                }
                
                return null;
            }
            
        }
    }

    public class NbpApiCurrenciesResponse
    {
        [JsonProperty("rates")]
        public CurrencyRate[] Rates { get; set; }
    }

    public class CurrencyRate
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("mid")]
        public decimal Rate { get; set; }
    }
}