using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Convert type from TFrom to TTo
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    public class ChangeTypeJsonConverter<TFrom, TTo> : JsonConverter<TTo> where TFrom : TTo
    {
        /// <summary>
        /// Read from json
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override TTo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TFrom>(ref reader, options)!;
        }

        /// <summary>
        /// Write to json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, TTo value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (TFrom?)value, options);
        }
    }
}
