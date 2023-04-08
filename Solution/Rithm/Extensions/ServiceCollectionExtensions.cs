using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rithm.Blog;

namespace Rithm
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddRithm(this IServiceCollection services, Action<RithmOptions>? action = null)
        {

            var sp = services.BuildServiceProvider();
           

            var rithmOptions = createOptions(sp,action);

            services.AddScoped<IArticleHelper, ArticleHelper>();
            services.AddScoped<IBlogHelper, BlogHelper>();
            services.AddScoped<RithmOptions>(sp => rithmOptions);

            foreach(var distinctIngestorType in rithmOptions.IngestorInfos.Select(s=>s.IngestorType).Distinct())
            {
                services.AddTransient(distinctIngestorType);
            }

            return services;
        }

        private static RithmOptions createOptions(IServiceProvider serviceProvider, Action<RithmOptions>? action)
        {
            var rithmOptions = serviceProvider.GetRequiredService<IOptions<RithmOptions>>().Value;

            //set defaults
            rithmOptions.Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var env = serviceProvider.GetRequiredService<IWebAssemblyHostEnvironment>();
            if (!env.IsProduction() && rithmOptions.MinimumVersion == null)
                rithmOptions.MinimumVersion = new Version(0, 0, 0, 0);

            //apply any customizations
            action?.Invoke(rithmOptions);

            if (rithmOptions.AddDefaultIngestors)
            {
                rithmOptions.AddIngestor<ComponentIngestor>();
            }

            return rithmOptions;
        }
    }
}
