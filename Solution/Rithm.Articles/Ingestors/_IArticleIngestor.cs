namespace Rithm.Articles
{
    public interface IArticleIngestor
    {
        Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken = default);
    }
}
