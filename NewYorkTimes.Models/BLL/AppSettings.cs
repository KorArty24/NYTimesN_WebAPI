namespace NewYorkTimes.Models.BLL
{
    public sealed class AppSettings : IAppSettings
    {
        public string BaseUrl { get; set; }

        public string ApiKey { get; set; }

        public string BaseSection { get; set; }
    }
}
