namespace Rithm
{
    public interface IArticleHelper
    {
        #region DefaultImplementations
        async Task<T?> FindArticleAsync<T>(string key, CancellationToken cancellationToken = default) where T : class, IArticle  => (await FindArticleAsync(key, cancellationToken) as T);
        async Task<IEnumerable<T>> GetArticlesAsync<T>(CancellationToken cancellationToken = default) where T : IArticle => (await GetArticlesAsync()).OfType<T>();
        Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken = default) => GetArticlesAsync(new(), cancellationToken);
        async Task<IArticle?> FindArticleAsync(string key, CancellationToken cancellationToken = default)
        {
            var parameters = new ArticleParameters
            {
                Keys = new[] { key }
            };
            return (await GetArticlesAsync(parameters,cancellationToken)).SingleOrDefault();
        }

        public async Task<IEnumerable<IArticle>> GetArticlesByTagAsync(string tag, CancellationToken cancellationToken = default)
        {
            var parameters = new ArticleParameters
            {
                Tags = new[] { tag }
            };

            return (await GetArticlesAsync(parameters,cancellationToken));
        }

        public async Task<IEnumerable<IArticle>> GetArticlesByKindAsync(string kind, CancellationToken cancellationToken = default)
        {
            var parameters = new ArticleParameters
            {
                Kinds = new[] { kind }
            };

            return (await GetArticlesAsync(parameters, cancellationToken));
        }

        public async Task<IEnumerable<IArticle>> GetArticlesByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            var parameters = new ArticleParameters
            {
                Categories = new[] { category }
            };

            return (await GetArticlesAsync(parameters, cancellationToken));
        }
        #endregion


        Task<IEnumerable<IArticle>> GetArticlesAsync(ArticleParameters parameters, CancellationToken cancellationToken = default);
    }
}
