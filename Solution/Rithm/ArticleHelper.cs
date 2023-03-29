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

        public async Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<IArticle> articles = null;
            if (_articleConfiguration.Debug)
            {
                articles = await ingestArticlesAsync();
            }
            else
            {
                articles = await _lazyArticles.Value;
            }

            return articles.Where(w => w.Version >= _articleConfiguration.MinimumVersion);
        }

        public async Task<IArticle?> FindArticleAsync(string key, CancellationToken cancellationToken)
        {
            return (await GetArticlesAsync(cancellationToken)).Where(w => w.Key == key).SingleOrDefault();
        }

        public async Task<IEnumerable<IArticle>> GetArticlesByTagAsync(string tag, CancellationToken cancellationToken)
        {
            return (await GetArticlesAsync(cancellationToken)).Where(w => w.Tags.Contains(tag));
        }
        public async Task<IEnumerable<IArticle>> GetArticlesByKindAsync(string kind, CancellationToken cancellationToken)
        {
            return (await GetArticlesAsync(cancellationToken)).Where(w => w.Kind == kind);
        }
    }
}
