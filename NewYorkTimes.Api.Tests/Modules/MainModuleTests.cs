using AutoMapper;
using Moq;
using NewYorkTimes.Api.Modules;
using NewYorkTimes.BLL.Services;
using NewYorkTimes.Models.Api;
using NewYorkTimes.Models.BLL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewYorkTimes.Api.Tests.Modules
{
    public class MainModuleTests
    {
        private MainModule _mainModule;
        private Mock<IArticleService> _articleServiceMock;
        private Mock<IMapper> _mapperMock;

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

        private readonly List<ArticleView> expectedViewCollection = new List<ArticleView>
        {
            new ArticleView
            {
                Heading = "ABC",
                Link = "Link1",
                Updated = DateTime.Today
            },
            new ArticleView
            {
                Heading = "DEF",
                Link = "Link2",
                Updated = DateTime.Today.AddDays(1)
            },
            new ArticleView
            {
                Heading = "ASX",
                Link = "Link3",
                Updated = DateTime.Today.AddDays(-1)
            }
        };

        [SetUp]
        public void Setup()
        {
            _articleServiceMock = new Mock<IArticleService>();
            _mapperMock = new Mock<IMapper>();

            _mainModule = new MainModule(_articleServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CheckAvailabilityAsync_ReturnMessageApiIsFunctional()
        {
            // act
            object result = await _mainModule.CheckAvailabilityAsync();

            // assert
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual("New York Times API is functional.", result);
        }

        [Test]
        public async Task CheckAvailabilityAsync_ReturnMessageApiIsNotFunctional()
        {
            // arrange
            var exception = new Exception("Error message");

            _articleServiceMock.Setup(x => x.CheckAvailabilityAsync()).ThrowsAsync(exception);

            // act
            object result = await _mainModule.CheckAvailabilityAsync();

            // arrange
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual($"New York Times API is not functional - {exception.Message}.", result);
        }

        [Test]
        public async Task GetSectionArticlesCollectionAsync_ReturnArticleViewCollection()
        {
            // arrange
            string section = "home";
            _articleServiceMock.Setup(x => x.GetSectionArticlesCollectionAsync(section)).ReturnsAsync(articlesCollection);
            _mapperMock.Setup(x => x.Map<IEnumerable<ArticleView>>(articlesCollection)).Returns(expectedViewCollection);

            // act
            object result = await _mainModule.GetSectionArticlesCollectionAsync(section);

            // assert
            Assert.IsInstanceOf<IEnumerable<ArticleView>>(result);

            Assert.AreEqual(expectedViewCollection, result);
        }

        [Test]
        public async Task CheckAvailabilityAsync_ReturnExceptionMessage()
        {
            // arrange
            var exception = new Exception("Error message");

            string section = "home";
            _articleServiceMock.Setup(x => x.GetSectionArticlesCollectionAsync(section)).ThrowsAsync(exception);

            // act
            object result = await _mainModule.GetSectionArticlesCollectionAsync(section);

            // arrange
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual("Error message", result);
        }

        [Test]
        public async Task GetSectionArticlesCollectionAsync_ReturnArticleView()
        {
            // arrange
            string section = "home";
            _articleServiceMock.Setup(x => x.GetFirstSectionArticleAsync(section)).ReturnsAsync(articlesCollection.First());
            _mapperMock.Setup(x => x.Map<ArticleView>(articlesCollection.First())).Returns(expectedViewCollection.First());

            // act
            object result = await _mainModule.GetFirstSectionArticleAsync(section);

            // assert
            Assert.IsInstanceOf<ArticleView>(result);

            Assert.AreEqual(expectedViewCollection.First(), result);
        }

        [Test]
        public async Task GetSectionArticlesCollectionAsync_ReturnExceptionMessage()
        {
            // arrange
            var exception = new Exception("Error message");

            string section = "home";
            _articleServiceMock.Setup(x => x.GetFirstSectionArticleAsync(section)).ThrowsAsync(exception);

            // act
            object result = await _mainModule.GetFirstSectionArticleAsync(section);

            // arrange
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual("Error message", result);
        }

        [Test]
        public async Task GetSectionArticlesByDateAsync_ReturnArticleViewCollection()
        {
            // arrange
            string section = "home";
            string updatedDateString = "02-03-2022";

            _articleServiceMock.Setup(x => x.GetSectionArticlesByDateAsync(section, updatedDateString)).ReturnsAsync(articlesCollection);
            _mapperMock.Setup(x => x.Map<IEnumerable<ArticleView>>(articlesCollection)).Returns(expectedViewCollection);

            // act
            object result = await _mainModule.GetSectionArticlesByDateAsync(section, updatedDateString);

            // assert
            Assert.IsInstanceOf<IEnumerable<ArticleView>>(result);

            Assert.AreEqual(expectedViewCollection, result);
        }

        [Test]
        public async Task GetSectionArticlesByDateAsync_ReturnExceptionMessage()
        {
            // arrange
            var exception = new Exception("Error message");

            string section = "home";
            string updatedDateString = "02-03-2022";

            _articleServiceMock.Setup(x => x.GetSectionArticlesByDateAsync(section, updatedDateString)).ThrowsAsync(exception);

            // act
            object result = await _mainModule.GetSectionArticlesByDateAsync(section, updatedDateString);

            // arrange
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual("Error message", result);
        }

        [Test]
        public async Task GetSectionArticlesGroupedByDateAsync_ReturnArticleViewCollection()
        {
            // arrange
            string section = "home";
            var expectedGroupedViewCollection = new List<ArticleGroupByDateView>
            {
                new ArticleGroupByDateView
                {
                    Total = 1,
                    Date = DateTime.Today.ToString("yyyy-MM-dd")
                },
                new ArticleGroupByDateView
                {
                    Total = 1,
                    Date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")
                },
                new ArticleGroupByDateView
                {
                    Total = 1,
                    Date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")
                }
            };

            _articleServiceMock.Setup(x => x.GetSectionArticlesCollectionAsync(section)).ReturnsAsync(articlesCollection);

            // act
            object result = await _mainModule.GetSectionArticlesGroupedByDateAsync(section);

            // assert
            Assert.IsInstanceOf<IEnumerable<ArticleGroupByDateView>>(result);
            var mappedResult = (result as IEnumerable<ArticleGroupByDateView>).ToList();

            for(int i = 0; i < mappedResult.Count; i++)
            {
                Assert.AreEqual(expectedGroupedViewCollection[i].Date, mappedResult[i].Date);
                Assert.AreEqual(expectedGroupedViewCollection[i].Total, mappedResult[i].Total);
            }
        }

        [Test]
        public async Task GetSectionArticlesGroupedByDateAsync_ReturnExceptionMessage()
        {
            // arrange
            var exception = new Exception("Error message");

            string section = "home";

            _articleServiceMock.Setup(x => x.GetSectionArticlesCollectionAsync(section)).ThrowsAsync(exception);

            // act
            object result = await _mainModule.GetSectionArticlesGroupedByDateAsync(section);

            // arrange
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual("Error message", result);
        }

        [Test]
        public async Task GetSectionArticlesByShortUrlAsync_ReturnArticleView()
        {
            // arrange
            string shortUrl = "wqewqe";
            _articleServiceMock.Setup(x => x.GetSectionArticlesByShortUrlAsync(shortUrl)).ReturnsAsync(articlesCollection.First());
            _mapperMock.Setup(x => x.Map<ArticleView>(articlesCollection.First())).Returns(expectedViewCollection.First());

            // act
            object result = await _mainModule.GetSectionArticlesByShortUrlAsync(shortUrl);

            // assert
            Assert.IsInstanceOf<ArticleView>(result);

            Assert.AreEqual(expectedViewCollection.First(), result);
        }

        [Test]
        public async Task GetSectionArticlesByShortUrlAsync_ReturnExceptionMessage()
        {
            // arrange
            var exception = new Exception("Error message");

            string shortUrl = "wqewqe";
            _articleServiceMock.Setup(x => x.GetSectionArticlesByShortUrlAsync(shortUrl)).ThrowsAsync(exception);

            // act
            object result = await _mainModule.GetSectionArticlesByShortUrlAsync(shortUrl);

            // arrange
            Assert.IsInstanceOf<string>(result);

            Assert.AreEqual("Error message", result);
        }
    }
}