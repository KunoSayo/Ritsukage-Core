using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Convert unix timestamp to datetime and vice versa
    /// </summary>
    public class UnixTimeStampJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Milliseconds option
        /// </summary>
        public bool Milliseconds { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public UnixTimeStampJsonConverter() : this(false)
        {
        }

        /// <summary>
        /// Constructor with milliseconds option
        /// </summary>
        /// <param name="milliseconds"></param>
        public UnixTimeStampJsonConverter(bool milliseconds)
        {
            Milliseconds = milliseconds;
        }

        /// <summary>
        /// Read unix timestamp or datetime string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => Milliseconds
                    ? DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime.ToLocalTime()
                    : DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).DateTime.ToLocalTime(),
                JsonTokenType.String => DateTime.Parse(reader.GetString()!),
                _ => throw new ArgumentException($"Expected unix timestamp or datetime string, got {reader.TokenType}")
            };
        }

        /// <summary>
        /// Write unix timestamp
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (Milliseconds)
                writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
            else
                writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeSeconds());
        }
    }
}
