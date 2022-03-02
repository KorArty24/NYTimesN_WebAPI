using Moq;
using NewYorkTimes.BLL.Services;
using NewYorkTimes.Communication.RestApi;
using NewYorkTimes.Models.BLL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewYorkTimes.BLL.Tests.Services
{
    public class ArticleServiceTests
    {
        private ArticleService _articleService;
        private Mock<IRestApiService> _restApiServiceMock;
        private Mock<IAppSettings> _appSettingsMock;

        private readonly List<Article> articlesCollection = new List<Article>
        {
            new Article
            {
                Heading = "ABC",
                Link = "Link1",
                Updated = DateTime.Today
            },
            new Article
            {
                Heading = "DEF",
                Link = "Link2",
                Updated = DateTime.Today.AddDays(1)
            },
            new Article
            {
                Heading = "ASX",
                Link = "Link3",
                Updated = DateTime.Today.AddDays(-1)
            }
        };

        [SetUp]
        public void Setup()
        {
            _restApiServiceMock = new Mock<IRestApiService>();
            _appSettingsMock = new Mock<IAppSettings>();
            _appSettingsMock.Setup(x => x.BaseUrl).Returns("https://api.nytimes.com/svc/topstories/v2/{0}.json?api-key={1}");
            _appSettingsMock.Setup(x => x.ApiKey).Returns("k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ");
            _appSettingsMock.Setup(x => x.BaseSection).Returns("home");

            _articleService = new ArticleService(_restApiServiceMock.Object, _appSettingsMock.Object);
        }

        [Test]
        public async Task CheckAvailabilityAsync_CallGetAsync()
        {
            // act
            await _articleService.CheckAvailabilityAsync();

            // assert
            _restApiServiceMock.Verify(x => x.GetAsync<IEnumerable<Article>>("https://api.nytimes.com/svc/topstories/v2/home.json?api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ"), Times.Once);
        }

        [Test]
        public async Task GetSectionArticlesCollectionAsync_ReturnArticleCollection()
        {
            // arrange
            string section = "home";
            string url = $"https://api.nytimes.com/svc/topstories/v2/{section}.json?api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";

            _restApiServiceMock.Setup(x => x.GetAsync<IEnumerable<Article>>(url)).ReturnsAsync(articlesCollection);

            // act
            IEnumerable<Article> result = await _articleService.GetSectionArticlesCollectionAsync(section);

            // assert
            CollectionAssert.AreEqual(articlesCollection, result);
        }


        [Test]
        public async Task GetFirstSectionArticleAsync_ReturnArticleCollection()
        {
            // arrange
            string section = "home";
            string url = $"https://api.nytimes.com/svc/topstories/v2/{section}.json?api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";

            _restApiServiceMock.Setup(x => x.GetAsync<IEnumerable<Article>>(url)).ReturnsAsync(articlesCollection);

            // act
            Article result = await _articleService.GetFirstSectionArticleAsync(section);

            // assert
            Assert.AreEqual(articlesCollection.First(), result);
        }

        [Test]
        public async Task GetSectionArticlesByDateAsync_ThrowInvalidCastException()
        {
            // arrange
            string section = "home";
            string updatedDateString = "01-02-";

            // act and assert
            InvalidCastException ex = Assert.ThrowsAsync<InvalidCastException>(() => _articleService.GetSectionArticlesByDateAsync(section, updatedDateString));

            // assert
            Assert.AreEqual($"{updatedDateString} can not be recognized as a date.", ex.Message);
        }

        [Test]
        public async Task GetSectionArticlesByDateAsync_ReturnArticleCollection()
        {
            // arrange
            string section = "home";
            string updatedDateString = DateTime.Today.ToString();
            string url = $"https://api.nytimes.com/svc/topstories/v2/{section}.json?api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";
            
            _restApiServiceMock.Setup(x => x.GetAsync<IEnumerable<Article>>(url)).ReturnsAsync(articlesCollection);

            var expectedResult = new List<Article>
            {
                new Article
                {
                    Heading = "ABC",
                    Link = "Link1",
                    Updated = DateTime.Today
                }
            };

            // act
            IEnumerable<Article> result = await _articleService.GetSectionArticlesByDateAsync(section, updatedDateString);

            // assert
            var resultList = result.ToList();

            for (int i = 0; i < resultList.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].Heading, resultList[i].Heading);
                Assert.AreEqual(expectedResult[i].Link, resultList[i].Link);
                Assert.AreEqual(expectedResult[i].Updated, resultList[i].Updated);
            }
        }

        [Test]
        public async Task GetSectionArticlesByShortUrlAsync_ReturnArticleCollection()
        {
            // arrange
            string section = "home";
            string shortUrl = "1";
            string url = $"https://api.nytimes.com/svc/topstories/v2/{section}.json?api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";

            _restApiServiceMock.Setup(x => x.GetAsync<IEnumerable<Article>>(url)).ReturnsAsync(articlesCollection);

            var expectedResult = new Article
            {
                Heading = "ABC",
                Link = "Link1",
                Updated = DateTime.Today
            };

            // act
            Article result = await _articleService.GetSectionArticlesByShortUrlAsync(shortUrl);

            // assert
            Assert.AreEqual(expectedResult.Heading, result.Heading);
            Assert.AreEqual(expectedResult.Link, result.Link);
            Assert.AreEqual(expectedResult.Updated, result.Updated);
        }
    }
}