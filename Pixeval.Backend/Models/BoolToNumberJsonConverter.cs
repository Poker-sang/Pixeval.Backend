using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

internal class BoolToNumberJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetInt32() is not 0;

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteNumberValue(value ? 1 : 0);
}