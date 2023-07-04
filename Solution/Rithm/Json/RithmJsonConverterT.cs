using Rithm.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rithm;

internal abstract class RithmJsonConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var optionsWithoutThisConverter = new JsonSerializerOptions(options);
        optionsWithoutThisConverter.Converters.Remove(this);

        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var json = jsonDoc.RootElement.GetRawText();
            return JsonSerializer.Deserialize<T>(json, optionsWithoutThisConverter);
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var optionsWithoutThisConverter = new JsonSerializerOptions(options);
        optionsWithoutThisConverter.Converters.Remove(this);

        var json = JsonSerializer.Serialize(value, optionsWithoutThisConverter);

        writer.WriteRawValue(json);
    }
}
