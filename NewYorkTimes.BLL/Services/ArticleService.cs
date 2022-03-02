using NewYorkTimes.Communication.RestApi;
using NewYorkTimes.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewYorkTimes.BLL.Services
{
    public sealed class ArticleService : IArticleService
    {
        private readonly IRestApiService _httpClient;
        private readonly IAppSettings _appSettings;

        public ArticleService(IRestApiService httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        public async Task CheckAvailabilityAsync()
        {
            await GetBaseSectionArticlesAsync();
        }

        public async Task<IEnumerable<Article>> GetSectionArticlesCollectionAsync(string section)
        {
            return await GetSectionArticlesAsync(section);
        }

        public async Task<Article> GetFirstSectionArticleAsync(string section)
        {
            IEnumerable<Article> articles = await GetSectionArticlesAsync(section);

            return articles.FirstOrDefault();
        }

        public async Task<IEnumerable<Article>> GetSectionArticlesByDateAsync(string section, string updatedDateString)
        {
            if (!DateTime.TryParse(updatedDateString, out DateTime updatedDate))
            {
                throw new InvalidCastException($"{updatedDateString} can not be recognized as a date.");
            }

            IEnumerable<Article> articles = await GetSectionArticlesAsync(section);

            return articles.Where(x => x.Updated.Date == updatedDate);
        }

        public async Task<Article> GetSectionArticlesByShortUrlAsync(string shortUrl)
        {
            IEnumerable<Article> articles = await GetBaseSectionArticlesAsync();

            return articles.FirstOrDefault(x => x.Link.ToLower().Contains(shortUrl.ToLower()));
        }

        private async Task<IEnumerable<Article>> GetSectionArticlesAsync(string section)
        {
            return await _httpClient.GetAsync<IEnumerable<Article>>(UrlBuilder.BuildSectionUrl(_appSettings.BaseUrl, section, _appSettings.ApiKey));
        }

        private async Task<IEnumerable<Article>> GetBaseSectionArticlesAsync()
        {
            return await _httpClient.GetAsync<IEnumerable<Article>>(UrlBuilder.BuildSectionUrl(_appSettings.BaseUrl, _appSettings.BaseSection, _appSettings.ApiKey));
        }
    }
}
