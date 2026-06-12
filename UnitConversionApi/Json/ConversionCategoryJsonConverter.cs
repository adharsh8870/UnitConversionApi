using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnitConversionApi.Models;

namespace UnitConversionApi.Json
{
    public class ConversionCategoryJsonConverter : JsonConverter<ConversionCategory>
    {
        public override ConversionCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (!string.IsNullOrEmpty(s) && Enum.TryParse<ConversionCategory>(s, true, out var val))
                    return val;
                throw new JsonException($"Unknown ConversionCategory value '{s}'.");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out var i) && Enum.IsDefined(typeof(ConversionCategory), i))
                    return (ConversionCategory)i;
            }
            throw new JsonException("Unable to convert to ConversionCategory.");
        }

        public override void Write(Utf8JsonWriter writer, ConversionCategory value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
