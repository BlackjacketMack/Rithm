using System.Net.Http.Json;

namespace Rithm
{
    public class RemoteJsonIngestor : IArticleIngestor
    {
        private readonly RithmOptions _articleConfiguration;
        private readonly HttpClient _httpClient;

        public RemoteJsonIngestor(RithmOptions articleConfiguration, HttpClient httpClient)
        {
            _articleConfiguration = articleConfiguration;
            _httpClient = httpClient;
        }

        private string? _jsonUrl;

        public Type? _articleType;

        public virtual async Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken)
        {

            var jsonArticles = (await _httpClient.GetFromJsonAsync(_jsonUrl, _articleType, cancellationToken) as IEnumerable<IArticle>);

            Console.WriteLine(jsonArticles.Count());

            return jsonArticles;
        }

        public RemoteJsonIngestor WithUrl(string jsonUrl)
        {
            _jsonUrl = jsonUrl;

            return this;
        }

        public RemoteJsonIngestor WithType<TReturnType>()
        {
            _articleType = typeof(List<TReturnType>);

            return this;
        }

        public Task LoadArticle(IArticle article, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
