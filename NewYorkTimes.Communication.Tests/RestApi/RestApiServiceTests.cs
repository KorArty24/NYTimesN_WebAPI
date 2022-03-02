using AutoMapper;
using Moq;
using Moq.Protected;
using NewYorkTimes.Communication.RestApi;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NewYorkTimes.Communication.Tests.RestApi
{
    public class RestApiServiceTests
    {
        private RestApiService _restApiService;
        private Mock<HttpClient> _httpClientMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"results\":[{\"title\":\"AAA\"}]}")
                })
                .Verifiable();

            _httpClientMock = new Mock<HttpClient>(httpMessageHandler.Object);

            _mapperMock = new Mock<IMapper>();

            _restApiService = new RestApiService(_httpClientMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAsync_ReturnMappedResult()
        {
            // arrange
            string expectedResult = "AAA";
            _mapperMock.Setup(x => x.Map<string>(It.IsAny<string>())).Returns(expectedResult);

            // act
            string result = await _restApiService.GetAsync<string>("https://api.nytimes.com/svc/topstories/v2/{0}.json?api-key={1}");

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}