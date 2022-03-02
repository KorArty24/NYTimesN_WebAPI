using AutoMapper;
using NewYorkTimes.Models.Api;
using NewYorkTimes.Models.BLL;
using System;

namespace NewYorkTimes.Models.Mapper
{
    public class ModelsMapper : Profile
    {
        public ModelsMapper()
        {
            CreateMap<dynamic, Article>()
                .ForMember(d => d.Heading, m => m.MapFrom((s, v) => s.title))
                .ForMember(d => d.Link, m => m.MapFrom((s, v) => s.short_url))
                .ForMember(d => d.Updated,
                    m => m.MapFrom((s, v) =>
                        DateTime.Parse(s.updated_date.ToString())));

            CreateMap<Article, ArticleView>();
        }
    }
}
