using Microsoft.AspNetCore.Components;

namespace Rithm
{
    public class ComponentIngestor : IArticleIngestor
    {
        private readonly RithmOptions _articleConfiguration;

        public ComponentIngestor(RithmOptions articleConfiguration)
        {
            _articleConfiguration = articleConfiguration;
        }

        public Task<IEnumerable<IArticle>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            var types = _articleConfiguration.Assemblies.SelectMany(sm => sm.GetTypes());
            var articles = types.Where(w =>
                    !w.IsAbstract &&
                    w.IsAssignableTo(typeof(ComponentBase)) &&
                    w.IsAssignableTo(typeof(IArticle)) &&
                    !w.Name.EndsWith("_Imports"))
                    .Select(t => Activator.CreateInstance(t))
                    .Cast<IArticle>();

            return Task.FromResult(articles);
        }

        public Task LoadArticle(IArticle article, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
