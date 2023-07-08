using Microsoft.Extensions.DependencyInjection;
using Rithm.Blog;
using Rithm.Documentation;
using System.Text.Json;

namespace Rithm
{
    public static class RithmOptionsExtensions 
    {
        /// <summary>
        /// Default usage creates a BlogArticle from embedded resources
        /// </summary>
        public static RithmOptions AddBlog(this RithmOptions options, JsonSerializerOptions? serializerOptions = null) => AddBlog<BlogArticle>(options, serializerOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="serializerOptions">Provide serializer options or use the default ones found in JsonHelper.GetJsonSerializerOptions</param>
        /// <returns></returns>
        public static RithmOptions AddBlog<T>(this RithmOptions options, JsonSerializerOptions? serializerOptions = null) where T : BlogArticle
        {
            serializerOptions ??= JsonHelper.GetJsonSerializerOptions();

            options.AddIngestor<EmbeddedMarkdownIngestor>(ing =>
            {
                ing.WithType<T>();

                if(serializerOptions != null)
                    ing.WithJsonSerializerOptions(serializerOptions);
            });
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
