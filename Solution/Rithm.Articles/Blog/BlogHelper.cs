using Microsoft.AspNetCore.Components;

namespace Rithm.Articles.Blog
{
    public interface IBlogHelper
    {
        Task<BlogArticle> FindArticleAsync(string key, CancellationToken cancellationToken = default);
        Task<IEnumerable<BlogArticle>> GetPostsAsync(bool? isDraft = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<BlogArticle>> GetSeriesAsync(BlogArticle article, CancellationToken cancellationToken = default);
        IEnumerable<BlogSeries> CreateSeries(IEnumerable<BlogArticle> articles);
        Task<BlogArticle?> GetPrevInSeriesAsync(BlogArticle article, CancellationToken cancellationToken = default);
        Task<BlogArticle?> GetNextInSeriesAsync(BlogArticle article, CancellationToken cancellationToken = default);
    }

    public class BlogHelper : IBlogHelper
    {
        private readonly IArticleHelper _articleHelper;

        public BlogHelper(IArticleHelper articleHelper)
        {
            _articleHelper = articleHelper;
        }

        public IEnumerable<BlogSeries> CreateSeries(IEnumerable<BlogArticle> articles)
        {
            return articles
                    .GroupBy(gb => gb.Series ?? Guid.NewGuid().ToString())
                    .Select(g => new BlogSeries(g));
        }

        public async Task<IEnumerable<BlogArticle>> GetPostsAsync(bool? isDraft = null, CancellationToken cancellationToken = default)
        {
            return (await _articleHelper.GetArticlesAsync<BlogArticle>(cancellationToken))
                                .Where(w=>isDraft == null || w.Draft == isDraft);
        }

        public async Task<IEnumerable<BlogArticle>> GetSeriesAsync(BlogArticle article, CancellationToken cancellationToken)
        {
            return (await _articleHelper.GetArticlesAsync<BlogArticle>(cancellationToken))
                                .Where(w=>w.Series == article.Series);
        }

        public async Task<BlogArticle?> GetPrevInSeriesAsync(BlogArticle article, CancellationToken cancellationToken)
        {
            if (article.Series == null) return null;

            var series = (await GetSeriesAsync(article,cancellationToken)).ToList();

            var articleNumber = series.IndexOf(article);
            var prevArticleNumnber = articleNumber - 1;

            if (prevArticleNumnber >= 0)
                return series[prevArticleNumnber];
            else
                return null;
        }

        public async Task<BlogArticle?> GetNextInSeriesAsync(BlogArticle article, CancellationToken cancellationToken)
        {
            if (article.Series == null) return null;

            var series = (await GetSeriesAsync(article,cancellationToken)).ToList();

            var articleIndex = series.IndexOf(article);
            var nextArticleIndex = articleIndex + 1;

            if ((nextArticleIndex) < series.Count)
                return series[nextArticleIndex];
            else
                return null;
        }

        public Task<BlogArticle> FindArticleAsync(string key, CancellationToken cancellationToken) => _articleHelper.FindArticleAsync<BlogArticle>(key, cancellationToken)!;
    }
}
