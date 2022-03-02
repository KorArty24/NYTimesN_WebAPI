using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NewYorkTimes.BLL;
using NewYorkTimes.Communication;
using NewYorkTimes.Models.BLL;
using System.Configuration;

namespace NewYorkTimes.Api
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            Register(container);
        }

        private void Register(TinyIoCContainer container)
        {
            container.Register<IAppSettings>(new AppSettings
            {
                BaseUrl = ConfigurationManager.AppSettings["BaseUrl"],
                ApiKey = ConfigurationManager.AppSettings["ApiKey"],
                BaseSection = ConfigurationManager.AppSettings["BaseSection"]
            });

            container.BusinessLogicRegistration();
            container.CommunicationRegistration();
            container.ModelsRegistration();

        }
    }
}