using System;

namespace NewYorkTimes.Models.Api
{
    public class ArticleView
    {
        public string Heading { get; set; }

        public DateTime Updated { get; set; }

        public string Link { get; set; }
    }
}
