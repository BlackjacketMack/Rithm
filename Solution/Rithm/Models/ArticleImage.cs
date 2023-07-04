using System.Text.Json.Serialization;

namespace Rithm.Models;

public class ArticleImage
{
    public string? Url { get; set; }
    public string? Credit { get; set; }
    public string? Caption { get; set; }


    /// <summary>
    /// An image can be set by a simple string url
    /// </summary>
    /// <param name="url"></param>
    //public static implicit operator ArticleImage?(string url) => url == null ? null : new ArticleImage { Url = url };
    //public static implicit operator string?(ArticleImage? image) => image?.Url;

    //public static explicit operator ArticleImage?(string url) => url == null ? null : new ArticleImage(url);
    //public static explicit operator string?(ArticleImage? image) => image?.Url;
}
