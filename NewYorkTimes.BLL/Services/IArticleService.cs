using NewYorkTimes.Models.BLL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewYorkTimes.BLL.Services
{
    public interface IArticleService
    {
        Task CheckAvailabilityAsync();
        Task<Article> GetFirstSectionArticleAsync(string section);
        Task<IEnumerable<Article>> GetSectionArticlesCollectionAsync(string section);
        Task<IEnumerable<Article>> GetSectionArticlesByDateAsync(string section, string updatedDateString);
        Task<Article> GetSectionArticlesByShortUrlAsync(string shortUrl);
    }
}