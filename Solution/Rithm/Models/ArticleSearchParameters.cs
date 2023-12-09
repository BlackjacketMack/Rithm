using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleSearchParameters
{
    public int SurroundingTextLength { get; set; } = 100;
    public string Keywords { get; set; }
}
