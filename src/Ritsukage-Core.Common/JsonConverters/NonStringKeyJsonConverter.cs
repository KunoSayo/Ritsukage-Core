using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// Convert Dictionary to Json
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="T"></typeparam>
    public sealed class NonStringKeyJsonConverter<TKey, TValue, T> : JsonConverter<T>
        where T : IDictionary<TKey, TValue>, new()
    {
        /// <summary>
        /// Read
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            T dictionary = new();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                    {
                        TKey key = (TKey?)Convert.ChangeType(reader.GetString(), typeof(TKey?))!;
                        reader.Read();
                        dictionary[key] = JsonSerializer.Deserialize<TValue>(ref reader, options!)!;
                        break;
                    }
                    case JsonTokenType.EndObject:
                    {
                        return dictionary;
                    }
                }
            }

            throw new JsonException($"Can't deserialize type {typeof(T).FullName}.");
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<TKey, TValue> pair in value)
            {
                writer.WritePropertyName(pair.Key!.ToString()!);
                JsonSerializer.Serialize(writer, pair.Value, options);
            }

            writer.WriteEndObject();
        }
    }
}
