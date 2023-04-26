using Microsoft.Extensions.DependencyInjection;
using Rithm.Blog;
using Rithm.Documentation;

namespace Rithm
{
    public static class RithmOptionsExtensions 
    {
        public static RithmOptions AddBlog(this RithmOptions options)
        {
            options.AddIngestor<EmbeddedMarkdownIngestor>(ing=>ing.WithType<BlogArticle>());
            options.ServiceCollection.AddScoped<IBlogHelper, BlogHelper>();

            return options;
        }

        public static RithmOptions AddDocumentation(this RithmOptions options)
        {
            options.AddIngestor<EmbeddedMarkdownIngestor>(ing => ing.WithType<DocumentationArticle>());

            return options;
        }
    }
}
