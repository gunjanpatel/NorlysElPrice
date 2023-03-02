namespace NorlysElPrice
{
    public class Config
	{
        public static bool IsProd => Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT") != "dev";
    }
}

