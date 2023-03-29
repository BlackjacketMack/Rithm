using Microsoft.AspNetCore.Components;

namespace Rithm
{
    public abstract class MarkdownArticle : IArticle
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }

        /// <summary>
        /// A unique key for the article.  For blog posts this is used to generate the permalink.
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// This should be set and sealed by derived classes (e.g. BlogArticle)
        /// </summary>
        public virtual string Kind => "Default";

        /// <summary>
        /// The description of the article.  
        /// </summary>
        public string? Description { get; set; }

        public virtual string? Url { get; set; }

        /// <summary>
        /// The optional date of the article. For blog posts this is the post date.
        /// </summary>
        public DateTime Date { get; set; }

        public IList<string> Tags { get; set; } = new List<string>();

        public MarkupString Content => LazyContent.Value;

        internal Lazy<MarkupString> LazyContent { get; set; } = new();

        public Version Version { get; set; } = new Version("1.0.0.0");

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
