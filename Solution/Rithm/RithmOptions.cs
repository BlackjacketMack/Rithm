using System.Reflection;

namespace Rithm
{
    public class RithmOptions
    {
        /// <summary>
        /// Assemblies to look for component articles
        /// </summary>
        public IEnumerable<Assembly> Assemblies { get; set; } = Enumerable.Empty<Assembly>();

        /// <summary>
        /// Set to true to always load the lazy article collection.  There would be big performance implications so do not set to true in any production environment.
        /// </summary>
        public bool Debug { get; set; }

        internal IList<IngestorInfo> IngestorInfos { get; private set; } = new List<IngestorInfo>();

        /// <summary>
        /// When true adds default ingestors
        /// </summary>
        public bool AddDefaultIngestors { get; internal set; } = true;

        /// <summary>
        /// Gets the minimum version that will be returned when querying articles.
        /// This is usually attached to the environment configuration.
        /// By default non production environments will show versions less than 1 and production environments will require
        /// a major version of 1 or greater.  Less than 1 equates to a 'draft'.
        /// </summary>
        public Version MinimumVersion { get; set; } = new Version("1.0.0.0");

        public RithmOptions AddIngestor<TIngestor>(Action<TIngestor>? configActions = null) where TIngestor : IArticleIngestor
        {
            IngestorInfos.Add(new IngestorInfo
            {
                IngestorType = typeof(TIngestor),
                ConfigActions = configActions != null ? (ai => configActions((TIngestor)ai)) : null
            });

            return this;
        }
    }
}
