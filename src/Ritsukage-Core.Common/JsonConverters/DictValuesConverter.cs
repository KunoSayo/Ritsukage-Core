using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUCore.Common.JsonConverters
{
    /// <summary>
    /// DictKeyAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DictKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// Convert Dictionary to Json
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class DictValuesConverter<TKey, TValue, T> : JsonConverter<T> where T : IDictionary<TKey, TValue>, new()
    {
        /// <summary>
        /// Read
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="JsonException"></exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PropertyInfo? property =
                typeof(TValue).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(
                    p => p.IsDefined(typeof(DictKeyAttribute)) && typeof(TKey).IsAssignableFrom(p.PropertyType)) ??
                throw new NotSupportedException();
            T dictionary = new();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                    {
                        TValue? value = JsonSerializer.Deserialize<TValue?>(ref reader, options);
                        dictionary[(TKey)property.GetValue(value)!] = value!;
                        break;
                    }
                    case JsonTokenType.EndArray:
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
            JsonSerializer.Serialize(writer, value.Values, options);
        }
    }
}
