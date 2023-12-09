using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleSearchParameters
{
    public int SurroundingTextLength { get; set; } = 100;
    public int MatchLimit { get; set; } = 3;
    public string Keywords { get; set; } = default!;
}
