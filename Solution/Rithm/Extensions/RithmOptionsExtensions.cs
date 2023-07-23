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
        /// <typeparam name="TBlogArticle"></typeparam>
        /// <param name="options"></param>
        /// <param name="serializerOptions">Provide serializer options or use the default ones found in JsonHelper.GetJsonSerializerOptions</param>
        /// <returns></returns>
        public static RithmOptions AddBlog<TBlogArticle>(this RithmOptions options, JsonSerializerOptions? serializerOptions = null) where TBlogArticle : BlogArticle
        {
            serializerOptions ??= JsonHelper.GetJsonSerializerOptions();

            options.AddIngestor<EmbeddedMarkdownIngestor>(ing =>
            {
                ing.WithType<TBlogArticle>();

                if(serializerOptions != null)
                    ing.WithJsonSerializerOptions(serializerOptions);
            });
            options.ServiceCollection.AddScoped<IBlogHelper, BlogHelper>();
            options.ServiceCollection.AddScoped<IBlogHelper<TBlogArticle>, BlogHelper<TBlogArticle>>();

            return options;
        }

        public static RithmOptions AddDocumentation(this RithmOptions options)
        {
            options.AddIngestor<EmbeddedMarkdownIngestor>(ing => ing.WithType<DocumentationArticle>());

            return options;
        }
    }
}
