using System.Text.Json;

namespace NorlysElPrice
{
    public record class Price()
    {
        public DateTime PriceDate { get; set; }
        public List<PriceData>? DisplayPrices { get; set; }
    }
}

