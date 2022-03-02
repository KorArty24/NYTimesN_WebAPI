namespace NewYorkTimes.Models.BLL
{
    public interface IAppSettings
    {
        string BaseUrl { get; set; }

        string ApiKey { get; set; }

        string BaseSection { get; set; }
    }
}
