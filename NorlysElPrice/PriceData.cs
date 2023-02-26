using System.Text.Json;

namespace NorlysElPrice
{
    public record class PriceData()
    {
        public string Time { get; set; }
        public double Value { get; set; }
    }
}

