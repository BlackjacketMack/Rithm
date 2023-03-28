using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Rithm.Articles.Abstract;
using Rithm.Articles.Blog;
using System;
using System.Reflection;

namespace Rithm.Articles
{
    public static class ServiceCollectionExtensions 
    {
        public static void AddArticles(this IServiceCollection services, Action<ArticleConfiguration>? action = null)
        {

            var sp = services.BuildServiceProvider();
           

            var articleConfiguration = createConfiguration(sp,action);


           


            services.AddScoped<IArticleHelper, ArticleHelper>();
            services.AddScoped<IBlogHelper, BlogHelper>();
            services.AddScoped<ArticleConfiguration>(sp => articleConfiguration);

            foreach(var distinctIngestorType in articleConfiguration.IngestorInfos.Select(s=>s.IngestorType).Distinct())
            {
                services.AddTransient(distinctIngestorType);
            }
        }

        private static ArticleConfiguration createConfiguration(IServiceProvider serviceProvider, Action<ArticleConfiguration>? action)
        {
            var articleConfiguration = new ArticleConfiguration();

            //set defaults
            articleConfiguration.Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var env = serviceProvider.GetRequiredService<IWebAssemblyHostEnvironment>();
            if (!env.IsProduction())
                articleConfiguration.MinimumVersion = new Version(0, 0, 0, 0);

            //apply any customizations
            action?.Invoke(articleConfiguration);

            if (articleConfiguration.AddDefaultIngestors)
            {
                articleConfiguration.AddIngestor<ComponentIngestor>()
                                    .AddIngestor<EmbeddedMarkdownIngestor>();
            }

            return articleConfiguration;
        }
    }
}
