using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Convert DateTime to string
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Default DateTime format
        /// </summary>
        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// DateTime format
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DateTimeJsonConverter() : this(DefaultDateTimeFormat)
        {
        }

        /// <summary>
        /// Constructor with DateTime format
        /// </summary>
        /// <param name="format"></param>
        public DateTimeJsonConverter(string format)
        {
            DateTimeFormat = format;
        }

        /// <summary>
        /// Read DateTime from string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()!);
        }

        /// <summary>
        /// Write DateTime to string
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateTimeFormat));
        }
    }
}
