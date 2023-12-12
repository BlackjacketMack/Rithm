using Microsoft.Extensions.Options;
using Rithm.Models;
using System.Text;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

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

        public async Task<IEnumerable<ArticleSearchResult>> SearchArticlesAsync(ArticleSearchParameters parameters, CancellationToken cancellationToken)
        {
            var allArticles = (await GetArticlesAsync(new(), cancellationToken))
                                .OrderByDescending(a => a.Date);

            return await SearchArticlesAsync(parameters, allArticles, cancellationToken);
        }

        public async Task<IEnumerable<ArticleSearchResult>> SearchArticlesAsync(ArticleSearchParameters parameters, IEnumerable<IArticle> articlesToSearch, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (String.IsNullOrWhiteSpace(parameters.Keywords)) return Enumerable.Empty<ArticleSearchResult>();

            //for now just concat all article content together and search that
            var results = new List<ArticleSearchResult>();

            foreach(var article in articlesToSearch)
            {

                //we'll find any of the words in the search string in addition to the whole phrase
                
                var articleSearchResult = new ArticleSearchResult();
                articleSearchResult.Article = article;

                var searchMatches = new List<ArticleSearchMatch>();

                var searchEntries = new (string Name, string? Value)[]
                {
                    ("Title", article.Title),
                    ("Subtitle", article.Subtitle),
                    ("Description", article.Description),
                    ("Content", article.ToSearchString())
                };          

                foreach(var searchEntry in searchEntries)
                {
                    search(searchEntry.Name, searchEntry.Value, parameters.Keywords, parameters.PhraseLength, searchMatches);
                }

                //if there are no search matches, skip this article
                if (!searchMatches.Any()) continue;

                var rawScore = searchMatches.Sum(m => m.Score);
                var finalScore = rawScore;

                //if the rawscore is >= 100 but there is no exact match, limit it to 99
                if(rawScore >= 100 && !searchMatches.Any(m => m.Score == 100))
                    finalScore = 99;

                //sum the scores
                articleSearchResult.Score = finalScore;

                //limit the number of matches returned
                articleSearchResult.Matches = searchMatches.OrderByDescending(m => m.Score).Take(parameters.MatchLimit).ToList();

                results.Add(articleSearchResult);
            }

            return results;
        }

        private void search(string sourceName, string searchString, string keywords, int phraseLength, List<ArticleSearchMatch> searchMatches)
        {
            if (searchString == null) return;

            var keywordsLength = keywords.Length;
            var searchPattern = $"{keywords}|{keywords.Replace(" ", "|")}";
            var partialCount = keywords.Split(" ").Length;
            var matches = Regex.Matches(searchString, searchPattern, RegexOptions.IgnoreCase);
            var searchPatternLength = searchPattern.Length;
            var phraseLengthPerSide = phraseLength / 2;

            if (!matches.Any()) return;
            
            foreach (Match match in matches)
            {
                var matchIndex = match.Index;

                //assume exact match
                var score = 100d;

                //partials get scored fractionally 
                var isPartialMatch = match.Value.Length != keywords.Length;

                //if it's a partial match, score it fractionally
                if (isPartialMatch)
                    score = (1d / partialCount) * 100;

                //try to get the surrounding text
                var start = matchIndex - phraseLengthPerSide;
                if (start < 0)
                    start = 0;

                var end = matchIndex + phraseLengthPerSide;
                if (end > searchString.Length)
                    end = searchString.Length;

                var phrase = searchString.Substring(start, end - start);

                searchMatches.Add(new ArticleSearchMatch()
                {
                    Section = sourceName,
                    Term = match.Value,
                    Phrase = phrase,
                    Score = (int)Math.Ceiling(score),
                    IsExactMatch = !isPartialMatch
                });
            }
        }
    }
}
