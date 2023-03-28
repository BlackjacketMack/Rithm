using Microsoft.AspNetCore.Components;

namespace Rithm.Articles.Abstract
{
    public class JsonArticle : IArticle
    {
        public string? Title { get; set; }

        /// <summary>
        /// A unique key for the article.  For blog posts this is used to generate the permalink.
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// This should be set and sealed by derived classes (e.g. BlogArticle)
        /// </summary>
        public virtual string Kind { get; set; } = "Default";

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
