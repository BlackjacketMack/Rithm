namespace Rithm
{
    public class ArticleHelper : IArticleHelper
    {
        private static Lazy<Task<List<IArticle>>> _lazyArticles = default!;
        private readonly IServiceProvider _serviceProvider;
        private readonly ArticleConfiguration _articleConfiguration;

        public ArticleHelper(IServiceProvider serviceProvider, ArticleConfiguration articleConfiguration)
        {
            _serviceProvider = serviceProvider;
            _articleConfiguration = articleConfiguration;
            if (_lazyArticles == null)
                _lazyArticles = new(() => ingestArticlesAsync());
        }

        private async Task<List<IArticle>> ingestArticlesAsync()
        {
            var articles = new List<IArticle>();

            foreach(var ingestorInfo in _articleConfiguration.IngestorInfos)
            {
                var ingestor = _serviceProvider.GetService(ingestorInfo.IngestorType) as IArticleIngestor;
                if (ingestor == null)
                {
                    Console.WriteLine("Ingestor not found");
                    continue;
                }

                ingestorInfo.ConfigActions?.Invoke(ingestor);

                var ingestedArticles = await ingestor.GetArticlesAsync(default);
                articles.AddRange(ingestedArticles);
            }
            
            return articles;
        }

        public async Task<IEnumerable<IArticle>> GetArticlesAsync(ArticleParameters? parameters, CancellationToken cancellationToken)
        {
            parameters ??= new ArticleParameters();

            var minimumVersion = _articleConfiguration.MinimumVersion;
            if (parameters.MinimumVersion != null)
                minimumVersion = parameters.MinimumVersion;

            IEnumerable<IArticle> articles = null;
            if (_articleConfiguration.Debug)
            {
                articles = await ingestArticlesAsync();
            }
            else
            {
                articles = await _lazyArticles.Value;
            }

            articles = articles.Where(w => w.Version >= minimumVersion);

            if (parameters.Tags.Any())
                articles = articles.Where(a => parameters.Tags.Intersect(a.Tags).Any());

            if(parameters.Kinds.Any())
                articles = articles.Where(a => parameters.Kinds.Contains(a.Kind));

            if (parameters.Keys.Any())
                articles = articles.Where(a => parameters.Keys.Contains(a.Key));

            return articles;
        }
    }
}
