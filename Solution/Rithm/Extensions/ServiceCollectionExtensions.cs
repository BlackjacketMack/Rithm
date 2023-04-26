using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rithm.Blog;

namespace Rithm
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddRithm(this IServiceCollection serviceCollection, Action<RithmOptions>? action = null)
        {

           

            var rithmOptions = createOptions(serviceCollection, action);

            serviceCollection.AddScoped<IArticleHelper, ArticleHelper>();
            serviceCollection.AddScoped<RithmOptions>(sp => rithmOptions);

            foreach(var distinctIngestorType in rithmOptions.IngestorInfos.Select(s=>s.IngestorType).Distinct())
            {
                serviceCollection.AddTransient(distinctIngestorType);
            }

            return serviceCollection;
        }

        private static RithmOptions createOptions(IServiceCollection serviceCollection, Action<RithmOptions>? action)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var rithmOptions = serviceProvider.GetRequiredService<IOptions<RithmOptions>>().Value;
            rithmOptions.ServiceCollection = serviceCollection;

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
