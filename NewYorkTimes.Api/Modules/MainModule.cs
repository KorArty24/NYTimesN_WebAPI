using AutoMapper;
using Nancy;
using Newtonsoft.Json;
using NewYorkTimes.BLL.Services;
using NewYorkTimes.Models.Api;
using NewYorkTimes.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewYorkTimes.Api.Modules
{
    public class MainModule : NancyModule
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public MainModule(IArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
            SetEndpoints();
        }

        private void SetEndpoints()
        {
            Get("/", async _ => JsonConvert.SerializeObject(await CheckAvailabilityAsync()));

            Get("/list/{section}", async _ => JsonConvert.SerializeObject(await GetSectionArticlesCollectionAsync(_.section)));

            Get("/list/{section}/first", async _ => JsonConvert.SerializeObject(await GetFirstSectionArticleAsync(_.section)));

            Get("/list/{section}/{updatedDate}", async _ => JsonConvert.SerializeObject(await GetSectionArticlesByDateAsync(_.section, _.updatedDate)));

            Get("/article/{shortUrl}", async _ => JsonConvert.SerializeObject(await GetSectionArticlesByShortUrlAsync(_.shortUrl)));

            Get("/group/{section}", async _ => JsonConvert.SerializeObject(await GetSectionArticlesGroupedByDateAsync(_.section)));
        }

        public async Task<object> CheckAvailabilityAsync()
        {
            try
            {
                await _articleService.CheckAvailabilityAsync();

                return "New York Times API is functional.";
            }
            catch (Exception ex)
            {
                return $"New York Times API is not functional - {ex.Message}.";
            }
        }

        public async Task<object> GetSectionArticlesCollectionAsync(string section)
        {
            try
            {
                IEnumerable<Article> articles = await _articleService.GetSectionArticlesCollectionAsync(section);

                return _mapper.Map<IEnumerable<ArticleView>>(articles);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object> GetFirstSectionArticleAsync(string section)
        {
            try
            {
                Article article = await _articleService.GetFirstSectionArticleAsync(section);

                return _mapper.Map<ArticleView>(article);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object> GetSectionArticlesByDateAsync(string section, string updatedDateString)
        {
            try
            {
                IEnumerable<Article> articles = await _articleService.GetSectionArticlesByDateAsync(section, updatedDateString);

                return _mapper.Map<IEnumerable<ArticleView>>(articles);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object> GetSectionArticlesGroupedByDateAsync(string section)
        {
            try
            {
                IEnumerable<Article> articles = await _articleService.GetSectionArticlesCollectionAsync(section);

                return articles.GroupBy(x => x.Updated.Date).Select(x => new ArticleGroupByDateView
                {
                    Date = x.Key.ToString("yyyy-MM-dd"),
                    Total = x.Count()
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object> GetSectionArticlesByShortUrlAsync(string shortUrl)
        {
            try
            {
                Article article = await _articleService.GetSectionArticlesByShortUrlAsync(shortUrl);

                return _mapper.Map<ArticleView>(article);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
