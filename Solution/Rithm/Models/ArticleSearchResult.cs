using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleSearchResult
{
    public IArticle Article { get; set; } = default!;
    
    /// <summary>
    /// matches phrases
    /// </summary>
    public IList<ArticleSearchMatch> Matches { get; set; } = new List<ArticleSearchMatch>();
}
