using Microsoft.Extensions.Options;
using Rithm.Models;
using System.Text;
using System.Text.RegularExpressions;

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

        public async Task<IEnumerable<ArticleSearchResult>> SearchArticlesAsync(ArticleSearchParameters? parameters, CancellationToken cancellationToken)
        {
            var allArticles = (await GetArticlesAsync(new(), cancellationToken))
                                .OrderByDescending(a => a.Date);

            return await SearchArticlesAsync(parameters, allArticles, cancellationToken);
        }

        public async Task<IEnumerable<ArticleSearchResult>> SearchArticlesAsync(ArticleSearchParameters? parameters, IEnumerable<IArticle> articlesToSearch, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (String.IsNullOrWhiteSpace(parameters.Keywords)) return Enumerable.Empty<ArticleSearchResult>();

            //for now just concat all article content together and search that
            var results = new List<ArticleSearchResult>();
            
            var surroundingTextLengthPerSide = parameters.SurroundingTextLength / 2;

            foreach(var article in articlesToSearch)
            {
                var sb = new StringBuilder();
                sb.Append(article.Title);
                sb.Append(article.Description);

                if(article is MarkdownArticle asMarkdownArticle)
                    sb.Append(asMarkdownArticle.Content.ToString());

                var searchString = sb.ToString();
                var matches = Regex.Matches(searchString, parameters.Keywords, RegexOptions.IgnoreCase);

                foreach(Match match in matches)
                {
                    var matchIndex = match.Index;

                    //try to get the surrounding text
                    var start = matchIndex - surroundingTextLengthPerSide;
                    if (start < 0)
                        start = 0;
                    
                    var end = matchIndex + surroundingTextLengthPerSide;
                    if (end > sb.Length)
                        end = sb.Length;

                    var phrase = searchString.Substring(start, end - start);

                    var articleSearchResult = new ArticleSearchResult();
                    articleSearchResult.Article = article;
                    articleSearchResult.Matches.Add(new ArticleSearchMatch()
                    {
                        Phrase = phrase
                    });

                    results.Add(articleSearchResult);
                }
            }

            return results;
        }
    }
}
