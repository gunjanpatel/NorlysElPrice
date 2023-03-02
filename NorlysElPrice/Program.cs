using System.Text.Json;
using NorlysElPrice.DataObject;
using NorlysElPrice.Service;

namespace NorlysElPrice;

internal class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length > 0)
        {
            var fromDateOrTime = args[0];
        }
        
        var prices = await ProcessFlexElPriceList();

        var selectedDisplayPrice = prices.FirstOrDefault(p => p.PriceDate == DateTime.Today, new Price()).DisplayPrices;

        if (selectedDisplayPrice == null)
        {
            Console.WriteLine("No matching data found");

            return;
        }

        Console.WriteLine(DateTime.Today.ToString("dddd, dd MMMM yyyy"));

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

    private static async Task<List<Price>> ProcessFlexElPriceList()
    {
        var prices = await JsonSerializer.DeserializeAsync<List<Price>>(
            await Client.GetData(),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );

        return prices ?? new();
    }

    public static void GenerateOutput(string context, PriceData? priceData)
    {
        if (priceData == null)
        {
            return;
        }

        Console.WriteLine("");
        Console.WriteLine("--- " + context + " ---");
        Console.WriteLine("Time: " + priceData.Time + " Price: " + Math.Round(priceData.Value / 100, 2));
    }
}
