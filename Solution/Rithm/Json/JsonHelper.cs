using System.Text.Json;

namespace Rithm;

internal class JsonHelper
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        options.Converters.Add(new ArticleImageConverter());

        return options;
    }
}
