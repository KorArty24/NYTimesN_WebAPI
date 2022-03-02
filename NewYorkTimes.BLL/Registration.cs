using Nancy.TinyIoc;
using NewYorkTimes.BLL.Services;

namespace NewYorkTimes.BLL
{
    public static class Registration
    {
        public static TinyIoCContainer BusinessLogicRegistration(this TinyIoCContainer container)
        {
            container.Register<IArticleService, ArticleService>();

            return container;
        }
    }
}
