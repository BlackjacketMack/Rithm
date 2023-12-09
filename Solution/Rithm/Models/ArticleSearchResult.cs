using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleSearchResult
{
    /// <summary>
    /// The higher the score the more matches
    /// </summary>
    public int Score { get; set; }

    public IArticle Article { get; set; } = default!;
    
    /// <summary>
    /// matches phrases
    /// </summary>
    public IList<ArticleSearchMatch> Matches { get; set; } = new List<ArticleSearchMatch>();
}
