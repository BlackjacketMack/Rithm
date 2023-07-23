namespace Rithm.Blog;

/// <summary>
/// This interface exists to give users a simple basic type access to the BlogArticle module
/// </summary>
public interface IBlogHelper : IBlogHelper<BlogArticle>
{
    
}

public class BlogHelper : BlogHelper<BlogArticle>, IBlogHelper
{
    public BlogHelper(IArticleHelper articleHelper) : base(articleHelper)
    {
    }
}
