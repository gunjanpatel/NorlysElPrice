using System.Net.Http.Headers;

namespace NorlysElPrice.Service
{
	public class Client
	{
        public static async Task<Stream> GetData()
        {
            if (Config.IsProd)
            {
                using HttpClient client = new();

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.GetStreamAsync("https://norlys.dk/api/flexel/getall?days=1&sector=DK2");
            }

            return File.OpenRead(@"../../../SampleData/prices.json");
        }
    }
}

