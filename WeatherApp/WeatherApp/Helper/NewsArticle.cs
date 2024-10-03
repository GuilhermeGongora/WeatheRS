using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Helper
{
    public class NewsArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public string PublishedAt { get; set; }
        public string SourceName { get; set; } // Adicione esta propriedade
    }

    public class Source
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
