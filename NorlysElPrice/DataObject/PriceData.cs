namespace NorlysElPrice.DataObject
{
    public record class PriceData()
    {
        public string? Time { get; set; }
        public double Value { get; set; } = 0.0;
    }
}

