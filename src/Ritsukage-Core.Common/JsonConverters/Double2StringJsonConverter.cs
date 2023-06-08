using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Convert double to string with precision.
    /// </summary>
    public class Double2StringJsonConverter : JsonConverter<double>
    {
        private readonly int _precision;

        /// <summary>
        /// Constructor without precision.
        /// </summary>
        public Double2StringJsonConverter()
        {
            _precision = -1;
        }

        /// <summary>
        /// Constructor with precision.
        /// </summary>
        /// <param name="precision"></param>
        public Double2StringJsonConverter(int precision)
        {
            _precision = precision;
        }

        /// <summary>
        /// Read double value from string.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return double.Parse(reader.GetString()!);
        }

        /// <summary>
        /// Write double value to string with precision.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_precision switch
            {
                -1 => null,
                0  => "0",
                _  => "0." + new string('0', _precision),
            }));
        }
    }
}
