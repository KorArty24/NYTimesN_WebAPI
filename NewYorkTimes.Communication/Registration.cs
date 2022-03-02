using Nancy.TinyIoc;
using NewYorkTimes.Communication.RestApi;

namespace NewYorkTimes.Communication
{
    public static class Registration
    {
        public static TinyIoCContainer CommunicationRegistration(this TinyIoCContainer container)
        {
            container.Register<IRestApiService, RestApiService>();

            return container;
        }
    }
}
