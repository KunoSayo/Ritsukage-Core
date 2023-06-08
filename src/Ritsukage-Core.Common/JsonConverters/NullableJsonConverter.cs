using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Convert Dictionary to Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TConverter"></typeparam>
    public class NullableJsonConverter<T, TConverter> : JsonConverter<T?> where T : struct
                                                                          where TConverter : JsonConverter<T>, new()
    {
        /// <summary>
        /// Read
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TConverter converter = new();
            return converter.Read(ref reader, typeof(T), options);
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            TConverter converter = new();
            converter.Write(writer, value.GetValueOrDefault(), options);
        }
    }
}
