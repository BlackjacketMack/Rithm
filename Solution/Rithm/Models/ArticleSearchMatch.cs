using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleSearchMatch
{
    /// <summary>
    /// The section of an article searched
    /// </summary>
    public string? Section { get; set; }
    
    /// <summary>
    /// The matching keywords
    /// </summary>
    public string Term { get; set; } = default!;
    
    /// <summary>
    /// The score
    /// </summary>
    public int Score { get; set; }
    
    /// <summary>
    /// A contextual phrase
    /// </summary>
    public string Phrase { get; set; } = default!;

    public bool IsExactMatch { get; set; }
}
