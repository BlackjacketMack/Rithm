using Microsoft.AspNetCore.Components;

namespace Rithm.Blog;

public interface IBlogHelper<TBlogArticle> where TBlogArticle : BlogArticle
{
    Task<TBlogArticle> FindArticleAsync(string key, CancellationToken cancellationToken = default);
    Task<IEnumerable<TBlogArticle>> GetPostsAsync(bool? isDraft = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<TBlogArticle>> GetSeriesAsync(TBlogArticle article, CancellationToken cancellationToken = default);
    IEnumerable<BlogSeries> CreateSeries(IEnumerable<TBlogArticle> articles);
    Task<TBlogArticle?> GetPrevInSeriesAsync(TBlogArticle article, CancellationToken cancellationToken = default);
    Task<TBlogArticle?> GetNextInSeriesAsync(TBlogArticle article, CancellationToken cancellationToken = default);
}

public class BlogHelper<TBlogArticle> : IBlogHelper<TBlogArticle> where TBlogArticle : BlogArticle
{
    private readonly IArticleHelper _articleHelper;

    public BlogHelper(IArticleHelper articleHelper)
    {
        _articleHelper = articleHelper;
    }

    public IEnumerable<BlogSeries> CreateSeries(IEnumerable<TBlogArticle> articles)
    {
        return articles
                .GroupBy(gb => gb.Series ?? Guid.NewGuid().ToString())
                .Select(g => new BlogSeries(g));
    }

    public async Task<IEnumerable<TBlogArticle>> GetPostsAsync(bool? isDraft = null, CancellationToken cancellationToken = default)
    {
        return (await _articleHelper.GetArticlesAsync<TBlogArticle>(cancellationToken))
                            .Where(w=>isDraft == null || w.Draft == isDraft);
    }

    public async Task<IEnumerable<TBlogArticle>> GetSeriesAsync(TBlogArticle article, CancellationToken cancellationToken)
    {
        return (await _articleHelper.GetArticlesAsync<TBlogArticle>(cancellationToken))
                            .Where(w=>w.Series == article.Series);
    }

    public async Task<TBlogArticle?> GetPrevInSeriesAsync(TBlogArticle article, CancellationToken cancellationToken)
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

    public async Task<TBlogArticle?> GetNextInSeriesAsync(TBlogArticle article, CancellationToken cancellationToken)
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

    public Task<TBlogArticle> FindArticleAsync(string key, CancellationToken cancellationToken) => _articleHelper.FindArticleAsync<TBlogArticle>(key, cancellationToken)!;
}
