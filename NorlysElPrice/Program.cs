using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NorlysElPrice;

internal class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        var prices = await ProcessFlexElPriceList(client);

        foreach (var price in prices)
        {
            Console.WriteLine(price.PriceDate);

            foreach(var displayPrice in price.DisplayPrices ?? new())
            {
                Console.WriteLine("Time: " + displayPrice.Time);
                Console.WriteLine("Value: " + displayPrice.Value);
            }
        }
    }


    static async Task<List<Price>> ProcessFlexElPriceList(HttpClient client)
    {

        using FileStream stream = File.OpenRead(@"../../../SampleData/prices.json");
        //await using Stream stream =
        //    await client.GetStreamAsync("https://norlys.dk/api/flexel/getall?days=1&sector=DK2");

        var prices = await JsonSerializer.DeserializeAsync<List<Price>>(
            stream,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );

        return prices ?? new();
    }
}
