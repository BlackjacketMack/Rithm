using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rithm.Blog;
using Rithm.Documentation;

namespace Rithm
{
    public static class RithmOptionsExtensions 
    {
        public static RithmOptions AddBlog(this RithmOptions options)
        {
            options.AddIngestor<EmbeddedMarkdownIngestor>(ing=>ing.WithType<BlogArticle>());

            return options;
        }

        public static RithmOptions AddDocumentation(this RithmOptions options)
        {
            options.AddIngestor<EmbeddedMarkdownIngestor>(ing => ing.WithType<DocumentationArticle>());

            return options;
        }
    }
}
