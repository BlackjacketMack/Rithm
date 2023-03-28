using Rithm.Articles.Abstract;
using Rithm.Articles.Interfaces;

namespace Rithm.Articles.Blog
{
    public class BlogArticle : MarkdownArticle, IArticleImage
    {
        public sealed override string Kind => "Posts";

        public override string Url => $"blog/{Key}";

        public virtual string Slug {get => Key; set => Key = value;}

        public string? Image { get; set; }

        public string? Series { get; set; }

        /// <summary>
        /// Draft sets the major version to 0.
        /// </summary>
        public bool Draft
        {
            get => base.Version.Major < 1;
            set => base.Version = new Version(0,base.Version.Minor,base.Version.Build,base.Version.Revision);
        }
    }
}
