using System.Text.Json;

namespace Rithm;

public class JsonHelper
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        options.Converters.Add(new ArticleImageJsonConverter());

        return options;
    }
}
