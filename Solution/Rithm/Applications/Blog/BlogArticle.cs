using Rithm.Models;

namespace Rithm.Blog
{
    public class BlogArticle : MarkdownArticle, IArticleImage2
    {
        public sealed override string Kind { get; set; } = "Posts";

        public override string Url => $"blog/{Key}";

        public virtual string Slug {get => Key; set => Key = value;}

        public ArticleImage? Image { get; set; }
        //public string? ImageCaption { get; set; }
        //public string? ImageCredit { get; set; }

        public string? Series { get; set; }

        /// <summary>
        /// Draft sets the major version to 0.
        /// </summary>
        public bool Draft
        {
            get => base.Version.Major < 1;
            set{
                if(value)
                    base.Version = new Version(0,base.Version.Minor,base.Version.Build,base.Version.Revision);
                
            }
        }
    }
}
