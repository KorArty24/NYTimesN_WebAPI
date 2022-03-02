using AutoMapper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewYorkTimes.Communication.RestApi
{
    public class RestApiService : IRestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public RestApiService(HttpClient client, IMapper mapper)
        {
            _httpClient = client;
            _mapper = mapper;
        }

        public async Task<TResponse> GetAsync<TResponse>(string path)
        {
            using (var response = await _httpClient.GetAsync(path))
            {
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<dynamic>(content).results;

                var reslt = _mapper.Map<TResponse>(results);
                return _mapper.Map<TResponse>(results);
            }
        }
    }
}
