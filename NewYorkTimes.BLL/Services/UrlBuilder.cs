namespace NewYorkTimes.BLL.Services
{
    public static class UrlBuilder
    {
        public static string BuildSectionUrl(string baseUrl, string section, string apiKey)
        {
            return string.Format(baseUrl, section, apiKey);
        }
    }
}
