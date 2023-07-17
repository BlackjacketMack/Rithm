using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleImage
{
    public string? Url { get; set; }
    public string? Credit { get; set; }
    public string? Caption { get; set; }
}
