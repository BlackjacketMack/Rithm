namespace Rithm
{
    public interface IArticle
    {
        /// <summary>
        /// A unique key for the article.  For blog posts this is used to generate the permalink.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The title of the article.
        /// </summary>
        string? Title { get => null; }

        string? Subtitle { get => null; }

        /// <summary>
        /// This should be set and sealed by derived classes (e.g. BlogArticle)
        /// </summary>
        string? Kind { get => null; }

        /// <summary>
        /// The description of the article.  
        /// </summary>
        string? Description { get => null; }

        string? Url { get => null; }

        public DateTime Date { get => default;}

        public string? Series { get => null; }

        IList<string> Tags { get => new List<string>(); }

        /// <summary>
        /// Intended for semantic versioning (e.g. major/minor/build/revision).
        /// </summary>
        public Version? Version { get => new Version("1.0.0.0"); }
    }
}
