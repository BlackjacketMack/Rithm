using Microsoft.Extensions.Options;

namespace Rithm
{
    public class ArticleHelper : IArticleHelper
    {
        private static Lazy<Task<List<IArticle>>> _lazyArticles = default!;
        private readonly IServiceProvider _serviceProvider;
        private readonly RithmOptions _rithmOptions;

        public ArticleHelper(IServiceProvider serviceProvider, RithmOptions rithmOptions)
        {
            _serviceProvider = serviceProvider;
            _rithmOptions = rithmOptions;
            if (_lazyArticles == null)
                _lazyArticles = new(() => ingestArticlesAsync());

        }

        private async Task<List<IArticle>> ingestArticlesAsync()
        {
            var articles = new List<IArticle>();
            Console.WriteLine(String.Concat("Ingestor Infos=",_rithmOptions.IngestorInfos.Count()));
            foreach(var ingestorInfo in _rithmOptions.IngestorInfos)
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

            var minimumVersion = _rithmOptions.MinimumVersion;
            if (parameters.MinimumVersion != null)
                minimumVersion = parameters.MinimumVersion;

            Console.WriteLine("RithmOptions.MinimumVersion=" + _rithmOptions.MinimumVersion);

            IEnumerable<IArticle> articles = null!;
            if (_rithmOptions.Debug)
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

            if (parameters.Categories.Any())
                articles = articles.Where(a => parameters.Categories.Contains(a.Key));

            return articles;
        }
    }
}
