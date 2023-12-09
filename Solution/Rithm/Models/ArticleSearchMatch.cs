using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleSearchMatch
{
    public string Section { get; set; }
    public string Phrase { get; set; } = default!;
}
