using System;
using System.Net.Http;
using System.Threading.Tasks;
using TypiCode.Domain.Interfaces;

namespace TypiCode.Business
{
    public class TypiCodeClient: ITypiCodeClient
    {
        public async Task<T> Execute<T>(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }

            throw new Exception($"Response t {Constants.PostsUrl} not successful.");
        }
    }
}
