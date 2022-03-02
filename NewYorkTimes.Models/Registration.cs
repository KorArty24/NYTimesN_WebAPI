using AutoMapper;
using Nancy.TinyIoc;
using NewYorkTimes.Models.Mapper;

namespace NewYorkTimes.BLL
{
    public static class Registration
    {
        public static TinyIoCContainer ModelsRegistration(this TinyIoCContainer container)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelsMapper());
            });

            var mapper = mappingConfig.CreateMapper();
            container.Register(mapper);

            return container;
        }
    }
}
