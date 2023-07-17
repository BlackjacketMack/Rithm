using Rithm.Models;
using System.Text.Json;

namespace Rithm;

internal class ArticleImageJsonConverter : RithmJsonConverter<ArticleImage>
{
    public override ArticleImage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return new ArticleImage { Url = reader.GetString() };
        else
            return base.Read(ref reader, typeToConvert, options);
    }
}
