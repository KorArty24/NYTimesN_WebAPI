using System.Threading.Tasks;

namespace NewYorkTimes.Communication.RestApi
{
    public interface IRestApiService
    {
        Task<TResponse> GetAsync<TResponse>(string path);
    }
}