using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq; // Adicione isso para usar LINQ
using System.Threading.Tasks;
using WeatherApp.Helper;

public class NewsService
{
    private const string ApiKey = "28157a775eee43ef8bb6739605164b24"; // Adicione sua chave da API aqui
    private const string BaseUrl = "https://newsapi.org/v2/everything?q=queimadas OR \"mudanças climáticas\" OR sustentabilidade OR \"aquecimento global\" OR poluição OR \"energia renovável\"&sortBy=publishedAt&language=pt&apiKey=";

    public async Task<List<NewsArticle>> GetNewsAsync()
    {
        var client = new RestClient(BaseUrl + ApiKey);
        var request = new RestRequest();

        request.Method = Method.Get; // Atualize para a versão mais recente
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var jsonResult = JsonConvert.DeserializeObject<NewsApiResponse>(response.Content);
            var articles = jsonResult.Articles.Select(article => {
                article.SourceName = article.Source?.Name; // Mapeia o nome da fonte
                return (NewsArticle)article; // Converte para NewsArticle
            }).ToList();
            return articles;
        }
        return null;
    }
}

public class NewsApiResponse
{
    public string Status { get; set; }
    public int TotalResults { get; set; }
    public List<ArticleWrapper> Articles { get; set; }
}

public class ArticleWrapper : NewsArticle
{
    public Source Source { get; set; } // Adiciona a propriedade Source para mapear os dados da fonte
}
