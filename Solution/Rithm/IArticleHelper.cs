namespace Rithm
{
    public interface IArticleHelper
    {
        async Task<T?> FindArticleAsync<T>(string key, CancellationToken cancellationToken = default) where T : class, IArticle  => (await FindArticleAsync(key) as T);
        async Task<IEnumerable<T>> GetArticlesAsync<T>(CancellationToken cancellationToken = default) where T : IArticle => (await GetArticlesAsync()).OfType<T>();
        Task<IArticle?> FindArticleAsync(string key, CancellationToken cancellationToken = default);
        Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<IArticle>> GetArticlesByTagAsync(string tag, CancellationToken cancellationToken = default);
        Task<IEnumerable<IArticle>> GetArticlesByKindAsync(string kind, CancellationToken cancellationToken = default);
    }
}
