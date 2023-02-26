using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace NorlysElPrice;

internal class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length > 0)
        {
            var fromDateOrTime = args[0];
        }

        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        var prices = await ProcessFlexElPriceList(client);

        var selectedDisplayPrice = prices.First(p => p.PriceDate == DateTime.Today).DisplayPrices;

        if (selectedDisplayPrice != null)
        {
            GenerateOutput("Lowest", selectedDisplayPrice.MinBy(p => p.Value));
            GenerateOutput("Highest", selectedDisplayPrice.MaxBy(p => p.Value));
            GenerateOutput(
                    DateTime.Now.Hour.ToString(),
                    selectedDisplayPrice
                        .Where(
                            p => p.Time != null && Int16.Parse(p.Time) == DateTime.Now.Hour
                        ).FirstOrDefault()
                );

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

    public static void GenerateOutput(string time, PriceData? priceData)
    {
        if (priceData == null)
        {
            return;
        }

        Console.WriteLine("");
        Console.WriteLine("--- Now " + time + " ---");
        Console.WriteLine("Time: " + priceData.Time + " Price: " + Math.Round(priceData.Value / 100, 2));
    }
}
