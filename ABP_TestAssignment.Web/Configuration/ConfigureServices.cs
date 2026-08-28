using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ABP_TestAssignment.Web
{
    public static class ConfigureServices
    {
        public static IMvcBuilder SetUpJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opts.JsonSerializerOptions.Converters.Add(new FixedDecimalConverter());
            });
            return builder;
        }

        class FixedDecimalConverter : JsonConverter<decimal>
        {
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Number)
                    return reader.GetDecimal();

                if (reader.TokenType == JsonTokenType.String &&
                    decimal.TryParse(reader.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                    return result;

                throw new JsonException($"Invalid decimal format: {reader.GetString()}");
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("F2", CultureInfo.InvariantCulture));
            }
        }
    }
}
