using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Helper;

public class NewsService
{
    private const string ApiKey = "28157a775eee43ef8bb6739605164b24";
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
            return jsonResult.Articles;
        }
        return null;
    }
}

public class NewsApiResponse
{
    public List<NewsArticle> Articles { get; set; }
}
